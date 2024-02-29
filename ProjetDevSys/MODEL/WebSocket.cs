using Newtonsoft.Json;
using ProjetDevSys.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys.VueModel;

namespace ProjetDevSys.MODEL
{
    public static class Server
    {
        public static readonly ConcurrentBag<Socket> clients = new ConcurrentBag<Socket>();
        public static bool serverRunning = false;
        public static Socket SeConnecter()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 1324)); // Change le port en fonctions des besoins
            serverSocket.Listen(10); // Limite de connexions en attente
            return serverSocket;
        }
        public static Socket AccepterConnexion(Socket serverSocket)
        {
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine($"Client connecté Adresse IP: {((IPEndPoint)clientSocket.RemoteEndPoint).Address}, Port: {((IPEndPoint)clientSocket.RemoteEndPoint).Port}");
            return clientSocket;
        }
        public static void EcouterReseau(Socket clientSocket)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int received = clientSocket.Receive(buffer);
                    if (received == 0) break; // Le client s'est déconnecté proprement

                    byte[] data = new byte[received];
                    Array.Copy(buffer, data, received);
                    string text = Encoding.ASCII.GetString(data);
                    Console.WriteLine($"Message reçu: {text}");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Une exception réseau s'est produite: {e.Message}");
            }
            finally
            {
                Deconnecter(clientSocket);
            }
        }

        public static void Deconnecter(Socket socket)
        {
            socket.Close();
        }

        public static void GestionClient(Socket clientSocket)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int received = clientSocket.Receive(buffer);
                    if (received == 0) break; // Le client s'est déconnecté

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    string[] parts = message.Split('_');
                    if (message == "get_backup")
                    {
                        string backupsJson = SerializeBackups();
                        byte[] dataToSend = Encoding.UTF8.GetBytes(backupsJson);
                        clientSocket.Send(dataToSend);
                        continue; // Passe au prochain cycle de la boucle
                    }
                    if (message == "get_backup_progress")
                    {
                        string progressJson = SerializeBackupProgress();
                        byte[] dataToSend = Encoding.UTF8.GetBytes(progressJson);
                        clientSocket.Send(dataToSend);
                        continue; // Continue à écouter pour plus de commandes
                    }
                    if (message == "get_backup_state")
                    {
                        string stateJson = SerializeBackupState();
                        byte[] dataToSend = Encoding.UTF8.GetBytes(stateJson);
                        clientSocket.Send(dataToSend);
                        continue;
                    }
                    else if (message == "get_event_state")
                    {
                        string eventJson = SerializeEventState();
                        byte[] dataToSend = Encoding.UTF8.GetBytes(eventJson);
                        clientSocket.Send(dataToSend);
                        continue;
                    }
                    if (parts.Length > 1) // Vérifie si le message contient plus d'une partie
                    {
                        string command = parts[0];
                        string backupName = parts[1];
                        switch (command.ToLower()) // Utilisez ToLower pour ignorer la casse
                        {
                            case "pause":
                                ProjetDevSys.AppConstants.PauseBackup(backupName);
                                clientSocket.Send(Encoding.UTF8.GetBytes("Backup paused"));
                                break;
                            case "resume":
                                ProjetDevSys.AppConstants.ResumeBackup(backupName);
                                clientSocket.Send(Encoding.UTF8.GetBytes("Backup resumed"));
                                break;
                            case "stop":
                                ProjetDevSys.AppConstants.StopBackup(backupName);
                                clientSocket.Send(Encoding.UTF8.GetBytes("Backup stopped"));
                                break;
                        }
                        BroadcasterMessage(message, clientSocket);
                    }
                    if (parts[0].ToLower() == "launchbackup" && parts.Length > 1)
                    {
                        int[] backupIds = parts.Skip(1).Select(id =>
                        {
                            int.TryParse(id, out int parsedId);
                            return parsedId;
                        }).ToArray();

                        BackupManager.AddBackupToQueue(backupIds);

                        string confirmation = "Backups added to queue";
                        clientSocket.Send(Encoding.UTF8.GetBytes(confirmation));
                        continue; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                clientSocket.Close();
                clients.TryTake(out var _);
            }
        }

        private static void BroadcasterMessage(string message, Socket senderSocket)
        {
            foreach (var client in clients)
            {
                if (client != senderSocket)
                {
                    try
                    {
                        client.Send(Encoding.UTF8.GetBytes(message));
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static string SerializeBackups()
        {
            // Assurez-vous que la classe Backup et ses membres sont sérialisables.
            return JsonConvert.SerializeObject(BackupFactory._backups);
        }

        private static string SerializeBackupProgress()
        {
            // Utilisation de Newtonsoft.Json pour la sérialisation
            return JsonConvert.SerializeObject(ProjetDevSys.AppConstants.backupProgress);
        }
        private static string SerializeBackupState()
        {
            return JsonConvert.SerializeObject(ProjetDevSys.AppConstants.backupState);
        }

        private static string SerializeEventState()
        {
            return JsonConvert.SerializeObject(ProjetDevSys.AppConstants.EventState);
        }
    }
}
