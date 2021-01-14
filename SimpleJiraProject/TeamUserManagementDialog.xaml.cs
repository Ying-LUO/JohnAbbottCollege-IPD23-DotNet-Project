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
                MessageBox.Show("Fatal error connecting to database:\n" + ex.Message, "Error Information");
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
                MessageBox.Show("Error Updating User into database:\n" + ex.Message, "Error Information");
            }
        }

        private void btUpdateMyAccount_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                if (string.IsNullOrEmpty(tbLoginName.Text) || string.IsNullOrEmpty(tbFirstName.Text) || string.IsNullOrEmpty(tbLastName.Text)
                    || string.IsNullOrEmpty(tbRole.Text) || string.IsNullOrEmpty(tbEmail.Text) || string.IsNullOrEmpty(tbPassword.Password))
                {
                    MessageBox.Show("Please input value", "User Information");
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
                    MessageBox.Show("My Account updated", "User Information");
                    ResetAndLoadDataFromDB();
                    if (currentUserInDialog != null)
                    {
                        this.DialogResult = true;
                        TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot find this user to update", "User Information");
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("User Name Must be Unique", "Error Information");
                Debug.WriteLine(ex.ToString());
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Updating User into database:\n" + ex.Message, "Error Information");
            }
        }

        private void btAddTeam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbNewTeamList.Text))
                {
                    MessageBox.Show("Please input value", "Team Information");
                    return;
                }
                if (cmbNewTeamList.SelectedItem != null)
                {
                    Team teamUpdate = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbNewTeamList.SelectedItem.ToString())).FirstOrDefault<Team>();
                    if (teamUpdate != null)
                    {
                        if (string.IsNullOrEmpty(tbTeamUpdate.Text))
                        {
                            MessageBox.Show("Please enter new team name", "User Information");
                            return;
                        }
                        teamUpdate.Name = tbTeamUpdate.Text;
                        Globals.simpleJiraDB.SaveChanges();
                        MessageBox.Show("Team Updated", "Team Information");
                        ResetAndLoadDataFromDB();
                        if (currentUserInDialog != null)
                        {
                            this.DialogResult = true;
                            TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot find team to update", "Team Information");
                    }
                }
                else
                {
                    Team newTeam = new Team { Name = cmbNewTeamList.Text };
                    Globals.simpleJiraDB.Teams.Add(newTeam);
                    Globals.simpleJiraDB.SaveChanges();
                    MessageBox.Show("Added new Team", "Team Information");
                    ResetAndLoadDataFromDB();
                }
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Team Name Must be Unique", "Error Information");
                Debug.WriteLine(ex.ToString());
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Updating User into database:\n" + ex.Message, "Error Information");
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
                            MessageBox.Show("Team Deleted", "Team Information");
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
                        MessageBox.Show("Please remove the projects and users under this team before deleting team", "Delete Team");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Cannot find team to Delete", "Team Information");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Deleting User from database:\n" + ex.Message, "Error Information");
            }
        }

        private void btUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string.IsNullOrEmpty(cmbUserList.Text)) || (string.IsNullOrEmpty(cmbUpdateTeamList.Text)) || (string.IsNullOrEmpty(cmbTeamList.Text)))
                {
                    MessageBox.Show("Please choose user and team to update", "User Information");
                    return;
                }
                if (cmbTeamList.SelectedItem.Equals(cmbUpdateTeamList.SelectedItem))
                {
                    MessageBox.Show("Please choose a new team update", "User Information");
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
                        MessageBox.Show("User Updated", "User Information");
                        ResetAndLoadDataFromDB();
                        if (currentUserInDialog != null)
                        {
                            this.DialogResult = true;
                            TeamUserUpdateCallback?.Invoke(currentUserInDialog);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot find user to update", "User Information");
                    }
                }
                else
                {
                    MessageBox.Show("Cannot find user's team to update", "User Information");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Updating User into database:\n" + ex.Message, "Error Information");
            }
        }

        private void btDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string.IsNullOrEmpty(cmbUserList.Text)) || (string.IsNullOrEmpty(cmbTeamList.Text)))
                {
                    MessageBox.Show("Please choose user to delete", "User Information");
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
                            MessageBox.Show("User Deleted", "User Information");
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
                        MessageBox.Show("Cannot find User to Delete", "User Information");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Deleting User from database:\n" + ex.Message, "Error Information");
            }
        }
    }
}