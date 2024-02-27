using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSysGraphical.VueModel
{
    internal class ProcessWatcherViewModel
    {
        // Get the current processWatcher
        private ProcessWatcher processWatcher = new ProcessWatcher();

        public bool Blockerprocess()
        {
            return processWatcher.blockerProcessFound;
            // return true;
        }
    }
}
