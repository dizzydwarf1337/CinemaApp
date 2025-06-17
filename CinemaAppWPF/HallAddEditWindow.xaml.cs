using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
using System;
using System.Windows;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace CinemaAppWPF.Views
{
    public partial class HallAddEditWindow : Window
    {
        private readonly HallService _hallService = new HallService();
        private readonly hallDto? _hallDto;

        public HallAddEditWindow(hallDto? hallDto)
        {
            InitializeComponent();
            _hallDto = hallDto;

            if (_hallDto != null)
            {
                Title = "Edit Hall";
                NumberTextBox.Text = _hallDto.Number;
                SeatsTextBox.Text = _hallDto.Seats.ToString();
                CinemaIdTextBox.Text = _hallDto.CinemaId.ToString();
            }
            else
            {
                Title = "Add Hall";
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NumberTextBox.Text))
                {
                    MessageBox.Show("Hall number is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(SeatsTextBox.Text, out int seats) || seats <= 0)
                {
                    MessageBox.Show("Number of seats must be a valid positive integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Guid.TryParse(CinemaIdTextBox.Text, out Guid cinemaId) || cinemaId == Guid.Empty)
                {
                    MessageBox.Show("Cinema ID must be a valid GUID format and cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var hallToSave = _hallDto ?? new hallDto { Id = Guid.NewGuid() };
                hallToSave.Number = NumberTextBox.Text;
                hallToSave.Seats = seats;
                hallToSave.CinemaId = cinemaId;

                if (_hallDto == null)
                {
                    await _hallService.CreateHallAsync(hallToSave);
                    MessageBox.Show("Hall added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _hallService.UpdateHallAsync(hallToSave);
                    MessageBox.Show("Hall updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"API error saving hall: {ex.Message}. Check server connection.", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}