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
    /// Interaction logic for AddEditProjectDialog.xaml
    /// </summary>
    public partial class AddEditProjectDialog : Window
    {
        public AddEditProjectDialog()
        {
            InitializeComponent();
            try
            {
                //simpleJiraDB = new SimpleJiraDBEntities();
                comboTeam.ItemsSource = Globals.simpleJiraDB.Teams.AsEnumerable().Select(t => t.Name).ToList<string>();
            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Fatal error connecting to database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                Environment.Exit(1);
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentTeamName = (string)comboTeam.SelectedItem;
                int currentTeamId = Globals.simpleJiraDB.Teams.Where(t => t.Name.Equals(currentTeamName)).Select(t => t.TeamId).FirstOrDefault();


                if (!GeneralValidation.IsValidName(tbProjectName.Text))
                {
                    new MessageBoxCustom("Project Name must be between 2-30 characters", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                    return;
                }
                else
                {
                    Project p = new Project
                    {
                        Name = tbProjectName.Text,
                        TeamId = currentTeamId,
                    };
                    Globals.simpleJiraDB.Projects.Add(p);
                    Globals.simpleJiraDB.SaveChanges();
                    //Globals.currentTeamProjectList = Globals.simpleJiraDB.Projects.Include("Team").ToList();

                }
                DialogResult = true;

            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Database operation failed:\n" + ex.Message, MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }
    }
}
