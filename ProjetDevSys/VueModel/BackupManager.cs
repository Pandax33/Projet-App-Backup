using ProjetDevSys.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetDevSys.VueModel
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
                if (backup != null && !AppConstants.backupState.ContainsKey(backup.Name))
                {
                    BackupJob backupJob = new BackupJob(backup);
                    backupJob.CreateLogRealTime();
                    backupQueue.Add(backupJob);
                    AppConstants.backupProgress.TryAdd(backup.Name, backupJob.LogRealTime.Progress);
                    ManualResetEvent mre = new ManualResetEvent(true);
                    AppConstants.BackupPauseHandles.TryAdd(backup.Name, mre);
                    CancellationTokenSource cts = new CancellationTokenSource();
                    AppConstants.BackupCancellations[backup.Name] = cts;
                }
            }
            ExecuteBackups();
        }

        private static async void ExecuteBackups()
        {
            foreach (BackupJob backupJob in backupQueue.ToList())
            {
                if (!AppConstants.backupState.ContainsKey(backupJob.Backup.Name))
                {
                    AppConstants.backupState.TryAdd(backupJob.Backup.Name, "In Progress");
                    ExecuteBackupAsync(backupJob);
                }
            }
        }

        private static Task ExecuteBackupAsync(BackupJob backupJob)
        {
            return Task.Run(() =>
            {
                try
                {
                    AppConstants.EventState.TryAdd("priotiryEvent", "Pause");
                    AppConstants.priorityEvent.Reset();
                    backupJob.SavePrio();
                    AppConstants.priorityEvent.Set();
                    AppConstants.EventState.TryAdd("priotiryEvent", "Libre");
                    backupJob.Save();
                    string value;
                    AppConstants.backupState[backupJob.Backup.Name] = "Completed";
                    AppConstants.BackupCancellations.TryRemove(backupJob.Backup.Name, out _);
                    AppConstants.BackupPauseHandles.TryRemove(backupJob.Backup.Name, out _);
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