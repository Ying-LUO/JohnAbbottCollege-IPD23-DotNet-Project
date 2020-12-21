﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Team9SimpleJira.UserControls;

namespace Team9SimpleJira
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.contentControl.Content = new Projects();
        }

        private void btSprints_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new Sprints();
        }

        private void btProjects_Click(object sender, RoutedEventArgs e)
        {
            this.contentControl.Content = new Projects();
        }
    }
}
