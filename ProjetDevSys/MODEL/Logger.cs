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

        private static readonly object _lock = new object();
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
            lock (_lock)
            {
                if (AppConstants.ExtensionType == ".json")
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                    string logEntry = JsonConvert.SerializeObject(this, settings);

                    // Add the log entry to the JSON file
                    string completeFilePath = JsonPath + AppConstants.ExtensionType;

                    using (StreamWriter streamWriter = File.AppendText(completeFilePath))
                    {
                        streamWriter.WriteLine(logEntry);
                    }
                }
                else if (AppConstants.ExtensionType == ".xml")
                {
                    string completeFilePath = JsonPath + AppConstants.ExtensionType;

                    using (StreamWriter streamWriter = File.AppendText(completeFilePath))
                    {
                        streamWriter.WriteLine("<LogEntry>");
                        streamWriter.WriteLine("  <Name>" + Name + "</Name>");
                        streamWriter.WriteLine("  <FileSource>" + FileSource + "</FileSource>");
                        streamWriter.WriteLine("  <FileTarget>" + FileTarget + "</FileTarget>");
                        streamWriter.WriteLine("  <FileSize>" + FileSize + "</FileSize>");
                        streamWriter.WriteLine("  <FileTransferTime>" + FileTransferTime + "</FileTransferTime>");
                        streamWriter.WriteLine("  <Time>" + Time + "</Time>");
                        streamWriter.WriteLine("</LogEntry>");
                    }
                }
            }
        }
    }
}
