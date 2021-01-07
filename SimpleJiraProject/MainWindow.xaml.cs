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
        //SimpleJiraDBEntities simpleJiraDB;
        User currentUser;
        List<Project> currentTeamProjectList;
        List<Sprint> currentSprintList;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Globals.simpleJiraDB = new SimpleJiraDBEntities();
                cmbLoginTeam.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Fatal error connecting to database:\n" + ex.Message, "Error Information");
                Environment.Exit(1);
            }
        }

        private void LoadDataFromDb()
        {
                currentTeamProjectList = Globals.simpleJiraDB.Projects.Where(p=>p.TeamId == currentUser.TeamId).ToList<Project>();
                ProjectListView.ItemsSource = currentTeamProjectList;
                IEnumerable<int> projectIds = currentTeamProjectList.Select(p => p.ProjectId).Distinct();

                currentSprintList = Globals.simpleJiraDB.Sprints.Where(s => projectIds.Contains(s.ProjectId)).ToList<Sprint>();
                SprintListView.ItemsSource = currentSprintList;

                UserStoryListView.ItemsSource = Globals.simpleJiraDB.UserStories.Where(u=>u.OwnerId == currentUser.UserId).ToList<UserStory>();
                TaskListView.ItemsSource = Globals.simpleJiraDB.Issues.Where(i => i.Category == "Task").Where(u => u.OwnerId == currentUser.UserId).ToList<Issue>();
                DefectListView.ItemsSource = Globals.simpleJiraDB.Issues.Where(i => i.Category == "Defect").Where(u => u.OwnerId == currentUser.UserId).ToList<Issue>();
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
            if(ProjectView.IsVisible)
            {
                AddEditProjectDialog addEditProject = new AddEditProjectDialog();
                addEditProject.ShowDialog();
            }

            if (SprintView.IsVisible)
            {
                AddEditSprintDialog addEditSprint = new AddEditSprintDialog();
                addEditSprint.ShowDialog();
            }

            if (UserStoryView.IsVisible)
            {
                AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog();
                addEditUserStory.ShowDialog();
            }

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
                int currentTeamId = Globals.simpleJiraDB.Teams.SingleOrDefault(t => t.Name.Equals(cmbLoginTeam.SelectedItem.ToString())).TeamId;
                currentUser = Globals.simpleJiraDB.Users.Where(u => u.Name.Equals(tbLoginUserName.Text)).Where(ut => ut.TeamId.Equals(currentTeamId)).FirstOrDefault();
                if (currentUser != null)
                {
                    MessageBox.Show($"Welcome! {currentUser.Name} from {cmbLoginTeam.SelectedItem.ToString()}", "Login Information");
                    tblTeam.Text = currentUser.Team.Name;
                    tblUser.Text = currentUser.Name;
                    tblRole.Text = currentUser.Role;
                    btLogOut.Content = "Log out";
                    btMyAccount.Visibility = Visibility.Visible;
                    MenuList.SelectedIndex = 0;
                    LoadDataFromDb();
                    tbLoginUserName.Text = string.Empty;
                    cmbLoginTeam.SelectedIndex = -1;
                    btManageTeam.Visibility = Visibility.Hidden;
                    btManageUser.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show($"Cannot find {tbLoginUserName.Text} in {cmbLoginTeam.SelectedItem.ToString()}", "Login Information");
                    return;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error Login into system:\n" + ex.Message, "Error Information");
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
                cmbLoginTeam.ItemsSource = simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
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
            currentUser = null;
            MenuList.SelectedIndex = -1;
            HiddenView();
            LoginView.Visibility = Visibility.Visible;
            btManageTeam.Visibility = Visibility.Visible;
            btManageUser.Visibility = Visibility.Visible;
            tblTeam.Text = string.Empty;
            tblUser.Text = string.Empty;
            tblRole.Text = string.Empty;
            btLogOut.Content = "LogIn";
            btMyAccount.Visibility = Visibility.Hidden;
        }

        private void btMyAccount_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null)
            {
                MessageBox.Show("Please login first", "Login Information");
                return;
            }
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(currentUser);
            //TODO: IF CURRENT USER CHANGED INFORMATION, NEED CALL BACK FROM DIALOG
            userManagementDialog.Owner = this;
            userManagementDialog.ShowDialog();
        }

    }
}
