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

        private void ButtonValider_Click(object sender, RoutedEventArgs e)
        {
            GestionTask gestionTask = new GestionTask();

            if (Name != null && FileSource != null && FileTarget != null && SaveType != null)
            {
                gestionTask.VerifSource(FileSource);
                gestionTask.VerifSource(FileTarget);
            }
            if (gestionTask.VerifSource(FileSource) == true && gestionTask.VerifSource(FileTarget) == true)
            {
                gestionTask.CreateTask(Name, FileSource, FileTarget, SaveType);

                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.GenerateGrid();
            }
            if (gestionTask.VerifSource(FileSource) == false && gestionTask.VerifSource(FileTarget) == true)
            {
                PopUpWPF popUpWPF = new PopUpWPF("Le chemin source n'existe pas");
                popUpWPF.ShowDialog();
            }
            if (gestionTask.VerifSource(FileTarget) == false && gestionTask.VerifSource(FileSource) == true)
            {
                PopUpWPF popUpWPF = new PopUpWPF("Le chemin cible n'existe pas");
                popUpWPF.ShowDialog();
            }
            if (gestionTask.VerifSource(FileSource) == false && gestionTask.VerifSource(FileTarget) == false)
            {
                PopUpWPF popUpWPF = new PopUpWPF("Les chemins source et cible n'existent pas");
                popUpWPF.ShowDialog();
            }
            Hide();
        }
    }
}
