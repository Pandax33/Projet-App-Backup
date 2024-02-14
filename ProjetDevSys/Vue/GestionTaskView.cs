using ProjetDevSys.Model;
using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.Vue
{
    public class GestionTaskView
    {
        public string SelectGestionTaskView()
        {
            Console.WriteLine(ResourceHelper.GetString("Form1"));
            Console.WriteLine(ResourceHelper.GetString("GestionTaskView1"));
            Console.WriteLine(ResourceHelper.GetString("GestionTaskView18"));
            Console.WriteLine(ResourceHelper.GetString("GestionTaskView19"));
            Console.WriteLine(ResourceHelper.GetString("GestionTaskView20"));
            Console.WriteLine(ResourceHelper.GetString("Form1"));
            string input = Console.ReadLine();
            GestionTask gestionTask = new GestionTask();
            switch (input)
            {
                case "1":
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView13"));
                    string fileName;
                    bool isValidName = false;
                    do
                    {
                        fileName = Console.ReadLine().Trim();
                        isValidName = ((BackupFactory.GetBackupByName(fileName) == null) && !String.IsNullOrWhiteSpace(fileName)) ? true : false;
                        if (!isValidName)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("GestionTaskViewText35"));
                            Console.ResetColor();
                        }

                    } while (!isValidName);

                    string sourcePath;
                    bool isValidSource = false;
                    do
                    {
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView14"));
                        sourcePath = Console.ReadLine();
                        isValidSource = AppConstants.VerifExist(sourcePath); 
                        if (!isValidSource)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("GestionTaskView15"));
                            Console.ResetColor();
                        }
                    } while (!isValidSource);
                    
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView16"));
                    string destinationPath;

                    do
                    {
                        destinationPath = Console.ReadLine();
                        if (!AppConstants.VerifPath(destinationPath) || destinationPath.Contains(sourcePath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("GestionTaskView22"));
                            Console.ResetColor(); 
                            Console.WriteLine(ResourceHelper.GetString("GestionTaskView16"));
                        }
                    } while (!AppConstants.VerifPath(destinationPath) || destinationPath.Contains(sourcePath));

                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView17"));
                    string backupType = Console.ReadLine().Trim().ToUpper(); 

                    while (!gestionTask.verifInputBackupType(backupType))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView23"));
                        Console.ResetColor();
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView17")); 
                        backupType = Console.ReadLine().Trim().ToUpper(); 
                    }

                    return gestionTask.CreateTask(fileName, sourcePath, destinationPath, backupType);
                    
                case "2":
                    IEnumerable<Backup> BackupList = BackupFactory.GetAllBackups();
                    if (BackupList == null || !BackupList.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView24"));
                        Console.ResetColor();
                        return ResourceHelper.GetString("RunTaskView18");
                    }

                    // Print all backups
                    int index2 = 0;
                    foreach (Backup backup in BackupList)
                    {
                        Console.WriteLine($"{index2}. [{backup.Name}]");
                        index2++;
                    }

                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView6"));
                    string deleteIdInput = Console.ReadLine();
                    if (gestionTask.VerifyId(AppConstants.StringToInt(deleteIdInput)))
                    {
                        // Call the function to delete the task
                        return gestionTask.DeleteTask(AppConstants.StringToInt(deleteIdInput));
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView7"));
                        Console.ResetColor();
                        return ResourceHelper.GetString("GestionTaskView21");
                    }

                case "3":
                    // Print all backups
                    IEnumerable<Backup> BackupList2 = BackupFactory.GetAllBackups();
                    if (BackupList2 == null || !BackupList2.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView24"));
                        Console.ResetColor();
                        return ResourceHelper.GetString("RunTaskView18");
                    }

                    int index = 0;
                    foreach (Backup backup in BackupList2)
                    {
                        Console.WriteLine($"{index}. [{backup.Name}]");
                        index++;
                    }

                    // Ask the user to select the backup to edit
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView8"));
                    string editIdInput = Console.ReadLine();
                    if (gestionTask.VerifyId(AppConstants.StringToInt(editIdInput)))
                    {
                        int id = AppConstants.StringToInt(editIdInput);
                        Backup selectedBackup = BackupList2.ElementAt(id);

                        // Ask the user if he wants to modify the source
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView25"));
                        if (Console.ReadLine().Trim().ToLower() == "y")
                        {
                            string newPath;
                            do
                            {
                                Console.WriteLine(ResourceHelper.GetString("GestionTaskView26"));
                                newPath = Console.ReadLine();
                                if (!AppConstants.VerifExist(newPath))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView27"));
                                    Console.ResetColor();
                                }
                            } while (!AppConstants.VerifExist(newPath));

                            // Call the function to modify the source
                            string resultSource = gestionTask.EditNewSource(id, newPath);
                            Console.WriteLine(resultSource);
                        }

                        // Ask the user if he wants to modify the destination
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView28"));
                        if (Console.ReadLine().Trim().ToLower() == "y")
                        {
                            string newDestination;
                            do
                            {
                                Console.WriteLine(ResourceHelper.GetString("GestionTaskView29"));
                                newDestination = Console.ReadLine();
                                if (!AppConstants.VerifPath(newDestination))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView30"));
                                    Console.ResetColor();
                                }
                            } while (!AppConstants.VerifPath(newDestination));

                            // Call the function to modify the destination
                            string resultDestination = gestionTask.EditNewDestination(id, newDestination);
                            Console.WriteLine(resultDestination);
                        }

                        // Ask to user if he want to change Type
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView31"));
                        string modifyTypeResponse = Console.ReadLine().Trim().ToLower();

                        if (modifyTypeResponse == "y")
                        {
                            string newType;
                            do
                            {
                                Console.WriteLine(ResourceHelper.GetString("GestionTaskView32"));
                                newType = Console.ReadLine().Trim().ToUpper();
                                if (newType != "A" && newType != "B")
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView33"));
                                    Console.ResetColor();
                                }
                            } while (newType != "A" && newType != "B");

                            string resultType = gestionTask.EditNewType(id, newType);
                            Console.WriteLine(resultType);
                        }
                        else if (modifyTypeResponse != "n")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ResourceHelper.GetString("GestionTaskView34"));
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        return ResourceHelper.GetString("GestionTaskView9");
                    }
                    return ResourceHelper.GetString("GestionTaskViewSuccess");

                case "4":
                    return (ResourceHelper.GetString("GestionTaskView21"));
                default:
                    return(ResourceHelper.GetString("GestionTaskView11"));
            }
        }
    }
}
