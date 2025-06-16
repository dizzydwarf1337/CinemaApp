using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace CinemaAppWPF
{
    public partial class SessionAddEditWindow : Window
    {
        private readonly SessionService _sessionService;
        private readonly MovieService _movieService;
        private readonly HallService _hallService;
        private sessionDto _currentSession;
        public SessionAddEditWindow(sessionDto session = null)
        {
            InitializeComponent();

            _sessionService = new SessionService();
            _movieService = new MovieService();
            _hallService = new HallService();

            _currentSession = session;

            Loaded += SessionAddEditWindow_Loaded;
        }
        private async void SessionAddEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComboBoxes();

            if (_currentSession != null)
            {
                Title = "Edit Session";
                PopulateFields(_currentSession);
            }
            else
            {
                Title = "Add New Session";
                datePicker.SelectedDate = DateTime.Today;
                timeTextBox.Text = "00:00";
            }
        }

        private async Task LoadComboBoxes()
        {
            try
            {
                var movies = await _movieService.GetMoviesAsync();
                movieComboBox.ItemsSource = movies;

                var halls = await _hallService.GetHallsAsync();
                hallComboBox.ItemsSource = halls;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to load movies or halls. Please check your API connection. {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurred while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopulateFields(sessionDto session)
        {
            if (movieComboBox.ItemsSource is List<MovieDto> movies)
                movieComboBox.SelectedItem = movies.FirstOrDefault(m => m.Id == session.MovieId);

            if (hallComboBox.ItemsSource is List<hallDto> halls)
                hallComboBox.SelectedItem = halls.FirstOrDefault(h => h.Id == session.HallId);

            datePicker.SelectedDate = session.Date.Date;
            timeTextBox.Text = session.Date.ToString("HH:mm");
            priceTextBox.Text = session.TicketPrice.ToString();
            seatsTextBox.Text = session.AvailibleSeats.ToString();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            var selectedMovie = movieComboBox.SelectedItem as MovieDto;
            var selectedHall = hallComboBox.SelectedItem as hallDto;

            if (selectedMovie == null || selectedHall == null)
            {
                MessageBox.Show("Please select both movie and hall.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TimeSpan.TryParse(timeTextBox.Text, out TimeSpan selectedTime))
            {
                MessageBox.Show("Invalid time format. Use HH:mm format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime sessionDateTime = datePicker.SelectedDate.Value.Add(selectedTime);

            if (!decimal.TryParse(priceTextBox.Text, out decimal ticketPrice))
            {
                MessageBox.Show("Invalid ticket price. Please enter a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(seatsTextBox.Text, out int availableSeats))
            {
                MessageBox.Show("Invalid seat count. Please enter a whole number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_currentSession == null)
                {
                    var newSession = new sessionDto
                    {
                        Id = Guid.NewGuid(),
                        MovieId = selectedMovie.Id,
                        HallId = selectedHall.Id,
                        Date = sessionDateTime,
                        TicketPrice = ticketPrice,
                        AvailibleSeats = availableSeats
                    };
                    await _sessionService.CreateSessionAsync(newSession);
                    MessageBox.Show("Session successfully added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _currentSession.MovieId = selectedMovie.Id;
                    _currentSession.HallId = selectedHall.Id;
                    _currentSession.Date = sessionDateTime;
                    _currentSession.TicketPrice = ticketPrice;
                    _currentSession.AvailibleSeats = availableSeats;

                    await _sessionService.UpdateSessionAsync(_currentSession);
                    MessageBox.Show("Session successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                this.Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"API error: Failed to communicate with server. {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (movieComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a movie.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (hallComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a hall.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!datePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(timeTextBox.Text))
            {
                MessageBox.Show("Please enter time.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!TimeSpan.TryParse(timeTextBox.Text, out _))
            {
                MessageBox.Show("Invalid time format. Use HH:mm format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(priceTextBox.Text))
            {
                MessageBox.Show("Please enter ticket price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!decimal.TryParse(priceTextBox.Text, out _))
            {
                MessageBox.Show("Invalid ticket price. Please enter a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(seatsTextBox.Text))
            {
                MessageBox.Show("Please enter available seats count.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!int.TryParse(seatsTextBox.Text, out _))
            {
                MessageBox.Show("Invalid seat count. Please enter a whole number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}

