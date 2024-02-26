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
                    AppConstants.processEvent.Reset(); // Met en pause toutes les sauvegardes.
                }
                else
                {
                    AppConstants.processEvent.Set(); // Reprend toutes les sauvegardes.
                }

                Thread.Sleep(2000); // Fréquence de vérification des processus bloquants.
            }
        }

    }
}
