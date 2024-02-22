using ProjetDevSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys;


namespace ProjetDevSysGraphical.VueModel
{
    public class RunTaskAsync
    {
        public async Task<string> RunTaskMultipleAsync(int[] tab, SynchronizationContext context)
        {
            if (ProjetDevSys.AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");

            int tasksCount = tab.Length;
            if (tasksCount == 0) return "No backups specified.";

            List<Task> tasks = new List<Task>();

            foreach (int id in tab)
            {
                Backup backup = BackupFactory.GetBackupByIndex(id);

                if (backup == null)
                {
                    continue;
                }
                BackupJob backupJob = new BackupJob(backup);
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        var mre = new ManualResetEvent(true); // true signifie qu'il n'est pas en attente au départ
                        ProjetDevSys.AppConstants.BackupPauseHandles.TryAdd(backup.Name, mre);
                        // Utilisation de SynchronizationContext pour la mise à jour de l'UI
                        context.Post(_ =>
                        {
                            ProjetDevSys.AppConstants.backupProgress.TryAdd(backup.Name, 0);
                            
                            
                        }, null);
                        backupJob.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during backup for {backup.Name}: {ex}");
                    }
                }));
            }

            await Task.WhenAll(tasks);

            return ResourceHelper.GetString("RunTaskView11");
        }

        public async Task<string> RunMultipleTaskAsync(int idDebut, int idFin)
        {
            if (ProjetDevSys.AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");

            IEnumerable<Backup> allBackups = BackupFactory.GetBackupsInRange(idDebut, idFin);
            int tasksCount = allBackups.Count();
            if (tasksCount == 0) return "No backups found in the specified range.";

            var tasks = new List<Task>();

            foreach (Backup backup in allBackups)
            {
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        BackupJob backupJob = new BackupJob(backup);
                        backupJob.Save();
                        ProjetDevSys.AppConstants.backupProgress.TryAdd(backup.Name, 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }));
            }

            // Attendez que toutes les tâches soient terminées
            await Task.WhenAll(tasks);
            return ResourceHelper.GetString("RunTaskView6");
        }

    }
}
