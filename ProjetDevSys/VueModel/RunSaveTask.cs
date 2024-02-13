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
        public string RunTask(int id)
        {
            if (AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");
            Backup backup = BackupFactory.GetBackupByIndex(id);
            BackupJob backupJob = new BackupJob(backup);
            Console.WriteLine(backup.Name);
            try
            {
                backupJob.Save();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return ResourceHelper.GetString("RunTaskView3");
        }

        public string RunMultipleTask(int idDebut, int idFin)
        {
            if (AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");

            IEnumerable<Backup> allBackups = BackupFactory.GetBackupsInRange(idDebut,idFin);
            foreach (Backup backup in allBackups)
            {
                BackupJob backupJob = new BackupJob(backup);
                try
                {
                    backupJob.Save();
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return ResourceHelper.GetString("RunTaskView6");
        }
        public string RunTaskMultiple(int[] tab)
        {
            if (AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");
            foreach (int id in tab)
            {
                Backup backup = BackupFactory.GetBackupByIndex(id);
                BackupJob backupJob = new BackupJob(backup);
                try
                {
                    backupJob.Save();
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return ResourceHelper.GetString("RunTaskView11");
        }

        public bool VerifyId(int id)
        {

            if (BackupFactory.GetBackupByIndex(id) != null)
            {
                return true;
            }
            return false;
        }

        public bool VerifyContinueId(string input)
        {
            string[] inputs = input.Split(',');
            if(inputs.Length == 2)
            {
                int idDebut = AppConstants.StringToInt(inputs[0]);
                int idFin = AppConstants.StringToInt(inputs[1]);
                if (idDebut != -1 && idFin != -1)
                {
                    if (idDebut < idFin)
                    {
                        if(VerifyId(idDebut) && VerifyId(idFin))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
