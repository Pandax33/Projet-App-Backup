using ProjetDevSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.VueModel
{
    public class RunSaveTask
    {
        public bool RunTask(int id)
        {
            Backup backup = BackupFactory.GetBackupByIndex(id);
            BackupJob backupJob = new BackupJob(backup);
            Console.WriteLine(backup.Name);
            backupJob.Save();
            return true;
        }

        public bool RunMultipleTask(int idDebut, int idFin)
        {
            IEnumerable<Backup> allBackups = BackupFactory.GetBackupsInRange(idDebut,idFin);
            foreach (Backup backup in allBackups)
            {
                BackupJob backupJob = new BackupJob(backup);
                backupJob.Save();
            }
            return true;
        }
        public bool RunTaskMultiple(int[] tab)
        {
            foreach(int id in tab)
            {
                Backup backup = BackupFactory.GetBackupByIndex(id);
                BackupJob backupJob = new BackupJob(backup);
                backupJob.Save();
            }
            return true;
        }
    }
}
