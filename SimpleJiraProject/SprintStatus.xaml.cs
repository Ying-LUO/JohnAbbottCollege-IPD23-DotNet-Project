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

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for SprintStatus.xaml
    /// </summary>
    /// 
    public enum SprintStatusEnum2 { Planning, Ongoing, Released }
    public partial class SprintStatus : UserControl
    {
        public string status;
        public SprintStatus()
        {
            InitializeComponent();
            //getStatus();


        }

        //private void planning_Selected(object sender, RoutedEventArgs e)
        //{
        //    cmbStat.Background = planning.Background;
        //    status = "Planning";
        //    UpdateSprintStatus(status);
        //}

        //private void ongoing_Selected(object sender, RoutedEventArgs e)
        //{
        //    cmbStat.Background = ongoing.Background;
        //    status = "Ongoing";
        //    UpdateSprintStatus(status);
        //}

        //private void released_Selected(object sender, RoutedEventArgs e)
        //{
        //    cmbStat.Background = released.Background;
        //    status = "Released";
        //    UpdateSprintStatus(status);
        //}

        //public void UpdateSprintStatus(string status)
        //{
            
        //    Globals.SelectedSprint.Status = status;
        //    Globals.simpleJiraDB.SaveChanges();
        //    Globals.AppWindow.LoadDataFromDb(Globals.currentUser);

        //}

        //public void getStatus()
        //{
        //    foreach (Sprint s in Globals.currentSprintList)
        //    {
                
        //            cmbStat.DisplayMemberPath = s.Status;
                
        //    }
        //}

        private void btStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miPlanning_Click(object sender, RoutedEventArgs e)
        {
            
            status = "Planning";
            UpdateSprintStatus(status);
            btStatus.Background = Brushes.LightGray;
        }

        private void miOngoing_Click(object sender, RoutedEventArgs e)
        {
            
            status = "Ongoing";
            UpdateSprintStatus(status);
            btStatus.Background = Brushes.LightBlue;
        }

        private void miReleased_Click(object sender, RoutedEventArgs e)
        {
            
            status = "Released";
            UpdateSprintStatus(status);
            btStatus.Background = Brushes.LightGreen;
        }

        public void UpdateSprintStatus(string status)
        {

            Globals.SelectedSprint.Status = status;
            Globals.simpleJiraDB.SaveChanges();
            Globals.AppWindow.LoadDataFromDb(Globals.currentUser);
            if (status == "Planning")
            {
                btStatus.Background = Brushes.LightGray;
            }
            if (status == "Ongoing")
            {
                btStatus.Background = Brushes.LightBlue;
            }
            if (status == "Released")
            {
                btStatus.Background = Brushes.LightGreen;
            }


        }
    }
}
