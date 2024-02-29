using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO.Compression;
using NewtonSoft = Newtonsoft.Json.JsonConvert;
using ProjetDevSys.MODEL;



namespace ProjetDevSys.Model
{
    public static class Config
    {
        public static string JsonPath { get; set; }
        public static string Langage { get; set; }
        public static string JsonPathRealTime { get; set; }
        public static string JsonPathSave { get; set; }
        public static string ExtensionType { get; set; }
        public static List<string> ExtensionListCrypt { get; set; }
        public static string KeyCrypt { get; set; }
        public static string CryptPath { get; set; }
        public static string Theme {  get; set; }
        public static List<string> BlockerProcess { get; set; }
        public static List<string> ExtensionListPriority { get; set; }
        public static long FileSize { get; set; }
        public static bool IsServOn { get; set; }
        public static string FileSizeUnit { get; set; }

        static Config()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySaveFolder = Path.Combine(appDataPath, "EasySaveGP5");
            string cryptoSoftZIP = Path.Combine(easySaveFolder, "CryptoSoftGP5");
            string cryptoSoftPath = Path.Combine(cryptoSoftZIP, "CryptoSoftGP5-main");
            string url = "https://github.com/alexandrethurel/CryptoSoftGP5/archive/refs/heads/main.zip";
            string downloadPath = Path.Combine(easySaveFolder, "CryptoSoftGP5.zip");
        }
        

        private static string GetLanguage()
        {
            return CultureInfo.CurrentUICulture.Name.StartsWith("fr") ? "fr-FR" : "en-US";
        }

        public static void Initialize()
        {
            JsonPath = AppConstants.LogFilePath;
            JsonPathRealTime = AppConstants.LogFilePathRealTime;
            JsonPathSave = AppConstants.JsonSave;
            Langage = AppConstants.Langage;
            ExtensionType = AppConstants.ExtensionType;
            ExtensionListCrypt = AppConstants.ExtensionListCrypt;
            CryptPath = AppConstants.CryptPath;
            KeyCrypt = AppConstants.KeyCrypt;
            Theme = AppConstants.Theme;
            ExtensionListPriority = AppConstants.ExtensionListPriority;
            IsServOn = AppConstants.IsServOn;
            FileSize = AppConstants.FileSize;
            FileSizeUnit = AppConstants.FileSizeUnit;

        }
        public static void InitializeDefault()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySaveGP5");
            string cryptoSoftZIP = Path.Combine(easySavePath, "CryptoSoftGP5");
            string cryptoSoftPath = Path.Combine(cryptoSoftZIP, "CryptoSoftGP5-main");
            JsonPath = Path.Combine(easySavePath, $"Log_{DateTime.Now:yyyyMMdd}");
            JsonPathRealTime = Path.Combine(easySavePath, "LogRealTime");
            Langage = GetLanguage();
            JsonPathSave = Path.Combine(easySavePath, "Backlist.json");
            ExtensionType = ".json";
            ExtensionListCrypt = new List<string> {};
            CryptPath = Path.Combine(cryptoSoftPath, "CryptoSoft.exe");
            KeyCrypt = generateKey();
            BlockerProcess = new List<string> { };
            ExtensionListPriority = new List<string> { };
            IsServOn = false;
            FileSize = 1000000000;
            FileSizeUnit = "Octet";
            Theme = "Default";
        }

        public static dynamic GetDefaultConfig()
        {
            InitializeDefault();
            // Création de l'objet de configuration par défaut
            var defaultConfig = new
            {
                Logging = new { JsonPath },
                Langage = new { Langage },
                RealTimeLogging = new { JsonPathRealTime},
                LoadSave = new { JsonPathSave},
                LogType = new { ExtensionType },
                ExtensionListCrypt = new { ExtensionListCrypt },
                CryptPath = new { CryptPath },
                KeyCrypt = new { KeyCrypt },
                BlockerProcess = new { BlockerProcess },
                ExtensionListPriority = new { ExtensionListPriority },
                IsServOn = new {IsServOn},
                FileSize = new { FileSize },
                FileSizeUnit = new { FileSizeUnit},
                WPF = new { Theme }
            };

            return defaultConfig;
        }

        // Crée le dossier et le fichier de configuration avec les valeurs par défaut si nécessaire
        public static void CreateSetting()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySaveGP5");
            string configFilePath = Path.Combine(easySavePath, "appsettings.json");
            string cryptoSoftZIP = Path.Combine(easySavePath, "CryptoSoftGP5");
            string cryptoSoftPath = Path.Combine(cryptoSoftZIP, "CryptoSoftGP5-main");
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySaveGP5");
            Directory.CreateDirectory(folderPath);

            InitializeDefault();
            CreateFileWithExtensionIfNotExists(JsonPath);
            CreateFileWithExtensionIfNotExists(JsonPathRealTime);
            CreateFileIfNotExists(JsonPathSave);
            if (!Directory.Exists(easySavePath)) Directory.CreateDirectory(easySavePath);
            if (!Directory.Exists(cryptoSoftZIP)) Task.Run(async () => await DownloadCryptoSoftIfNeeded(easySavePath, cryptoSoftZIP)).Wait();

            // Création du fichier de configuration avec les valeurs par défaut si nécessaire
            if (!File.Exists(configFilePath))
            {
                var defaultConfig = GetDefaultConfig();
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
                LogType = new { ExtensionType },
                ExtensionListCrypt = new { ExtensionListCrypt },
                CryptPath = new { CryptPath },
                KeyCrypt = new { KeyCrypt },
                BlockerProcess = new { BlockerProcess },
                ExtensionListPriority = new { ExtensionListPriority },
                IsServOn = new { IsServOn },
                FileSize = new { FileSize },
                FileSizeUnit = new { FileSizeUnit },
                WPF = new { Theme }
            };

            // Sérialisation et écriture dans le fichier
            string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
            AppConstants.reloadConfig();
            if(IsServOn)
            {
                if(AppConstants.serverThread == null)
                {
                    ProjetDevSys.AppConstants.serverThread = new Thread(ProjetDevSys.AppConstants.StartServer) { IsBackground = true };
                    ProjetDevSys.AppConstants.serverThread.Start();
                }
            }
            else
            {
                Server.serverRunning = false;
                if (ProjetDevSys.AppConstants.serverSocket != null)
                {
                    ProjetDevSys.AppConstants.serverSocket.Close();
                }
            }
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
                Dictionary<string, JsonElement> config = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonContent);

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

        public static string generateKey()
        { // Génère un nombre aléatoire de 64 bits
            byte[] randomNumber = new byte[8]; // 64 bits = 8 octets
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            long key = BitConverter.ToInt64(randomNumber, 0);

            string keyString = BitConverter.ToString(randomNumber).Replace("-", string.Empty);

            if (keyString.Length > 16)
            {
                keyString = keyString.Substring(0, 16);
            }
            else if (keyString.Length < 16)
            {
                keyString = keyString.PadRight(16, '0');
            }
            return keyString;
        }

        public static async Task DownloadCryptoSoftIfNeeded(string easySaveFolder, string cryptoSoftPath)
        {
            string url = "https://github.com/alexandrethurel/CryptoSoftGP5/archive/refs/heads/main.zip";
            string downloadPath = Path.Combine(easySaveFolder, "CryptoSoftGP5.zip");

            // Vérifie si le dossier CryptoSoft n'existe pas
            if (!Directory.Exists(cryptoSoftPath))
            {
                Console.WriteLine(ResourceHelper.GetString("ConfigText1"));

                try
                {
                    // Télécharge le fichier
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] fileBytes = await client.GetByteArrayAsync(url);
                        await File.WriteAllBytesAsync(downloadPath, fileBytes);
                    }

                    Console.WriteLine(ResourceHelper.GetString("ConfigText2"));

                    // Extrait le fichier téléchargé
                    string extractPath = Path.Combine(easySaveFolder, "CryptoSoftGP5");
                    ZipFile.ExtractToDirectory(downloadPath, extractPath);

                    Console.WriteLine(ResourceHelper.GetString("ConfigText3"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ResourceHelper.GetString("ConfigText4")} {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine(ResourceHelper.GetString("ConfigText5"));
            }
        }

        public static void ResetSetting()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string easySavePath = Path.Combine(appDataPath, "EasySaveGP5");
            string configFilePath = Path.Combine(easySavePath, "appsettings.json");
            string cryptoSoftZIP = Path.Combine(easySavePath, "CryptoSoftGP5");

            if (!Directory.Exists(easySavePath))
            {
                Directory.CreateDirectory(easySavePath);
            }

            var defaultConfig = GetDefaultConfig();
            string json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFilePath, json);
            AppConstants.reloadConfig();
            
        }

        public static void VerifyAndAddMissingConfigElements(string configFilePath, dynamic defaultConfig)
        {
            // Lire le contenu du fichier de configuration existant ou créer un nouveau dictionnaire si le fichier n'existe pas
            Dictionary<string, JsonElement> config = File.Exists(configFilePath)
                ? JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(File.ReadAllText(configFilePath))
                : new Dictionary<string, JsonElement>();

            // Définir les valeurs par défaut pour chaque élément de configuration nécessaire
            Dictionary<string, object> defaultValues = new Dictionary<string, object>
            {
                {"Logging", new { JsonPath = defaultConfig.Logging.JsonPath }},
                {"Langage", new { Langage = defaultConfig.Langage.Langage }},
                {"RealTimeLogging", new { JsonPathRealTime = defaultConfig.RealTimeLogging.JsonPathRealTime }},
                {"LoadSave", new { JsonPathSave = defaultConfig.LoadSave.JsonPathSave }},
                {"LogType", new { ExtensionType = defaultConfig.LogType.ExtensionType }},
                {"ExtensionListCrypt", new { ExtensionListCrypt = defaultConfig.ExtensionListCrypt.ExtensionListCrypt }},
                {"CryptPath", new { CryptPath = defaultConfig.CryptPath.CryptPath }},
                {"KeyCrypt", new { KeyCrypt = defaultConfig.KeyCrypt.KeyCrypt }},
                {"BlockerProcess", new { BlockerProcess = defaultConfig.BlockerProcess.BlockerProcess }},
                {"IsServOn", new { IsServOn = defaultConfig.IsServOn.IsServOn }},
                {"FileSize", new { FileSize = defaultConfig.FileSize.FileSize }},
                {"FileSizeUnit", new { FileSizeUnit = defaultConfig.FileSizeUnit.FileSizeUnit }},
                {"ExtensionListPriority", new { ExtensionListPriority = defaultConfig.ExtensionListPriority.ExtensionListPriority }}

            };

            // Parcourir chaque élément par défaut pour s'assurer qu'il est présent dans la configuration; sinon, l'ajouter
            bool isUpdated = false;
            foreach (var item in defaultValues)
            {
                if (!config.ContainsKey(item.Key) || config[item.Key].ValueKind == JsonValueKind.Undefined)
                {
                    // Convertit l'objet de valeur par défaut en JsonElement
                    JsonElement defaultValueElement = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(item.Value));
                    config[item.Key] = defaultValueElement;
                    isUpdated = true;
                }
            }

            // Si des mises à jour ont été effectuées, écrire le JSON mis à jour dans le fichier de configuration
            if (isUpdated)
            {
                string updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFilePath, updatedJson);
            }
        }


    }
}
