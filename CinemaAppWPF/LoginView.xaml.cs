using CinemaAppWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace CinemaAppWPF.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            // DataContext is set in XAML, so no need to set it here.
            // If you did set it here, ensure it's not duplicated:
            // DataContext = new LoginViewModel();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                // Set the password from the PasswordBox to the ViewModel property
                viewModel.Password = LoginPasswordBox.Password;
                await viewModel.LoginCommand.ExecuteAsync(null); // Execute the command
            }
        }

        // Event handler for the Register button (New)
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                // Set the passwords from the PasswordBoxes to the ViewModel properties
                viewModel.RegisterPassword = RegisterPasswordBox.Password;
                viewModel.RegisterConfirmPassword = RegisterConfirmPasswordBox.Password;
                await viewModel.RegisterCommand.ExecuteAsync(null); // Execute the registration command
            }
        }
    }
}