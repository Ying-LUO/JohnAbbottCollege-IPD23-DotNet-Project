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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for IssueStatusControl.xaml
    /// </summary>
    public partial class IssueStatusControl : UserControl
    {
        string status;
        public IssueStatusControl()
        {
            InitializeComponent();
        }
            private void miTodo_Click(object sender, RoutedEventArgs e)
            {
                status = "Todo";
                UpdateIssueStatus(status);
                btStatus.Background = Brushes.LightGray;
            }

            private void miInProcess_Click(object sender, RoutedEventArgs e)
            {
                status = "InProcess";
                UpdateIssueStatus(status);
                btStatus.Background = Brushes.LightGray;
            }

            private void miBlocked_Click(object sender, RoutedEventArgs e)
            {
                status = "Blocked";
                UpdateIssueStatus(status);
                btStatus.Background = Brushes.LightGray;
            }

            private void miVerified_Click(object sender, RoutedEventArgs e)
            {
                status = "Verified";
                UpdateIssueStatus(status);
                btStatus.Background = Brushes.LightGray;
            }

            private void miResolved_Click(object sender, RoutedEventArgs e)
            {
                status = "Resolved";
                UpdateIssueStatus(status);
                btStatus.Background = Brushes.LightGray;
            }

            public void UpdateIssueStatus(string status)
            {

                Globals.SelectedIssue.Status = status;
                Globals.simpleJiraDB.SaveChanges();
                Globals.AppWindow.LoadDataFromDb(Globals.currentUser);
                if (status == "DONE")
                {
                    btStatus.Background = Brushes.LightGray;
                }
                if (status == "TEST")
                {
                    btStatus.Background = Brushes.LightBlue;
                }
                if (status == "InValidation")
                {
                    btStatus.Background = Brushes.LightGreen;
                }
            }

        
    }
    }
