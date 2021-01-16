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
    /// Interaction logic for UserStoryStatus.xaml
    /// </summary>
    public partial class UserStoryStatus : UserControl
    {
        public string status;
        public UserStoryStatus()
        {
            InitializeComponent();
        }

        private void miTodo_Click(object sender, RoutedEventArgs e)
        {
            status = "Todo";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miDEV_Click(object sender, RoutedEventArgs e)
        {
            status = "DEV";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miTEST_Click(object sender, RoutedEventArgs e)
        {
            status = "TEST";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miDONE_Click(object sender, RoutedEventArgs e)
        {
            status = "DONE";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miDocumenting_Click(object sender, RoutedEventArgs e)
        {
            status = "Documenting";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miReady_Click(object sender, RoutedEventArgs e)
        {
            status = "Ready";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miInValidation_Click(object sender, RoutedEventArgs e)
        {
            status = "InValidation";
            UpdateUserStoryStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        public void UpdateUserStoryStatus(string status)
        {

            Globals.SelectedUserStory.Status = status;
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
