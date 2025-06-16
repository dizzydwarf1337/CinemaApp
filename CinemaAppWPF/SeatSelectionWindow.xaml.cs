using CinemaAppWPF.DTO;
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

namespace CinemaAppWPF
{
    /// <summary>
    /// Логика взаимодействия для SeatSelectionWindow.xaml
    /// </summary>
    public partial class SeatSelectionWindow : Window
    {
        private readonly SeatSelectionViewModel _viewModel;

        public SeatSelectionWindow(sessionDto session)
        {
            InitializeComponent();
            _viewModel = new SeatSelectionViewModel(session);
            this.DataContext = _viewModel;
            _viewModel.RequestClose += () => this.DialogResult = _viewModel.IsSelectionConfirmed; // Set DialogResult and close
        }

        // Expose the selected seat back to the caller
        public string GetSelectedSeat()
        {
            return _viewModel.SelectedSeatString;
        }
    }
}
