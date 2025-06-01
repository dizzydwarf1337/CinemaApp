using CinemaAppWPF.DTO;
using CinemaAppWPF.Session;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CinemaAppWPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly LoginService _loginService = new LoginService();

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string loginMessage;

        public UserDto LoggedInUser
        {
            get => UserSession.Instance.LoggedInUser;
            set
            {
                if (UserSession.Instance.LoggedInUser != value)
                {
                    UserSession.Instance.LoggedInUser = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsLoggedIn));
                }
            }
        }

        public bool IsLoggedIn => LoggedInUser != null;

        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand LogoutCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            LogoutCommand = new AsyncRelayCommand(Logout);
            OnPropertyChanged(nameof(LoggedInUser));
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private async Task LoginAsync()
        {
            var loginRequest = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var response = await _loginService.LoginAsync(loginRequest);
            if (response != null && response.User != null)
            {
                LoggedInUser = response.User;
                LoginMessage = "";
            }
            else
            {
                LoginMessage = response?.Message ?? "Błędne dane logowania";
            }
        }

        private Task Logout()
        {
            LoggedInUser = null;
            Email = "";
            Password = "";
            LoginMessage = "";

            return Task.CompletedTask;
        }
    }
}
