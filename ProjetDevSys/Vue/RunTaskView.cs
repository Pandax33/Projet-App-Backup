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
                Console.WriteLine("Entrez l'ID de la tache que vous voulez effectuer");
                string input2 = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                Console.WriteLine(Convert.ToInt32(input2));
                //bool result = runSaveTask.RunTask(Convert.ToInt32(input2));
                return input2;
            }
            else if(input == "2")
            {
                Console.WriteLine("Entrez l'ID de la premiere tache que vous voulez effectuer");
                string input2 = Console.ReadLine();
                Console.WriteLine("Entrez l'ID de la derniere tache que vous voulez effectuer");
                string input3 = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunMultipleTask(Convert.ToInt32(input2), Convert.ToInt32(input3));
                return input2 + " " + input3;
            }
            else
            {
                Console.WriteLine("Erreur");
                return "Erreur";
            }
        }

       
    }
}
