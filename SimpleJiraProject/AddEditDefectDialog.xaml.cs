using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        string imageLocation;

        public AddEditDefectDialog(Issue defect)
        {
            InitializeComponent();
            if (defect != null)
            {
                currentDefect = defect;
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
    }
}
