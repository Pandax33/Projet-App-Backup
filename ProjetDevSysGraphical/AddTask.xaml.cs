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

        private void ButtonValider_Click(object sender, RoutedEventArgs e)
        {
            if (Name != null && FileSource != null && FileTarget != null && SaveType != null)
            {
                GestionTask gestionTask = new GestionTask();
                gestionTask.CreateTask(Name, FileSource, FileTarget, SaveType);
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
