using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for AddEditDefectDialog.xaml
    /// </summary>
    public partial class AddEditDefectDialog : Window
    {
        Issue currentDefect;
        string imageLocation = string.Empty;

        public AddEditDefectDialog(Issue defect)
        {
            InitializeComponent();
            cmbPriority.ItemsSource = Enum.GetValues(typeof(IssuePriorityEnum)).Cast<IssuePriorityEnum>();
            cmbStatus.ItemsSource = Enum.GetValues(typeof(IssueStatusEnum)).Cast<IssueStatusEnum>();
            List<string> userList = Globals.currentTeamUserList.AsEnumerable().Select(u=> u.LoginName).ToList<string>();
            List<string> userStoryList = Globals.currentUserStoryList.AsEnumerable().Select(us => us.Name).ToList<string>();
            cmbUserList.ItemsSource = userList;
            cmbUserStoryList.ItemsSource = userStoryList;

            if (defect != null)
            {
                this.DataContext = defect;
                currentDefect = defect;
                tbTitle.Text = "Update Defect";
                btAddUpdate.Content = "Update";
                cmbUserList.SelectedValue = Globals.currentTeamUserList.Where(ut=>ut.UserId == defect.OwnerId).Select(u => u.LoginName).ToString();
                cmbUserStoryList.SelectedItem = Globals.currentUserStoryList.Where(us=>us.UserStoryId == defect.UserStoryId).Select(ust=>ust.Name);
                if (defect.Photo!=null)
                {
                    image.Source = (ImageSource)((new ImageSourceConverter()).ConvertFrom(defect.Photo));
                }
            }
            else
            {
                cmbUserList.SelectedIndex = -1;
            }
        }


        private void gbPhoto_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFileDialog.Title = "Select Image for Defect";

                if (openFileDialog.ShowDialog() == true)
                {
                    string fileName = openFileDialog.FileName;
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        if (currentDefect!=null)
                        {
                            currentDefect.Photo = File.ReadAllBytes(fileName);
                            image.Source = new BitmapImage(new Uri(fileName));
                        }
                        else
                        {
                            imageLocation = fileName;
                            image.Source = new BitmapImage(new Uri(fileName));
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is FileNotFoundException)
            {
                MessageBox.Show("ERROR Select image: " + ex.Message, "Error Information");
            }
        }

        private void tbUserStory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentDefect != null)
            {
                AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog(currentDefect.UserStory);
                addEditUserStory.ShowDialog();
            }
        }

        private void clearForm()
        {
            tbDefectName.Text = string.Empty;
            tbDescription.Text = string.Empty;
            cmbPriority.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            cmbUserList.SelectedIndex = -1;
            cmbUserStoryList.SelectedIndex = -1;
            imageLocation = string.Empty;
            image.Source = null;
        }

        private bool validation()
        {
            if (string.IsNullOrEmpty(tbDefectName.Text) || string.IsNullOrEmpty(tbDescription.Text) )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validation())
                {
                    if (currentDefect != null)
                    {
                        currentDefect.Name = tbDefectName.Text;
                        currentDefect.Description = tbDescription.Text;
                        currentDefect.StartDate = dpStartDate.SelectedDate.Value.Date;
                        currentDefect.Priority = cmbPriority.SelectedItem.ToString();
                        currentDefect.Status = cmbStatus.SelectedItem.ToString();
                        currentDefect.Category = "Defect";
                        currentDefect.User = Globals.currentTeamUserList.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).FirstOrDefault();
                        currentDefect.UserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault();
                        Globals.simpleJiraDB.SaveChanges();
                        new MessageBoxCustom("New Defect Updated", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        clearForm();
                    }
                    else
                    {
                        Issue newDefect;
                        if (string.IsNullOrEmpty(imageLocation))
                        {
                            newDefect = new Issue
                            {
                                Name = tbDefectName.Text,
                                Description = tbDescription.Text,
                                StartDate = dpStartDate.SelectedDate.Value.Date,
                                Priority = cmbPriority.SelectedItem.ToString(),
                                Status = cmbStatus.SelectedItem.ToString(),
                                Category = "Defect",
                                OwnerId = Globals.currentTeamUserList.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).FirstOrDefault().UserId,
                                UserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault()
                            };
                        }
                        else
                        {
                            newDefect = new Issue
                            {
                                Name = tbDefectName.Text,
                                Description = tbDescription.Text,
                                StartDate = dpStartDate.SelectedDate.Value.Date,
                                Priority = cmbPriority.SelectedItem.ToString(),
                                Status = cmbStatus.SelectedItem.ToString(),
                                Category = "Defect",
                                OwnerId = Globals.currentTeamUserList.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).FirstOrDefault().UserId,
                                Photo = File.ReadAllBytes(imageLocation),
                                UserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault()
                            };
                        }
                        Globals.simpleJiraDB.Issues.Add(newDefect);
                        Globals.simpleJiraDB.SaveChanges();
                        new MessageBoxCustom("New Defect Added", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        clearForm();
                    }
                }
                else
                {
                    new MessageBoxCustom("Please input value", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                } 
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Saving Defect into database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }
    }
}
