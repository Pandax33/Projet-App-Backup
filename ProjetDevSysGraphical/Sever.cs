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
        private MainWindow mainWindow; // Référence à votre fenêtre principale

        public Server(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1234));
            serverSocket.Listen(10); // Permettre jusqu'à 10 connexions en attente
            isServerRunning = true;

            try
            {
                while (isServerRunning)
                {
                    Socket clientSocket = serverSocket.Accept(); // Accepter la connexion entrante
                    clientSockets.Add(clientSocket);

                    Thread clientThread = new Thread(() => HandleClient(clientSocket));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception
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

        private void HandleClient(Socket clientSocket)
        {
            try
            {
                while (isServerRunning)
                {
                    // Récupérer le rendu de la fenêtre principale sous forme d'image
                    BitmapSource image = CaptureMainWindowRender();

                    // Convertir l'image en tableau de bytes
                    byte[] imageData = ImageToByteArray(image);

                    // Envoyer l'image au client
                    clientSocket.Send(imageData);

                    Thread.Sleep(1000); // Attendre 1 seconde avant d'envoyer la prochaine mise à jour
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSockets.Remove(clientSocket);
            }
        }

        private BitmapSource CaptureMainWindowRender()
        {
            // Créer un RenderTargetBitmap
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)mainWindow.ActualWidth, (int)mainWindow.ActualHeight, 96, 96, PixelFormats.Default);
            renderBitmap.Render(mainWindow);

            return renderBitmap;
        }

        private byte[] ImageToByteArray(BitmapSource image)
        {
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
            return stream.ToArray();
        }
    }
}
