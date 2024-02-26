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
using ClientProjetDevSysGraphical;
using System.Net.Sockets;
using System.Net;

namespace ClientProjetDevSysGraphical
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
        private Accueil Accueil;
        private bool isConnect = false;

        private Socket clientSocket;

        public MainWindow()
        {
            CultureInfo ci = new CultureInfo(ProjetDevSys.AppConstants.Langage);
            CultureInfo.CurrentUICulture = ci;

            InitializeComponent();
            
            //position
            this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;

            Accueil = new Accueil();
            contentControl.Content = Accueil;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new Accueil();
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            if (isConnect == false)
            {
                MessageBox.Show("Vous devez être connecté pour accéder à cette fonctionnalité");
                return;
            }
            if (isConnect == true)
            {
                contentControl.Content = new BackupView();
            }
        }

        private void MainWindow_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
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

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (isConnect == true)
            {
                await StopClient();
            }
            if (isConnect == false)
            {
                await StartClient();
            }
        }

        private async Task StartClient()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await clientSocket.ConnectAsync(IPAddress.Parse("127.0.0.1"), 1234);

                isConnect = true;
                connectButton.Content = "Disconnect";

                // Démarrez une boucle de réception en arrière-plan
                await Task.Run(ReceiveFromServer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion au serveur : " + ex.Message);
            }
        }

        private async Task StopClient()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                await Task.Delay(100); // Attendre un bref délai pour permettre au serveur de recevoir la notification de fermeture
                clientSocket.Close();
                isConnect = false;
                connectButton.Content = "Connect";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la déconnexion du serveur : " + ex.Message);
            }
        }

        private async Task ReceiveFromServer()
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    if (bytesRead > 0)
                    {
                        // Traitez les données reçues du serveur ici
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        // Afficher le message ou effectuer d'autres opérations
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la réception des données du serveur : " + ex.Message);
            }
        }
    }
}
