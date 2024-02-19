using ProjetDevSys.Model;
using ProjetDevSys.MODEL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetDevSys.VueModel
{
    public class RunSaveTask
    {
        public string RunTask(int id)
        {
            if (AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");
            Backup backup = BackupFactory.GetBackupByIndex(id);
            BackupJob backupJob = new BackupJob(backup);
            Console.WriteLine(backup.Name);
            try
            {
                backupJob.Save();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return ResourceHelper.GetString("RunTaskView3");
        }

        public string RunMultipleTask(int idDebut, int idFin)
        {
            if (AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");

            IEnumerable<Backup> allBackups = BackupFactory.GetBackupsInRange(idDebut, idFin);
            int tasksCount = allBackups.Count();
            if (tasksCount == 0) return "No backups found in the specified range.";

            ManualResetEvent[] doneEvents = new ManualResetEvent[tasksCount];
            int i = 0;

            foreach (Backup backup in allBackups)
            {
                int currentIndex = i; // Capture de l'index actuel de manière explicite
                doneEvents[currentIndex] = new ManualResetEvent(false);
                BackupJob backupJob = new BackupJob(backup);
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        backupJob.Save();
                        AppConstants.backupProgress.TryAdd(backup.Name, 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {
                        doneEvents[currentIndex].Set(); // Utiliser currentIndex ici
                    }
                });
                i++;
            }

            // Attendez que tous les travaux de sauvegarde soient terminés
            WaitHandle.WaitAll(doneEvents);
            return ResourceHelper.GetString("RunTaskView6");
        }
        public string RunTaskMultiple(int[] tab)
        {
            if (AppConstants.RunningBlockerProcess()) return ResourceHelper.GetString("RunTaskView22");

            int tasksCount = tab.Length;
            if (tasksCount == 0) return "No backups specified.";

            ManualResetEvent[] doneEvents = new ManualResetEvent[tasksCount];
            int i = 0;

            foreach (int id in tab)
            {
                Backup backup = BackupFactory.GetBackupByIndex(id);
                if (backup == null)
                {
                    continue; // ou retourner une erreur spécifique si un backup n'est pas trouvé
                }

                doneEvents[i] = new ManualResetEvent(false);
                BackupJob backupJob = new BackupJob(backup);

                int currentIndex = i; // Capture de l'index actuel de manière explicite pour l'utiliser dans la lambda

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        backupJob.Save();
                        AppConstants.backupProgress.TryAdd(backup.Name, 0); // Assurez-vous que cette opération est thread-safe
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during backup for {backup.Name}: {ex}");
                    }
                    finally
                    {
                        doneEvents[currentIndex].Set(); // Signal que cette tâche de sauvegarde est terminée
                    }
                });

                i++;
            }

            // Attendez que tous les travaux de sauvegarde soient terminés
            WaitHandle.WaitAll(doneEvents);

            return ResourceHelper.GetString("RunTaskView11");
        }

        public bool VerifyId(int id)
        {

            if (BackupFactory.GetBackupByIndex(id) != null)
            {
                return true;
            }
            return false;
        }

        public bool VerifyContinueId(string input)
        {
            string[] inputs = input.Split(',');
            if(inputs.Length == 2)
            {
                int idDebut = AppConstants.StringToInt(inputs[0]);
                int idFin = AppConstants.StringToInt(inputs[1]);
                if (idDebut != -1 && idFin != -1)
                {
                    if (idDebut < idFin)
                    {
                        if(VerifyId(idDebut) && VerifyId(idFin))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
