using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientProjetDevSysGraphical
{
    public static class AppConstants
    {
        // Définissez les champs nécessaires ici

        private static Socket serverSocket;

        static AppConstants()
        {
            // Initialisez la connexion au serveur
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Connect("127.0.0.1", 1234); // Remplacez par l'adresse et le port de votre serveur
        }

        public static async Task<string> GetLogFilePathFromServerAsync()
        {
            try
            {
                // Envoie une demande au serveur
                await SendRequestAsync("GetLogFilePath");

                // Attend la réponse du serveur
                string response = await ReceiveResponseAsync();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération du chemin du fichier de journal à partir du serveur : " + ex.Message);
                return null;
            }
        }

        public static async Task SendRequestAsync(string request)
        {
            byte[] requestData = Encoding.UTF8.GetBytes(request);
            await serverSocket.SendAsync(new ArraySegment<byte>(requestData), SocketFlags.None);
        }

        public static async Task<string> ReceiveResponseAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await serverSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }
}
