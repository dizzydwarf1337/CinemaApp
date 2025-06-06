﻿using CinemaAppWPF.Services;
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
    public partial class MoviesView : UserControl
    {
        public MoviesView()
        {
            InitializeComponent();
            var movieService = new MovieService();
            DataContext = new ViewModels.MoviesViewModel(movieService);
        }
    }
}