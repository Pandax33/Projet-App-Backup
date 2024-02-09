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
            Console.WriteLine("Entrez le chemin du fichier Json, la langue, le chemin du fichier Json en temps réel, et le chemin du fichier Json sauvegardé séparés par des virgules:");
            string[] createParams = Console.ReadLine().Split(',');
            if (createParams.Length == 4)
            {
                ConfigViewModel configViewModel = new ConfigViewModel();
                string result = configViewModel.EditerConfig(createParams[0], createParams[1], createParams[2], createParams[3]);
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Paramètres incorrects.");
            }
            return "Changement fini";
        }

        
    }
}
