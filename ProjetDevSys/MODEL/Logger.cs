using System.Text.Json;
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

        // La fonction CreateLog utilise Serialize pour écrire ses propres propriétés dans un fichier JSON.
        public void CreateLog()
        {
            var logEntry = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

            // Ajoute le log sous forme de nouvelle ligne à la fin du fichier
            using (var streamWriter = File.AppendText(JsonPath))
            {
                streamWriter.WriteLine(logEntry);
            }
        }
    }
}