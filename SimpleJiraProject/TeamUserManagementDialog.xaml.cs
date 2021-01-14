using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
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
        User currentUserInDialog;
        public event Action<User> TeamUserUpdateCallback;

        public TeamUserManagementDialog(User currentUser)
        {
            InitializeComponent();
            currentUserInDialog = currentUser;
            ResetAndLoadDataFromDB();

            if (currentUser.LoginName.Equals("Admin"))
            {
                tiTeam.Visibility = Visibility.Visible;
                tiUser.Visibility = Visibility.Visible;
            }
            else
            {
                tiTeam.Visibility = Visibility.Hidden;
                tiUser.Visibility = Visibility.Hidden;
            }
        }

        private void ResetAndLoadDataFromDB()
        {
            try
            {
                tbLoginName.Text = currentUserInDialog.LoginName;
                tbFirstName.Text = currentUserInDialog.FirstName;
                tbLastName.Text = currentUserInDialog.LastName;
                tbTeam.Text = currentUserInDialog.Team.Name;
                tbEmail.Text = currentUserInDialog.EMAIL;
                tbRole.Text = currentUserInDialog.Role;

                cmbNewTeamList.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).Distinct().ToList<string>();
                cmbNewTeamList.Items.Refresh();

                cmbTeamList.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).Distinct().ToList<string>();
                cmbTeamList.Items.Refresh();

                cmbUpdateTeamList.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).Distinct().ToList<string>();
                cmbUpdateTeamList.Items.Refresh();

                tbTeamUpdate.Text = string.Empty;

                cmbNewTeamList.SelectedIndex = -1;
                cmbUserList.SelectedIndex = -1;
                cmbUpdateTeamList.SelectedIndex = -1;
                cmbTeamList.SelectedIndex = -1;

                tblStatus.Text = $"Current User: {currentUserInDialog.LoginName} : {currentUserInDialog.FirstName} {currentUserInDialog.LastName}, {currentUserInDialog.Role} From {currentUserInDialog.Team.Name}";
                
            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Fatal error connecting to database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                Environment.Exit(1);
            }
        }

        private void cmbNewTeamList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cmbNewTeamList.SelectedItem != null)
            {
                tbTeamUpdate.Visibility = Visibility.Visible;
                btDeleteTeam.Visibility = Visibility.Visible;
                btAddTeam.Content = "Update Team Name";

            }
            else
            {
                tbTeamUpdate.Visibility = Visibility.Hidden;
                btDeleteTeam.Visibility = Visibility.Hidden;
                btAddTeam.Content = "Add New Team";
            }
        }

        
        private void cmbUserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbUpdateTeamList.Text))
            {
                btDeleteUser.Visibility = Visibility.Visible;
            }
            else
            {
                btDeleteUser.Visibility = Visibility.Hidden;
            }
        }

        private void cmbTeamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbTeamList.SelectedItem != null)
                {
                    List<string> userList = Globals.simpleJiraDB.Users.Include("Team").Where(ut => ut.Team.Name.Equals(cmbTeamList.SelectedItem.ToString())).Where(u=>!u.LoginName.Equals("Admin")).AsEnumerable().Select(u => u.LoginName).ToList<string>();

                    if (userList != null)
                    {
                        cmbUserList.ItemsSource = userList;
                        cmbUserList.Items.Refresh();
                        cmbUserList.Text = string.Empty;
                        cmbUserList.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbUserList.Text = string.Empty;
                    }
                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Updating User into database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void btUpdateMyAccount_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                if (string.IsNullOrEmpty(tbLoginName.Text) || string.IsNullOrEmpty(tbFirstName.Text) || string.IsNullOrEmpty(tbLastName.Text)
                    || string.IsNullOrEmpty(tbRole.Text) || string.IsNullOrEmpty(tbEmail.Text) || string.IsNullOrEmpty(tbPassword.Password))
                {
                    new MessageBoxCustom("Please input value", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                else if (!UserValidation.IsValidEmail(tbEmail.Text))
                {
                    new MessageBoxCustom("Please input correct email address", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                else if (!UserValidation.IsValidPassword(tbPassword.Password))
                {
                    new MessageBoxCustom("Password length Must be 8-12 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }

                User myAccount = Globals.simpleJiraDB.Users.Include("Team").Where(u => u.UserId == currentUserInDialog.UserId).FirstOrDefault<User>();

                if (myAccount != null)
                {
                    myAccount.LoginName = tbLoginName.Text;
                    myAccount.FirstName = tbFirstName.Text;
                    myAccount.LastName = tbLastName.Text;
                    myAccount.EMAIL = tbEmail.Text;
                    myAccount.Role = tbRole.Text;
                    myAccount.PWDEncrypted = SecurePassword.Encrypt(tbPassword.Password);
                    Globals.simpleJiraDB.SaveChanges();
                    new MessageBoxCustom("My Account Updated", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    ResetAndLoadDataFromDB();
                    if (currentUserInDialog != null)
                    {
                        this.DialogResult = true;
                        TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                    }
                }
                else
                {
                    new MessageBoxCustom("Cannot find this user to update", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                }
            }
            catch (DbUpdateException ex)
            {
                new MessageBoxCustom("User Login Name must be unique", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                Debug.WriteLine(ex.ToString());
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Updating User into database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void btAddTeam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbNewTeamList.Text))
                {
                    new MessageBoxCustom("Please input value", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                if (cmbNewTeamList.SelectedItem != null)
                {
                    Team teamUpdate = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbNewTeamList.SelectedItem.ToString())).FirstOrDefault<Team>();
                    if (teamUpdate != null)
                    {
                        if (string.IsNullOrEmpty(tbTeamUpdate.Text))
                        {
                            new MessageBoxCustom("Please enter new team name", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                            return;
                        }
                        teamUpdate.Name = tbTeamUpdate.Text;
                        Globals.simpleJiraDB.SaveChanges();
                        new MessageBoxCustom("Team Updated", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        ResetAndLoadDataFromDB();
                        if (currentUserInDialog != null)
                        {
                            this.DialogResult = true;
                            TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cannot find team to update", MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    }
                }
                else
                {
                    Team newTeam = new Team { Name = cmbNewTeamList.Text };
                    Globals.simpleJiraDB.Teams.Add(newTeam);
                    Globals.simpleJiraDB.SaveChanges();
                    new MessageBoxCustom("New Team Added", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    ResetAndLoadDataFromDB();
                }
            }
            catch (DbUpdateException ex)
            {
                new MessageBoxCustom("Team Name must be unique", MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                Debug.WriteLine(ex.ToString());
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Updating User into database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void btDeleteTeam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Team teamDelete = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbNewTeamList.Text)).FirstOrDefault<Team>();
                if (teamDelete != null)
                {
                    int userCount = Globals.simpleJiraDB.Users.Include("Team").Where(u => u.TeamId == teamDelete.TeamId).ToList<User>().Count;

                    int projectCount = Globals.simpleJiraDB.Projects.Include("Team").Where(p => p.TeamId == teamDelete.TeamId).ToList<Project>().Count;
                    
                    if (userCount == 0 && projectCount == 0)
                    {
                        bool? Result = new MessageBoxCustom("Are you sure to delete this Team? ", MessageBoxCustom.MessageType.Confirmation, MessageBoxCustom.MessageButtons.YesNo).ShowDialog();

                        if (Result.Value)
                        {
                            Globals.simpleJiraDB.Teams.Remove(teamDelete);
                            Globals.simpleJiraDB.SaveChanges();
                            new MessageBoxCustom("Team deleted", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                            ResetAndLoadDataFromDB();
                            if (currentUserInDialog != null)
                            {
                                this.DialogResult = true;
                                TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                            }
                        } 
                    }
                    else
                    {
                        new MessageBoxCustom("Please remove the projects and users under this team before delete", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        return;
                    }
                }
                else
                {
                    new MessageBoxCustom("Cannot find team to Delete", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();

                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Deleting User from database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void btUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string.IsNullOrEmpty(cmbUserList.Text)) || (string.IsNullOrEmpty(cmbUpdateTeamList.Text)) || (string.IsNullOrEmpty(cmbTeamList.Text)))
                {
                    new MessageBoxCustom("Please choose user and team to update", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                if (cmbTeamList.SelectedItem.Equals(cmbUpdateTeamList.SelectedItem))
                {
                    new MessageBoxCustom("Please choose a new team to update", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                Team fromTeam = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbTeamList.SelectedItem.ToString())).FirstOrDefault<Team>();

                if (fromTeam != null)
                {
                    User userUpdate = Globals.simpleJiraDB.Users.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).Where(ut => ut.TeamId == fromTeam.TeamId).FirstOrDefault<User>();
                    Team toTeam = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbUpdateTeamList.SelectedItem.ToString())).FirstOrDefault<Team>();

                    if ((userUpdate != null) && (toTeam != null))
                    {
                        userUpdate.TeamId = toTeam.TeamId;
                        Globals.simpleJiraDB.SaveChanges();
                        new MessageBoxCustom("User Updated", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        ResetAndLoadDataFromDB();
                        if (currentUserInDialog != null)
                        {
                            this.DialogResult = true;
                            TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                        }
                    }
                    else
                    {
                        new MessageBoxCustom("Cannot find user to update", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    }
                }
                else
                {
                    new MessageBoxCustom("Cannot find user's team to update", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Updating User into database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void btDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string.IsNullOrEmpty(cmbUserList.Text)) || (string.IsNullOrEmpty(cmbTeamList.Text)))
                {
                    new MessageBoxCustom("Please choose user to delete", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                Team team = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbTeamList.SelectedItem.ToString())).FirstOrDefault<Team>();

                if (team != null)
                {
                    User userDelete = Globals.simpleJiraDB.Users.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).Where(ut => ut.TeamId == team.TeamId).FirstOrDefault<User>();
                    
                    if (userDelete != null)
                    {
                        bool? Result = new MessageBoxCustom("Are you sure to delete this user? ", MessageBoxCustom.MessageType.Confirmation, MessageBoxCustom.MessageButtons.YesNo).ShowDialog();

                        if (Result.Value)
                        {
                            Globals.simpleJiraDB.Users.Remove(userDelete);
                            Globals.simpleJiraDB.SaveChanges();
                            new MessageBoxCustom("User Deleted", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                            ResetAndLoadDataFromDB();
                            if (currentUserInDialog != null)
                            {
                                this.DialogResult = true;
                                TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                            }
                        }  
                    }
                    else
                    {
                        new MessageBoxCustom("Cannot find User to Delete", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    }
                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Deleting User from database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }
    }
}