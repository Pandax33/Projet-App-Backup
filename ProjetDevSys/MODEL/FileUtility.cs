using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.MODEL
{
    public static class FileUtility
    {
        public static void CopierFichier(string sourceFilePath, string destinationDir, LogRealTime LogRealTime, string name, string TimeCrypt = "0")
        {
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationDir, fileName);
            long fileSize = new FileInfo(sourceFilePath).Length;

            if (fileSize > AppConstants.FileSize)
            {
                AppConstants.EventState.TryAdd("sizeMutex", "Pause");
                AppConstants.sizeMutex.WaitOne();
            }

            try
            {
                File.Copy(sourceFilePath, destinationFilePath, true);
                MiseAJourLogEtProgression(LogRealTime, sourceFilePath, destinationFilePath, fileSize, name, TimeCrypt);
            }
            finally
            {
                if (fileSize > AppConstants.FileSize)
                {
                    AppConstants.sizeMutex.ReleaseMutex();
                    AppConstants.EventState.TryAdd("sizeMutex", "Libre");
                }
            }
        }

        public static void TraiterEtCopierFichierCrypte(string sourceFilePath, string destinationDir, LogRealTime LogRealTime, string name)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);

            string extension = Path.GetExtension(sourceFilePath);
            string fichierPathCrypto = $"{Path.GetDirectoryName(sourceFilePath)}{Path.DirectorySeparatorChar}{fileNameWithoutExtension}_crypto{extension}";
            string arguments = $" {sourceFilePath} {fichierPathCrypto} {AppConstants.KeyCrypt}";

            AppConstants.BackupCancellations.TryGetValue(name, out CancellationTokenSource cts);
            if (cts.Token.IsCancellationRequested)
            {
                return;
            }
            AppConstants.BackupPauseHandles[name].WaitOne();

            string executablePath = AppConstants.CryptPath;
            ProcessStartInfo startInfo = new ProcessStartInfo(executablePath, arguments)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();

                // Lit la sortie standard pour obtenir le temps de cryptage
                string timeCrypt = process.StandardOutput.ReadLine();

                if (process.ExitCode == 0)
                {
                    CopierFichier(fichierPathCrypto, destinationDir, LogRealTime, name, timeCrypt);

                    File.Delete(fichierPathCrypto);

                    LogRealTime.TimeCrypt = timeCrypt;
                }
                else
                {
                    Console.WriteLine($"Le cryptage du fichier {sourceFilePath} a échoué avec le code de sortie {process.ExitCode}.");
                }
            }
        }


        public static void MiseAJourLogEtProgression(LogRealTime logRealTime, string sourcePath, string targetPath, long fileSize, string name, string timeCrypt)
        {
            logRealTime.Timestamp = DateTime.Now;
            logRealTime.CurrentSourcePath = sourcePath;
            logRealTime.CurrentTargetPath = targetPath;
            logRealTime.TimeCrypt = timeCrypt;
            logRealTime.UpdateCurrentFileAndSize(fileSize);
            AppConstants.UpdateBackupProgress(name, logRealTime.Progress);

            logRealTime.CreateLog();
        }
    }
}
