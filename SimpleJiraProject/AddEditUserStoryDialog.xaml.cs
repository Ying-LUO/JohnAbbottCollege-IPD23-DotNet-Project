using System.Windows;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for AddEditUserStoryDialog.xaml
    /// </summary>
    public partial class AddEditUserStoryDialog : Window
    {
        public AddEditUserStoryDialog()
        {
            InitializeComponent();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
