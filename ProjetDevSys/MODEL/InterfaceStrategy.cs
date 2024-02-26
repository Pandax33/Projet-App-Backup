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
                CopierDossier(backup.Source, backup.Destination, LogRealTime, backup.Name);
            }
            else
            {
                throw new DirectoryNotFoundException(ResourceHelper.GetString("InterfaceStrategy1"));
            }

        }

        private void CopierDossier(string sourceDir, string destinationDir, LogRealTime LogRealTime, string name)
        {

            //file case
            if (!(Directory.Exists(sourceDir)) && File.Exists(sourceDir))
            {

                string fileName = Path.GetFileName(sourceDir);
                string destinationFilePath = Path.Combine(destinationDir, fileName);
                FileInfo fileInfo = new FileInfo(sourceDir);
                long fileSize = fileInfo.Length;

                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                if (cts.Token.IsCancellationRequested)
                {
                    return;
                }
                AppConstants.BackupPauseHandles[name].WaitOne();
                File.Copy(sourceDir, destinationFilePath, true);
                LogRealTime.Timestamp = DateTime.Now;
                LogRealTime.CurrentSourcePath = sourceDir;
                LogRealTime.CurrentTargetPath = destinationFilePath;
                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");

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
                    if (fileSize > AppConstants.FileSize)
                    {
                        AppConstants.sizeMutex.WaitOne();
                        try
                        {
                            if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                            {
                                string executablePath = AppConstants.CryptPath;
                                string fichierPathCrypto = fichierPath + ".crypto";
                                string arguments = $" {fichierPath} {fichierPathCrypto} {AppConstants.KeyCrypt}";

                                ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
                                {
                                    RedirectStandardOutput = true,
                                };
                                AppConstants.BackupPauseHandles[name].WaitOne();
                                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                                if (cts.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                using (Process process = new Process())
                                {
                                    process.StartInfo = startInfo;
                                    process.Start();

                                    // Read the output of the process
                                    string Timecrypt = process.StandardOutput.ReadToEnd();
                                    File.Copy(fichierPathCrypto, destinationFilePath, true);
                                    File.Delete(fichierPathCrypto);
                                    LogRealTime.Timestamp = DateTime.Now;
                                    LogRealTime.CurrentSourcePath = fichierPath;
                                    LogRealTime.CurrentTargetPath = destinationFilePath;
                                    LogRealTime.TimeCrypt = Timecrypt;
                                    LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                    AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                                    Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                    LogRealTime.CreateLog();
                                }

                            }
                            else
                            {
                                AppConstants.BackupPauseHandles[name].WaitOne();
                                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                                if (cts.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                File.Copy(fichierPath, destinationFilePath, true);
                                LogRealTime.Timestamp = DateTime.Now;
                                LogRealTime.CurrentSourcePath = fichierPath;
                                LogRealTime.CurrentTargetPath = destinationFilePath;
                                LogRealTime.TimeCrypt = "0";
                                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                LogRealTime.CreateLog();
                            }
                        }
                        finally
                        {
                            AppConstants.sizeMutex.ReleaseMutex();
                        }
                    }
                    else
                    {
                        if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                        {
                            string executablePath = AppConstants.CryptPath;
                            string fichierPathCrypto = fichierPath + ".crypto";
                            string arguments = $" {fichierPath} {fichierPathCrypto} {AppConstants.KeyCrypt}";

                            ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
                            {
                                RedirectStandardOutput = true,
                            };
                            AppConstants.BackupPauseHandles[name].WaitOne();
                            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                            if (cts.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            using (Process process = new Process())
                            {
                                process.StartInfo = startInfo;
                                process.Start();

                                // Read the output of the process
                                string Timecrypt = process.StandardOutput.ReadToEnd();
                                File.Copy(fichierPathCrypto, destinationFilePath, true);
                                File.Delete(fichierPathCrypto);
                                LogRealTime.Timestamp = DateTime.Now;
                                LogRealTime.CurrentSourcePath = fichierPath;
                                LogRealTime.CurrentTargetPath = destinationFilePath;
                                LogRealTime.TimeCrypt = Timecrypt;
                                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                LogRealTime.CreateLog();
                            }

                        }
                        else
                        {
                            AppConstants.BackupPauseHandles[name].WaitOne();
                            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                            if (cts.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            File.Copy(fichierPath, destinationFilePath, true);
                            LogRealTime.Timestamp = DateTime.Now;
                            LogRealTime.CurrentSourcePath = fichierPath;
                            LogRealTime.CurrentTargetPath = destinationFilePath;
                            LogRealTime.TimeCrypt = "0";
                            LogRealTime.UpdateCurrentFileAndSize(fileSize);
                            AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                            Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                            LogRealTime.CreateLog();
                        }
                    }
                    
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
                    CopierDossier(dossierPath, destinationFolderPath, LogRealTime,name);
                }
            }
            return;
        }

        private void CopierDossierPrio(string sourceDir, string destinationDir, LogRealTime LogRealTime, string name)
        {

            //file case
            if (!(Directory.Exists(sourceDir)) && File.Exists(sourceDir))
            {

                string fileName = Path.GetFileName(sourceDir);
                string destinationFilePath = Path.Combine(destinationDir, fileName);
                FileInfo fileInfo = new FileInfo(sourceDir);
                long fileSize = fileInfo.Length;

                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                if (cts.Token.IsCancellationRequested)
                {
                    return;
                }
                AppConstants.BackupPauseHandles[name].WaitOne();
                File.Copy(sourceDir, destinationFilePath, true);
                LogRealTime.Timestamp = DateTime.Now;
                LogRealTime.CurrentSourcePath = sourceDir;
                LogRealTime.CurrentTargetPath = destinationFilePath;
                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");

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
                    if (fileSize > AppConstants.FileSize)
                    {
                        AppConstants.sizeMutex.WaitOne();
                        try
                        {
                            if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                            {
                                string executablePath = AppConstants.CryptPath;
                                string fichierPathCrypto = fichierPath + ".crypto";
                                string arguments = $" {fichierPath} {fichierPathCrypto} {AppConstants.KeyCrypt}";

                                ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
                                {
                                    RedirectStandardOutput = true,
                                };
                                AppConstants.BackupPauseHandles[name].WaitOne();
                                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                                if (cts.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                using (Process process = new Process())
                                {
                                    process.StartInfo = startInfo;
                                    process.Start();

                                    // Read the output of the process
                                    string Timecrypt = process.StandardOutput.ReadToEnd();
                                    File.Copy(fichierPathCrypto, destinationFilePath, true);
                                    File.Delete(fichierPathCrypto);
                                    LogRealTime.Timestamp = DateTime.Now;
                                    LogRealTime.CurrentSourcePath = fichierPath;
                                    LogRealTime.CurrentTargetPath = destinationFilePath;
                                    LogRealTime.TimeCrypt = Timecrypt;
                                    LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                    AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                                    Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                    LogRealTime.CreateLog();
                                }

                            }
                            else
                            {
                                AppConstants.BackupPauseHandles[name].WaitOne();
                                AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                                if (cts.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                File.Copy(fichierPath, destinationFilePath, true);
                                LogRealTime.Timestamp = DateTime.Now;
                                LogRealTime.CurrentSourcePath = fichierPath;
                                LogRealTime.CurrentTargetPath = destinationFilePath;
                                LogRealTime.TimeCrypt = "0";
                                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                LogRealTime.CreateLog();
                            }
                        }
                        finally
                        {
                            AppConstants.sizeMutex.ReleaseMutex();
                        }
                    }
                    else
                    {
                        if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                        {
                            string executablePath = AppConstants.CryptPath;
                            string fichierPathCrypto = fichierPath + ".crypto";
                            string arguments = $" {fichierPath} {fichierPathCrypto} {AppConstants.KeyCrypt}";

                            ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
                            {
                                RedirectStandardOutput = true,
                            };
                            AppConstants.BackupPauseHandles[name].WaitOne();
                            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                            if (cts.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            using (Process process = new Process())
                            {
                                process.StartInfo = startInfo;
                                process.Start();

                                // Read the output of the process
                                string Timecrypt = process.StandardOutput.ReadToEnd();
                                File.Copy(fichierPathCrypto, destinationFilePath, true);
                                File.Delete(fichierPathCrypto);
                                LogRealTime.Timestamp = DateTime.Now;
                                LogRealTime.CurrentSourcePath = fichierPath;
                                LogRealTime.CurrentTargetPath = destinationFilePath;
                                LogRealTime.TimeCrypt = Timecrypt;
                                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                LogRealTime.CreateLog();
                            }

                        }
                        else
                        {
                            AppConstants.BackupPauseHandles[name].WaitOne();
                            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                            if (cts.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            File.Copy(fichierPath, destinationFilePath, true);
                            LogRealTime.Timestamp = DateTime.Now;
                            LogRealTime.CurrentSourcePath = fichierPath;
                            LogRealTime.CurrentTargetPath = destinationFilePath;
                            LogRealTime.TimeCrypt = "0";
                            LogRealTime.UpdateCurrentFileAndSize(fileSize);
                            AppConstants.UpdateBackupProgress(name, LogRealTime.Progress);

                            Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                            LogRealTime.CreateLog();
                        }
                    }

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
                    CopierDossierPrio(dossierPath, destinationFolderPath, LogRealTime, name);
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
                CopierDossierDifferenciel(backup.Source, backup.Destination, LogRealTime,backup.Name);
            }
            else
            {
                throw new DirectoryNotFoundException(ResourceHelper.GetString("InterfaceStrategy1"));
            }

        }

        private void CopierDossierDifferenciel(string sourceDir, string destinationDir,LogRealTime LogRealTime,string name)
        {


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
                AppConstants.backupProgress[name] = LogRealTime.Progress;
                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
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
                    if (fileSize > AppConstants.FileSize)
                    {
                        // Attendre que le Mutex soit libre puis le verrouiller
                        AppConstants.sizeMutex.WaitOne();
                        try
                        {
                            // Do the copy only if the file does not exist or if the source file is more recent than the destination file
                            if (!File.Exists(fichierDestination) || File.GetLastWriteTime(fichierSource) > File.GetLastWriteTime(fichierDestination))
                            {
                                if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                                {
                                    string executablePath = AppConstants.CryptPath;
                                    string fichierPathCrypto = fichierSource + ".crypto";
                                    string arguments = $" {fichierSource} {fichierPathCrypto} {AppConstants.KeyCrypt}";
                                    AppConstants.BackupPauseHandles[name].WaitOne();
                                    AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                                    if (cts.Token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
                                    {
                                        RedirectStandardOutput = true,
                                    };

                                    using (Process process = new Process())
                                    {
                                        process.StartInfo = startInfo;
                                        process.Start();

                                        // Read the output of the process
                                        string Timecrypt = process.StandardOutput.ReadToEnd();
                                        File.Copy(fichierPathCrypto, fichierDestination, true);
                                        File.Delete(fichierPathCrypto);
                                        LogRealTime.Timestamp = DateTime.Now;
                                        LogRealTime.CurrentSourcePath = fichierSource;
                                        LogRealTime.CurrentTargetPath = fichierDestination;
                                        LogRealTime.TimeCrypt = Timecrypt;
                                        LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                        AppConstants.backupProgress[name] = LogRealTime.Progress;
                                        Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                        LogRealTime.CreateLog();
                                    }

                                }
                                else
                                {
                                    AppConstants.BackupPauseHandles[name].WaitOne();
                                    AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                                    if (cts.Token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    File.Copy(fichierSource, fichierDestination, true);
                                    LogRealTime.Timestamp = DateTime.Now;
                                    LogRealTime.CurrentSourcePath = fichierSource;
                                    LogRealTime.CurrentTargetPath = fichierDestination;
                                    LogRealTime.TimeCrypt = "0";
                                    LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                    AppConstants.backupProgress[name] = LogRealTime.Progress;
                                    Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                    LogRealTime.CreateLog();
                                }

                            }
                        }
                        finally
                        {
                            // Libérer le Mutex une fois l'opération terminée
                            AppConstants.sizeMutex.ReleaseMutex();
                        }
                    }
                    else
                    {
                        if (AppConstants.ExtensionListCrypt.Contains(fileInfo.Extension))
                        {
                            string executablePath = AppConstants.CryptPath;
                            string fichierPathCrypto = fichierSource + ".crypto";
                            string arguments = $" {fichierSource} {fichierPathCrypto} {AppConstants.KeyCrypt}";
                            AppConstants.BackupPauseHandles[name].WaitOne();
                            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                            if (cts.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
                            {
                                RedirectStandardOutput = true,
                            };

                            using (Process process = new Process())
                            {
                                process.StartInfo = startInfo;
                                process.Start();

                                // Read the output of the process
                                string Timecrypt = process.StandardOutput.ReadToEnd();
                                File.Copy(fichierPathCrypto, fichierDestination, true);
                                File.Delete(fichierPathCrypto);
                                LogRealTime.Timestamp = DateTime.Now;
                                LogRealTime.CurrentSourcePath = fichierSource;
                                LogRealTime.CurrentTargetPath = fichierDestination;
                                LogRealTime.TimeCrypt = Timecrypt;
                                LogRealTime.UpdateCurrentFileAndSize(fileSize);
                                AppConstants.backupProgress[name] = LogRealTime.Progress;
                                Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
                                LogRealTime.CreateLog();
                            }

                        }
                        else
                        {
                            AppConstants.BackupPauseHandles[name].WaitOne();
                            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
                            if (cts.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            File.Copy(fichierSource, fichierDestination, true);
                            LogRealTime.Timestamp = DateTime.Now;
                            LogRealTime.CurrentSourcePath = fichierSource;
                            LogRealTime.CurrentTargetPath = fichierDestination;
                            LogRealTime.TimeCrypt = "0";
                            LogRealTime.UpdateCurrentFileAndSize(fileSize);
                            AppConstants.backupProgress[name] = LogRealTime.Progress;
                            Console.WriteLine($"Name: {name}, Progress: {AppConstants.backupProgress[name]}%");
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

                        CopierDossierDifferenciel(dossierSource, dossierDestination, LogRealTime, name);
                    }
                }
            }
            return;
        }

    }
}
