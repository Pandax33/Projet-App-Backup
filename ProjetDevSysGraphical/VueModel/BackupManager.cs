using ProjetDevSys.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetDevSysGraphical.VueModel
{
    public static class BackupManager
    {
        private static List<BackupJob> backupQueue = new List<BackupJob>();
        private static SynchronizationContext context = SynchronizationContext.Current;

        public static void SetSynchronizationContext(SynchronizationContext newContext)
        {
            context = newContext;
        }

        public static void AddBackupToQueue(int[] backupIds)
        {
            foreach (int id in backupIds)
            {
                Backup backup = BackupFactory.GetBackupByIndex(id);
                if (backup != null && !ProjetDevSys.AppConstants.backupState.ContainsKey(backup.Name))
                {
                    BackupJob backupJob = new BackupJob(backup);
                    backupQueue.Add(backupJob);
                    ProjetDevSys.AppConstants.backupProgress.TryAdd(backup.Name, backupJob.LogRealTime.Progress);
                    ManualResetEvent mre = new ManualResetEvent(true);
                    ProjetDevSys.AppConstants.BackupPauseHandles.TryAdd(backup.Name, mre);
                    var cts = new CancellationTokenSource();
                    ProjetDevSys.AppConstants.BackupCancellations[backup.Name] = cts;
                }
            }
            ExecuteBackups(); 
        }

        private static async void ExecuteBackups()
        {
            foreach (BackupJob backupJob in backupQueue.ToList())
            {
                if (!ProjetDevSys.AppConstants.backupState.ContainsKey(backupJob.Backup.Name))
                {
                    ProjetDevSys.AppConstants.backupState.TryAdd(backupJob.Backup.Name, "In Progress");
                    ExecuteBackupAsync(backupJob);
                }
            }

            if (ProjetDevSys.AppConstants.backupState.IsEmpty)
            {
                context.Post(_ => MessageBox.Show("All backups completed successfully.", "Backup Complete", MessageBoxButton.OK, MessageBoxImage.Information), null);
            }
        }

        private static Task ExecuteBackupAsync(BackupJob backupJob)
        {
            return Task.Run(() =>
            {
                try
                {
                    backupJob.CreateLogRealTime();
                    ProjetDevSys.AppConstants.priorityEvent.Reset();
                    backupJob.SavePrio();
                    ProjetDevSys.AppConstants.priorityEvent.Set();
                    backupJob.Save();
                    string value;
                    ProjetDevSys.AppConstants.backupState.TryRemove(backupJob.Backup.Name, out value);
                    ProjetDevSys.AppConstants.BackupCancellations.TryRemove(backupJob.Backup.Name, out _);
                    ProjetDevSys.AppConstants.BackupPauseHandles.TryRemove(backupJob.Backup.Name, out _);
                    lock (backupQueue)
                    {
                        backupQueue.Remove(backupJob);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during backup for {backupJob.Backup.Name}: {ex}");
                }
            });
        }
    }
}