using CinemaAppWPF.DTO;
using CinemaAppWPF.Services;
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

namespace CinemaAppWPF
{
    public partial class MovieAddEditWindow : Window
    {
        private readonly MovieService _movieService = new MovieService();
        private readonly MovieDto? _movieDto;

        public MovieAddEditWindow(MovieDto? movieDto)
        {
            InitializeComponent();
            _movieDto = movieDto;

            if (_movieDto != null)
            {
                Title = "Edytuj film";
                TitleTextBox.Text = _movieDto.Title;
                DescriptionTextBox.Text = _movieDto.Description;
                GenreTextBox.Text = _movieDto.Genre;
                DirectorTextBox.Text = _movieDto.Director;
                DurationTextBox.Text = _movieDto.Duration;
            }
            else
            {
                Title = "Dodaj film";
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
                {
                    MessageBox.Show("Tytuł jest wymagany.");
                    return;
                }

                var movieDto = _movieDto ?? new MovieDto { Id = Guid.NewGuid() };
                movieDto.Title = TitleTextBox.Text;
                movieDto.Description = DescriptionTextBox.Text;
                movieDto.Genre = GenreTextBox.Text;
                movieDto.Director = DirectorTextBox.Text;
                movieDto.Duration = DurationTextBox.Text;

                if (_movieDto == null)
                {
                    await _movieService.CreateMovieAsync(movieDto);
                }
                else
                {
                    await _movieService.UpdateMovieAsync(movieDto);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania filmu: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
