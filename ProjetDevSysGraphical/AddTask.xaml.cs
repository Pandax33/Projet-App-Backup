using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
        public AddTask()
        {
            InitializeComponent();

            //position
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;
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
                    //update grid
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


        private void nameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox name = sender as TextBox;
            if (name == null) return;

            int cursorPosition = name.SelectionStart; //Register cursor position to restore it

            string nospace = name.Text.Replace(" ", "_");
            string cleanText = Regex.Replace(nospace, "[^a-zA-Z0-9-_]", "");

            name.Text = cleanText;

            name.SelectionStart = Math.Min(cursorPosition, name.Text.Length);
        }
    }
}
