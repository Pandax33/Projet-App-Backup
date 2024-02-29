using System;
using System.Threading;
using System.Windows; // Nécessaire pour MessageBox

namespace ProjetDevSysGraphical.Watcher
{
    public class ProcessWatcher
    {
        private Thread watcherThread;
        public bool isWatching;
        public bool blockerProcessFound;
        private SynchronizationContext uiContext; // Contexte UI pour accéder au thread principal de l'UI

        public ProcessWatcher(SynchronizationContext context)
        {
            watcherThread = new Thread(new ThreadStart(WatchProcess));
            isWatching = true;
            uiContext = context; // Initialiser avec le contexte UI passé en paramètre
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
            watcherThread.Join(); // Attendre la fin du thread
        }

        private void WatchProcess()
        {
            while (isWatching)
            {
                bool previousState = blockerProcessFound;
                blockerProcessFound = ProjetDevSys.AppConstants.RunningBlockerProcess();

                if (blockerProcessFound && !previousState)
                {
                    uiContext.Post(_ => MessageBox.Show(ResourceHelper.GetString("BackupWatcher.TextPopupFind"), ResourceHelper.GetString("BackupWatcher.TittlePopupFind"), MessageBoxButton.OK, MessageBoxImage.Warning), null);
                    ProjetDevSys.AppConstants.EventState.TryAdd("processEvent", "Pause");
                    ProjetDevSys.AppConstants.processEvent.Reset();
                }
                else if (!blockerProcessFound && previousState)
                {
                    uiContext.Post(_ => MessageBox.Show(ResourceHelper.GetString("BackupWatcher.TextPopupResolve"), ResourceHelper.GetString("BackupWatcher.TittlePopupResolve"), MessageBoxButton.OK, MessageBoxImage.Information), null);
                    ProjetDevSys.AppConstants.processEvent.Set();
                    ProjetDevSys.AppConstants.EventState.TryAdd("processEvent", "Libre");
                }

                Thread.Sleep(2000);
            }
        }
    }
}