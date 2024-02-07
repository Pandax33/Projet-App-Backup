using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjetDevSys;

namespace ProjetDevSys.MODEL
{
    public class LogRealTime : JsonManager
    {
        public string BackupName { get; set; }
        public DateTime Timestamp { get; set; }
        public string State { get; set; }
        public int TotalFiles { get; set; }
        public long TotalSize { get; set; }
        public double Progress { get; set; }
        public int FilesRemaining { get; set; }
        public long SizeRemaining { get; set; }
        public string CurrentSourcePath { get; set; }
        public string CurrentTargetPath { get; set; }
        public int CurrentFile {  get; set; }
        public long CurrentFileSize { get; set; }

        public LogRealTime(string jsonPath) : base(jsonPath)
        {
            // Initialisation avec des valeurs par défaut ou chargement à partir du JSON
            BackupName = "";
            Timestamp = DateTime.Now;
            State = "In Progress";
            TotalFiles = 0;
            TotalSize = 0;
            Progress = 0.0;
            FilesRemaining = 0;
            SizeRemaining = 0;
            CurrentSourcePath = "";
            CurrentTargetPath = "";
            CurrentFile = 0;
            CurrentFileSize = 0;
        }

        public void UpdateCurrentFileAndSize(long fileSize)
        {
            CurrentFile = CurrentFile +1;
            CurrentFileSize = fileSize;
            SizeRemaining = SizeRemaining - CurrentFileSize;
            Progress = Progress + (CurrentFileSize * 100) / TotalSize;
            FilesRemaining = TotalFiles - CurrentFile;
        }
        public void CreateLog()
        {
            // Configure Newtonsoft.Json pour formater le JSON de manière lisible
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var logEntry = JsonConvert.SerializeObject(this, settings);

            // Ajoute le log sous forme de nouvelle ligne à la fin du fichier
            using (var streamWriter = File.AppendText(JsonPath))
            {
                streamWriter.WriteLine(logEntry);
            }
        }

        public void CalculateFolderSizeAndFileCount(string folderPath)
        {
            // Réinitialiser les compteurs
            TotalFiles = 0;
            TotalSize = 0;

            // Créer une instance de DirectoryInfo
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);

            // Appeler la fonction récursive
            CalculateFolder(dirInfo);

            // Fonction récursive pour calculer la taille et le nombre de fichiers
            void CalculateFolder(DirectoryInfo directory)
            {
                try
                {
                    // Compter tous les fichiers du dossier et additionner leur taille
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        TotalFiles++;
                        TotalSize += file.Length;
                    }

                    // Appel récursif pour tous les sous-dossiers
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        CalculateFolder(dir);
                    }
                }
                catch (System.Exception ex)
                {
                    // Gérer les exceptions, par exemple, accès refusé
                    System.Console.WriteLine($"Cannot access {directory.FullName}: {ex.Message}");
                }
            }

            SizeRemaining = TotalSize;
        }
    } 



}
