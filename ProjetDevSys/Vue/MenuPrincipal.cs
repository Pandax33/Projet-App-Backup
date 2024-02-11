using System;
using System.Collections.Generic;
using System.Drawing;
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.WriteLine("                                                                               \r\n`7MM\"\"\"YMM                               .M\"\"\"bgd                              \r\n  MM    `7                              ,MI    \"Y                              \r\n  MM   d     ,6\"Yb.  ,pP\"Ybd `7M'   `MF'`MMb.      ,6\"Yb.  `7M'   `MF' .gP\"Ya  \r\n  MMmmMM    8)   MM  8I   `\"   VA   ,V    `YMMNq. 8)   MM    VA   ,V  ,M'   Yb \r\n  MM   Y  ,  ,pm9MM  `YMMMa.    VA ,V   .     `MM  ,pm9MM     VA ,V   8M\"\"\"\"\"\" \r\n  MM     ,M 8M   MM  L.   I8     VVV    Mb     dM 8M   MM      VVV    YM.    , \r\n.JMMmmmmMMM `Moo9^Yo.M9mmmP'     ,V     P\"Ybmmd\"  `Moo9^Yo.     W      `Mbmmd' \r\n                                ,V                                             \r\n                             OOb\"                                              ");
                Console.WriteLine("---------------------------------------------------------------------------------------");
                Console.ResetColor();
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
