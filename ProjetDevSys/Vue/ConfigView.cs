using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys.VueModel;
using ProjetDevSys;

namespace ProjetDevSys.Vue
{
    public class ConfigView
    {
        public string EditerConfig()
        {
            Console.WriteLine(ResourceHelper.GetString("Form1"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText9"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText10"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText11"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText12"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText13"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText14"));
            Console.WriteLine(ResourceHelper.GetString("Form1"));
            string choice = Console.ReadLine();

            ConfigViewModel configViewModel = new ConfigViewModel();
            string result;

            switch (choice)
            {
                case "1":
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText5"));
                    string newPath;
                    do
                    {
                        newPath = Console.ReadLine();
                        if (!AppConstants.VerifJson(newPath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("ConfigViewText3"));
                            Console.ResetColor();
                            Console.WriteLine(ResourceHelper.GetString("ConfigViewText5"));
                        }
                    } while (!AppConstants.VerifJson(newPath));
                    return configViewModel.EditerJsonPath(newPath);

                case "2":
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText6"));
                    string newLangage = Console.ReadLine().Trim().ToLower(); // Normalise the entered language

                    while (newLangage != "fr" && newLangage != "en")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText15"));
                        Console.ResetColor();
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText6"));
                        newLangage = Console.ReadLine().Trim().ToLower();
                    }

                    // Once the language is correct, we can edit it
                    return configViewModel.EditerLangage(newLangage);

                case "3":
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText7"));
                    string newRealTimePath;
                    do
                    {
                        newRealTimePath = Console.ReadLine();
                        if (!AppConstants.VerifJson(newRealTimePath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("ConfigViewText3"));
                            Console.ResetColor();
                            Console.WriteLine(ResourceHelper.GetString("ConfigViewText7")); // Ask again
                        }
                    } while (!AppConstants.VerifJson(newRealTimePath));
                    return configViewModel.EditerJsonPathRealTime(newRealTimePath);

                case "4":
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText8"));
                    string newSavePath;
                    do
                    {
                        newSavePath = Console.ReadLine();
                        if (!AppConstants.VerifJson(newSavePath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("ConfigViewText3"));
                            Console.ResetColor();
                            Console.WriteLine(ResourceHelper.GetString("ConfigViewText8")); // Ask again
                        }
                    } while (!AppConstants.VerifJson(newSavePath));
                    return configViewModel.EditerJsonPathSave(newSavePath);

                case "5":
                    return ResourceHelper.GetString("ConfigViewText2");

                default:
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText4"));
                    return ResourceHelper.GetString("ConfigViewText2");
            }


        }


    }
}
