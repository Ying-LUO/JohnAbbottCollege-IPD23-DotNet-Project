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
                }
                else
                {
                    Globals.currentUser = null;
                    new MessageBoxCustom("Please login", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    new LoginDialog().ShowDialog();
                }
            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Fatal error connecting to database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                Environment.Exit(1);
            }
            


        }

        public void LoadDataFromDb(User currentUser)
        {
            if (currentUser != null)
            {
                if (!currentUser.LoginName.Equals("Admin"))
                {
                    btManageTeam.Content = "Update My Account";
                }
                tblTeam.Text = Globals.currentUser.Team.Name;
                tblUser.Text = Globals.currentUser.LoginName;
                tblRole.Text = Globals.currentUser.Role;

                Globals.currentTeamProjectList = Globals.simpleJiraDB.Projects.Where(p => p.TeamId == currentUser.TeamId).ToList<Project>();
                foreach (Project p in Globals.currentTeamProjectList)
                {
                    p.AllTeamNamesList = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
                }

                Globals.currentTeamUserList = Globals.simpleJiraDB.Users.Where(u => u.TeamId == currentUser.TeamId).ToList<User>();

                ProjectListView.ItemsSource = Globals.currentTeamProjectList;
                IEnumerable<int> projectIds = Globals.currentTeamProjectList.Select(p => p.ProjectId).Distinct();

                Globals.currentSprintList = Globals.simpleJiraDB.Sprints.Where(s => projectIds.Contains(s.ProjectId)).ToList<Sprint>();
                IEnumerable<int> sprintIds = Globals.currentSprintList.Select(sp => sp.SprintId).Distinct();

                SprintListView.ItemsSource = Globals.currentSprintList;

                Globals.currentUserStoryList = Globals.simpleJiraDB.UserStories.Where(us => sprintIds.Contains(us.SprintId)).ToList<UserStory>();
                IEnumerable<int> userStoryIds = Globals.currentUserStoryList.Select(us => us.UserStoryId).Distinct();

                UserStoryListView.ItemsSource = Globals.currentUserStoryList;

                TaskListView.ItemsSource = Globals.simpleJiraDB.Issues.Where(i => i.Category == "Task").Where(iss => userStoryIds.Contains(iss.UserStoryId)).ToList<Issue>();
                DefectListView.ItemsSource = Globals.simpleJiraDB.Issues.Where(i => i.Category == "Defect").Where(iss => userStoryIds.Contains(iss.UserStoryId)).ToList<Issue>();
            }
            else
            {
                new MessageBoxCustom("Please Login first", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
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
                    LoadDataFromDb(Globals.currentUser);

                    break;
                case 1:
                    HiddenView();
                   
                    SprintView.Visibility = Visibility.Visible;
                    LoadDataFromDb(Globals.currentUser);

                    
                    break;
                case 2:
                    HiddenView();
                    UserStoryView.Visibility = Visibility.Visible;
                    LoadDataFromDb(Globals.currentUser);
                    break;
                case 3:
                    HiddenView();
                    DefectView.Visibility = Visibility.Visible;
                    Globals.AppWindow = new MainWindow(Globals.currentUser);
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

            if (DefectView.IsVisible)
            {
                AddEditDefectDialog addEditDefect = new AddEditDefectDialog(null);
                addEditDefect.ShowDialog();
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
                EditSelectedUserStory();
            }

            if (DefectView.IsVisible)
            {
                int index = DefectListView.SelectedIndex;

                if ( index >= 0)
                {
                    AddEditDefectDialog addEditDefect = new AddEditDefectDialog( (Issue)DefectListView.SelectedItem);
                    addEditDefect.ShowDialog();
                    LoadDataFromDb(Globals.currentUser);
                }
                else
                {
                    new MessageBoxCustom("Please choose one defect to update", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok);
                }
                
            }

        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectView.IsVisible)
            {
                if (Globals.SelectedProject == null)
                {
                    new MessageBoxCustom("Select Sprint to delete", MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }

                Globals.simpleJiraDB.Projects.Remove(Globals.SelectedProject);
                Globals.simpleJiraDB.SaveChanges();
                LoadDataFromDb(Globals.currentUser);


            }

            if (SprintView.IsVisible)
            {
                if (SprintListView.SelectedIndex == -1)
                {
                    new MessageBoxCustom("Select Sprint to delete", MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                //Sprint sprint = (Sprint)SprintListView.SelectedItem;
                //Globals.currentSprintList.Remove(Globals.SelectedSprint);
                Globals.simpleJiraDB.Sprints.Remove(Globals.SelectedSprint);
                Globals.simpleJiraDB.SaveChanges();
                LoadDataFromDb(Globals.currentUser);
            }

            if (UserStoryView.IsVisible)
            {
                if (UserStoryListView.SelectedIndex == -1)
                {
                    new MessageBoxCustom("Select User Story to delete", MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                //UserStory userStory = (UserStory)UserStoryListView.SelectedItem;
                Globals.simpleJiraDB.UserStories.Remove(Globals.SelectedUserStory);
                Globals.simpleJiraDB.SaveChanges();
                LoadDataFromDb(Globals.currentUser);
            }
        }

        private void btManageTeam_Click(object sender, RoutedEventArgs e)
        {
            TeamUserManagementDialog userManagementDialog = new TeamUserManagementDialog(Globals.currentUser);
            userManagementDialog.Owner = this;

            userManagementDialog.TeamUserUpdateCallback += (u) => { Globals.currentUser = u; };
            bool? result = userManagementDialog.ShowDialog(); 
            if (result != null)
            {
                LoadDataFromDb(Globals.currentUser);
            }
        }

        private void btLogOut_Click(object sender, RoutedEventArgs e)
        {
            Globals.currentUser = null;
            this.Close();
            LoginDialog login = new LoginDialog();
            login.LoginCallback += (u) => { Globals.currentUser = u; };
            bool? result = login.ShowDialog();  

            if (result == true)
            {
                new MainWindow(Globals.currentUser).ShowDialog();
            }
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

        
        private void SprintListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int listIndex = SprintListView.SelectedIndex;
            if(listIndex != -1) 
            {
                Globals.SelectedSprint = Globals.currentSprintList[listIndex];
            }
           
            Console.WriteLine(SprintListView.SelectedIndex);

            //if (Globals.SelectedSprint == null) { return; }
        }

        private void UserStoryListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedUserStory();
        }

        private void UserStoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int listIndex = UserStoryListView.SelectedIndex;
            if(listIndex != -1)
            {
                Globals.SelectedUserStory = Globals.currentUserStoryList[listIndex];
            }
            
            Console.WriteLine(UserStoryListView.SelectedIndex);

            //if (Globals.SelectedSprint == null) { return; }
        }

        public void EditSelectedUserStory()
        {
            UserStory userStory = (UserStory)UserStoryListView.SelectedItem;
            if (userStory == null) { return; }
            AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog(userStory);
            addEditUserStory.ShowDialog();
            LoadDataFromDb(Globals.currentUser);

        }

        
    }
}
