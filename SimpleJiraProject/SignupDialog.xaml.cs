using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for SignupDialog.xaml
    /// </summary>
    public partial class SignupDialog : Window
    {
        User signupUser;
        public event Action<User> SignupCallback;

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
                new MessageBoxCustom("Please input all fields", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                return;
            } else if (Globals.Validator.LoginName_Check(tbLoginName.Text))
            {
                new MessageBoxCustom("Login Name exists already, please choose another one", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                return;
            } else if (!Globals.Validator.IsValidEmail(tbEmail.Text))
            {
                new MessageBoxCustom("Please input correct email address", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                return;
            } else if (!Globals.Validator.IsValidPassword(tbPassword.Password) || !Globals.Validator.IsValidPassword(tbConfirmPassword.Password)) 
            {
                new MessageBoxCustom("Password length Must be 8-12 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                return;
            } else if (!Globals.Validator.Password_Check(tbPassword.Password, tbConfirmPassword.Password))
            {
                new MessageBoxCustom("Please confirm the same password", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                return;
            } else
            {
                signupUser = new User
                {
                    LoginName = tbLoginName.Text,
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    TeamId = Globals.Validator.Team_Check(cmbTeamList.Text),
                    EMAIL = tbEmail.Text,
                    PWDEncrypted = SecurePassword.Encrypt(tbConfirmPassword.Password),
                    Role = cmbRoleList.Text
                };
                Globals.simpleJiraDB.Users.Add(signupUser);
                Globals.simpleJiraDB.SaveChanges();
                new MessageBoxCustom("New User Registered", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
            if (signupUser != null)
            {
                this.DialogResult = true;
                SignupCallback?.Invoke(signupUser);
                this.Close();
            }
        }

        private bool Blank_Check()
        {
            return string.IsNullOrEmpty(tbLoginName.Text) || string.IsNullOrEmpty(tbFirstName.Text)
                    || string.IsNullOrEmpty(tbLastName.Text) || string.IsNullOrEmpty(cmbTeamList.Text)
                    || string.IsNullOrEmpty(cmbRoleList.Text) || string.IsNullOrEmpty(tbPassword.Password)
                    || string.IsNullOrEmpty(tbEmail.Text) || string.IsNullOrEmpty(tbConfirmPassword.Password);
        }
    }
}
