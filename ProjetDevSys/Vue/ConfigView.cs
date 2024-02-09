using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys.VueModel;

namespace ProjetDevSys.Vue
{
    public class ConfigView
    {
        public string EditerConfig()
        {
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText1"));
            string[] createParams = Console.ReadLine().Split(',');
            if (createParams.Length == 4)
            {
                ConfigViewModel configViewModel = new ConfigViewModel();
                string result = configViewModel.EditerConfig(createParams[0], createParams[1], createParams[2], createParams[3]);
                return(result);
            }
            else
            {
                return(ResourceHelper.GetString("ConfigViewText2"));
            }
            
        }

        
    }
}
