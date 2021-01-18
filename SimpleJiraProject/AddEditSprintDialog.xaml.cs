using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddEditSprintDialog.xaml
    /// </summary>
    public partial class AddEditSprintDialog : Window
    {
        public List<string> ProjectsList = new List<string>();
        Sprint currentSprint; // null when adding, non-null when editing

        internal AddEditSprintDialog(Sprint sprint)
        {
            InitializeComponent();

            cmbStatus.ItemsSource = Enum.GetValues(typeof(SprintStatusEnum));
            createProjectNamesList();
            cmbProjectName.ItemsSource = ProjectsList;

            if(sprint != null) //Updating
            {
                currentSprint = sprint;
                tbSprintName.Text = sprint.Name;
                dpStartDate.SelectedDate = sprint.StartDate;
                dpReleaseDate.SelectedDate = sprint.ReleaseDate;
                
                Enum.TryParse(sprint.Status, out SprintStatusEnum enumStatus);
                cmbStatus.SelectedItem = enumStatus;
                tbDescription.Text = sprint.Description;
                cmbProjectName.SelectedItem = sprint.Project.Name;
                btAddUpdate.Content = "Update";
                tbAddEditSprintTitle.Text = String.Format("Update Sprint: {0}", sprint.Name);
            }           
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validation())
                {
                    string currentProjectName = (string)cmbProjectName.SelectedItem;
                    int currentProjectId = Globals.simpleJiraDB.Projects.Where(p => p.Name.Equals(currentProjectName)).Select(p => p.ProjectId).FirstOrDefault();

                    if (!Globals.Validator.IsValidLongName(tbSprintName.Text))
                    {
                        new MessageBoxCustom("Name must be between 1-100 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        return;
                    }
                    else if (!Globals.Validator.IsValidDescription(tbDescription.Text))
                    {
                        new MessageBoxCustom("Description must be between 1-255 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        return;
                    }
                    else if (!Globals.Validator.IsValidDate((DateTime)dpStartDate.SelectedDate, (DateTime)dpReleaseDate.SelectedDate))
                    {
                        new MessageBoxCustom("Release date must be after start date", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        return;
                    }
                    else
                    {

                        if (currentSprint != null)
                        {
                            currentSprint.Name = tbSprintName.Text;
                            currentSprint.Description = tbDescription.Text;
                            currentSprint.StartDate = (DateTime)dpStartDate.SelectedDate;
                            currentSprint.ReleaseDate = (DateTime)dpReleaseDate.SelectedDate;
                            currentSprint.Status = cmbStatus.SelectedItem.ToString();
                            currentSprint.ProjectId = currentProjectId;
                        }
                        else
                        {

                            Sprint s = new Sprint
                            {
                                Name = tbSprintName.Text,
                                Description = tbDescription.Text,
                                StartDate = (DateTime)dpStartDate.SelectedDate,
                                ReleaseDate = (DateTime)dpReleaseDate.SelectedDate,
                                Status = cmbStatus.SelectedItem.ToString(),
                                ProjectId = currentProjectId,

                            };
                            Globals.simpleJiraDB.Sprints.Add(s);
                        }
                        Globals.simpleJiraDB.SaveChanges();
                        List<Sprint> currentProjectList = Globals.simpleJiraDB.Sprints.Include("Project").ToList();


                    }

                    DialogResult = true;
                }
                else
                {
                    new MessageBoxCustom("All fields are required", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                

            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Database operation failed:\n" + ex.Message, MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();

            }
        }

        private bool validation()
        {
            if (cmbStatus.SelectedIndex < 0 || cmbProjectName.SelectedIndex < 0 || dpStartDate.SelectedDate == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void createProjectNamesList()
        {
            foreach(Project p in Globals.currentTeamProjectList)
            { 
                ProjectsList.Add(p.Name);
            }
        }
    }
}
