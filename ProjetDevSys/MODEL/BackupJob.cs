using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.Model
{
    internal class BackupJob
    {
        public DateTime DateDebut { get; set; }
        public int NbFichier { get; set; }
        public string FichierActuel { get; set; }
        public DateTime DateFin { get; set; }
        public string State { get; set; }
        public Backup Backup { get; set; }
        private IBackupStrategy _strategy;
        public LogRealTime LogRealTime;

        public BackupJob(Backup BackupObj)
        {
            Backup = BackupObj;
            LogRealTime = new LogRealTime(AppConstants.LogFilePathRealTime)
            {
                BackupName = Backup.Name
            };
            string type = Backup.Type;
            // Determine the strategy to use
            switch (type)
            {
                case "A":
                    _strategy = new SaveCompleteStrategy();
                    break;
                case "B":
                    _strategy = new SaveDiffStrategy();
                    break;
                default:
                    throw new ArgumentException(ResourceHelper.GetString("BackupJob1"));
            }
        }

        public void Save()
        {
            LogRealTime.CalculateFolderSizeAndFileCount(Backup.Source);
            DateTime TimeDebut = DateTime.Now;
            _strategy.Save(Backup,LogRealTime);
            DateTime TimeFin = DateTime.Now;
            TimeSpan duration = TimeFin - TimeDebut;
            long size;
            if (!(Directory.Exists(Backup.Source)) && File.Exists(Backup.Source)) size = new FileInfo(Backup.Source).Length;
            else
            {
                DirectoryInfo sourceInfoBackup = new DirectoryInfo(Backup.Source);
                size = CalculateFolder(sourceInfoBackup);
            }

            long CalculateFolder(DirectoryInfo directory)
            {
                long TotalSize = 0;
                try
                {
                    // Count the number of files and calculate the total size
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        TotalSize += file.Length;
                    }

                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        CalculateFolder(dir);
                    }
                    return TotalSize;
                }
                catch (System.Exception ex)
                {
                    // Manage the exception if the directory cannot be accessed
                    return 0;
                }
            }

            Logger Log = new Logger(AppConstants.LogFilePath, Backup.Name, Backup.Source, Backup.Destination, size, duration);
            Log.CreateLog();
        }
    }
}
