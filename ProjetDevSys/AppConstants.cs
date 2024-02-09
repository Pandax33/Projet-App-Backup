using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

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
                // Gestion des erreurs de lecture de fichier ou de désérialisation
                Console.WriteLine($"Erreur lors de la lecture de la configuration: {ex.Message}");
                // Initialisation avec des valeurs par défaut ou gestion d'erreur
            }
        }
    }
}
