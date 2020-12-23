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

namespace Team9SimpleJira.UserControls
{
    /// <summary>
    /// Interaction logic for Projects.xaml
    /// </summary>
    public partial class Projects : UserControl
    {
        public Projects()
        {
            InitializeComponent();

            if (GetSprints().Count > 0)
            {
                lvProjects.ItemsSource = GetSprints();
            }
        }

        private List<ProjectTest> GetSprints()
        {
            return new List<ProjectTest>()
            {
                new ProjectTest("Sprint1", "/Icons/p1.png"),
                new ProjectTest("Sprint2", "/Icons/p1.png"),
                new ProjectTest("Sprint3", "/Icons/p1.png"),
                new ProjectTest("Sprint4", "/Icons/p1.png"),
                new ProjectTest("Sprint5", "/Icons/p1.png"),
                new ProjectTest("Sprint5", "/Icons/p1.png")
            };
        }

        
    }
}
