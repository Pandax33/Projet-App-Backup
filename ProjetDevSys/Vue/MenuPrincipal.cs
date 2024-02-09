using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.Vue
{
    public class MenuPrincipal
    {

        public void PrincipalMenu()
        {
            RunTaskView runTaskView = new RunTaskView();
            GestionTaskView gestionTaskView = new GestionTaskView();
            ConfigView configView = new ConfigView();

            while (true) // Boucle infinie pour revenir au menu principal après chaque action
            {
                Console.WriteLine("Menu Principal:");
                Console.WriteLine("1. Exécuter une tâche");
                Console.WriteLine("2. Gestion des tâches");
                Console.WriteLine("3. Éditer la configuration");
                Console.WriteLine("4. Quitter");
                Console.Write("Veuillez entrer votre choix (1-4): ");

                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        Console.WriteLine(runTaskView.SelectTaskView());
                        break;
                    case "2":
                        Console.WriteLine(gestionTaskView.SelectGestionTaskView());
                        break;
                    case "3":
                        Console.WriteLine(configView.EditerConfig());
                        break;
                    case "4":
                        Console.WriteLine("Quitter le programme...");
                        return; // Sortie de la fonction pour quitter le programme
                    default:
                        Console.WriteLine("Choix invalide. Veuillez essayer à nouveau.");
                        break;
                }
            }
        }

    }
}
