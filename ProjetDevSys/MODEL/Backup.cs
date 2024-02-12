using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetDevSys.Model
{
    public class Backup
    {
        public string Name { get; set; }
        public string Destination { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }

        public Backup(){}

    }

    
}
