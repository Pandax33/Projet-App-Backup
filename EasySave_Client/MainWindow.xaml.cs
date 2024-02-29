using EasySave_Client;
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

//using System.Windows.Forms;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Button> deleteButtonList = new List<Button>();
        private List<Button> editButtonList = new List<Button>();
        private List<string> buttonNameList = new List<string>();
        private List<int> idToLaunch = new List<int>();
        private Accueil Accueil;

        //private NotifyIcon _notifyIcon;

        public MainWindow()
        {
            CultureInfo ci = new CultureInfo(AppConstants.Langage);
            CultureInfo.CurrentUICulture = ci;

            InitializeComponent();
            
            //position
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;

            Accueil =  new Accueil();
            contentControl.Content = Accueil;
        }

        

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Accueil();
        }
        private void MainWindow_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Application.Current.Shutdown();
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

        private void Ip_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new IpView();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new BackupView();
        }
    }
}
