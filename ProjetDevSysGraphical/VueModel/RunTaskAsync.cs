using ProjetDevSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys;
using System.Windows;


namespace ProjetDevSysGraphical.VueModel
{
    public class RunTaskAsync
    {
        public async Task<string> RunTaskMultipleAsync(int[] tab, SynchronizationContext context)
        {

            int tasksCount = tab.Length;
            if (tasksCount == 0) return "No backups specified.";

            List<Task> tasks = new List<Task>();
            IEnumerable<Backup> allBackups = BackupFactory.GetAllBackups();
            List<BackupJob> backupJobs = new List<BackupJob>();
            ProjetDevSys.AppConstants.priorityEvent.Reset();
            foreach (int id in tab)
            {
                Backup backup = BackupFactory.GetBackupByIndex(id);
                if (backup != null)
                {
                    BackupJob backupJob = new BackupJob(backup);
                    backupJob.CreateLogRealTime(); // Préparer le job de sauvegarde
                    backupJobs.Add(backupJob);
                }
            }

            foreach (BackupJob backupJob in backupJobs)
            {
                Backup backup = backupJob.Backup;
                if (backup == null)
                {
                    continue;
                }
                if (backup == null)
                {
                    continue;
                }
                backupJob.CreateLogRealTime();
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        ManualResetEvent mre = new ManualResetEvent(true); // true signifie qu'il n'est pas en attente au départ
                        ProjetDevSys.AppConstants.BackupPauseHandles.TryAdd(backup.Name, mre);
                        var cts = new CancellationTokenSource();
                        ProjetDevSys.AppConstants.BackupCancellations[backup.Name] = cts;
                        // Utilisation de SynchronizationContext pour la mise à jour de l'UI
                        context.Post(_ =>
                        {
                            
                            ProjetDevSys.AppConstants.backupProgress.TryAdd(backup.Name, backupJob.LogRealTime.Progress);
                            
                            
                        }, null);
                        backupJob.SavePrio();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during backup for {backup.Name}: {ex}");
                    }
                }));
            }
            ProcessWatcherViewModel processWatcherViewModel = new ProcessWatcherViewModel();
            if (processWatcherViewModel.Blockerprocess() == true)
            {
                MessageBox.Show(ResourceHelper.GetString("RunTaskView4"), ResourceHelper.GetString("RunTaskView5"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            await Task.WhenAll(tasks);
            ProjetDevSys.AppConstants.priorityEvent.Set();
            List<Task> tasksNoPriority = new List<Task>();
            foreach (BackupJob backupJob in backupJobs)
            {
                Backup backup = backupJob.Backup;
                if (backup == null)
                {
                    continue;
                }
                tasksNoPriority.Add(Task.Run(() =>
                {
                    try
                    {
                        backupJob.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during backup for {backup.Name}: {ex}");
                    }
                }));
            }

            await Task.WhenAll(tasksNoPriority);
            foreach (var backup in allBackups)
            {
                ProjetDevSys.AppConstants.BackupCancellations.TryRemove(backup.Name, out _);
                ProjetDevSys.AppConstants.BackupPauseHandles.TryRemove(backup.Name, out _);
            }
            if (backupJobs.Count() == 1)
            {
                string message = ResourceHelper.GetString("RunTaskView11");
                return message;
            }
            if (backupJobs.Count() > 1)
            {
                int count = backupJobs.Count();
                string message = ResourceHelper.GetString("RunTaskView12");
                return $"{count} {message}";
            }
            return null;
        }

        public async Task<string> RunMultipleTaskAsync(int idDebut, int idFin, SynchronizationContext context)
        {

            IEnumerable<Backup> allBackups = BackupFactory.GetBackupsInRange(idDebut, idFin);
            if (!allBackups.Any()) return "No backups found in the specified range.";


            List<Task> tasks = new List<Task>();
            List<BackupJob> backupJobs = new List<BackupJob>();
            ProjetDevSys.AppConstants.priorityEvent.Reset();
            foreach (Backup backup in allBackups)
            {
                if (backup != null)
                {
                    BackupJob backupJob = new BackupJob(backup);
                    backupJob.CreateLogRealTime(); // Préparer le job de sauvegarde
                    backupJobs.Add(backupJob);
                }
            }

            foreach (BackupJob backupJob in backupJobs)
            {
                Backup backup = backupJob.Backup;
                if (backup == null)
                {
                    continue;
                }
                if (backup == null)
                {
                    continue;
                }
                backupJob.CreateLogRealTime();
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        ManualResetEvent mre = new ManualResetEvent(true); // true signifie qu'il n'est pas en attente au départ
                        ProjetDevSys.AppConstants.BackupPauseHandles.TryAdd(backup.Name, mre);
                        var cts = new CancellationTokenSource();
                        ProjetDevSys.AppConstants.BackupCancellations[backup.Name] = cts;
                        // Utilisation de SynchronizationContext pour la mise à jour de l'UI
                        context.Post(_ =>
                        {

                            ProjetDevSys.AppConstants.backupProgress.TryAdd(backup.Name, backupJob.LogRealTime.Progress);


                        }, null);
                        backupJob.SavePrio();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during backup for {backup.Name}: {ex}");
                    }
                }));
            }
            ProcessWatcherViewModel processWatcherViewModel = new ProcessWatcherViewModel();
            if (processWatcherViewModel.Blockerprocess() == true)
            {
                MessageBox.Show(ResourceHelper.GetString("RunTaskView4"), ResourceHelper.GetString("RunTaskView5"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            await Task.WhenAll(tasks);
            ProjetDevSys.AppConstants.priorityEvent.Set();
            List<Task> tasksNoPriority = new List<Task>();
            foreach (BackupJob backupJob in backupJobs)
            {
                Backup backup = backupJob.Backup;
                if (backup == null)
                {
                    continue;
                }
                tasksNoPriority.Add(Task.Run(() =>
                {
                    try
                    {
                        backupJob.Save();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during backup for {backup.Name}: {ex}");
                    }
                }));
            }

            await Task.WhenAll(tasksNoPriority);
            foreach (var backup in allBackups)
            {
                ProjetDevSys.AppConstants.BackupCancellations.TryRemove(backup.Name, out _);
                ProjetDevSys.AppConstants.BackupPauseHandles.TryRemove(backup.Name, out _);
            }

            string message = ResourceHelper.GetString("RunTaskView6");
            return message;
        }
    }
}
