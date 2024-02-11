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
            Console.WriteLine(ResourceHelper.GetString("Form1"));
            Console.WriteLine(ResourceHelper.GetString("RunTaskView1"));
            Console.WriteLine(ResourceHelper.GetString("RunTaskView14"));
            Console.WriteLine(ResourceHelper.GetString("RunTaskView15"));
            Console.WriteLine(ResourceHelper.GetString("RunTaskView16"));
            Console.WriteLine(ResourceHelper.GetString("Form1"));
            string input = Console.ReadLine();
            if(input == "1")
            {
                IEnumerable<Backup> BackupList = BackupFactory.GetAllBackups();
                if (BackupList == null || !BackupList.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView24"));
                    Console.ResetColor();
                    return ResourceHelper.GetString("RunTaskView18");
                }

                // Print the list of backups
                int index = 0;
                foreach (Backup backup in BackupList)
                {
                    Console.WriteLine($"ID : {index} Save: {backup.Name}");
                    index++;
                }

                Console.WriteLine(ResourceHelper.GetString("RunTaskView2"));
                if (int.TryParse(Console.ReadLine(), out int inputId) && inputId >= 0 && inputId < BackupList.Count())
                {
                    RunSaveTask runSaveTask = new RunSaveTask();
                    bool result = runSaveTask.RunTask(inputId);
                    if (result)
                    {
                        return ResourceHelper.GetString("RunTaskView3");
                    }
                    else
                    {
                        return ResourceHelper.GetString("RunTaskView4");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ResourceHelper.GetString("RunTaskView20"));
                    Console.ResetColor();
                    return ResourceHelper.GetString("RunTaskView18");
                }

            }
            else if(input == "2")
            {
                IEnumerable<Backup> BackupList = BackupFactory.GetAllBackups();
                if (BackupList != null && BackupList.Any())
                {
                    int index = 0;
                    foreach (Backup backup in BackupList)
                    {
                        Console.WriteLine($"ID : {index} Save: {backup.Name}");
                        index++;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Aucune sauvegarde enregistrée");
                    Console.ResetColor();
                    return ResourceHelper.GetString("RunTaskView18");
                }

                // Ask for the start and end ID
                Console.WriteLine(ResourceHelper.GetString("RunTaskView5"));
                string input2 = Console.ReadLine();
                string[] inputs = input2.Split(',');

                // Check if the input is valid
                if (inputs.Length == 2 && int.TryParse(inputs[0], out int startId) && int.TryParse(inputs[1], out int endId))
                {
                    // Check if the start ID is smaller than the end ID
                    if (startId >= endId)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("RunTaskView17"));
                        Console.ResetColor();
                        return ResourceHelper.GetString("RunTaskView18");
                    }

                    // Check if the start and end ID are valid
                    if (startId < 0 || endId >= BackupList.Count())
                    {
                        return ResourceHelper.GetString("RunTaskView19");
                    }

                    // Do the task for the range of IDs
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ResourceHelper.GetString("RunTaskView8"));
                    Console.ResetColor();
                    return ResourceHelper.GetString("RunTaskView18");
                }


            }
            else if(input == "3")
            {
                IEnumerable<Backup> BackupList = BackupFactory.GetAllBackups();
                if (BackupList != null && BackupList.Any())
                {
                    int index = 0;
                    foreach (Backup backup in BackupList)
                    {
                        Console.WriteLine($"ID : {index} Save: {backup.Name}");
                        index++;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ResourceHelper.GetString("GestionTaskView24"));
                    Console.ResetColor();
                    return ResourceHelper.GetString("RunTaskView18");
                }

                Console.WriteLine(ResourceHelper.GetString("RunTaskView9"));
                string input5 = Console.ReadLine();
                string[] inputIds = input5.Split(',');

                // Convert the input to an array of IDs
                int[] ids = new int[inputIds.Length];
                for (int i = 0; i < inputIds.Length; i++)
                {
                    if (int.TryParse(inputIds[i], out int id) && id >= 0 && id < BackupList.Count())
                    {
                        ids[i] = id;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ResourceHelper.GetString("RunTaskView10"));
                        Console.ResetColor();
                        return ResourceHelper.GetString("RunTaskView18");
                    }
                }

                // Call the task to run the multiple tasks
                RunSaveTask runSaveTask = new RunSaveTask();
                bool result = runSaveTask.RunTaskMultiple(ids);

                if (result)
                {
                    return ResourceHelper.GetString("RunTaskView11");
                }
                else
                {
                    return ResourceHelper.GetString("RunTaskView12");
                }


            }
            else if (input == "4")
            {
                return (ResourceHelper.GetString("GestionTaskView21"));
            }
            else
            {
                return ResourceHelper.GetString("RunTaskView13");
            }
        }

       
    }
}
