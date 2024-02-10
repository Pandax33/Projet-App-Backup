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
                    string fileName = Console.ReadLine();

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
                        if (!AppConstants.VerifPath(destinationPath))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Chemin non valide, veuillez entrer un chemin valide.");
                            Console.ResetColor(); 
                            Console.WriteLine(ResourceHelper.GetString("GestionTaskView16"));
                        }
                    } while (!AppConstants.VerifPath(destinationPath));
                    Console.WriteLine(AppConstants.VerifPath(destinationPath));

                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView17"));
                    string backupType = Console.ReadLine().Trim().ToUpper(); 

                    while (backupType != "A" && backupType != "B")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Entrée non valide. Veuillez entrer 'A' ou 'B'.");
                        Console.ResetColor();
                        Console.WriteLine(ResourceHelper.GetString("GestionTaskView17")); 
                        backupType = Console.ReadLine().Trim().ToUpper(); 
                    }

                    bool created = gestionTask.CreateTask(fileName, sourcePath, destinationPath, backupType);
                    return created ? ResourceHelper.GetString("GestionTaskView3") : ResourceHelper.GetString("GestionTaskView4");
                    
                case "2":
                    IEnumerable<Backup> BackupList = BackupFactory.GetAllBackups();
                    if (BackupList != null && BackupList.Any())
                    {
                        int index2 = 0;
                        foreach (Backup backup in BackupList)
                        {
                            Console.WriteLine($"ID : {index2} Save: {backup.Name}");
                            index2++;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Aucune sauvegarde enregistrée");
                        Console.ResetColor();
                        return (ResourceHelper.GetString("RunTaskView18"));
                    }
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView6"));
                    string deleteIdInput = Console.ReadLine();
                    if (int.TryParse(deleteIdInput, out int deleteId))
                    {
                        // Modifier pour appeler DeleteTask sur BackupFactory
                        string result = gestionTask.DeleteTask(deleteId);
                        return(result);
                    }
                    else
                    {
                        return(ResourceHelper.GetString("GestionTaskView7"));
                    }
                case "3":
                    // Affichage des sauvegardes disponibles
                    IEnumerable<Backup> BackupList2 = BackupFactory.GetAllBackups();
                    if (BackupList2 == null || !BackupList2.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Aucune sauvegarde enregistrée");
                        Console.ResetColor();
                        return ResourceHelper.GetString("RunTaskView18");
                    }

                    int index = 0;
                    foreach (Backup backup in BackupList2)
                    {
                        Console.WriteLine($"ID : {index} Save: {backup.Name}");
                        index++;
                    }

                    // Demande de l'ID à modifier
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView8"));
                    if (int.TryParse(Console.ReadLine(), out int id) && id >= 0 && id < BackupList2.Count())
                    {
                        Backup selectedBackup = BackupList2.ElementAt(id);

                        // Demande si l'utilisateur veut modifier la source
                        Console.WriteLine("Voulez-vous modifier la source de la sauvegarde ? (y/n)");
                        if (Console.ReadLine().Trim().ToLower() == "y")
                        {
                            string newPath;
                            do
                            {
                                Console.WriteLine("Entrez le nouveau chemin pour la source :");
                                newPath = Console.ReadLine();
                                if (!AppConstants.VerifExist(newPath))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Chemin non valide, veuillez entrer un chemin valide.");
                                    Console.ResetColor();
                                }
                            } while (!AppConstants.VerifExist(newPath));

                            // Appel de la fonction pour modifier la source
                            string resultSource = gestionTask.EditNewSource(id, newPath);
                            Console.WriteLine(resultSource);
                        }

                        // Demande si l'utilisateur veut modifier la destination
                        Console.WriteLine("Voulez-vous modifier la destination de la sauvegarde ? (y/n)");
                        if (Console.ReadLine().Trim().ToLower() == "y")
                        {
                            string newDestination;
                            do
                            {
                                Console.WriteLine("Entrez le nouveau chemin pour la destination :");
                                newDestination = Console.ReadLine();
                                if (!AppConstants.VerifPath(newDestination))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Chemin non valide, veuillez entrer un chemin valide.");
                                    Console.ResetColor();
                                }
                            } while (!AppConstants.VerifPath(newDestination));

                            // Appel de la fonction pour modifier la destination
                            string resultDestination = gestionTask.EditNewDestination(id, newDestination);
                            Console.WriteLine(resultDestination);
                        }

                        //Ask to user if he want to change Type
                        Console.WriteLine("Voulez-vous modifier le type de la sauvegarde ? (y/n)");
                        string modifyTypeResponse = Console.ReadLine().Trim().ToLower();

                        if (modifyTypeResponse == "y")
                        {
                            string newType;
                            do
                            {
                                Console.WriteLine("Entrez le nouveau type de la sauvegarde (A/B) :");
                                newType = Console.ReadLine().Trim().ToUpper();
                                if (newType != "A" && newType != "B")
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Type non valide, veuillez entrer A ou B.");
                                    Console.ResetColor();
                                }
                            } while (newType != "A" && newType != "B");

                            string resultType = gestionTask.EditNewType(id, newType);
                            Console.WriteLine(resultType);
                        }
                        else if (modifyTypeResponse != "n")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Réponse non valide, veuillez entrer 'y' pour oui ou 'n' pour non.");
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
