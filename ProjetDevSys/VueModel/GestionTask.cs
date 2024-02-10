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
        public bool CreateTask(string fileName, string sourcePath, string destinationPath, string backupType)
        {
            Backup backup = BackupFactory.CreateBackup(fileName, sourcePath, destinationPath, backupType);

            if (backup != null)
            {

                return true; 
            }
            else
            {
                return false; 
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
            // Récupère le backup existant par son index
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



        public bool VerifSource(string path)
        {
            return Directory.Exists(path);
        }
    }
}
