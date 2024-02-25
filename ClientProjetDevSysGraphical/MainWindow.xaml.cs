using System.IO;
using System.Net.Sockets;
using System.Net;
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

namespace ClientProjetDevSysGraphical
{
    public partial class MainWindow : Window
    {
        private Socket clientSocket;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartClient()
        {
            try
            {
                // Créer une socket TCP
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connecter la socket au serveur
                clientSocket.Connect(IPAddress.Parse("127.0.0.1"), 1234);

                // Boucle pour recevoir et afficher les images du serveur
                while (true)
                {
                    byte[] imageData = ReceiveImageData();
                    if (imageData != null)
                    {
                        BitmapSource image = ByteArrayToBitmapImage(imageData);
                        DisplayImage(image);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion au serveur : " + ex.Message);
            }
        }

        private byte[] ReceiveImageData()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = clientSocket.Receive(buffer);
            if (bytesRead > 0)
            {
                byte[] imageData = new byte[bytesRead];
                Array.Copy(buffer, imageData, bytesRead);
                return imageData;
            }
            return null;
        }

        private BitmapSource ByteArrayToBitmapImage(byte[] imageData)
        {
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        private void DisplayImage(BitmapSource image)
        {
            Dispatcher.Invoke(() =>
            {
                // Afficher l'image dans un contrôle Image
                imgDisplay.Source = image;
            });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartClient();
        }
    }
}