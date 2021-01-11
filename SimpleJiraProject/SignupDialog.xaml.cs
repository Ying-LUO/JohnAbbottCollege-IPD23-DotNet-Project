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
    /// Interaction logic for SignupDialog.xaml
    /// </summary>
    public partial class SignupDialog : Window
    {
        public SignupDialog()
        {
            InitializeComponent();
            try
            {
                Globals.simpleJiraDB = new SimpleJiraDBEntities();
                cmbTeamList.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
                cmbRoleList.ItemsSource = Globals.simpleJiraDB.Users.AsEnumerable().Select(u => u.Role).Distinct().ToList<string>();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Fatal error connecting to database:\n" + ex.Message, "Error Information");
                Environment.Exit(1);
            }
        }

        private void btSignup_Click(object sender, RoutedEventArgs e)
        {
            if (Blank_Check())
            {

            }

        }

        private bool Blank_Check()
        {
            if (string.IsNullOrEmpty(tbLoginName.Text) || string.IsNullOrEmpty(tbFirstName.Text)
                || string.IsNullOrEmpty(tbLastName.Text) || string.IsNullOrEmpty(cmbTeamList.Text)
                || string.IsNullOrEmpty(cmbRoleList.Text) || string.IsNullOrEmpty(tbPassword.Password)
                || string.IsNullOrEmpty(tbConfirmPassword.Password))
            {
                MessageBox.Show("Please input value");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void LoginName_Check()
        {
            List<string> NameList = Globals.simpleJiraDB.Users.AsEnumerable().Select(u => u.LoginName).ToList<string>();
            if (NameList.Contains(tbLoginName.Text))
            {
                MessageBox.Show("Login Name exists, please choose another one");
                return;
            }
        }

        private void Password_Check()
        {

        }
    }
}
