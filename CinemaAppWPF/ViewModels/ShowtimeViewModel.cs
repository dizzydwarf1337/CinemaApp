using CinemaAppWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using CinemaAppWPF.DTO;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using CinemaAppWPF.Session;
using System.Linq;
using System.Collections.Generic;

namespace CinemaAppWPF.ViewModels
{
    public class ShowtimesViewModel : INotifyPropertyChanged
    {
        private readonly SessionService _sessionService;
        private readonly MovieService _movieService;
        private readonly HallService _hallService;
        private readonly TicketService _ticketService;
        private readonly UserSession _userSession;
        private readonly CinemaService _cinemaService;
        private readonly Func<sessionDto, bool?> _showSessionAddEditWindow;

        public ShowtimesViewModel(Func<sessionDto, bool?> showSessionAddEditWindow)
        {
            _sessionService = new SessionService();
            _movieService = new MovieService();
            _hallService = new HallService();
            _ticketService = new TicketService();
            _cinemaService = new CinemaService();
            _showSessionAddEditWindow = showSessionAddEditWindow;
            _userSession = UserSession.Instance;

            LoadSessionsCommand = new AsyncRelayCommand(LoadSessionsAsync);
            AddCommand = new AsyncRelayCommand(AddSession);
            EditCommand = new AsyncRelayCommand(EditSession, CanEditOrDeleteSession);
            DeleteCommand = new AsyncRelayCommand(DeleteSession, CanEditOrDeleteSession);
            BuyTicketCommand = new AsyncRelayCommand(BuyTicket);
            _ = LoadSessionsAsync();
        }

        public ShowtimesViewModel() : this(session =>
        {
            var window = new SessionAddEditWindow(session);
            return window.ShowDialog();
        })
        {
        }

        public ObservableCollection<sessionDto> Sessions { get; } = new();

        private sessionDto? _selectedSession;
        public sessionDto? SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    OnPropertyChanged();
                    EditCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string CurrentUserRole
        {
            get => _userSession.LoggedInUser?.Role;
        }

        public IRelayCommand LoadSessionsCommand { get; }
        public IRelayCommand AddCommand { get; }
        public IRelayCommand EditCommand { get; }
        public IRelayCommand DeleteCommand { get; }
        public IRelayCommand BuyTicketCommand { get; }

        private bool CanEditOrDeleteSession() => SelectedSession != null;

        private async Task LoadSessionsAsync()
        {
            try
            {
                var sessions = await _sessionService.GetSessionsAsync();
                var movies = await _movieService.GetMoviesAsync();
                var halls = await _hallService.GetHallsAsync();
                var cinemas = await _cinemaService.GetCinemasAsync();
                var movieDict = movies.ToDictionary(m => m.Id);
                var hallDict = halls.ToDictionary(h => h.Id);
                var cinemaDict = cinemas.ToDictionary(x => x.Id);
                foreach (var session in sessions)
                {
                    movieDict.TryGetValue(session.MovieId, out MovieDto movie);
                    hallDict.TryGetValue(session.HallId, out hallDto hall);
                    cinemaDict.TryGetValue(hall.CinemaId, out CinemaDto cinema);
                    session.CinemaName = cinema.Name ?? "UnknownCinema";
                    session.MovieTitle = movie?.Title ?? "Unknown Movie";
                    session.HallNumber = hall?.Number ?? "Unknown Hall";
                }

                var orderedSessions = sessions.OrderBy(s => s.Date).ToList();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Sessions.Clear();
                    foreach (var s in orderedSessions)
                        Sessions.Add(s);
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error loading data: Check API connection. {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while loading sessions: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddSession()
        {
            bool? result = _showSessionAddEditWindow(null);

            if (result == true)
            {
                await LoadSessionsAsync();
            }
        }

        private async Task EditSession()
        {
            if (SelectedSession == null) return;

            bool? result = _showSessionAddEditWindow(SelectedSession);

            if (result == true)
            {
                await LoadSessionsAsync();
            }
        }

        private async Task DeleteSession()
        {
            if (SelectedSession == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete the session for movie '{SelectedSession.MovieTitle}' in hall '{SelectedSession.HallNumber}' on {SelectedSession.Date:dd.MM.yyyy HH:mm}?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _sessionService.DeleteSessionAsync(SelectedSession.Id);
                    MessageBox.Show("Session deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadSessionsAsync();
                }
                catch (HttpRequestException ex)
                {

                    MessageBox.Show($"Error deleting session: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred while deleting the session: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async Task BuyTicket()
        {
            if (SelectedSession == null)
            {
                MessageBox.Show("Please select a session to buy a ticket for.", "No Session Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (SelectedSession.AvailibleSeats <= 0)
            {
                MessageBox.Show("No available seats for this session.", "Sold Out", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_userSession.LoggedInUser == null)
            {
                MessageBox.Show("You must be logged in to buy a ticket.", "Authentication Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var seatSelectionWindow = new SeatSelectionWindow(SelectedSession);
            bool? dialogResult = seatSelectionWindow.ShowDialog();

            if (dialogResult == true)
            {
                string selectedSeat = seatSelectionWindow.GetSelectedSeat();
                int numberOfTicketsToBuy = 1; 

                var newTicket = new ticketDto
                {
                    SessionId = Guid.Parse(SelectedSession.Id.ToString()), 
                    Seat = selectedSeat,
                    Price = (double)SelectedSession.TicketPrice,
                    Status = "Confirmed",
                    Created = DateTime.UtcNow,
                    NumberOfSeats = numberOfTicketsToBuy,
                    UserId = _userSession.LoggedInUser.Id
                };

                try
                {
                    Guid createdTicketId = await _ticketService.CreateTicketAsync(newTicket);
                    MessageBox.Show($"Ticket purchased successfully! Ticket ID: {createdTicketId}\nSeat: {selectedSeat}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadSessionsAsync(); 
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error purchasing ticket: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                
                MessageBox.Show("Seat selection cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}