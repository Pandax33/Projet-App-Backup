using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace ProjetDevSys.MODEL
{
    public class ProcessWatcher
    {
        private Thread watcherThread;
        private bool isWatching;

        public ProcessWatcher()
        {
            watcherThread = new Thread(new ThreadStart(WatchProcess));
            isWatching = true;
        }

        public void StartWatching()
        {
            if (!watcherThread.IsAlive)
            {
                watcherThread.Start();
            }
        }

        public void StopWatching()
        {
            isWatching = false;
        }

        private void WatchProcess()
        {
            while (isWatching)
            {
                bool blockerProcessFound = AppConstants.RunningBlockerProcess();

                if (blockerProcessFound)
                {
                    foreach (var backupName in AppConstants.backupProgress.Keys)
                    {
                        AppConstants.PauseBackup(backupName);
                    }
                }
                else
                {
                    // Reprend toutes les sauvegardes en pause
                    foreach (var backupName in AppConstants.backupProgress.Keys)
                    {
                        AppConstants.ResumeBackup(backupName);
                    }
                }

                Thread.Sleep(2000);
            }
        }
    }
}
