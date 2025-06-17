using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CinemaAppWPF.Views;

namespace CinemaAppWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowMoviesCommand { get; }
        public ICommand ShowShowtimesCommand { get; }
        public ICommand ShowTicketsCommand { get; }
        public ICommand ShowCinemasCommand { get; }
        public ICommand ShowHallsCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            // Domyślnie startujemy od logowania (lub innego widoku)
            CurrentView = new LoginView();

            ShowMoviesCommand = new RelayCommand(_ => CurrentView = new MoviesView());
            ShowShowtimesCommand = new RelayCommand(_ => CurrentView = new ShowtimesView());
            ShowTicketsCommand = new RelayCommand(_ => CurrentView = new TicketsView());
            ShowCinemasCommand = new RelayCommand(_ => CurrentView = new CinemasView());
            ShowHallsCommand = new RelayCommand(_ => CurrentView = new HallsView());
            LogoutCommand = new RelayCommand(_ => CurrentView = new LoginView()); // reset widoku na logout
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
