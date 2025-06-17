// CinemaAppWPF.ViewModels/LoginViewModel.cs
using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
using CinemaAppWPF.Session;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks; // For Task
using System; 

namespace CinemaAppWPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly LoginService _loginService;

        // Login Fields
        [ObservableProperty]
        private string email = string.Empty; // Initialize to avoid null reference warnings

        [ObservableProperty]
        private string password = string.Empty; // Initialize

        [ObservableProperty]
        private string loginMessage = string.Empty; // Initialize

        [ObservableProperty]
        private string registerName = string.Empty; // New
        [ObservableProperty]
        private string registerLastName = string.Empty; // New
        [ObservableProperty]
        private string registerEmail = string.Empty; // New
        [ObservableProperty]
        private string registerPassword = string.Empty; // New
        [ObservableProperty]
        private string registerConfirmPassword = string.Empty; // New for password confirmation
        [ObservableProperty]
        private string registrationMessage = string.Empty; // New for registration feedback

        [ObservableProperty]
        private bool isLoginViewActive = true; // True for login, false for registration

        // LoggedInUser and IsLoggedIn remain the same
        public UserDto? LoggedInUser
        {
            get => UserSession.Instance.LoggedInUser;
            set
            {
                if (UserSession.Instance.LoggedInUser != value)
                {
                    UserSession.Instance.LoggedInUser = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsLoggedIn));
                    ResetFormFields(); // Clear fields on login/logout
                }
            }
        }

        public bool IsLoggedIn => LoggedInUser != null;

        // Commands
        public IAsyncRelayCommand LoginCommand { get; }
        public IAsyncRelayCommand LogoutCommand { get; }
        public IAsyncRelayCommand RegisterCommand { get; } // New command for registration
        public IRelayCommand ToggleViewCommand { get; } // New command to switch between login/register

        public LoginViewModel()
        {
            _loginService = new LoginService(); // Instantiate the service

            LoginCommand = new AsyncRelayCommand(LoginAsync);
            LogoutCommand = new AsyncRelayCommand(Logout);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync); 
            ToggleViewCommand = new AsyncRelayCommand(ToggleView); 
            // Ensure initial state reflects session
            OnPropertyChanged(nameof(LoggedInUser));
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private async Task LoginAsync()
        {
            // Reset messages
            LoginMessage = string.Empty;
            RegistrationMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                LoginMessage = "Email and password are required.";
                return;
            }

            var loginRequest = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var response = await _loginService.LoginAsync(loginRequest);
            if (response != null && response.User != null)
            {
                LoggedInUser = response.User;
                LoginMessage = "Login successful!";
                // Automatically switch to logged-in view (handled by IsLoggedIn binding)
            }
            else
            {
                LoginMessage = response?.Message ?? "Invalid login credentials.";
            }
        }

        private async Task RegisterAsync()
        {
            // Reset messages
            LoginMessage = string.Empty;
            RegistrationMessage = string.Empty;

            // Basic client-side validation
            if (string.IsNullOrWhiteSpace(RegisterName) || string.IsNullOrWhiteSpace(RegisterLastName) ||
                string.IsNullOrWhiteSpace(RegisterEmail) || string.IsNullOrWhiteSpace(RegisterPassword) ||
                string.IsNullOrWhiteSpace(RegisterConfirmPassword))
            {
                RegistrationMessage = "All fields are required.";
                return;
            }

            if (RegisterPassword != RegisterConfirmPassword)
            {
                RegistrationMessage = "Passwords do not match.";
                return;
            }

            var userRegistrationDto = new UserDto
            {
                Name = RegisterName,
                LastName = RegisterLastName,
                Email = RegisterEmail,
                Password = RegisterPassword
            };

            var response = await _loginService.RegisterAsync(userRegistrationDto); // Using new RegisterAsync
            if (response != null && response.Success)
            {
                RegistrationMessage = "Registration successful! You can now log in.";
                IsLoginViewActive = true;
                ResetFormFields();
            }
            else
            {
                RegistrationMessage = response?.Message ?? "Registration failed. Please try again.";
            }
        }

        private Task Logout()
        {
            LoggedInUser = null; // Clears the session
            ResetFormFields(); // Clear all fields
            LoginMessage = "You have been logged out.";
            IsLoginViewActive = true; // Ensure we go back to the login form
            return Task.CompletedTask;
        }

        private Task ToggleView()
        {
            IsLoginViewActive = !IsLoginViewActive; // Toggle the active view
            ResetFormFields(); // Clear fields when switching views
            LoginMessage = string.Empty;
            RegistrationMessage = string.Empty;
            return Task.CompletedTask;
        }

        private void ResetFormFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            RegisterName = string.Empty;
            RegisterLastName = string.Empty;
            RegisterEmail = string.Empty;
            RegisterPassword = string.Empty;
            RegisterConfirmPassword = string.Empty;
        }
    }
}