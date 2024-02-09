using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys.Model;

namespace ProjetDevSys.VueModel
{
    public class ConfigViewModel
    {
        public string EditerConfig(string JsonPath, string Langage, string JsonPathRealTime, string JsonPathSave)
        {
            Config.JsonPath = JsonPath;
            Config.Langage = Langage;
            Config.JsonPathRealTime = JsonPathRealTime;
            Config.JsonPathSave = JsonPathSave;
            Config.EditConfig();


            return "Changement fini";
        }
    }
}
