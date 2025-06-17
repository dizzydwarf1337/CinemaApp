using CinemaAppWPF.Services;
using System.Windows.Controls;

namespace CinemaAppWPF.Views
{
    public partial class HallsView : UserControl
    {
        public HallsView()
        {
            InitializeComponent();
            // Instantiate services and pass them to the ViewModel
            var hallService = new HallService();
            var cinemaService = new CinemaService(); // CinemaService is still needed for mapping CinemaId to CinemaName for display
            DataContext = new ViewModels.HallsViewModel(hallService, cinemaService);
        }
    }
}