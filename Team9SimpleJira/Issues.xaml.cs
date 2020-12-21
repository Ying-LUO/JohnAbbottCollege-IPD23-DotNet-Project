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

namespace Team9SimpleJira
{
    /// <summary>
    /// Interaction logic for Issues.xaml
    /// </summary>
    public partial class Issues : Page
    {
        public Issues()
        {
            InitializeComponent();
        }

        private void SprintDetails_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SprintsOld());
        }
    }
}
