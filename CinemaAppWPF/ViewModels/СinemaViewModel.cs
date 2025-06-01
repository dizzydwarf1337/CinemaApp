using CinemaAppWPF.DTO;
using CinemaAppWPF.Session;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;


namespace CinemaAppWPF.ViewModels 
{ 
    public partial class CinemaViewModel : ObservableObject
    {
        public IRelayCommand AddCommand { get; }
        public IRelayCommand EditCommand { get; }
        public IRelayCommand DeleteCommand { get; }
        [ObservableProperty]
        private CinemaDto selectedCinema ;

        private readonly CinemaService _cinemaService;

        public string UserRole => UserSession.Instance.LoggedInUser?.Role ?? string.Empty;
        public CinemaViewModel(CinemaService cinemaService)
        {
            _cinemaService = cinemaService;
            LoadCinemasCommand = new AsyncRelayCommand(LoadCinemasAsync);

            AddCommand = new AsyncRelayCommand(OnAddAsync);
            EditCommand = new AsyncRelayCommand(OnEditAsync, CanEditOrDelete);
            DeleteCommand = new AsyncRelayCommand(OnDeleteAsync, CanEditOrDelete);
        }
        public ObservableCollection<CinemaDto> Cinemas { get; } = new();

        public IAsyncRelayCommand LoadCinemasCommand { get; }

        private async Task LoadCinemasAsync()
        {
            try
            {
                var cinemas = await _cinemaService.GetCinemasAsync();
                Cinemas.Clear();
                foreach (var cinema in cinemas)
                    Cinemas.Add(cinema);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private bool CanEditOrDelete() => selectedCinema != null;

        private async Task OnAddAsync()
        {
            var newCinema = new CinemaDto(); 
            var editWindow = new CinemaEditWindow(newCinema)
            {
                Owner = Application.Current.MainWindow
            };

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    var createRequest = new CreateCinemaRequest
                    {
                        CinemaDto = new CinemaDto
                        {
                            Name = newCinema.Name,
                            Address = newCinema.Address,
                            ImagePath="null"
                        },
                        NumOfHalls = 3

                    };

                    await _cinemaService.CreateCinemaAsync(createRequest);
                    await LoadCinemasAsync();
                    MessageBox.Show("Kino dodane");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas dodawania: {ex.Message}");
                }
            }
        }

        private async Task OnEditAsync()
        {
            if (SelectedCinema == null) return;

            var cinemaCopy = new CinemaDto
            {
                Id = SelectedCinema.Id,
                Name = SelectedCinema.Name,
                Address = SelectedCinema.Address
            };

            var editWindow = new CinemaEditWindow(cinemaCopy)
            {
                Owner = Application.Current.MainWindow
            };

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    await _cinemaService.UpdateCinemaAsync(cinemaCopy);
                    await LoadCinemasAsync();
                    MessageBox.Show("Kino zaktualizowane");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas edycji: {ex.Message}");
                }
            }
        }

        private async Task OnDeleteAsync()
        {
            if (SelectedCinema == null) return;

            var result = MessageBox.Show($"Usunąć kino {SelectedCinema.Name}?", "Potwierdzenie", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _cinemaService.DeleteCinemaAsync(SelectedCinema.Id);
                    Cinemas.Remove(SelectedCinema);
                    MessageBox.Show("Kino usunięte");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd usuwania: {ex.Message}");
                }
            }
        }
        partial void OnSelectedCinemaChanged(CinemaDto value)
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }
}
