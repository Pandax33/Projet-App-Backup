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
            // Utiliser la factory pour créer le backup avec les paramètres fournis
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
                return "Delete fini";
            }
            else
            {
                return "Le fichier n'existe pas";
            }
        }

        public string EditTask(int taskId, string newDestination, string newSource, string newType)
        {
            bool result = BackupFactory.EditBackup(BackupFactory.GetBackupByIndex(taskId).Name, newDestination, newSource, newType);
            if (result)
            {
                return "Edit fini";
            }
            else
            {
                return "Le fichier n'existe pas";
            }
        }
    }
}
