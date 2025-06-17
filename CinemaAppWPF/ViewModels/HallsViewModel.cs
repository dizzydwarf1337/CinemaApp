using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
using CinemaAppWPF.Session;
using CinemaAppWPF.Views;
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
    public class HallsViewModel : INotifyPropertyChanged
    {
        private readonly HallService _hallService;
        private readonly CinemaService _cinemaService;
        private readonly UserSession _userSession;

        public HallsViewModel(HallService hallService, CinemaService cinemaService)
        {
            _hallService = hallService;
            _cinemaService = cinemaService;
            _userSession = UserSession.Instance;

            Halls = new ObservableCollection<hallDto>();

            LoadHallsCommand = new AsyncRelayCommand(LoadHallsAsync);
            AddHallCommand = new AsyncRelayCommand(AddHall);
            EditHallCommand = new AsyncRelayCommand(EditHall, CanEditOrDeleteHall);
            DeleteHallCommand = new AsyncRelayCommand(DeleteHall, CanEditOrDeleteHall);

            _ = LoadHallsAsync();
        }

        public ObservableCollection<hallDto> Halls { get; }

        private hallDto? _selectedHall;
        public hallDto? SelectedHall
        {
            get => _selectedHall;
            set
            {
                if (_selectedHall != value)
                {
                    _selectedHall = value;
                    OnPropertyChanged();
                    EditHallCommand.NotifyCanExecuteChanged();
                    DeleteHallCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public string CurrentUserRole
        {
            get => _userSession.LoggedInUser?.Role ?? string.Empty;
        }

        public IRelayCommand LoadHallsCommand { get; }
        public IRelayCommand AddHallCommand { get; }
        public IRelayCommand EditHallCommand { get; }
        public IRelayCommand DeleteHallCommand { get; }

        private bool CanEditOrDeleteHall() => SelectedHall != null;

        private async Task LoadHallsAsync()
        {
            try
            {
                var halls = await _hallService.GetHallsAsync();
                var cinemas = await _cinemaService.GetCinemasAsync(); 

                var cinemaDict = cinemas.ToDictionary(c => c.Id, c => c.Name);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Halls.Clear();
                    foreach (var hall in halls)
                    {
                        if (cinemaDict.TryGetValue(hall.CinemaId, out string? cinemaName))
                        {
                            hall.CinemaName = cinemaName;
                        }
                        else
                        {
                            hall.CinemaName = "Unknown Cinema";
                        }
                        Halls.Add(hall);
                    }
                });
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error loading halls: Check API connection. {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddHall()
        {
            var addEditWindow = new HallAddEditWindow(null); 
            bool? result = addEditWindow.ShowDialog(); 
            if (result == true) 
            {
                await LoadHallsAsync(); 
            }
        }

        private async Task EditHall()
        {
            if (SelectedHall == null) return;

            var hallToEdit = new hallDto
            {
                Id = SelectedHall.Id,
                Number = SelectedHall.Number,
                Seats = SelectedHall.Seats,
                CinemaId = SelectedHall.CinemaId
            };

            var addEditWindow = new HallAddEditWindow(hallToEdit); 
            bool? result = addEditWindow.ShowDialog(); 
            if (result == true) 
            {
                await LoadHallsAsync(); 
            }
        }

        private async Task DeleteHall()
        {
            if (SelectedHall == null) return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete hall number '{SelectedHall.Number}'?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _hallService.DeleteHallAsync(SelectedHall.Id);
                    MessageBox.Show("Hall deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadHallsAsync();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error deleting hall: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred while deleting the hall: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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