using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
using CinemaAppWPF.Session; // Assuming UserSession is here
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaAppWPF.ViewModels
{
    public class TicketsViewModel : INotifyPropertyChanged
    {
        private readonly TicketService _ticketService;
        private readonly SessionService _sessionService; 
        private readonly MovieService _movieService;     
        private readonly HallService _hallService;       
        private readonly CinemaService _cinemaService;   
        private readonly UserSession _userSession;

        public TicketsViewModel(
            TicketService ticketService,
            SessionService sessionService,
            MovieService movieService,
            HallService hallService,
            CinemaService cinemaService)
        {
            _ticketService = ticketService;
            _sessionService = sessionService;
            _movieService = movieService;
            _hallService = hallService;
            _cinemaService = cinemaService;
            _userSession = UserSession.Instance;

            Tickets = new ObservableCollection<ticketDto>();

            LoadTicketsCommand = new AsyncRelayCommand(LoadTicketsAsync);
            DeleteTicketCommand = new AsyncRelayCommand(DeleteTicket, CanEditOrDeleteTicket);

            _ = LoadTicketsAsync(); 
        }

        public ObservableCollection<ticketDto> Tickets { get; }

        private ticketDto? _selectedTicket;
        public ticketDto? SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                if (_selectedTicket != value)
                {
                    _selectedTicket = value;
                    OnPropertyChanged();
                    DeleteTicketCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string CurrentUserRole
        {
            get => _userSession.LoggedInUser?.Role ?? string.Empty;
        }

        public IRelayCommand LoadTicketsCommand { get; }
        public IRelayCommand DeleteTicketCommand { get; }

        private bool CanEditOrDeleteTicket() => SelectedTicket != null;

        private async Task LoadTicketsAsync()
        {
            try
            {
                ICollection<ticketDto> fetchedTickets;
                if (CurrentUserRole == "Admin")
                {
                    fetchedTickets = await _ticketService.GetAllTicketsAsync();
                }
                else if (_userSession.LoggedInUser?.Id != null)
                {
                    fetchedTickets = await _ticketService.GetTicketsByUserIdAsync(_userSession.LoggedInUser.Id);
                }
                else
                {
                    fetchedTickets = new List<ticketDto>();
                    MessageBox.Show("Please log in to view your tickets.", "Login Required", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                var sessions = await _sessionService.GetSessionsAsync(); 
                var movies = await _movieService.GetMoviesAsync();
                var halls = await _hallService.GetHallsAsync();
                var cinemas = await _cinemaService.GetCinemasAsync();

                var movieDict = movies.ToDictionary(m => m.Id);
                var hallDict = halls.ToDictionary(h => h.Id);
                var cinemaDict = cinemas.ToDictionary(c => c.Id);
                var sessionDict = sessions.ToDictionary(s => s.Id);


                Application.Current.Dispatcher.Invoke(() =>
                {
                    Tickets.Clear();
                    foreach (var ticket in fetchedTickets)
                    {
                        if (sessionDict.TryGetValue(ticket.SessionId, out var session))
                        {
                            ticket.SessionTime = session.Date; 
                            if (movieDict.TryGetValue(session.MovieId, out MovieDto movie))
                            {
                                ticket.MovieTitle = movie.Title;
                            }
                            if (hallDict.TryGetValue(session.HallId, out hallDto hall))
                            {
                                ticket.HallNumber = hall.Number;
                            }
                            if (cinemaDict.TryGetValue(hall.CinemaId, out CinemaDto cinema))
                            {
                                ticket.CinemaName = cinema.Name;
                            }
                        }
                        Tickets.Add(ticket);
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"API Error loading tickets: {ex.Message}. Check server connection.", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while loading tickets: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        


        private async Task DeleteTicket()
        {
            if (SelectedTicket == null) return;

            bool canDelete = CurrentUserRole == "Admin" ||
                             (_userSession.LoggedInUser?.Id == SelectedTicket.UserId && SelectedTicket.Status != "Used"); 
            if (!canDelete)
            {
                MessageBox.Show("You do not have permission to delete this ticket.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete the ticket for session '{SelectedTicket.MovieTitle}'?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _ticketService.DeleteTicketAsync(SelectedTicket.Id);
                    MessageBox.Show("Ticket deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadTicketsAsync();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error deleting ticket: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred while deleting the ticket: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}