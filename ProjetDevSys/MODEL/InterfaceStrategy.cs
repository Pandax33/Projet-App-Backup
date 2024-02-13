using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            // Check if the source directory exists
            if (File.Exists(backup.Source) || Directory.Exists(backup.Source))
            {
                // Be sure that the destination directory exists
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                }
                // Copy folder and under folders
                CopierDossier(backup.Source, backup.Destination, LogRealTime);
            }
            else
            {
                throw new DirectoryNotFoundException(ResourceHelper.GetString("InterfaceStrategy1"));
            }

        }

        private void CopierDossier(string sourceDir, string destinationDir, LogRealTime LogRealTime)
        {
            if (AppConstants.RunningBlockerProcess()) new Exception(ResourceHelper.GetString("InterfaceStrategy2"));

            //file case
            if (!(Directory.Exists(sourceDir)) && File.Exists(sourceDir))
            {

                string fileName = Path.GetFileName(sourceDir);
                string destinationFilePath = Path.Combine(destinationDir, fileName);
                FileInfo fileInfo = new FileInfo(sourceDir);
                long fileSize = fileInfo.Length;

                File.Copy(sourceDir, destinationFilePath, true);
                LogRealTime.Timestamp = DateTime.Now;
                LogRealTime.CurrentSourcePath = sourceDir;
                LogRealTime.CurrentTargetPath = destinationFilePath;
                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                LogRealTime.CreateLog();
            }

            else
            {
                // Copy every file from the source directory to the destination directory
                foreach (string fichierPath in Directory.GetFiles(sourceDir))
                {
                    string fileName = Path.GetFileName(fichierPath);
                    string destinationFilePath = Path.Combine(destinationDir, fileName);
                    FileInfo fileInfo = new FileInfo(fichierPath);
                    long fileSize = fileInfo.Length;

                    File.Copy(fichierPath, destinationFilePath, true);
                    LogRealTime.Timestamp = DateTime.Now;
                    LogRealTime.CurrentSourcePath = fichierPath;
                    LogRealTime.CurrentTargetPath = destinationFilePath;
                    LogRealTime.UpdateCurrentFileAndSize(fileSize);
                    LogRealTime.CreateLog();
                }

                // Copy all subdirectories recursively
                foreach (string dossierPath in Directory.GetDirectories(sourceDir))
                {
                    string folderName = Path.GetFileName(dossierPath);
                    string destinationFolderPath = Path.Combine(destinationDir, folderName);
                    if (!Directory.Exists(destinationFolderPath))
                    {
                        Directory.CreateDirectory(destinationFolderPath);
                    }
                }
            }
            return;
        }
    }

    class SaveDiffStrategy : IBackupStrategy
    {
       
        public void Save(Backup backup, LogRealTime LogRealTime)
        {
            if (File.Exists(backup.Source) || Directory.Exists(backup.Source))
            {
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                }

                // Call the recursive method to copy the files
                CopierDossierDifferenciel(backup.Source, backup.Destination, LogRealTime);
            }
            else
            {
                throw new DirectoryNotFoundException(ResourceHelper.GetString("InterfaceStrategy1"));
            }

        }

        private void CopierDossierDifferenciel(string sourceDir, string destinationDir,LogRealTime LogRealTime)
        {
            if (AppConstants.RunningBlockerProcess()) new Exception(ResourceHelper.GetString("InterfaceStrategy2"));

            //file case
            if (!(Directory.Exists(sourceDir)) && File.Exists(sourceDir))
            {

                string fileName = Path.GetFileName(sourceDir);
                string destinationFilePath = Path.Combine(destinationDir, fileName);
                FileInfo fileInfo = new FileInfo(sourceDir);
                long fileSize = fileInfo.Length;

                File.Copy(sourceDir, destinationFilePath, true);
                LogRealTime.Timestamp = DateTime.Now;
                LogRealTime.CurrentSourcePath = sourceDir;
                LogRealTime.CurrentTargetPath = destinationFilePath;
                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                LogRealTime.CreateLog();
            }
            else
            {
                // Copy every file from the source directory to the destination directory
                foreach (string fichierSource in Directory.GetFiles(sourceDir))
                {
                    string fileName = Path.GetFileName(fichierSource);
                    string fichierDestination = Path.Combine(destinationDir, fileName);
                    FileInfo fileInfo = new FileInfo(fichierSource);
                    long fileSize = fileInfo.Length;

                    // Do the copy only if the file does not exist or if the source file is more recent than the destination file
                    if (!File.Exists(fichierDestination) || File.GetLastWriteTime(fichierSource) > File.GetLastWriteTime(fichierDestination))
                    {
                        File.Copy(fichierSource, fichierDestination, true);
                        LogRealTime.Timestamp = DateTime.Now;
                        LogRealTime.CurrentSourcePath = fichierSource;
                        LogRealTime.CurrentTargetPath = fichierDestination;
                        LogRealTime.UpdateCurrentFileAndSize(fileSize);
                        LogRealTime.CreateLog();
                    }
                }

                // Recursively call the method for each subdirectory
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
            return;
        }

    }
}
