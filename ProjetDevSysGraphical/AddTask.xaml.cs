using ProjetDevSys.VueModel;
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

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Logique d'interaction pour AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string SaveType { get; set; }

        public AddTask()
        {
            InitializeComponent();
        }

        #region Enter
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = textBoxName.Text;
            Name = name;
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
        #endregion

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            GestionTask gestionTask = new GestionTask();

            if (Name != null && FileSource != null && FileTarget != null && SaveType != null)
            {
                if (ProjetDevSys.AppConstants.VerifExist(FileSource) == true && ProjetDevSys.AppConstants.VerifExist(FileTarget) == true)
                {
                    MessageBox.Show(gestionTask.CreateTask(Name, FileSource, FileTarget, SaveType), "Creation", MessageBoxButton.OK, MessageBoxImage.Information);

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
                    MessageBox.Show("Le chemin source n'existe pas","probleme", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (ProjetDevSys.AppConstants.VerifExist(FileTarget) == false && ProjetDevSys.AppConstants.VerifExist(FileSource) == true)
                {
                    MessageBox.Show("Le chemin cible n'existe pas", "probleme", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if (ProjetDevSys.AppConstants.VerifExist(FileSource) == false && ProjetDevSys.AppConstants.VerifExist(FileTarget) == false)
                {
                    MessageBox.Show("Les chemins source et cible n'existent pas", "problème", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else MessageBox.Show("Veuillez valider tous les champs", "probleme", MessageBoxButton.OK, MessageBoxImage.Warning);
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
