using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
                while (isServerRunning)
                {
                    Socket clientSocket = await serverSocket.AcceptAsync(); // Accepter la connexion entrante de manière asynchrone
                    clientSockets.Add(clientSocket);

                    Task.Run(() => HandleClient(clientSocket)); // Démarrer un nouveau thread pour gérer le client
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

        private async Task HandleClient(Socket clientSocket)
        {
            try
            {
                while (isServerRunning)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Traiter la demande du client et renvoyer la réponse
                    string response = await ProcessRequestAsync(request);

                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    await clientSocket.SendAsync(new ArraySegment<byte>(responseData), SocketFlags.None);
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

        private async Task<string> ProcessRequestAsync(string request)
        {
            if (request == "GetLogFilePath")
            {
                return ProjetDevSys.AppConstants.LogFilePath;
            }

            // Si la demande n'est pas reconnue, renvoyez une réponse indiquant une demande non valide
            return "InvalidRequest";
        }
    }
}
