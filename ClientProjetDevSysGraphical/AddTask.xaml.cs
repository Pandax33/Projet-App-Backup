using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ClientProjetDevSysGraphical
{
    /// <summary>
    /// Logique d'interaction pour AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        public AddTask()
        {
            InitializeComponent();
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameEntry.Text;
            string source = sourcePathEntry.Text;
            string target = targetPathEntry.Text;
            string type = typeSelector();

            GestionTask gestionTask = new GestionTask();

            if (name == null || source == null || target == null || type == null)
            {
                MessageBox.Show(ResourceHelper.GetString("Task.Popup.Add4"), ResourceHelper.GetString("Task.Popup.Warning"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                if (ProjetDevSys.AppConstants.VerifExist(source) == true && ProjetDevSys.AppConstants.VerifExist(target) == true)
                {
                    MessageBox.Show(gestionTask.CreateTask(name, source, target, type), ResourceHelper.GetString("Task.Popup.Out"), MessageBoxButton.OK, MessageBoxImage.Information);

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

        private async void sourcePathExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Envoyer une requête au serveur pour ouvrir le dialogue de sélection de dossier
                await AppConstants.SendRequestAsync("OpenFolderDialog");

                // Attendre la réponse du serveur contenant le chemin du dossier sélectionné
                string selectedFolderPath = await AppConstants.ReceiveResponseAsync();

                // Mettre à jour l'interface utilisateur avec le chemin du dossier sélectionné
                sourcePathEntry.Text = selectedFolderPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la sélection du dossier source : " + ex.Message);
            }
            finally
            {
                Activate();
            }
        }

        private async void targetPathExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Envoyer une requête au serveur pour ouvrir le dialogue de sélection de dossier
                await AppConstants.SendRequestAsync("OpenFolderDialog");

                // Attendre la réponse du serveur contenant le chemin du dossier sélectionné
                string selectedFolderPath = await AppConstants.ReceiveResponseAsync();

                // Mettre à jour l'interface utilisateur avec le chemin du dossier sélectionné
                targetPathEntry.Text = selectedFolderPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la sélection du dossier cible : " + ex.Message);
            }
            finally
            {
                Activate();
            }
        }


        private string typeSelector()
        {
            if (typeRadioButtonComplete.IsChecked == true) return "A";
            else if (typeRadioButtonDifferential.IsChecked == true) return "B";
            return null;
        }
    }
}
