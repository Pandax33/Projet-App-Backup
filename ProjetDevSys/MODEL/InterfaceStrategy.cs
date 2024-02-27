using Newtonsoft.Json.Linq;
using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjetDevSys.Model
{
    interface IBackupStrategy
    {
        void Save(Backup backupContexte, LogRealTime LogRealTime);
        void SavePrio(Backup backupContexte, LogRealTime LogRealTime);
    }

    class SaveCompleteStrategy : IBackupStrategy
    {

        public void Save(Backup backup, LogRealTime LogRealTime)
        {
            // Check if the source directory exists
            if (File.Exists(backup.Source) || Directory.Exists(backup.Source))
            {
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                }
                // Copy folder and under folders
                CopierDossier(backup.Source, backup.Destination, LogRealTime, backup.Name);
            }
            else
            {
                throw new DirectoryNotFoundException(ResourceHelper.GetString("InterfaceStrategy1"));
            }

        }

        public void SavePrio(Backup backup, LogRealTime LogRealTime)
        {
            // Check if the source directory exists
            if (File.Exists(backup.Source) || Directory.Exists(backup.Source))
            {
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                }
                // Copy folder and under folders
                CopierDossierPrio(backup.Source, backup.Destination, LogRealTime, backup.Name);
            }
            else
            {
                throw new DirectoryNotFoundException(ResourceHelper.GetString("InterfaceStrategy1"));
            }

        }

        private void CopierDossier(string sourceDir, string destinationDir, LogRealTime LogRealTime, string name)
        {
            if (!Directory.Exists(sourceDir) && File.Exists(sourceDir)) // Cas copie fichier unique
            {
                FileInfo fileInfo = new FileInfo(sourceDir);
                if (!AppConstants.ExtensionListPriority.Contains(fileInfo.Extension.ToLower())) // Exclut les extensions prioritaires
                {
                    FileUtility.CopierFichier(sourceDir, destinationDir, LogRealTime, name);
                }
            }
            else // Cas copie dossier
            {
                foreach (string fichierPath in Directory.GetFiles(sourceDir))
                {
                    FileInfo fileInfo = new FileInfo(fichierPath);
                    AppConstants.BackupPauseHandles[name].WaitOne();
                    AppConstants.processEvent.WaitOne();
                    AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                    if (cts.Token.IsCancellationRequested)
                    {
                        return;
                    }
                    if (!AppConstants.ExtensionListPriority.Contains(fileInfo.Extension.ToLower())) // Exclut les extensions prioritaires
                    {
                        if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                        {
                            FileUtility.TraiterEtCopierFichierCrypte(fichierPath, destinationDir, LogRealTime, name);
                        }
                        else
                        {
                            FileUtility.CopierFichier(fichierPath, destinationDir, LogRealTime, name);
                        }
                    }
                }

                foreach (string dossierPath in Directory.GetDirectories(sourceDir))
                {
                    string destinationFolderPath = Path.Combine(destinationDir, Path.GetFileName(dossierPath));
                    Directory.CreateDirectory(destinationFolderPath);
                    CopierDossier(dossierPath, destinationFolderPath, LogRealTime, name);
                }
            }
        }

        private void CopierDossierPrio(string sourceDir, string destinationDir, LogRealTime LogRealTime, string name)
        {
            if (!Directory.Exists(sourceDir) && File.Exists(sourceDir))
            {
                AppConstants.BackupPauseHandles[name].WaitOne();
                AppConstants.processEvent.WaitOne();
                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                if (cts.Token.IsCancellationRequested)
                {
                    return;
                }

                // Vérifie si le fichier unique doit être copié en fonction de son extension.
                CopierFichierSiPrioritaire(sourceDir, destinationDir, LogRealTime, name);
            }
            else
            {
                foreach (string fichierPath in Directory.GetFiles(sourceDir))
                {
                    AppConstants.BackupPauseHandles[name].WaitOne();
                    AppConstants.processEvent.WaitOne();
                    AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                    if (cts.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    FileInfo fileInfo = new FileInfo(fichierPath);
                    if (AppConstants.ExtensionListPriority.Contains(fileInfo.Extension.ToLower())) // Filtre par extension prioritaire
                    {
                        if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                        {
                            FileUtility.TraiterEtCopierFichierCrypte(fichierPath, destinationDir, LogRealTime, name);
                        }
                        else
                        {
                            FileUtility.CopierFichier(fichierPath, destinationDir, LogRealTime, name);
                        }
                    }
                }

                foreach (string dossierPath in Directory.GetDirectories(sourceDir))
                {
                    string destinationFolderPath = Path.Combine(destinationDir, Path.GetFileName(dossierPath));
                    Directory.CreateDirectory(destinationFolderPath);
                    CopierDossierPrio(dossierPath, destinationFolderPath, LogRealTime, name);
                }
            }
        }

        private void CopierFichierSiPrioritaire(string sourceFilePath, string destinationDir, LogRealTime LogRealTime, string name)
        {
            FileInfo fileInfo = new FileInfo(sourceFilePath);
            if (AppConstants.ExtensionListPriority.Contains(fileInfo.Extension.ToLower()))
            {
                FileUtility.CopierFichier(sourceFilePath, destinationDir, LogRealTime, name);
            }
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

                CopierDossierDifferenciel(backup.Source, backup.Destination, LogRealTime, backup.Name);
            }
            else
            {
                throw new DirectoryNotFoundException($"La source spécifiée n'existe pas : {backup.Source}");
            }
        }

        public void SavePrio(Backup backup, LogRealTime LogRealTime)
        {
            if (File.Exists(backup.Source) || Directory.Exists(backup.Source))
            {
                if (!Directory.Exists(backup.Destination))
                {
                    Directory.CreateDirectory(backup.Destination);
                }

                CopierDossierDifferencielPrio(backup.Source, backup.Destination, LogRealTime, backup.Name);
            }
            else
            {
                throw new DirectoryNotFoundException($"La source spécifiée n'existe pas : {backup.Source}");
            }
        }

        private void CopierDossierDifferenciel(string sourceDir, string destinationDir, LogRealTime LogRealTime, string name)
        {
            if (!Directory.Exists(sourceDir) && File.Exists(sourceDir))
            {
                CopierFichierSiNonPrioritaireEtNecessaire(sourceDir, destinationDir, LogRealTime, name);
            }
            else
            {
                foreach (string fichierSource in Directory.GetFiles(sourceDir))
                {
                    AppConstants.BackupPauseHandles[name].WaitOne();
                    AppConstants.processEvent.WaitOne();
                    AppConstants.priorityEvent.WaitOne();
                    AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                    if (cts.Token.IsCancellationRequested)
                    {
                        return; // Si stop on ferme la fonction
                    }
                    CopierFichierSiNonPrioritaireEtNecessaire(fichierSource, destinationDir, LogRealTime, name);
                }

                foreach (string dossierSource in Directory.GetDirectories(sourceDir))
                {
                    string dossierDestination = Path.Combine(destinationDir, Path.GetFileName(dossierSource));
                    if (!Directory.Exists(dossierDestination))
                    {
                        Directory.CreateDirectory(dossierDestination);
                    }
                    CopierDossierDifferenciel(dossierSource, dossierDestination, LogRealTime, name);
                }
            }
        }

        private void CopierFichierSiNonPrioritaireEtNecessaire(string sourceFilePath, string destinationDir, LogRealTime LogRealTime, string name)
        {
            FileInfo fileInfo = new FileInfo(sourceFilePath);
            if (!AppConstants.ExtensionListPriority.Contains(fileInfo.Extension.ToLower()) &&(!File.Exists(Path.Combine(destinationDir, fileInfo.Name)) ||fileInfo.LastWriteTimeUtc > new FileInfo(Path.Combine(destinationDir, fileInfo.Name)).LastWriteTimeUtc))
            {
                if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                {
                    FileUtility.TraiterEtCopierFichierCrypte(sourceFilePath, destinationDir, LogRealTime, name);
                }
                else
                {
                    FileUtility.CopierFichier(sourceFilePath, destinationDir, LogRealTime, name);
                }
            }
        }

        private void CopierDossierDifferencielPrio(string sourceDir, string destinationDir, LogRealTime LogRealTime, string name)
        {
            // La logique de base reste similaire, mais avec un filtre supplémentaire sur les extensions prioritaires
            if (!Directory.Exists(sourceDir) && File.Exists(sourceDir))
            {
                CopierFichierSiPrioritaireEtNecessaire(sourceDir, destinationDir, LogRealTime, name);
            }
            else
            {
                foreach (string fichierSource in Directory.GetFiles(sourceDir))
                {
                    AppConstants.BackupPauseHandles[name].WaitOne();
                    AppConstants.processEvent.WaitOne();
                    AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                    if (cts.Token.IsCancellationRequested)
                    {
                        return;
                    }
                    CopierFichierSiPrioritaireEtNecessaire(fichierSource, destinationDir, LogRealTime, name);
                }

                foreach (string dossierSource in Directory.GetDirectories(sourceDir))
                {
                    string dossierDestination = Path.Combine(destinationDir, Path.GetFileName(dossierSource));
                    if (!Directory.Exists(dossierDestination))
                    {
                        Directory.CreateDirectory(dossierDestination);
                    }
                    CopierDossierDifferencielPrio(dossierSource, dossierDestination, LogRealTime, name);
                }
            }
        }

        private void CopierFichierSiPrioritaireEtNecessaire(string sourceFilePath, string destinationDir, LogRealTime LogRealTime, string name)
        {
            FileInfo fileInfo = new FileInfo(sourceFilePath);
            if (AppConstants.ExtensionListPriority.Contains(fileInfo.Extension.ToLower()) &&(!File.Exists(Path.Combine(destinationDir, fileInfo.Name)) ||fileInfo.LastWriteTimeUtc > new FileInfo(Path.Combine(destinationDir, fileInfo.Name)).LastWriteTimeUtc))
            {
                if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                {
                    FileUtility.TraiterEtCopierFichierCrypte(sourceFilePath, destinationDir, LogRealTime, name);
                }
                else
                {
                    FileUtility.CopierFichier(sourceFilePath, destinationDir, LogRealTime, name);
                }
            }
        }
    }
}
