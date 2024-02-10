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
            Console.WriteLine(ResourceHelper.GetString("RunTaskView1"));
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
                Console.WriteLine(ResourceHelper.GetString("RunTaskView2"));
                string input2 = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunTask(Convert.ToInt32(input2));
                if(result) 
                {
                    return ResourceHelper.GetString("RunTaskView3");
                }
                else
                {
                    return ResourceHelper.GetString("RunTaskView4");
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

                Console.WriteLine(ResourceHelper.GetString("RunTaskView5"));
                string input2 = Console.ReadLine();
                string[] inputs = input2.Split(',');

                if (inputs.Length == 2 && int.TryParse(inputs[0], out int startId) && int.TryParse(inputs[1], out int endId))
                {
                    RunSaveTask runSaveTask = new RunSaveTask();
                    bool result = runSaveTask.RunMultipleTask(startId, endId);

                    if (result)
                    {
                        return ResourceHelper.GetString("RunTaskView6");
                    }
                    else
                    {
                        return ResourceHelper.GetString("RunTaskView7");
                    }
                }
                else
                {
                    return ResourceHelper.GetString("RunTaskView8");
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

                Console.WriteLine(ResourceHelper.GetString("RunTaskView9"));
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
                        return (ResourceHelper.GetString("RunTaskView10"));
                        
                    }
                }

                // Appel de RunTaskMultiple avec le tableau d'ID
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunTaskMultiple(ids);

                if (result)
                {
                    return(ResourceHelper.GetString("RunTaskView11"));
                }
                else
                {
                    return(ResourceHelper.GetString("RunTaskView12"));
                }

            }
            else
            {
                return ResourceHelper.GetString("RunTaskView13");
            }
        }

       
    }
}
