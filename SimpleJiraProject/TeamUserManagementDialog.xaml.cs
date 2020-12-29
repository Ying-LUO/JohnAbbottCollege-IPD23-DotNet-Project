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

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for TeamUserManagementDialog.xaml
    /// </summary>
    public partial class TeamUserManagementDialog : Window
    {
        public TeamUserManagementDialog(User currentUser)
        {
            InitializeComponent();
        }

        private void cmbNewTeamList_Selected(object sender, RoutedEventArgs e)
        {
            btTeam.Content = "Update Team Name";
        }

        private void cmbNewUserList_Selected(object sender, RoutedEventArgs e)
        {
            btUser.Content = "Update User";
        }

        private void btUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btTeam_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btUpdateMyAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
