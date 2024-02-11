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


            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerJsonPath(string JsonPath)
        {
            Config.JsonPath = JsonPath;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerLangage(string Langage)
        {
            Config.Langage = Langage;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel16");
        }

        public string EditerJsonPathRealTime(string JsonPathRealTime)
        {
            Config.JsonPathRealTime = JsonPathRealTime;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerJsonPathSave(string JsonPathSave)
        {
            Config.JsonPathSave = JsonPathSave;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }
    }
}
