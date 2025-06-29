﻿using CinemaAppWPF.ViewModels;
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
    public partial class ShowtimesView : UserControl
    {
            public ShowtimesView()
            {
                InitializeComponent();

                DataContext = new ShowtimesViewModel(session =>
                {
                    var window = new SessionAddEditWindow(session);
                    return window.ShowDialog();
                });
            }
    }
}
