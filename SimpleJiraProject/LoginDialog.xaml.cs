using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        User loginUser;
        public event Action<User> LoginCallback;

        public LoginDialog()
        {
            InitializeComponent();
            try
            {
                Globals.simpleJiraDB = new SimpleJiraDBEntities();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Fatal error connecting to database:\n" + ex.Message, "Error Information");
                Environment.Exit(1);
            }
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginUser = Globals.simpleJiraDB.Users.Where(u => u.LoginName == tbLoginName.Text).FirstOrDefault();

                //TODO: ADD VALIDATION CLASS FOR USER NAME & PASSWORD INPUT
                if (loginUser != null)
                {
                    this.DialogResult = true;
                    LoginCallback?.Invoke(loginUser);
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Login into system:\n" + ex.Message, "Error Information");
            }
        }
    }
}
