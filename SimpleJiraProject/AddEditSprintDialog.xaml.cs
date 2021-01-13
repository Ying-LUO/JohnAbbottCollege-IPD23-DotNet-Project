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
        //public event Action<Sprint> AddNewSprintCallback;

        //public AddEditSprintDialog()
        //{
        //    InitializeComponent();

        //    cmbStatus.ItemsSource = Globals.simpleJiraDB.Sprints.AsEnumerable().Select(s => s.Status).ToList<string>();
        //    createProjectNamesList();
        //    cmbProjectName.ItemsSource = ProjectsList;
        //}

        internal AddEditSprintDialog(Sprint sprint)
        {
            InitializeComponent();

            cmbStatus.ItemsSource = Globals.simpleJiraDB.Sprints.AsEnumerable().Select(s => s.Status).ToList<string>();
            createProjectNamesList();
            cmbProjectName.ItemsSource = ProjectsList;

            if(sprint != null) //Updating
            {
                currentSprint = sprint;
                tbSprinttName.Text = sprint.Name;
                dpStartDate.SelectedDate = sprint.StartDate;
                dpReleaseDate.SelectedDate = sprint.ReleaseDate;
                cmbStatus.SelectedItem = sprint.Status;
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
                string currentProjectName = (string)cmbProjectName.SelectedItem;
                int currentProjectId = Globals.simpleJiraDB.Projects.Where(p => p.Name.Equals(currentProjectName)).Select(p => p.ProjectId).FirstOrDefault();

                if(currentSprint != null)
                {
                    currentSprint.Name = tbSprinttName.Text;
                    currentSprint.Description = tbDescription.Text;
                    currentSprint.StartDate = (DateTime)dpStartDate.SelectedDate;
                    currentSprint.ReleaseDate = (DateTime)dpReleaseDate.SelectedDate;
                    currentSprint.Status = (string)cmbStatus.SelectedItem;
                    currentSprint.ProjectId = currentProjectId;
                }
                else
                {
                    Sprint s = new Sprint
                    {
                        Name = tbSprinttName.Text,
                        Description = tbDescription.Text,
                        StartDate = (DateTime)dpStartDate.SelectedDate,
                        ReleaseDate = (DateTime)dpReleaseDate.SelectedDate,
                        Status = (string)cmbStatus.SelectedItem,
                        ProjectId = currentProjectId,

                    };
                    Globals.simpleJiraDB.Sprints.Add(s);
                }

                
                Globals.simpleJiraDB.SaveChanges();
                List<Sprint> currentProjectList = Globals.simpleJiraDB.Sprints.Include("Project").ToList();


                DialogResult = true;

            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message, "Database operation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
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
