using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
using CinemaAppWPF.Session;
using CinemaAppWPF.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace CinemaAppWPF.ViewModels
{
    public class MoviesViewModel : INotifyPropertyChanged
    {
        private readonly MovieService _movieService;
        private readonly UserSession userSession;

        public MoviesViewModel(MovieService movieService)
        {
            _movieService = movieService;
            userSession = UserSession.Instance;
            _ = LoadMoviesAsync();

            AddMovieCommand = new AsyncRelayCommand(AddMovieAsync);
            EditMovieCommand = new AsyncRelayCommand(EditMovieAsync, CanEditOrDelete);
            DeleteMovieCommand = new AsyncRelayCommand(DeleteMovieAsync, CanEditOrDelete);
        }

        public ObservableCollection<MovieDto> Movies { get; set; } = new ObservableCollection<MovieDto>();

        private MovieDto? _selectedMovie;
        public MovieDto? SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
                EditMovieCommand.NotifyCanExecuteChanged();
                DeleteMovieCommand.NotifyCanExecuteChanged();
            }
        }

        public string UserRole => userSession.LoggedInUser?.Role ?? string.Empty;

        public IAsyncRelayCommand AddMovieCommand { get; }
        public IAsyncRelayCommand EditMovieCommand { get; }
        public IAsyncRelayCommand DeleteMovieCommand { get; }

        private async Task LoadMoviesAsync()
        {
            try
            {
                var movies = await _movieService.GetMoviesAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Movies.Clear();
                    foreach (var movie in movies)
                    {
                        Movies.Add(movie);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania filmów: {ex.Message}");
            }
        }

        private async Task AddMovieAsync()
        {
            var window = new MovieAddEditWindow(null);
            if (window.ShowDialog() == true)
            {
                await LoadMoviesAsync();
            }
        }

        private async Task EditMovieAsync()
        {
            if (SelectedMovie == null)
                return;

            var window = new MovieAddEditWindow(SelectedMovie);
            if (window.ShowDialog() == true)
            {
                await LoadMoviesAsync();
            }
        }

        private async Task DeleteMovieAsync()
        {
            if (SelectedMovie == null)
                return;

            if (MessageBox.Show($"Czy na pewno chcesz usunąć film \"{SelectedMovie.Title}\"?", "Potwierdzenie",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    await _movieService.DeleteMovieByIdAsync(SelectedMovie.Id);
                    await LoadMoviesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas usuwania filmu: {ex.Message}");
                }
            }
        }

        private bool CanEditOrDelete()
        {
            return SelectedMovie != null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
