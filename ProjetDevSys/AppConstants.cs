using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using ProjetDevSys.Model;

namespace ProjetDevSys
{
    public static class AppConstants
    {
        public static readonly string LogFilePath;
        public static readonly string Langage;
        public static readonly string LogFilePathRealTime;
        public static readonly string JsonSave;
        public static readonly string ExtensionType;

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

    }
}
