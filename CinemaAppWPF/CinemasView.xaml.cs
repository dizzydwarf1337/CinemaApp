﻿using CinemaAppWPF.ViewModels;
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

namespace CinemaAppWPF.Views
{
    public partial class CinemasView : UserControl
    {
        public CinemasView()
        {
            InitializeComponent();
            var cinemaService = new CinemaService();
            var viewModel = new CinemaViewModel(cinemaService);
            DataContext = viewModel;

            Loaded += async (_, __) => await viewModel.LoadCinemasCommand.ExecuteAsync(null);
        }
    }
}
