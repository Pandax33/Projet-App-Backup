using System;
using System.IO;
using System.Text.Json;

namespace ProjetDevSys.MODEL
{
    public static class Config
    {
        public static string JsonPath { get; set; } = @"C:\Users\leanb\Pictures\Json\test.json";
        public static string Langage { get; set; } = "en";
        public static string JsonPathRealTime { get; set; } = @"C:\Users\leanb\Pictures\Json\test2.json";
        public static string JsonPathSave { get; set; } = @"C:\Users\leanb\Pictures\Json\test3.json";

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
                    JsonPath,
                    Langage,
                    JsonPathRealTime,
                    JsonPathSave
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
    }
}
