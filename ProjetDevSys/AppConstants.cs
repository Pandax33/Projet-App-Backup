using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using ProjetDevSys.Model;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using ProjetDevSys.MODEL;

namespace ProjetDevSys
{
    public static class AppConstants
    {
        public static string LogFilePath;
        public static string Langage;
        public static string LogFilePathRealTime;
        public static string JsonSave;
        public static string ExtensionType;
        public static List<string> ExtensionListCrypt;
        public static string CryptPath;
        public static string KeyCrypt;
        public static List<string> ExtensionListPriority;
        public static bool IsServOn;
        public static long FileSize;
        public static string FileSizeUnit;
        public static Thread serverThread;
        public static Socket serverSocket;
        public static ConcurrentDictionary<string, double> backupProgress = new ConcurrentDictionary<string, double>();
        public static ConcurrentDictionary<string, ManualResetEvent> BackupPauseHandles = new ConcurrentDictionary<string, ManualResetEvent>();
        public static ConcurrentDictionary<string, CancellationTokenSource> BackupCancellations = new ConcurrentDictionary<string, CancellationTokenSource>();
        public static ConcurrentDictionary<string, string> backupState = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> EventState = new ConcurrentDictionary<string, string>();
        public static List<string> BlockerProcess;
        public static readonly Mutex appMutex = new Mutex(true, "AppConstantsMutex");
        public static string Theme;
        public delegate void BackupProgressUpdatedEventHandler(string backupName, double progress);
        public static event BackupProgressUpdatedEventHandler BackupProgressUpdated;
        public static readonly Mutex sizeMutex = new Mutex();
        public static ManualResetEvent processEvent = new ManualResetEvent(true);
        public static ManualResetEvent priorityEvent = new ManualResetEvent(true);

        static AppConstants()
        {
            EventState.TryAdd("sizeMutex", "Libre");
            EventState.TryAdd("processEvent", "Libre");
            EventState.TryAdd("priorityEvent", "Libre");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySaveGP5");
            string filePath = Path.Combine(easySavePath, "appsettings.json");
            if (!VerifJson(filePath))
            {
                Config.CreateSetting();
            }
            Config.UpdateLogFilePathIfNeeded();
            Config.VerifyAndAddMissingConfigElements(filePath,Config.GetDefaultConfig());
            
            try
            {
                // Read the file and deserialize the JSON to a dynamic type
                string json = File.ReadAllText(filePath);

                // Deserialize the JSON to a dynamic type
                dynamic config = JsonConvert.DeserializeObject(json);

                // Assign the values to the static fields
                LogFilePath = config.Logging.JsonPath;
                Langage = config.Langage.Langage;
                LogFilePathRealTime = config.RealTimeLogging.JsonPathRealTime;
                JsonSave = config.LoadSave.JsonPathSave;
                ExtensionType = config.LogType.ExtensionType;
                ExtensionListCrypt = new List<string>(config.ExtensionListCrypt.ExtensionListCrypt.ToObject<List<string>>());
                CryptPath = config.CryptPath.CryptPath;
                KeyCrypt = config.KeyCrypt.KeyCrypt;
                ExtensionListPriority = new List<string>(config.ExtensionListPriority.ExtensionListPriority.ToObject<List<string>>());
                Theme = config.WPF.Theme;
                BlockerProcess = new List<string>(config.BlockerProcess.BlockerProcess.ToObject<List<string>>());
                IsServOn = config.IsServOn.IsServOn;
                FileSize = config.FileSize.FileSize;
                FileSizeUnit = config.FileSizeUnit.FileSizeUnit;
                Config.Initialize();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ResourceHelper.GetString("AppConstants1"));
            }
        }

        public static bool VerifJson(string path)
        {
            return File.Exists(path);
        }
        public static void UpdateBackupProgress(string name, double progress)
        {
            backupProgress[name] = progress;
            BackupProgressUpdated?.Invoke(name, progress);
        }
        public static bool VerifExist(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        public static bool VerifPath(string path)
        {
            try
            {
                string absolutePath = Path.GetFullPath(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static int StringToInt(string id)
        {
            bool isSuccess = int.TryParse(id, out int numericId);
            if (isSuccess)
            {
                return numericId;
            }
            return -1;
        }

        public static void reloadConfig()
        {
            string appsettings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySaveGP5", "appsettings.json");
            string json = File.ReadAllText(appsettings);

            // Deserialize the JSON to a dynamic type
            dynamic config = JsonConvert.DeserializeObject(json);

            // Assign the values to the static fields
            LogFilePath = config.Logging.JsonPath;
            Langage = config.Langage.Langage;
            LogFilePathRealTime = config.RealTimeLogging.JsonPathRealTime;
            JsonSave = config.LoadSave.JsonPathSave;
            ExtensionType = config.LogType.ExtensionType;
            ExtensionListCrypt = new List<string>(config.ExtensionListCrypt.ExtensionListCrypt.ToObject<List<string>>());
            ExtensionListPriority = new List<string>(config.ExtensionListPriority.ExtensionListPriority.ToObject<List<string>>());
            CryptPath = config.CryptPath.CryptPath;
            KeyCrypt = config.KeyCrypt.KeyCrypt;
            BlockerProcess = new List<string>(config.BlockerProcess.BlockerProcess.ToObject<List<string>>());
            Theme = config.WPF.Theme;
            IsServOn = config.IsServOn.IsServOn;
            FileSize = config.FileSize.FileSize;
            FileSizeUnit = config.FileSizeUnit.FileSizeUnit;
            CultureInfo ci = new CultureInfo(Langage);
            CultureInfo.CurrentUICulture = ci;
        }

        public static bool RunningBlockerProcess()
        {
            if (BlockerProcess == null || !BlockerProcess.Any()) return false;

            foreach (string processName in BlockerProcess)
            {
                if (Process.GetProcessesByName(processName).Any())
                {
                    return true;
                }
            }

            return false;
        }
        public static void PauseBackup(string backupName)
        {
            if (AppConstants.BackupPauseHandles.TryGetValue(backupName, out ManualResetEvent handle))
            {
                handle.Reset(); // Met en pause
                backupState.TryAdd(backupName, "Pause");
            }
        }

        public static void ResumeBackup(string backupName)
        {
            if (AppConstants.BackupPauseHandles.TryGetValue(backupName, out ManualResetEvent handle))
            {
                handle.Set(); // Reprend l'exécution
                backupState.TryAdd(backupName, "In Progress");
            }
        }

        public static void StopBackup(string backupName)
        {
            if (BackupCancellations.TryGetValue(backupName, out CancellationTokenSource cts))
            {
                cts.Cancel(); // Envoie une demande d'annulation à la tâche
                backupState.TryAdd(backupName, "Stop");
            }
      
        }

        public static void StartServer()
        {
            try
            {
                Socket serverSocket = Server.SeConnecter();
                Server.serverRunning = true;
                while (Server.serverRunning)
                {
                    Socket clientSocket = Server.AccepterConnexion(serverSocket);
                    Server.clients.Add(clientSocket);

                    // Gérer chaque client dans un thread séparé
                    Thread clientThread = new Thread(() => Server.GestionClient(clientSocket));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du démarrage du serveur: {ex.Message}");
            }
        }


    }
}
