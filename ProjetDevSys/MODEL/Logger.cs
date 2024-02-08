using System;
using System.IO;
using Newtonsoft.Json;
using ProjetDevSys.MODEL;

namespace ProjetDevSys
{
    public class Logger : JsonManager
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public TimeSpan FileTransferTime { get; set; }
        public DateTime Time { get; set; }

        public Logger(string jsonPath, string name, string fileSource, string fileTarget, long fileSize, TimeSpan fileTransferTime) : base(jsonPath)
        {
            Name = name;
            FileSource = fileSource;
            FileTarget = fileTarget;
            FileSize = fileSize;
            FileTransferTime = fileTransferTime;
            Time = DateTime.Now;
        }

        public void CreateLog()
        {
            // Configure Newtonsoft.Json pour formater le JSON de manière lisible
            JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            string logEntry = JsonConvert.SerializeObject(this, settings);

            // Ajoute le log sous forme de nouvelle ligne à la fin du fichier
            using (StreamWriter streamWriter = File.AppendText(JsonPath))
            {
                streamWriter.WriteLine(logEntry);
            }
        }
    }
}
