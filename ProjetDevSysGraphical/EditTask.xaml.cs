using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
using System.Xml.Linq;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Logique d'interaction pour EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        int Id;
        public EditTask(int id, string name, string fileSource, string fileTarget, string saveType)
        {
            InitializeComponent();

            //position
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;

            Id = id;
            taskName.Content = name;
            sourcePathEntry.Text = fileSource;
            targetPathEntry.Text = fileTarget;

            if (saveType == "A")
            {
                typeRadioButtonComplete.IsChecked = true;
            }
            if (saveType == "B")
            {
                typeRadioButtonDifferential.IsChecked = true;
            }
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            string source = sourcePathEntry.Text;
            string target = targetPathEntry.Text;
            string type = typeSelector();

            GestionTask gestionTask = new GestionTask();

            if (source == null || target == null || type == null)
            {
                MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add4"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                if (ProjetDevSys.AppConstants.VerifExist(source) == true && ProjetDevSys.AppConstants.VerifExist(target) == true)
                {
                    MessageBox.Show(gestionTask.EditTask(Id, target,source, type), ResourceHelper.GetString("Task.Popup.Out"), MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    // Appeler GenerateGrid sur cette instance
                    if (mainWindow != null && mainWindow.contentControl.Content is BackupView backup) backup.GenerateGrid();
                    Close();
                }

                else if (ProjetDevSys.AppConstants.VerifExist(source) == false && ProjetDevSys.AppConstants.VerifExist(target) == false)
                {
                    MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add3"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ProjetDevSys.AppConstants.VerifExist(source) == false)
                {
                    MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add1"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (ProjetDevSys.AppConstants.VerifExist(target) == false)
                {
                    MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add2"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void sourcePathExplorer_Click(object sender, RoutedEventArgs e)
        {
            sourcePathEntry.Text = AppConstants.OpenFolderDialog();
            Activate();
        }
        private void targetPathExplorer_Click(Object sender, RoutedEventArgs e)
        {
            targetPathEntry.Text = AppConstants.OpenFolderDialog();
            Activate();
        }

        private string typeSelector()
        {
            if (typeRadioButtonComplete.IsChecked == true) return "A";
            else if (typeRadioButtonDifferential.IsChecked == true) return "B";
            return null;
        }
    }
}
