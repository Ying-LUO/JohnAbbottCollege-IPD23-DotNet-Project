using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                User currentUser = new User();
                LoginDialog login = new LoginDialog();
                login.LoginCallback += (u) => { currentUser = u; };
                bool? result = login.ShowDialog();  // this line must be stay after the assignment, otherwise value is not assigned

                if (result == true)
                {
                    MessageBox.Show("Login Successfully");
                    new MainWindow(currentUser).ShowDialog();
                }
                else
                {
                    MessageBox.Show("Login Failed");
                }
            }
            finally
            {
                Shutdown();
            }
        }
    }
}
