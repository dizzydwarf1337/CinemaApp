using CinemaAppWPF.DTO;
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
    public partial class CinemaEditWindow : Window
    {
        public CinemaDto Cinema { get; private set; }

        public CinemaEditWindow(CinemaDto cinema)
        {
            InitializeComponent();
            Cinema = cinema;
            DataContext = Cinema;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
