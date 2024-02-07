using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.Model
{
    interface IBackupStrategy
    {
        void Save(Backup backupContexte, LogRealTime LogRealTime);
    }

    class SaveCompleteStrategy : IBackupStrategy
    {

        public void Save(Backup backup, LogRealTime LogRealTime)
        {
            // Vérifiez si le dossier source existe
            if (Directory.Exists(backup.Source))
            {
                // Assurez-vous que le dossier de destination existe, sinon créez-le
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                    Console.WriteLine("Fin de la copie");

                }
                // Copiez tous les fichiers et sous-dossiers récursivement
                Console.WriteLine("Début de la copie");
                CopierDossier(backup.Source, backup.Destination, LogRealTime);
                Console.WriteLine("Fin de la copie");
            }
            else
            {
                Console.WriteLine("Le dossier source n'existe pas.");
            }
        }

        private void CopierDossier(string sourceDir, string destinationDir, LogRealTime LogRealTime)
        {
            // Copiez tous les fichiers du dossier
            foreach (string fichierPath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(fichierPath);
                string destinationFilePath = Path.Combine(destinationDir, fileName);
                FileInfo fileInfo = new FileInfo(fichierPath);
                long fileSize = fileInfo.Length;

                File.Copy(fichierPath, destinationFilePath, true);
                Console.WriteLine($"Copié: {fichierPath} -> {destinationFilePath}");
                LogRealTime.Timestamp = DateTime.Now;
                LogRealTime.CurrentSourcePath = fichierPath;
                LogRealTime.CurrentTargetPath = destinationFilePath;
                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                LogRealTime.CreateLog();
            }

            // Copiez récursivement tous les sous-dossiers
            foreach (string dossierPath in Directory.GetDirectories(sourceDir))
            {
                string folderName = Path.GetFileName(dossierPath);
                string destinationFolderPath = Path.Combine(destinationDir, folderName);
                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }

                CopierDossier(dossierPath, destinationFolderPath, LogRealTime);
            }
        }
    }

    class SaveDiffStrategy : IBackupStrategy
    {
       
        public void Save(Backup backup, LogRealTime LogRealTime)
        {
            if (Directory.Exists(backup.Source))
            {
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                }

                // Appelle CopierDossier pour une copie différentielle
                Console.WriteLine("Début de la copie");
                CopierDossierDifferenciel(backup.Source, backup.Destination, LogRealTime) ;
                Console.WriteLine("Fin de la copie");
            }
            else
            {
                Console.WriteLine("Le dossier source n'existe pas.");
            }
        }

        private void CopierDossierDifferenciel(string sourceDir, string destinationDir,LogRealTime LogRealTime)
        {
            // Copier tous les fichiers du dossier source vers le dossier de destination s'ils sont nouveaux ou modifiés
            foreach (string fichierSource in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(fichierSource);
                string fichierDestination = Path.Combine(destinationDir, fileName);
                FileInfo fileInfo = new FileInfo(fichierSource);
                long fileSize = fileInfo.Length;

                // Effectuez la copie si le fichier de destination n'existe pas ou si le fichier source est plus récent
                if (!File.Exists(fichierDestination) || File.GetLastWriteTime(fichierSource) > File.GetLastWriteTime(fichierDestination))
                {
                    File.Copy(fichierSource, fichierDestination, true);
                    Console.WriteLine($"Copié: {fichierSource} -> {fichierDestination}");
                    LogRealTime.Timestamp = DateTime.Now;
                    LogRealTime.CurrentSourcePath = fichierSource;
                    LogRealTime.CurrentTargetPath = fichierDestination;
                    LogRealTime.UpdateCurrentFileAndSize(fileSize);
                    LogRealTime.CreateLog();
                }
            }

            // Récursivement copier tous les sous-dossiers
            foreach (string dossierSource in Directory.GetDirectories(sourceDir))
            {
                string nomDossier = Path.GetFileName(dossierSource);
                string dossierDestination = Path.Combine(destinationDir, nomDossier);

                if (!Directory.Exists(dossierDestination))
                {
                    Directory.CreateDirectory(dossierDestination);
                }

                CopierDossierDifferenciel(dossierSource, dossierDestination, LogRealTime);
            }
        }

    }
}
