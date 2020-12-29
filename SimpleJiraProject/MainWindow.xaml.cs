﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SimpleJiraDBEntities simpleJiraDB;
        User currentUser;
        List<Project> currentTeamProjectList;
        List<Sprint> currentSprintList;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                using (simpleJiraDB = new SimpleJiraDBEntities())
                {
                    cmbLoginTeam.ItemsSource = simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Fatal error connecting to database:\n" + ex.Message, "Error Information");
                Environment.Exit(1);
            }
        }

        private void LoadDataFromDb()
        {
            using (simpleJiraDB = new SimpleJiraDBEntities())
            {
                currentTeamProjectList = simpleJiraDB.Projects.Where(p=>p.TeamId == currentUser.TeamId).ToList<Project>();
                ProjectListView.ItemsSource = currentTeamProjectList;
                IEnumerable<int> projectIds = currentTeamProjectList.Select(p => p.ProjectId).Distinct();

                currentSprintList = simpleJiraDB.Sprints.Where(s => projectIds.Contains(s.ProjectId)).ToList<Sprint>();
                SprintListView.ItemsSource = currentSprintList;

                UserStoryListView.ItemsSource = simpleJiraDB.UserStories.Where(u=>u.OwnerId == currentUser.UserId).ToList<UserStory>();
                TaskListView.ItemsSource = simpleJiraDB.Issues.Where(i => i.Category == "Task").Where(u => u.OwnerId == currentUser.UserId).ToList<Issue>();
                DefectListView.ItemsSource = simpleJiraDB.Issues.Where(i => i.Category == "Defect").Where(u => u.OwnerId == currentUser.UserId).ToList<Issue>();
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //Application.Current.Shutdown();
        }

        private void HiddenView()
        {
            LoginView.Visibility = Visibility.Hidden;
            ProjectView.Visibility = Visibility.Hidden;
            SprintView.Visibility = Visibility.Hidden;
            UserStoryView.Visibility = Visibility.Hidden;
            DefectView.Visibility = Visibility.Hidden;
            TaskView.Visibility = Visibility.Hidden;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MenuList.SelectedIndex)
            {
                case 0:
                    HiddenView();
                    ProjectView.Visibility = Visibility.Visible;
                    break;
                case 1:
                    HiddenView();
                    SprintView.Visibility = Visibility.Visible;
                    break;
                case 2:
                    HiddenView();
                    UserStoryView.Visibility = Visibility.Visible;
                    break;
                case 3:
                    HiddenView();
                    DefectView.Visibility = Visibility.Visible;
                    break;
                case 4:
                    HiddenView();
                    TaskView.Visibility = Visibility.Visible;
                    break;
                default:
                    HiddenView();
                    break;
            }
        }

        private void btNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbLoginUserName.Text)||string.IsNullOrEmpty(cmbLoginTeam.SelectedItem.ToString()))
                {
                    MessageBox.Show("Please choose a Team and input your User Name", "Login Information");
                    return;
                }
                using (simpleJiraDB = new SimpleJiraDBEntities())
                {
                    int currentTeamId = simpleJiraDB.Teams.SingleOrDefault(t => t.Name.Equals(cmbLoginTeam.SelectedItem.ToString())).TeamId;
                    currentUser = simpleJiraDB.Users.Where(u => u.Name.Equals(tbLoginUserName.Text)).Where(ut => ut.TeamId.Equals(currentTeamId)).FirstOrDefault();
                    if (currentUser != null)
                    {
                        MessageBox.Show($"Welcome! {currentUser.Name} from {cmbLoginTeam.SelectedItem.ToString()}", "Login Information");
                        tblTeam.Text = cmbLoginTeam.SelectedItem.ToString();
                        tblUser.Text = currentUser.Name;
                        btSwitchUser.Content = "Swtich User";
                        MenuList.SelectedIndex = 0;
                        LoadDataFromDb();
                        tbLoginUserName.Text = string.Empty;
                        cmbLoginTeam.SelectedIndex = -1;
                    }
                    else
                    {
                        MessageBox.Show($"Cannot find {tbLoginUserName.Text} in {cmbLoginTeam.SelectedItem.ToString()}", "Login Information");
                        return;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Login into system:\n" + ex.Message, "Error Information");
            }
        }

        private void btManageUser_Click(object sender, RoutedEventArgs e)
        {
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(currentUser);
            userManagementDialog.Owner = this;
            userManagementDialog.ShowDialog();
        }

        private void btManageTeam_Click(object sender, RoutedEventArgs e)
        {
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(currentUser);
            userManagementDialog.Owner = this;
            userManagementDialog.ShowDialog();

        }

        private void btSwitchUser_Click(object sender, RoutedEventArgs e)
        {
            MenuList.SelectedIndex = -1;
            HiddenView();
            LoginView.Visibility = Visibility.Visible;
        }
    }
}
