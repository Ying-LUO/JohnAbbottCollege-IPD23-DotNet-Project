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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //User currentUser;

        
        List<string> allTeamsList;

        public MainWindow(User loginUser)
        {
            InitializeComponent();
            Globals.AppWindow = this;
            ProjectView.Visibility = Visibility.Visible;
            try
            {
                if (loginUser != null)
                {
                    Globals.currentUser = loginUser;
                    LoadDataFromDb(Globals.currentUser);
                    tblTeam.Text = Globals.currentUser.Team.Name;
                    tblUser.Text = Globals.currentUser.LoginName;
                    tblRole.Text = Globals.currentUser.Role;
                }
                else
                {
                    Globals.currentUser = null;
                    MessageBox.Show("Please Login first", "Login Information");
                    new LoginDialog().ShowDialog();
                }
                //Globals.simpleJiraDB = new SimpleJiraDBEntities();

                allTeamsList = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();

                Globals.currentUser = loginUser;
                LoadDataFromDb(Globals.currentUser);
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Fatal error connecting to database:\n" + ex.Message, "Error Information");
                Environment.Exit(1);
            }
        }

        public void LoadDataFromDb(User currentUser)
        {
            if (currentUser != null)
            {
                Globals.currentTeamProjectList = Globals.simpleJiraDB.Projects.Where(p => p.TeamId == currentUser.TeamId).ToList<Project>();
                foreach (Project p in Globals.currentTeamProjectList)
                {
                    p.AllTeamNamesList = allTeamsList;
                }
                ProjectListView.ItemsSource = Globals.currentTeamProjectList;
                IEnumerable<int> projectIds = Globals.currentTeamProjectList.Select(p => p.ProjectId).Distinct();
                Globals.currentSprintList = Globals.simpleJiraDB.Sprints.Where(s => projectIds.Contains(s.ProjectId)).ToList<Sprint>();
                SprintListView.ItemsSource = Globals.currentSprintList;
                Globals.currentUserStoryList = Globals.simpleJiraDB.UserStories.Where(u => u.OwnerId == currentUser.UserId).ToList<UserStory>();
                UserStoryListView.ItemsSource = Globals.currentUserStoryList;
                TaskListView.ItemsSource = Globals.simpleJiraDB.Issues.Where(i => i.Category == "Task").Where(u => u.OwnerId == currentUser.UserId).ToList<Issue>();
                DefectListView.ItemsSource = Globals.simpleJiraDB.Issues.Where(i => i.Category == "Defect").Where(u => u.OwnerId == currentUser.UserId).ToList<Issue>();
            }
            else
            {
                MessageBox.Show("Please Login first", "Login Information");
            }
        }

       
        private void HiddenView()
        {
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
            if (ProjectView.IsVisible)
            {
                AddEditProjectDialog addEditProject = new AddEditProjectDialog();
                addEditProject.ShowDialog();
                LoadDataFromDb(Globals.currentUser);
                

            }

            if (SprintView.IsVisible)
            {
                AddEditSprintDialog addEditSprint = new AddEditSprintDialog(null);
                addEditSprint.ShowDialog();
                LoadDataFromDb(Globals.currentUser);
            }

            if (UserStoryView.IsVisible)
            {
                AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog(null);
                addEditUserStory.ShowDialog();
                LoadDataFromDb(Globals.currentUser);
            }

        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectView.IsVisible)
            {
                AddEditProjectDialog addEditProject = new AddEditProjectDialog();
                addEditProject.ShowDialog();
                LoadDataFromDb(Globals.currentUser);


            }

            if (SprintView.IsVisible)
            {
                EditSelectedSprint();
            }

            if (UserStoryView.IsVisible)
            {
                AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog(null);
                addEditUserStory.ShowDialog();
                LoadDataFromDb(Globals.currentUser);
            }

        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectView.IsVisible)
            {
                
                LoadDataFromDb(Globals.currentUser);


            }

            if (SprintView.IsVisible)
            {
                if (SprintListView.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Sprint to delete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                Sprint sprint = (Sprint)SprintListView.SelectedItem;
                Globals.simpleJiraDB.Sprints.Remove(sprint);
                Globals.simpleJiraDB.SaveChanges();
                LoadDataFromDb(Globals.currentUser);
            }

            if (UserStoryView.IsVisible)
            {
                if (UserStoryListView.SelectedIndex == -1)
                {
                    MessageBox.Show("Select User Story to delete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                UserStory userStory = (UserStory)UserStoryListView.SelectedItem;
                AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog(null);
                addEditUserStory.ShowDialog();
                Globals.simpleJiraDB.UserStories.Remove(userStory);
                LoadDataFromDb(Globals.currentUser);
            }
        }

        private void btManageUser_Click(object sender, RoutedEventArgs e)
        {
            int index = 1;
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(index);
            userManagementDialog.Owner = this;
            bool? result = userManagementDialog.ShowDialog();  // this line must be stay after the assignment, otherwise value is not assigned

            if (result == true)
            {
                //cmbLoginTeam.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
            }
            //TODO: AFTER UPDATING TEAM NAME, THE TEAM COMBO BOX LIST IS NOT UPDATED ACCORDINGLY
        }

        private void btManageTeam_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(index);
            userManagementDialog.Owner = this;
            userManagementDialog.ShowDialog();
            //TODO: AFTER UPDATING TEAM NAME, THE TEAM COMBO BOX LIST IS NOT UPDATED ACCORDINGLY
        }

        private void btLogOut_Click(object sender, RoutedEventArgs e)
        {
            Globals.currentUser = null;
            this.Close();
            LoginDialog login = new LoginDialog();
            login.LoginCallback += (u) => { Globals.currentUser = u; };
            bool? result = login.ShowDialog();  // this line must be stay after the assignment, otherwise value is not assigned

            if (result == true)
            {
                MessageBox.Show("Login Successfully");
                new MainWindow(Globals.currentUser).ShowDialog();
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }

        private void btMyAccount_Click(object sender, RoutedEventArgs e)
        {
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(Globals.currentUser);
            //TODO: IF CURRENT USER CHANGED INFORMATION, NEED CALL BACK FROM DIALOG
            userManagementDialog.Owner = this;
            userManagementDialog.ShowDialog();
        }

        

        private void SprintListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedSprint();
        }

        public void EditSelectedSprint()
        {
            Sprint sprint = (Sprint)SprintListView.SelectedItem;
            if (sprint == null) { return; }
            AddEditSprintDialog addEditSprint = new AddEditSprintDialog(sprint);
            addEditSprint.ShowDialog();
            LoadDataFromDb(Globals.currentUser);
        }
    }
}
