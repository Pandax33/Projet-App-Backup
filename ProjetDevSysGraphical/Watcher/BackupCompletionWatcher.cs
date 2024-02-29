using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSysGraphical.Watcher
{
    public class BackupCompletionWatcher
    {
        private Thread watcherThread;
        public bool isWatching;
        private SynchronizationContext uiContext;

        public BackupCompletionWatcher(SynchronizationContext uiContext)
        {
            this.uiContext = uiContext;
            watcherThread = new Thread(new ThreadStart(WatchBackupCompletion));
            isWatching = false; // Initialisé à false et sera mis à true quand StartWatching sera appelé.
        }

        public void StartWatching()
        {
            isWatching = true;
            if (!watcherThread.IsAlive)
            {
                watcherThread.Start();
            }
        }

        public void StopWatching()
        {
            isWatching = false;
            if (watcherThread.IsAlive)
            {
                watcherThread.Join(); // Attend que le thread se termine proprement.
            }
        }

        private void WatchBackupCompletion()
        {
            while (isWatching)
            {
                List<string> completedBackups = ProjetDevSys.AppConstants.backupState.Where(kvp => kvp.Value == "Completed").Select(kvp => kvp.Key).ToList();

                foreach (string backupName in completedBackups)
                {
                    string unused;
                    ProjetDevSys.AppConstants.backupState.TryRemove(backupName, out unused);

                    uiContext.Post(_ => ShowBackupCompletePopup(backupName), null);
                }

                Thread.Sleep(2000);
            }
        }

        private void ShowBackupCompletePopup(string backupName)
        {
            System.Windows.MessageBox.Show($"Backup {backupName} completed successfully!", "Backup Complete", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}
