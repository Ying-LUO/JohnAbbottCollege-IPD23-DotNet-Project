using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for AddEditUserStoryDialog.xaml
    /// </summary>
    public partial class AddEditUserStoryDialog : Window
    {
        public List<string> SprintsList = new List<string>();
        public List<string> UserStoriesList = new List<string>();
        UserStory currentUserStory;
        internal AddEditUserStoryDialog(UserStory userStory)
        {
            InitializeComponent();

            cmbStatus.ItemsSource = Globals.simpleJiraDB.UserStories.AsEnumerable().Select(us => us.Status).ToList<string>();
            createSprintNamesList();
            cmbSprintName.ItemsSource = SprintsList;
            createOwnerNamesList();
            cmbOwnerName.ItemsSource = Globals.simpleJiraDB.UserStories.AsEnumerable().Select(us => us.User.LoginName).ToList<string>();

            if (userStory != null) //Updating
            {
                currentUserStory = userStory;
                tbUserStoryName.Text = userStory.Name;
                dpStartDate.SelectedDate = userStory.CreateDate;
                dpCompleteDate.SelectedDate = userStory.CompleteDate;
                cmbStatus.SelectedItem = userStory.Status;
                tbPoints.Text = userStory.Point.ToString();
                tbDescription.Text = userStory.Description;
                cmbSprintName.SelectedItem = userStory.Sprint.Name;
                cmbOwnerName.SelectedItem = userStory.User.LoginName;
                btAddUpdate.Content = "Update";
                tbAddEditUserStoryTitle.Text = string.Format("Update Sprint: {0}", userStory.Name);
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
                string currentSprinttName = (string)cmbSprintName.SelectedItem;
                int currentSprintId = Globals.simpleJiraDB.Sprints.Where(s => s.Name.Equals(currentSprinttName)).Select(s => s.SprintId).FirstOrDefault();
                string currentOwnerName = (string)cmbOwnerName.SelectedItem;
                int currentUserStoryId = Globals.simpleJiraDB.Users.Where(us => us.LoginName.Equals(currentOwnerName)).Select(us => us.UserId).FirstOrDefault();

                if (currentUserStory != null)
                {
                    currentUserStory.Name = tbUserStoryName.Text;
                    currentUserStory.Description = tbDescription.Text;
                    currentUserStory.CreateDate = (DateTime)dpStartDate.SelectedDate;
                    currentUserStory.CompleteDate = (DateTime)dpCompleteDate.SelectedDate;
                    currentUserStory.Status = (string)cmbStatus.SelectedItem;
                    int point = 0;
                    int.TryParse(tbPoints.Text, out point);
                    currentUserStory.Point = point;
                    currentUserStory.SprintId = currentSprintId;
                    currentUserStory.OwnerId = currentUserStoryId ;
                }
                else
                {
                    int point = 0;
                    int.TryParse(tbPoints.Text, out point);
                    UserStory u = new UserStory
                    {
                        Name = tbUserStoryName.Text,
                        Description = tbDescription.Text,
                        CreateDate = (DateTime)dpStartDate.SelectedDate,
                        CompleteDate = (DateTime)dpCompleteDate.SelectedDate,
                        Status = (string)cmbStatus.SelectedItem,
                        Point = point,
                        SprintId = currentSprintId,
                        OwnerId = currentUserStoryId,

                    };
                    Globals.simpleJiraDB.UserStories.Add(u);
                }


                Globals.simpleJiraDB.SaveChanges();
                List<UserStory> currentSprinttList = Globals.simpleJiraDB.UserStories.Include("Sprint").ToList();


                DialogResult = true;

            }
            catch (SystemException ex)
            {
                new MessageBoxCustom("Database operation failed:\n" + ex.Message, MessageBoxCustom.MessageType.Warning, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        public void createSprintNamesList()
        {
            foreach (Sprint s in Globals.currentSprintList)
            {

                SprintsList.Add(s.Name);
            }
        }

        public void createOwnerNamesList()
        {
            foreach (UserStory us in Globals.currentUserStoryList)
            {

                UserStoriesList.Add(us.User.LoginName);
            }
        }
    }
}
