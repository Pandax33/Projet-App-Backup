using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using ProjetDevSys.Model;
using System.Diagnostics;

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
        
        public static readonly string BlockerProcess;

        static AppConstants()
        {
            // Pass to your JSON path
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySaveGP5");
            string filePath = Path.Combine(easySavePath, "appsettings.json");
            if (!VerifJson(filePath))
            {
                Config.CreateSetting();
            }
            Config.UpdateLogFilePathIfNeeded();

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
                BlockerProcess = config.BlockerProcess.BlockerProcess;
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

        public static bool VerifExist(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        public static bool VerifPath(string path)
        {
            try
            {
                var absolutePath = Path.GetFullPath(path);
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
            CultureInfo ci = new CultureInfo(Langage);
            CultureInfo.CurrentUICulture = ci;

        }

        public static bool RunningBlockerProcess()
        {
            if (BlockerProcess == null) return false;
            return Process.GetProcessesByName(BlockerProcess).Any();
        }

    }
}
