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

            while (true) // Infinity loop for the main menu
            {
                Console.WriteLine(ResourceHelper.GetString("Form1"));
                Console.WriteLine(ResourceHelper.GetString("MenuPrincipal1"));
                Console.WriteLine(ResourceHelper.GetString("MenuPrincipal2"));
                Console.WriteLine(ResourceHelper.GetString("MenuPrincipal3"));
                Console.WriteLine(ResourceHelper.GetString("MenuPrincipal4"));
                Console.WriteLine(ResourceHelper.GetString("MenuPrincipal5"));
                Console.WriteLine(ResourceHelper.GetString("MenuPrincipal6"));
                Console.WriteLine(ResourceHelper.GetString("Form1"));

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
                        Console.WriteLine(ResourceHelper.GetString("MenuPrincipal7"));
                        return; // Exit the main menu
                    default:
                        Console.WriteLine(ResourceHelper.GetString("MenuPrincipal8"));
                        break;
                }
            }
        }

    }
}
