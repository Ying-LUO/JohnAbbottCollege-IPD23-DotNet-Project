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

        public TeamUserManagementDialog(int index)
        {
            InitializeComponent();
            ResetAndLoadDataFromDB();

            tcTeamUser.SelectedIndex = index;
            
            tiMyAccount.Visibility = Visibility.Hidden;
            tiTeam.Visibility = Visibility.Visible;
            tiUser.Visibility = Visibility.Visible;
        }

        public TeamUserManagementDialog(User currentUser)
        {
            InitializeComponent();
            ResetAndLoadDataFromDB();

            tcTeamUser.SelectedIndex = 2;

            cmbTeamListMyAccount.SelectedItem = currentUser.Team.Name;
            cmbTeamListMyAccount.IsEnabled = false;
            //TODO: tbUserNameMyAccount.Text = currentUser.LoginName;
            //TODO: tbRoleMyAccount.Text = currentUser.Role;
            currentUserInDialog = currentUser;
            
            tiMyAccount.Visibility = Visibility.Visible;
            tiTeam.Visibility = Visibility.Hidden;
            tiUser.Visibility = Visibility.Hidden;

            tblStatus.Text = $"Current User: {currentUser.LoginName}, {currentUser.Role} From {currentUser.Team.Name}";
        }

        private void ResetAndLoadDataFromDB()
        {
            try
            {
                Globals.simpleJiraDB = new SimpleJiraDBEntities();
                cmbNewTeamList.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).Distinct().ToList<string>();
                cmbNewTeamList.Items.Refresh();

                cmbTeamList.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).Distinct().ToList<string>();
                cmbTeamList.Items.Refresh();

                cmbTeamListMyAccount.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).Distinct().ToList<string>();
                cmbTeamListMyAccount.Items.Refresh();

                cmbNewRoleList.ItemsSource = Globals.simpleJiraDB.Users.AsEnumerable().Select(u => u.Role).Distinct().ToList<string>();
                cmbNewRoleList.Items.Refresh();

                //TODO: tbRoleMyAccount.Text = string.Empty;
                tbTeamUpdate.Text = string.Empty;
                tbUserUpdate.Text = string.Empty;
                //TODO: tbUserNameMyAccount.Text = string.Empty;

                cmbNewRoleList.SelectedIndex = -1;
                cmbNewTeamList.SelectedIndex = -1;
                cmbNewUserList.SelectedIndex = -1;

                cmbNewTeamList.Text = string.Empty;
                cmbNewUserList.Text = string.Empty;
                cmbNewRoleList.Text = string.Empty;
                cmbTeamList.SelectedIndex = -1;
                cmbTeamListMyAccount.SelectedIndex = -1;

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
                btTeam.Content = "Update Team Name";
            }
            else
            {
                tbTeamUpdate.Visibility = Visibility.Hidden;
                btTeam.Content = "Add New Team";
            }
        }

        private void cmbTeamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbTeamList.SelectedItem != null)
                {
                    List<string> userList = Globals.simpleJiraDB.Users.Include("Team").Where(ut => ut.Team.Name.Equals(cmbTeamList.SelectedItem.ToString())).AsEnumerable().Select(u => u.LoginName).ToList<string>();
                        
                    if (userList != null)
                    {
                        cmbNewUserList.ItemsSource = userList;
                        cmbNewUserList.Items.Refresh();
                        cmbNewUserList.Text = string.Empty;
                        cmbNewUserList.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbNewUserList.Text = string.Empty;
                    }
                }    
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Updating User into database:\n" + ex.Message, "Error Information");
            }
        }

        private void cmbNewUserList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cmbNewUserList.SelectedItem != null)
            {
                tbUserUpdate.Visibility = Visibility.Visible;
                btUser.Content = "Update User";
            }
            else
            {
                tbUserUpdate.Visibility = Visibility.Hidden;
                btUser.Content = "Add New User";
            }
        }

        private void btUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string.IsNullOrEmpty(cmbNewUserList.Text)) || (string.IsNullOrEmpty(cmbNewRoleList.Text)) || (string.IsNullOrEmpty(cmbTeamList.Text)))
                {
                    MessageBox.Show("Please input value", "User Information");
                    return;
                }
                if (cmbNewUserList.SelectedItem != null)
                {
                    User userUpdate = Globals.simpleJiraDB.Users.Where(u => u.LoginName.Equals(cmbNewUserList.SelectedItem.ToString())).FirstOrDefault<User>();

                    if (userUpdate != null)
                    {
                        if (string.IsNullOrEmpty(tbUserUpdate.Text))
                        {
                            MessageBox.Show("Please enter new user name","User Information");
                            return;
                        }
                        userUpdate.LoginName = tbUserUpdate.Text;
                        userUpdate.Role = cmbNewRoleList.Text;
                        Globals.simpleJiraDB.SaveChanges();
                        MessageBox.Show("User Updated", "User Information");
                        ResetAndLoadDataFromDB();
                    }
                    else
                    {
                        MessageBox.Show("Cannot find user to update", "User Information");
                    }
                }
                else
                {
                    Team teamForUser = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(cmbTeamList.SelectedItem.ToString())).FirstOrDefault<Team>();
                    User newUser = new User { Team = teamForUser, LoginName = cmbNewUserList.Text, Role = cmbNewRoleList.Text };
                    Globals.simpleJiraDB.Users.Add(newUser);
                    Globals.simpleJiraDB.SaveChanges();
                    MessageBox.Show("Added new User", "User Information");
                    ResetAndLoadDataFromDB();
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

        private void btTeam_Click(object sender, RoutedEventArgs e)
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
                    }
                    else
                    {
                        MessageBox.Show("Cannot find team to update", "User Information");
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

       
        private void btUpdateMyAccount_Click(object sender, RoutedEventArgs e)
        { /*
            try
            {
                if ((string.IsNullOrEmpty(tbUserNameMyAccount.Text)) || (string.IsNullOrEmpty(tbRoleMyAccount.Text)))
                {
                    MessageBox.Show("Please input value", "User Information");
                    return;
                }
                User myAccount = simpleJiraDB.Users.Include("Team").Where(u => u.UserId == currentUserInDialog.UserId).FirstOrDefault<User>();
                if (myAccount != null)
                {
                    myAccount.LoginName = tbUserNameMyAccount.Text;
                    myAccount.Role = tbRoleMyAccount.Text;
                    simpleJiraDB.SaveChanges();
                    MessageBox.Show("My Account updated", "User Information");
                    tblStatus.Text = $"Update Current User To : {tbUserNameMyAccount.Text}, {tbRoleMyAccount.Text} From {cmbTeamListMyAccount.SelectedItem.ToString()}";
                    ResetAndLoadDataFromDB();
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
                MessageBox.Show("Error Updating User into database:\n" btUpdateMyAccount_Click ex.Message, "Error Information");
            }*/
        }
        
        }
}
