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
                MessageBox.Show("Please input all fields", "SignUp Information");
                return;
            }else if (LoginName_Check(tbLoginName.Text))
            {
                MessageBox.Show("Login Name exists already, please choose another one", "SignUp Information");
                return;
            }else if (!IsValidEmail(tbEmail.Text))
            {
                MessageBox.Show("Please input correct email address", "SignUp Information");
                return;
            }else if (!Password_Check(tbPassword.Password, tbConfirmPassword.Password))
            {
                MessageBox.Show("Please confirm the same password", "SignUp Information");
                return;
            }else
            {

                User newUser = new User
                {
                    LoginName = tbLoginName.Text,
                    FirstName = tbFirstName.Text,
                    LastName = tbLastName.Text,
                    TeamId = Team_Check(cmbTeamList.Text),
                    EMAIL = tbEmail.Text,
                    PasswordHash = SecurePassword.Encrypt(Encoding.UTF8.GetBytes(tbConfirmPassword.Password)),
                    Role = cmbRoleList.Text
                };
                Globals.simpleJiraDB.SaveChanges();
                MessageBox.Show("New User Registered", "SignUp Information");
            }
        }

        private bool Blank_Check()
        {
            return string.IsNullOrEmpty(tbLoginName.Text) || string.IsNullOrEmpty(tbFirstName.Text)
                    || string.IsNullOrEmpty(tbLastName.Text) || string.IsNullOrEmpty(cmbTeamList.Text)
                    || string.IsNullOrEmpty(cmbRoleList.Text) || string.IsNullOrEmpty(tbPassword.Password)
                    || string.IsNullOrEmpty(tbEmail.Text) || string.IsNullOrEmpty(tbConfirmPassword.Password);
        }

        private bool LoginName_Check(string loginName)
        {
            List<string> NameList = Globals.simpleJiraDB.Users.AsEnumerable().Select(u => u.LoginName).ToList<string>();
            return NameList.Contains(loginName);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool Password_Check(string pwd, string confirmPwd)
        {
            return pwd == confirmPwd;
        }

        private int Team_Check(string team)
        {
            Team chooseTeam = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(team)).FirstOrDefault();
            int teamId = chooseTeam != null ? chooseTeam.TeamId : 0;
            if(teamId == 0)
            {
                Team newTeam = new Team { Name = team };
                Globals.simpleJiraDB.Teams.Add(newTeam);
                Globals.simpleJiraDB.SaveChanges();
                return newTeam.TeamId;
            }
            else
            {
                return chooseTeam.TeamId;
            }
            
        }

    }
}
