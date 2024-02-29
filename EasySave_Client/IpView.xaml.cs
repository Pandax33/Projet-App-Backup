using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using ProjetDevSysGraphical;

namespace EasySave_Client
{
    /// <summary>
    /// Interaction logic for IpView.xaml
    /// </summary>
    public partial class IpView : UserControl
    {
        public IpView()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {

            ClientSocket.ServerIp = ipTextBox.Text;
            try
            {
                ClientSocket.EnsureConnected();
                ClientSocket.ListenServer();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
    }
}
