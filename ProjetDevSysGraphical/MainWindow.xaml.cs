using System.Globalization;
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
//using System.Windows.Forms;

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
        private BackupView Backup;

        //private NotifyIcon _notifyIcon;

        public MainWindow()
        {
            CultureInfo ci = new CultureInfo(ProjetDevSys.AppConstants.Langage);
            CultureInfo.CurrentUICulture = ci;

            InitializeComponent();
            
            //position
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;

            Backup = new BackupView();
            contentControl.Content = Backup;
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
            //Application.Current.Shutdown();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new ConfigControl();
        }
        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void buttonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                maximizeImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/maximize.png"));
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                maximizeImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/restore.png"));
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Enter(object sender, MouseEventArgs e)
        {
            mainBorder.BorderBrush = Brushes.Red;
        }

        private void CloseButton_Leave(object sender, MouseEventArgs e)
        {
            mainBorder.BorderBrush = Brushes.Transparent;
        }

        public void HotReload()
        {
            ThemeLoader.LoadTheme();
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            this.Close();
        }
    }
}
