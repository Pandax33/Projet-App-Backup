using ProjetDevSys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.VueModel
{
    public class GestionTask
    {
        public string CreateTask(string fileName, string sourcePath, string destinationPath, string backupType)
        {

            if ((BackupFactory.GetBackupByName(fileName) != null) || String.IsNullOrWhiteSpace(fileName)) return ResourceHelper.GetString("GestionTaskView35");

            Backup backup = BackupFactory.CreateBackup(fileName, sourcePath, destinationPath, backupType);

            if (backup != null)
            {

                return ResourceHelper.GetString("GestionTaskView3");
            }
            else
            {
                return ResourceHelper.GetString("GestionTaskView4");
            }
        }

        public string DeleteTask(int  taskId)
        {

            bool result = BackupFactory.DeleteBackup(BackupFactory.GetBackupByIndex(taskId).Name);
            if(result)
            {
                return ResourceHelper.GetString("GestionTask1");
            }
            else
            {
                return ResourceHelper.GetString("GestionTask2");
            }
        }

        public string EditTask(int taskId, string newDestination, string newSource, string newType)
        {
            bool result = BackupFactory.EditBackup(BackupFactory.GetBackupByIndex(taskId).Name, newDestination, newSource, newType);
            if (result)
            {
                return ResourceHelper.GetString("GestionTask3");
            }
            else
            {
                return ResourceHelper.GetString("GestionTask4");
            }
        }
        public string EditNewDestination(int taskId, string newDestination)
        {
            // Get the backup by index
            Backup backup = BackupFactory.GetBackupByIndex(taskId);
            if (backup == null)
            {
                return ResourceHelper.GetString("GestionTask4");
            }

            bool result = BackupFactory.EditBackup(backup.Name, newDestination, backup.Source, backup.Type);
            if (result)
            {
                return ResourceHelper.GetString("GestionTask5"); 
            }
            else
            {
                return ResourceHelper.GetString("GestionTask4");
            }
        }


        public string EditNewSource(int taskId, string newSource)
        {
            Backup backup = BackupFactory.GetBackupByIndex(taskId);
            if (backup == null)
            {
                return ResourceHelper.GetString("GestionTask4");
            }

            bool result = BackupFactory.EditBackup(backup.Name, backup.Destination, newSource, backup.Type);
            if (result)
            {
                return ResourceHelper.GetString("GestionTask6");
            }
            else
            {
                return ResourceHelper.GetString("GestionTask4");
            }
        }

        public string EditNewType(int taskId, string newType)
        {
        
            Backup backup = BackupFactory.GetBackupByIndex(taskId);
            if (backup == null)
            {
                return ResourceHelper.GetString("GestionTask4");
            }
        
            bool result = BackupFactory.EditBackup(backup.Name, backup.Destination, backup.Source, newType);
            if (result)
            {
                return ResourceHelper.GetString("GestionTask7");
            }
            else
            {
                return ResourceHelper.GetString("GestionTask4");
            }
        }

        public bool VerifyId(int id)
        {

            if (BackupFactory.GetBackupByIndex(id) != null)
            {
                return true;
            }
            return false;
        }

        public bool verifInputBackupType(string input)
        {
            if (input == "A" || input == "B")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool VerifSource(string path)
        {
            return Directory.Exists(path);
        }
    }
}
