using System;
using System.IO;
using System.Text.Json;

namespace ProjetDevSys.Model
{
    public static class Config
    {
        public static string JsonPath { get; set; }
        public static string Langage { get; set; }
        public static string JsonPathRealTime { get; set; }
        public static string JsonPathSave { get; set; }

        static Config()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySaveFolder = Path.Combine(appDataPath, "EasySave");

            // Assurez-vous que le dossier EasySave existe
            if (!Directory.Exists(easySaveFolder))
            {
                Directory.CreateDirectory(easySaveFolder);
            }

            // Construire les chemins avec les noms spécifiques
            JsonPath = Path.Combine(easySaveFolder, $"Log_{DateTime.Now:yyyyMMdd}.json");
            JsonPathRealTime = Path.Combine(easySaveFolder, "LogRealTime.json");
            JsonPathSave = Path.Combine(easySaveFolder, "Backlist.json");
            Langage = AppConstants.Langage;
            CreateFileIfNotExists(JsonPath);
            CreateFileIfNotExists(JsonPathRealTime);
            CreateFileIfNotExists(JsonPathSave);
        }

        // La méthode pour initialiser ou mettre à jour les propriétés si nécessaire
        public static void Initialize(string jsonPath, string langage, string jsonPathRealTime, string jsonPathSave)
        {
            JsonPath = jsonPath;
            Langage = langage;
            JsonPathRealTime = jsonPathRealTime;
            JsonPathSave = jsonPathSave;
        }

        // Crée le dossier et le fichier de configuration avec les valeurs par défaut si nécessaire
        public static void CreateSetting()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySave");
            string configFilePath = Path.Combine(easySavePath, "appsettings.json");

            if (!Directory.Exists(easySavePath))
            {
                Directory.CreateDirectory(easySavePath);
            }

            if (!File.Exists(configFilePath))
            {
                var defaultConfig = new
                {
                    Logging = new
                    {
                        JsonPath
                    },
                    Langage = new
                    {
                        Langage 
                    },
                    RealTimeLogging = new
                    {
                        JsonPathRealTime
                    },
                    LoadSave = new
                    {
                        JsonPathSave
                    }
                };
                string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFilePath, json);
            }
        }


        public static void EditConfig()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "appsettings.json");

            // Création de l'objet anonyme reflétant la structure du fichier JSON
            var settings = new
            {
                Logging = new { JsonPath },
                Langage = new { Langage },
                RealTimeLogging = new { JsonPathRealTime },
                LoadSave = new { JsonPathSave }
            };

            // Sérialisation et écriture dans le fichier
            string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }

        private static void CreateFileIfNotExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // Créer un fichier vide
                File.Create(filePath).Dispose();
            }
        }
    }
}
