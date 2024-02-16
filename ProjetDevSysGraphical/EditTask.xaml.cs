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
        public int Id { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string SaveType { get; set; }

        public EditTask(int id, string name, string fileSource, string fileTarget, string saveType)
        {
            InitializeComponent();
            Id = id;
            FileSource = fileSource;
            FileTarget = fileTarget;
            SaveType = saveType;

            task_name.Text = name;
            textBoxEnterPath.Text = FileSource;
            textBoxOutPath.Text = FileTarget;
            if (SaveType == "A")
            {
                radioButtonComplete.IsChecked = true;
            }
            if (SaveType == "B")
            {
                radioButtonDifferentielle.IsChecked = true;
            }
        }

        private void textBoxEnterPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            string source = textBoxEnterPath.Text;
            FileSource = source;
        }

        private void textBoxOutPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            string target = textBoxOutPath.Text;
            FileTarget = target;
        }

        private void radioButtonComplete_Checked(object sender, RoutedEventArgs e)
        {
            SaveType = "A";
        }

        private void radioButtonDifferentielle_Checked(object sender, RoutedEventArgs e)
        {
            SaveType = "B";
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileSource != null && FileTarget != null && SaveType != null)
            {
                GestionTask gestionTask = new GestionTask();

                if (ProjetDevSys.AppConstants.VerifExist(FileSource) == true && ProjetDevSys.AppConstants.VerifExist(FileTarget) == true)
                {
                    
                    MessageBox.Show(gestionTask.EditTask(Id, FileTarget, FileSource, SaveType), ResourceHelper.GetString("Task.Popup.Out"), MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null && mainWindow.contentControl.Content is Backup backup)
                    {
                        // Appeler GenerateGrid sur cette instance
                        backup.GenerateGrid();
                    }
                    Close();
                }
                if (ProjetDevSys.AppConstants.VerifExist(FileSource) == false && ProjetDevSys.AppConstants.VerifExist(FileTarget) == true)
                {
                    MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add1"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Information);

                }
                if (ProjetDevSys.AppConstants.VerifExist(FileTarget) == false && ProjetDevSys.AppConstants.VerifExist(FileSource) == true)
                {
                    MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add2"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (ProjetDevSys.AppConstants.VerifExist(FileSource) == false && ProjetDevSys.AppConstants.VerifExist(FileTarget) == false)
                {
                    MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add3"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add4"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ButtonEnterPath_Click(object sender, RoutedEventArgs e)
        {
            textBoxEnterPath.Text = AppConstants.OpenFolderDialog();
            Activate();
        }

        private void ButtonOutPath_Click(object sender, RoutedEventArgs e)
        {
            textBoxOutPath.Text = AppConstants.OpenFolderDialog();
            Activate();
        }
    }
}
