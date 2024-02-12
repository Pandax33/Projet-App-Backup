using ProjetDevSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.VueModel
{
    public class BackupGridViewModel
    {
        public IEnumerable<Backup> GetAllBackupsModel()
        {
            return BackupFactory.GetAllBackups();
        }
    }
}
