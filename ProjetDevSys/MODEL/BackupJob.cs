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
            // Déterminer la stratégie en fonction du type
            switch (type)
            {
                case "A":
                    _strategy = new SaveCompleteStrategy();
                    break;
                case "B":
                    _strategy = new SaveDiffStrategy();
                    break;
                default:
                    throw new ArgumentException("Type de sauvegarde non supporté.");
            }
        }

        public void Save()
        {
            LogRealTime.CalculateFolderSizeAndFileCount(Backup.Source);
            DateTime TimeDebut = DateTime.Now;
            _strategy.Save(Backup,LogRealTime);
            DateTime TimeFin = DateTime.Now;
            TimeSpan duration = TimeFin - TimeDebut;
            Logger Log = new Logger(AppConstants.LogFilePath, Backup.Name, Backup.Source, Backup.Destination, 10, duration);
            Log.CreateLog();
            Console.WriteLine("Sauvegarde terminée avec la stratégie: " + Backup.Type);
        }
    }
}
