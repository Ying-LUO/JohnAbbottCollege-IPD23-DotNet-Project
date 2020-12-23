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
    /// Interaction logic for Sprints.xaml
    /// </summary>
    public partial class Sprints : UserControl
    {
        public Sprints()
        {
            InitializeComponent();
            var sprints = GetSprints();
            if (sprints.Count > 0)
            {
                lvSprints.ItemsSource = sprints;
            }
        }

        private List<SprintTestTest> GetSprints()
        {
            return new List<SprintTestTest>()
            {
                new SprintTestTest("Sprint1", "2-2-2021", "2-3-2020", "Postpone", "dsdfsdfgagf sfdgfsdg dfgsgfd dfgdfgdfsg dfgdfgdfgfg dfgdfgsdfgsdg fsgdgdg fgsdfgfgdg sdfgsdfgfdsg"),
                    new SprintTestTest("Sprint2", "2-2-2021", "2-3-2020", "Postpone", "dsdfsdfgagf sfdgfsdg dfgsgfd dfgdfgdfsg dfgdfgdfgfg dfgdfgsdfgsdg fsgdgdg fgsdfgfgdg sdfgsdfgfdsg"),
                    new SprintTestTest("Sprint3", "2-2-2021", "2-3-2020", "Postpone", "dsdfsdfgagf sfdgfsdg dfgsgfd dfgdfgdfsg dfgdfgdfgfg dfgdfgsdfgsdg fsgdgdg fgsdfgfgdg sdfgsdfgfdsg"),
                    new SprintTestTest("Sprint4", "2-2-2021", "2-3-2020", "Postpone", "dsdfsdfgagf sfdgfsdg dfgsgfd dfgdfgdfsg dfgdfgdfgfg dfgdfgsdfgsdg fsgdgdg fgsdfgfgdg sdfgsdfgfdsg"),
                    new SprintTestTest("Sprint5", "2-2-2021", "2-3-2020", "Postpone", "dsdfsdfgagf sfdgfsdg dfgsgfd dfgdfgdfsg dfgdfgdfgfg dfgdfgsdfgsdg fsgdgdg fgsdfgfgdg sdfgsdfgfdsg"),
            };
        }

        private void lvSprints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
