using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using ProjetDevSys.VueModel;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GestionTask gestionTask = new GestionTask();
        private List<Button> deleteButtonList = new List<Button>();
        private List<Button> editButtonList = new List<Button>();
        private List<string> buttonNameList = new List<string>();
        private List<int> idToLaunch = new List<int>();
        private Backup Backup;

        public MainWindow()
        {
            InitializeComponent();
            Backup = new Backup();
            
        }

        

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Accueil();
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = Backup;

        }

       

        private void MainWindow_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new ConfigControl();
        }
    }
}
