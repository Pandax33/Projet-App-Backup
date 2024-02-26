using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ProjetDevSysGraphical
{
    public class Server
    {
        private Socket serverSocket;
        private List<Socket> clientSockets = new List<Socket>();
        private bool isServerRunning = false;

        public Server()
        {
            
        }

        public async Task Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1234));
            serverSocket.Listen(10); // Permettre jusqu'à 10 connexions en attente
            isServerRunning = true;

            try
            {
                Socket clientSocket = serverSocket.Accept(); // Accepter la connexion entrante
                clientSockets.Add(clientSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion avec le client : " + ex.Message);
            }
            finally
            {
                Stop();
            }
        }

        public void Stop()
        {
            foreach (var clientSocket in clientSockets)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

            clientSockets.Clear();

            if (serverSocket != null)
            {
                serverSocket.Close();
                isServerRunning = false;
            }
        }
    }
}
