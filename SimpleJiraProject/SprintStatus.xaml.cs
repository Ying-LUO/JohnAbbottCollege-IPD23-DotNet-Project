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
        public SprintStatus()
        {
            InitializeComponent();
            
        }

        private void planning_Selected(object sender, RoutedEventArgs e)
        {
            cmbStat.Background = planning.Background;
           //UpdateSprintStatus();
        }

        private void ongoing_Selected(object sender, RoutedEventArgs e)
        {
            cmbStat.Background = ongoing.Background;
            //UpdateSprintStatus();
        }

        private void released_Selected(object sender, RoutedEventArgs e)
        {
            cmbStat.Background = released.Background;
            //UpdateSprintStatus();
        }

        public void UpdateSprintStatus()
        {
            
            Globals.SelectedSprint.Status = cmbStat.SelectedItem.ToString();
            Globals.simpleJiraDB.SaveChanges();
            Globals.AppWindow.LoadDataFromDb(Globals.currentUser);

        }

       
    }
}
