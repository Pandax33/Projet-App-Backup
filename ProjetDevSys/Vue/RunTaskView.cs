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
                        Console.WriteLine($"{index}. [{backup.Name}]");
                    index++;
                }

                Console.WriteLine(ResourceHelper.GetString("RunTaskView2"));
                string inputId = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                int inputIdint = AppConstants.StringToInt(inputId);
                if (runSaveTask.VerifyId(inputIdint))
                {
            
                    return runSaveTask.RunTask(inputIdint);
                    
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
                        Console.WriteLine($"{index}. [{backup.Name}]");
                        index++;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ResourceHelper.GetString("RunTaskView21"));
                    Console.ResetColor();
                    return ResourceHelper.GetString("RunTaskView18");
                }

                // Ask for the start and end ID
                Console.WriteLine(ResourceHelper.GetString("RunTaskView5"));
                string input2 = Console.ReadLine();
                RunSaveTask runSaveTask = new RunSaveTask();
                // Check if the input is valid
                if (runSaveTask.VerifyContinueId(input2))
                {
                    string[] inputs = input2.Split(',');
                    int startId = AppConstants.StringToInt(inputs[0]);
                    int endId = AppConstants.StringToInt(inputs[1]);
                    // Check if the start ID is smaller than the end ID
   
                    return runSaveTask.RunMultipleTask(startId, endId);
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
                IEnumerable<Backup> BackupList2 = BackupFactory.GetAllBackups();
                if (BackupList2 != null && BackupList2.Any())
                {
                    int index = 0;
                    foreach (Backup backup in BackupList2)
                    {
                        Console.WriteLine($"{index}. [{backup.Name}]");
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
                RunSaveTask runSaveTask2 = new RunSaveTask();
                // Convert the input to an array of IDs
                int[] ids = new int[inputIds.Length];
                for (int i = 0; i < inputIds.Length; i++)
                {
                    int id = AppConstants.StringToInt(inputIds[i]);
                    if (runSaveTask2.VerifyId(id))
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
                
                return runSaveTask2.RunTaskMultiple(ids);



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
