using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjetDevSysGraphical;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using ProjetDevSys.Model;
using System.Diagnostics;
namespace EasySave_Client
{
    public static class ClientSocket
    {
        public static string ServerIp;
        private static readonly int ServerPort = 1324;
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public static ManualResetEvent PauseListen = new ManualResetEvent(true);
        public static bool isRunning;

        public static void EnsureConnected()
        {
            if (!_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Connect(ServerIp, ServerPort);
                    MessageBox.Show(ResourceHelper.GetString("Client.Connection"));
                    ClientSocket.GetBackup();
                    isRunning = true;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ResourceHelper.GetString("Client.ConnectionFailed") + ex.Message, ex);
                }
            }
        }

        public static void SendCommand(string command)
        {
            if (isRunning)
            {
                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(command);
                    _clientSocket.Send(buffer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ResourceHelper.GetString("Client.FailSend") + ex.Message);
                }
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
                    MessageBox.Show(ResourceHelper.GetString("Client.Close"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ResourceHelper.GetString("Client.FailClose") + ex.Message);
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
            string responseJson = SendAndReceiveCommand("get_backup");
            Dictionary<string, Backup> receivedBackups = JsonConvert.DeserializeObject<Dictionary<string, Backup>>(responseJson);

            AppConstants.backups = receivedBackups;

        }

        public static void GetBackupProgress()
        {

            string responseJson = SendAndReceiveCommand("get_backup_progress");
            Dictionary<string, double> progressDict = JsonConvert.DeserializeObject<Dictionary<string, double>>(responseJson);
            Dictionary<string, double> progress = new Dictionary<string, double>(progressDict);
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
            PauseListen.Reset();
            string response = SendAndReceiveCommand($"pause_{backupName}");
            PauseListen.Set();
        }

        public static void ResumeBackup(string backupName)
        {
            PauseListen.Reset();
            string response = SendAndReceiveCommand($"resume_{backupName}");
            PauseListen.Set();
        }

        public static void StopBackup(string backupName)
        {
            PauseListen.Reset();
            string response = SendAndReceiveCommand($"stop_{backupName}");
            PauseListen.Set();
        }

        public static async void ListenServer()
        {
            
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                PauseListen.WaitOne();
                ClientSocket.GetBackupProgress();
                Accueil.CurrentInstance?.GenerateGrid();

                await Task.Delay(1000, cancellationTokenSource.Token);

            }
        }

        public static void LaunchBackup(int[] backupIds)
        {
            string command = "launchBackup";
            foreach (var id in backupIds)
            {
                command += "_" + id; 
            }
            string response = SendAndReceiveCommand(command);
        }
    }
}