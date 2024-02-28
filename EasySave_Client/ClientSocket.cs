using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjetDevSysGraphical;
using System.Text.Json;
using System.Collections.Concurrent;
namespace EasySave_Client
{
    public static class ClientSocket
    {
        private static readonly string ServerIp = "10.176.128.236"; // Exemple d'adresse IP du serveur
        private static readonly int ServerPort = 1324; // Exemple de port
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Assurez-vous que la connexion est établie avant d'envoyer des commandes
        public static void EnsureConnected()
        {
            if (!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(ServerIp, ServerPort);
                    MessageBox.Show("Connecté au serveur.");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Impossible de se connecter au serveur : {ex.Message}", ex);
                }
            }
        }

        public static void SendCommand(string command)
        {
            EnsureConnected(); // Assurez-vous d'être connecté avant d'envoyer une commande
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(command);
                _clientSocket.Send(buffer);
                MessageBox.Show($"Commande envoyée : {command}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'envoi de la commande : {ex.Message}");
                // Tentative de reconnexion ou gestion de l'erreur
            }
        }

        public static string ReceiveResponse()
        {
            try
            {
                byte[] buffer = new byte[2048];
                int received = _clientSocket.Receive(buffer);
                if (received == 0) return null;

                string response = Encoding.UTF8.GetString(buffer, 0, received);
                MessageBox.Show($"Réponse reçue du serveur :\n{response}");
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la réception de la réponse : {ex.Message}");
                return null;
            }
        }

        public static void CloseConnection()
        {
            if (_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
                    _clientSocket.Close();
                    MessageBox.Show("Connexion fermée.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la fermeture de la connexion : {ex.Message}");
                }
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Préparer pour une nouvelle connexion
            }
        }

        public static string SendAndReceiveCommand(string command)
        {
            _clientSocket.Send(Encoding.UTF8.GetBytes(command));

            byte[] buffer = new byte[4096];
            int received = _clientSocket.Receive(buffer);
            string response = Encoding.UTF8.GetString(buffer, 0, received);
            return response;
        }

        public static void GetBackup()
        {
            SendCommand("get_backup");
        }

        public static void GetBackupProgress()
        {

            string responseJson = SendAndReceiveCommand("get_backup_progress");
            var progressDict = JsonSerializer.Deserialize<Dictionary<string, double>>(responseJson);
            var progress = new ConcurrentDictionary<string, double>(progressDict);
            if (progress != null)
            {
                // Mise à jour de backupProgress dans AppConstants
                foreach (var entry in progress)
                {
                    AppConstants.backupProgress.AddOrUpdate(entry.Key, entry.Value, (key, oldValue) => entry.Value);
                }
            }
        }

        public static void GetBackupState()
        {
            SendCommand("get_backup_state");
        }

        public static void GetEventState()
        {
            SendCommand("get_event_state");
        }

        public static void PauseBackup(string backupName)
        {
            SendCommand($"pause_{backupName}");
        }

        public static void ResumeBackup(string backupName)
        {
            SendCommand($"resume_{backupName}");
        }

        public static void StopBackup(string backupName)
        {
            SendCommand($"stop_{backupName}");
        }
    }
}