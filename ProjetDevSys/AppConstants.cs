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

        static AppConstants()
        {
            // Chemin vers votre fichier appsettings.json
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySave");
            string filePath = Path.Combine(easySavePath, "appsettings.json");
            Config.CreateSetting();
            try
            {
                // Lecture du fichier JSON en tant que string
                string json = File.ReadAllText(filePath);

                // Désérialisation du JSON en un objet dynamique ou dans une structure fortement typée
                dynamic config = JsonConvert.DeserializeObject(json);

                // Assignation des valeurs. Ajustez les chemins d'accès selon votre structure JSON.
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
    }
}
