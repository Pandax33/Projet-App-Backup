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

        public BackupJob(Backup BackupObj)
        {
            Backup = BackupObj;
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
            _strategy.Save(Backup);
            Logger Log = new Logger(AppConstants.LogFilePath, Backup.Name, Backup.Source, Backup.Destination, 10, TimeSpan.FromSeconds(40));
            Log.CreateLog();
            Console.WriteLine("Sauvegarde terminée avec la stratégie: " + Backup.Type);
        }
    }
}
