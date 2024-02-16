using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys.VueModel;
using ProjetDevSys;
using ProjetDevSys.Model;

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
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText18"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText23"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText24"));
            Console.WriteLine(ResourceHelper.GetString("ConfigViewText28"));
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

                    while (!configViewModel.verifInputLanguage(newLangage))
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
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText16"));
                    string newExtension = Console.ReadLine().Trim().ToLower();

                    while (!configViewModel.verifInputExtension(newExtension))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText17"));
                        Console.ResetColor();
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText16"));
                        newExtension = Console.ReadLine().Trim().ToLower();
                    }

                    // Once the language is correct, we can edit it
                    return configViewModel.EditExtensionType(newExtension);

                case "6":
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText19"));
                    string newExtensionCrypt = Console.ReadLine().Trim().ToLower();

                    // Once the language is correct, we can edit it
                    return configViewModel.EditExtensionListCrypt(newExtensionCrypt);

                case "7":
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText21"));
                    string newCryptPath = Console.ReadLine().Trim().ToLower();

                    while (!configViewModel.verifCryptPath(newCryptPath))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText22"));
                        Console.ResetColor();
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText21"));
                        newCryptPath = Console.ReadLine().Trim().ToLower();
                    }

                    // Once the language is correct, we can edit it
                    return configViewModel.EditCryptPath(newCryptPath);

                case "8":
                    for (int i = 0; i < Config.ExtensionListCrypt.Count; i++)
                    {
                        Console.WriteLine($"{i}. [{Config.ExtensionListCrypt[i]}]");
                    }
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText26"));
                    string newDeleteExtensionCrypt = Console.ReadLine().Trim().ToLower();

                    while (!configViewModel.verifDeleteExtensionList(AppConstants.StringToInt(newDeleteExtensionCrypt)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText27"));
                        Console.ResetColor();
                        Console.WriteLine(ResourceHelper.GetString("ConfigViewText26"));
                        newDeleteExtensionCrypt = Console.ReadLine().Trim().ToLower();
                    }
                    return configViewModel.removeExtensionListCrypt(AppConstants.StringToInt(newDeleteExtensionCrypt));

                case "9":
                    return ResourceHelper.GetString("ConfigViewText2");

                default:
                    Console.WriteLine(ResourceHelper.GetString("ConfigViewText4"));
                    return ResourceHelper.GetString("ConfigViewText2");
            }


        }


    }
}
