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
    /// Interaction logic for AddEditIssuetDialog.xaml
    /// </summary>
    public partial class AddEditIssueDialog : Window
    {
        Issue currentIssue;
        string imageLocation = string.Empty;
        static long threshold = 2621440;    // 2.5MB

        public AddEditIssueDialog(IssueListItem issue)
        {
            InitializeComponent();
            try
            {
                cmbCategory.ItemsSource = Enum.GetValues(typeof(IssueCategoryEnum)).Cast<IssueCategoryEnum>(); 
                cmbPriority.ItemsSource = Enum.GetValues(typeof(IssuePriorityEnum)).Cast<IssuePriorityEnum>();
                cmbStatus.ItemsSource = Enum.GetValues(typeof(IssueStatusEnum)).Cast<IssueStatusEnum>();
                List<string> userList = Globals.currentTeamUserList.AsEnumerable().Select(u => u.LoginName).ToList<string>();
                List<string> userStoryList = Globals.currentUserStoryList.AsEnumerable().Select(us => us.Name).ToList<string>();
                cmbUserList.ItemsSource = userList;
                cmbUserStoryList.ItemsSource = userStoryList;

            

                if (issue != null)
                {
                    if (issue.Category.Equals(IssueCategoryEnum.Defect.ToString()))
                    {
                        tbTitle.Text = "Update Defect";
                    }
                    else if (issue.Category.Equals(IssueCategoryEnum.Task.ToString()))
                    {
                        tbTitle.Text = "Update Task";
                    }
                    this.DataContext = issue;
                    currentIssue = Globals.simpleJiraDB.Issues.Where(iss => iss.IssueId == issue.IssueId).FirstOrDefault();
                    
                    btAddUpdate.Content = "Update";
                    cmbUserList.SelectedItem = Globals.currentTeamUserList.Where(ut => ut.UserId == issue.OwnerId).SingleOrDefault().LoginName;
                    cmbUserStoryList.SelectedItem = Globals.currentUserStoryList.Where(us => us.UserStoryId == issue.UserStoryId).SingleOrDefault().Name;
                    if (currentIssue.Photo != null)
                    {
                        image.Source = (ImageSource)((new ImageSourceConverter()).ConvertFrom(currentIssue.Photo));
                    }
                }
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("ERROR Loading data from database: \n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }


        private void gbPhoto_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFileDialog.Title = "Select Image for Issue";

                if (openFileDialog.ShowDialog() == true)
                {
                    string fileName = openFileDialog.FileName;
                    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                    {
                        var fileLength = new FileInfo(fileName).Length;

                        if(fileLength < threshold)
                        {
                            if (currentIssue != null)
                            {
                                currentIssue.Photo = File.ReadAllBytes(fileName);
                                image.Source = new BitmapImage(new Uri(fileName));
                            }
                            else
                            {
                                imageLocation = fileName;
                                image.Source = new BitmapImage(new Uri(fileName));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Image size too larger(over 2.5MB)");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is FileNotFoundException)
            {
                new MessageBoxCustom("ERROR Select image: \n"+ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }

        private void tbUserStory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentIssue != null)
            {
                UserStory selectedUserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault();
                AddEditUserStoryDialog addEditUserStory = new AddEditUserStoryDialog(selectedUserStory);
                addEditUserStory.ShowDialog();
                cmbUserStoryList.ItemsSource = Globals.currentUserStoryList.AsEnumerable().Select(us => us.Name).ToList<string>();
                cmbUserStoryList.Items.Refresh();
            }
        }

        private void clearForm()
        {
            cmbCategory.SelectedIndex = -1;
            tbIssueName.Text = string.Empty;
            tbDescription.Text = string.Empty;
            dpStartDate.SelectedDate = DateTime.Now;
            dpCompleteDate.SelectedDate = DateTime.Now;
            cmbPriority.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            cmbUserList.SelectedIndex = -1;
            cmbUserStoryList.SelectedIndex = -1;
            imageLocation = string.Empty;
            image.Source = null;
        }

        private bool validation()
        {
            if (string.IsNullOrEmpty(tbIssueName.Text) || string.IsNullOrEmpty(tbDescription.Text) 
                || cmbCategory.SelectedIndex<0 || cmbPriority.SelectedIndex < 0 || cmbStatus.SelectedIndex <0
                || cmbUserList.SelectedIndex<0 || cmbUserStoryList.SelectedIndex<0 || !Globals.Validator.IsValidLongName(tbIssueName.Text)
                || !Globals.Validator.IsValidDescription(tbDescription.Text) || dpStartDate.SelectedDate == null)
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
                    if (currentIssue != null)
                    {
                        currentIssue.Name = tbIssueName.Text;
                        currentIssue.Description = tbDescription.Text;
                        currentIssue.StartDate = dpStartDate.SelectedDate.Value.Date;
                        currentIssue.Priority = cmbPriority.SelectedItem.ToString();
                        currentIssue.Status = cmbStatus.SelectedItem.ToString();
                        Console.WriteLine("Category: " + cmbCategory.SelectedValue.ToString());
                        currentIssue.Category = cmbCategory.SelectedItem.ToString();
                        currentIssue.User = Globals.currentTeamUserList.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).FirstOrDefault();
                        currentIssue.UserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault();

                        if (cmbStatus.SelectedItem.Equals(IssueStatusEnum.Resolved))
                        {
                            currentIssue.CompleteDate = DateTime.Now;
                            if (!Globals.Validator.IsValidDate((DateTime)dpStartDate.SelectedDate, (DateTime)currentIssue.CompleteDate))
                            {
                                new MessageBoxCustom("Complete date must be after start date", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                                return;
                            }
                        }
                        else
                        {
                            currentIssue.CompleteDate = null;
                        }
                        Globals.simpleJiraDB.SaveChanges();
                        new MessageBoxCustom("New Issue Updated", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        clearForm();
                        this.Close();
                    }
                    else
                    {
                        Issue newIssue;
                        if (string.IsNullOrEmpty(imageLocation))
                        {
                            newIssue = new Issue
                            {
                                Name = tbIssueName.Text,
                                Description = tbDescription.Text,
                                StartDate = dpStartDate.SelectedDate.Value.Date,
                                Priority = cmbPriority.SelectedItem.ToString(),
                                Status = cmbStatus.SelectedItem.ToString(),
                                Category = cmbCategory.SelectedItem.ToString(),
                                OwnerId = Globals.currentTeamUserList.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).FirstOrDefault().UserId,
                                UserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault()
                            };
                        }
                        else
                        {
                            newIssue = new Issue
                            {
                                Name = tbIssueName.Text,
                                Description = tbDescription.Text,
                                StartDate = dpStartDate.SelectedDate.Value.Date,
                                Priority = cmbPriority.SelectedItem.ToString(),
                                Status = cmbStatus.SelectedItem.ToString(),
                                Category = cmbCategory.SelectedItem.ToString(),
                                OwnerId = Globals.currentTeamUserList.Where(u => u.LoginName.Equals(cmbUserList.SelectedItem.ToString())).FirstOrDefault().UserId,
                                Photo = File.ReadAllBytes(imageLocation),
                                UserStory = Globals.currentUserStoryList.Where(us => us.Name.Equals(cmbUserStoryList.SelectedItem.ToString())).FirstOrDefault()
                            };
                        }
                        Globals.simpleJiraDB.Issues.Add(newIssue);
                        Globals.simpleJiraDB.SaveChanges();
                        new MessageBoxCustom("New Issue Added", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                        clearForm();
                        this.Close();
                    }
                }
                else
                {
                    new MessageBoxCustom("All fields are required", MessageBoxCustom.MessageType.Info, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
                } 
            }
            catch (SqlException ex)
            {
                new MessageBoxCustom("Error Saving Issue into database:\n" + ex.Message, MessageBoxCustom.MessageType.Error, MessageBoxCustom.MessageButtons.Ok).ShowDialog();
            }
        }
    }
}
