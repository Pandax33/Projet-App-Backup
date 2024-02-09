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
    public class RunTaskView
    {
        public string SelectTaskView()
        {
            Console.WriteLine("Voulez vous effectuez une tache(1) ou plusieurs taches ?(2)");
            string input = Console.ReadLine();
            if(input == "1")
            {
                int index = 0;
                IEnumerable<Backup> Backuplist = BackupFactory.GetAllBackups();
                foreach (Backup backup in Backuplist)
                {
                    Console.WriteLine($"ID : {index} Save: {backup.Name}");
                    index = index + 1;
                }
                Console.WriteLine(index);
                Console.WriteLine("Entrez l'ID de la tache que vous voulez effectuer");
                string input2 = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunTask(Convert.ToInt32(input2));
                if(result) 
                {
                    return "Sauvegarde fini";
                }
                else
                {
                    return "Un probleme est survenue";
                }
            }
            else if(input == "2")
            {
                int index = 0;
                IEnumerable<Backup> Backuplist = BackupFactory.GetAllBackups();
                foreach (Backup backup in Backuplist)
                {
                    Console.WriteLine($"ID : {index} Save: {backup.Name}");
                    index += 1;
                }

                Console.WriteLine("Entrez l'ID de la première et de la dernière tâche que vous voulez effectuer, séparés par une virgule (exemple: 2,5):");
                string input2 = Console.ReadLine();
                string[] inputs = input2.Split(',');

                if (inputs.Length == 2 && int.TryParse(inputs[0], out int startId) && int.TryParse(inputs[1], out int endId))
                {
                    RunSaveTask runSaveTask = new RunSaveTask();
                    bool result = runSaveTask.RunMultipleTask(startId, endId);

                    if (result)
                    {
                        Console.WriteLine("Sauvegarde finie.");
                        return "Sauvegarde finie.";
                    }
                    else
                    {
                        Console.WriteLine("Un problème est survenu.");
                        return "Un problème est survenu.";
                    }
                }
                else
                {
                    Console.WriteLine("Entrée invalide. Assurez-vous de séparer les deux nombres par une virgule.");
                    return "Entrée invalide.";
                }

            }
            else if(input == "3")
            {
                int index = 0;
                IEnumerable<Backup> backupList = BackupFactory.GetAllBackups();
                foreach (Backup backup in backupList)
                {
                    Console.WriteLine($"ID : {index} Save: {backup.Name}");
                    index += 1;
                }

                Console.WriteLine("Entrez les ID des tâches que vous voulez effectuer, séparés par des virgules (exemple: 1,3,5):");
                string input5 = Console.ReadLine();
                string[] inputIds = input5.Split(',');

                // Conversion des inputs en tableau d'entiers
                int[] ids = new int[inputIds.Length];
                for (int i = 0; i < inputIds.Length; i++)
                {
                    if (int.TryParse(inputIds[i], out int id))
                    {
                        ids[i] = id;
                    }
                    else
                    {
                        return ($"Entrée invalide pour l'ID: {inputIds[i]}");
                        
                    }
                }

                // Appel de RunTaskMultiple avec le tableau d'ID
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunTaskMultiple(ids);

                if (result)
                {
                    return("Sauvegarde de toutes les tâches sélectionnées terminée avec succès.");
                }
                else
                {
                    return("Un problème est survenu lors de la sauvegarde des tâches.");
                }

            }
            else
            {
                Console.WriteLine("Erreur");
                return "Erreur";
            }
        }

       
    }
}
