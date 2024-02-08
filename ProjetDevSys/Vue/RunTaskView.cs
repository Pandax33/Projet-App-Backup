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
                    index = index + 1;
                }
                Console.WriteLine("Entrez l'ID de la premiere tache que vous voulez effectuer");
                string input2 = Console.ReadLine();
                Console.WriteLine("Entrez l'ID de la derniere tache que vous voulez effectuer");
                string input3 = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunMultipleTask(Convert.ToInt32(input2), Convert.ToInt32(input3));
                if (result)
                {
                    return "Sauvegarde fini";
                }
                else
                {
                    return "Un probleme est survenue";
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
