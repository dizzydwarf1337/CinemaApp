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

namespace CinemaAppWPF.Views
{
    public partial class TicketsView : UserControl
    {
        public TicketsView()
        {
            InitializeComponent();
            var ticketService = new TicketService();
            var sessionService = new SessionService(); 
            var movieService = new MovieService();     
            var hallService = new HallService();       
            var cinemaService = new CinemaService();   

            DataContext = new ViewModels.TicketsViewModel(
                ticketService,
                sessionService,
                movieService,
                hallService,
                cinemaService
            );
        }
    }
}
