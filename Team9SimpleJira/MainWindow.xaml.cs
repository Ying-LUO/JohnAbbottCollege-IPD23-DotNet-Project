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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Team9SimpleJira.UserControls;

namespace Team9SimpleJira
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDataFromDb();
        }

        private void LoadDataFromDb()
        {
            SimpleJiraDBEntities simpleJiraDB = new SimpleJiraDBEntities();
            ProjectContent.Content = simpleJiraDB.Projects.ToList<Project>();
            SprintListView.ItemsSource = simpleJiraDB.Sprints.ToList<Sprint>();
            UserStoryListView.ItemsSource = simpleJiraDB.UserStories.ToList<UserStory>();
            TaskListView.ItemsSource = simpleJiraDB.Issues.Where(i => i.Category == "Task").ToList<Issue>();
            DefectListView.ItemsSource = simpleJiraDB.Issues.Where(i => i.Category == "Defect").ToList<Issue>();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //Application.Current.Shutdown();
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedIndex == 0)
            {
                ProjectView.Visibility = Visibility.Visible;
                SprintView.Visibility = Visibility.Hidden;
                UserStoryView.Visibility = Visibility.Hidden;
                DefectView.Visibility = Visibility.Hidden;
                TaskView.Visibility = Visibility.Hidden;
            }
            if (MenuList.SelectedIndex == 1)
            {
                ProjectView.Visibility = Visibility.Hidden;
                SprintView.Visibility = Visibility.Visible;
                UserStoryView.Visibility = Visibility.Hidden;
                DefectView.Visibility = Visibility.Hidden;
                TaskView.Visibility = Visibility.Hidden;
            }
            if (MenuList.SelectedIndex == 2)
            {
                ProjectView.Visibility = Visibility.Hidden;
                SprintView.Visibility = Visibility.Hidden;
                UserStoryView.Visibility = Visibility.Visible;
                DefectView.Visibility = Visibility.Hidden;
                TaskView.Visibility = Visibility.Hidden;
            }
            if (MenuList.SelectedIndex == 3)
            {
                ProjectView.Visibility = Visibility.Hidden;
                SprintView.Visibility = Visibility.Hidden;
                UserStoryView.Visibility = Visibility.Hidden;
                DefectView.Visibility = Visibility.Visible;
                TaskView.Visibility = Visibility.Hidden;
            }
            if (MenuList.SelectedIndex == 4)
            {
                ProjectView.Visibility = Visibility.Hidden;
                SprintView.Visibility = Visibility.Hidden;
                UserStoryView.Visibility = Visibility.Hidden;
                DefectView.Visibility = Visibility.Hidden;
                TaskView.Visibility = Visibility.Visible;
            }
        }
    }
}
