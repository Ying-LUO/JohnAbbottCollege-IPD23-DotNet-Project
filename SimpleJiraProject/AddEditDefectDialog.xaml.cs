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
using System.Windows.Shapes;

namespace SimpleJiraProject
{
    /// <summary>
    /// Interaction logic for AddEditDefectDialog.xaml
    /// </summary>
    public partial class AddEditDefectDialog : Window
    { 
        public AddEditDefectDialog(Issue defect)
        {
            InitializeComponent();
            if (defect != null)
            {
                tbTitle.Text = "Update Defect";
                cmbUserList.SelectedItem = defect.OwnerId;
            }
            else
            {
                cmbUserList.SelectedIndex = -1;
            }
        }

        private void gbPhoto_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Add photo");
        }
    }
}
