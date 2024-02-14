using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProjetDevSys.Model
{
    public static class Config
    {
        public static string JsonPath { get; set; }
        public static string Langage { get; set; }
        public static string JsonPathRealTime { get; set; }
        public static string JsonPathSave { get; set; }
        public static string ExtensionType { get; set; }

        static Config()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySaveFolder = Path.Combine(appDataPath, "EasySaveGP5");

            // Assurez-vous que le dossier EasySave existe
            if (!Directory.Exists(easySaveFolder))
            {
                Directory.CreateDirectory(easySaveFolder);
            }

            JsonPath = AppConstants.LogFilePath ?? Path.Combine(easySaveFolder, $"Log_{DateTime.Now:yyyyMMdd}");
            JsonPathRealTime = AppConstants.LogFilePathRealTime ?? Path.Combine(easySaveFolder, "LogRealTime");
            JsonPathSave = AppConstants.JsonSave ?? Path.Combine(easySaveFolder, "Backlist.json");
            Langage = AppConstants.Langage ?? GetLanguage();
            ExtensionType = AppConstants.ExtensionType ?? ".json";

            CreateFileWithExtensionIfNotExists(JsonPath);
            CreateFileWithExtensionIfNotExists(JsonPathRealTime);
            CreateFileIfNotExists(JsonPathSave);
        }
        

        private static string GetLanguage()
        {
            return CultureInfo.CurrentUICulture.Name.StartsWith("fr") ? "fr-FR" : "en-US";
        }

        // Crée le dossier et le fichier de configuration avec les valeurs par défaut si nécessaire
        public static void CreateSetting()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySaveGP5");
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
                        Langage = CultureInfo.CurrentUICulture.Name.StartsWith("fr") ? "fr-FR" : "en-US"
                    },
                    RealTimeLogging = new
                    {
                        JsonPathRealTime
                    },
                    LoadSave = new
                    {
                        JsonPathSave
                    },
                    LogType = new
                    {
                        ExtensionType = ".json"
                    }
                };
                string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFilePath, json);
            }
        }


        public static void EditConfig()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySaveGP5");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "appsettings.json");

            // Création de l'objet anonyme reflétant la structure du fichier JSON
            var settings = new
            {
                Logging = new { JsonPath },
                Langage = new { Langage },
                RealTimeLogging = new { JsonPathRealTime },
                LoadSave = new { JsonPathSave },
                LogType = new { ExtensionType }
            };

            // Sérialisation et écriture dans le fichier
            string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
            AppConstants.reloadConfig();
            CreateFileWithExtensionIfNotExists(JsonPath);
            CreateFileWithExtensionIfNotExists(JsonPathRealTime);
        }

        private static void CreateFileIfNotExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
        }

        private static void CreateFileWithExtensionIfNotExists(string filePath)
        {
            string fullFilePath = filePath + ExtensionType;

            if (!File.Exists(fullFilePath))
            {
                File.Create(fullFilePath).Dispose();
            }
        }

        public static void UpdateLogFilePathIfNeeded()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySaveFolder = Path.Combine(appDataPath, "EasySaveGP5");
            string configFilePath = Path.Combine(easySaveFolder, "appsettings.json");

            try
            {
                string jsonContent = File.ReadAllText(configFilePath);
                var config = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);

                if (config != null && config.ContainsKey("Logging") && config["Logging"].TryGetProperty("JsonPath", out JsonElement jsonPathElement))
                {
                    string currentLogFilePath = jsonPathElement.GetString();
                    string currentLogFileName = Path.GetFileNameWithoutExtension(currentLogFilePath);

                    Regex regex = new Regex(@"^Log_\d{8}$");
                    if (regex.IsMatch(currentLogFileName))
                    {
                        string dateString = currentLogFileName.Replace("Log_", "");

                        if (DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fileDate))
                        {
                            if (fileDate.Date < DateTime.Today)
                            {
                                string newFileName = $"Log_{DateTime.Today:yyyyMMdd}.json";
                                string newFilePath = Path.Combine(Path.GetDirectoryName(currentLogFilePath), newFileName).Replace("\\", "\\\\");

                                var loggingConfig = new
                                {
                                    JsonPath = newFilePath
                                };

                                // Mettre à jour la configuration avec la nouvelle structure de Logging
                                config["Logging"] = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(loggingConfig));

                                // Sérialiser l'objet modifié en JSON
                                string updatedJsonContent = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

                                // Écrire le JSON mis à jour dans le fichier de configuration
                                File.WriteAllText(configFilePath, updatedJsonContent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
