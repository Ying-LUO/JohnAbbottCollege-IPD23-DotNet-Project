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
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for AddEditIssueDialog.xaml
    /// </summary>
    public partial class AddEditIssueDialog : Window
    {
        public AddEditIssueDialog()
        {
            InitializeComponent();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}