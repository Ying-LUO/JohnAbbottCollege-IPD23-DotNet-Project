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
    /// Interaction logic for ProjectView.xaml
    /// </summary>
    public partial class ProjectView : UserControl
    {
        string updatedTeamName;
        string projectName;
        
        public ProjectView()
        {
            InitializeComponent();
        }
        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!Globals.Validator.IsValidShortName(tbEditProjectName.Text))
            {
                new MessageBoxCustom("Project Name must be between 2-30 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                tbProjectName.Text = projectName;
                tbEditProjectName.Text = projectName;
                Globals.SelectedProject.Name = projectName;
            }
            else
            {
                Globals.SelectedProject.Name = tbEditProjectName.Text;
                updatedTeamName = (string)comboTeam.SelectedItem;
                Globals.SelectedProject.TeamId = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(updatedTeamName)).Select(t => t.TeamId).FirstOrDefault();
                Globals.simpleJiraDB.SaveChanges();
                Globals.currentTeamProjectList = Globals.simpleJiraDB.Projects.Include("Team").ToList();
                Globals.AppWindow.LoadDataFromDb(Globals.currentUser);
            }  
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            getSelectedProject();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                getSelectedProject();
                if (IsProjectHasChild())
                {
                    new MessageBoxCustom("Selected User Story has Issues attached you need to clear these issues to delete", MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                
                bool? Result = new MessageBoxCustom("Are you sure to delete Selected Project? ", MessageBoxCustom.MessageType.Confirmation, MessageBoxCustom.MessageButtons.YesNo).ShowDialog();

                if (Result.Value)
                {
                        Project SelectedProject = Globals.simpleJiraDB.Projects.Where(p => p.Name.Equals(tbProjectName.Text)).FirstOrDefault();
                        Globals.simpleJiraDB.Projects.Remove(SelectedProject);
                        Globals.simpleJiraDB.SaveChanges();
                        Globals.AppWindow.LoadDataFromDb(Globals.currentUser);
                }
                else
                {
                    return;
                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Deleting Project from database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }            
        }
        private bool IsProjectHasChild()
        {
            foreach (Sprint sprint in Globals.currentSprintList)
            {
                if (Globals.SelectedProject == sprint.Project)
                {
                    return true;
                }
            }
            return false;
        }

        private void getSelectedProject()
        {
            foreach (Project p in Globals.currentTeamProjectList)
            {
                if (p.Name == tbProjectName.Text)
                {
                    Globals.SelectedProject = p;
                    projectName = p.Name;
                    tbEditProjectName.Text = p.Name;
                    comboTeam.SelectedItem = p.Team.Name;
                }
            }
        }

    }
}
