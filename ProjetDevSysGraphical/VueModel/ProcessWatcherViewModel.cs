using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ProjetDevSysGraphical.Watcher;

namespace ProjetDevSysGraphical.VueModel
{
    internal class ProcessWatcherViewModel
    {
        private readonly ProcessWatcher processWatcher;
        private App app = Application.Current.Dispatcher.Invoke(() => Application.Current as App);

        public ProcessWatcherViewModel()
        {
            processWatcher = app.processWatcher;

        }

        public bool Blockerprocess()
        {
            return processWatcher.blockerProcessFound;
            // return true;
        }
    }
}
