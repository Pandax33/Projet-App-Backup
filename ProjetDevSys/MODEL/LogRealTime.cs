using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjetDevSys;

namespace ProjetDevSys.MODEL
{
    public class LogRealTime : JsonManager
    {
        public string BackupName { get; set; }
        public DateTime Timestamp { get; set; }
        public string State { get; set; }
        public int TotalFiles { get; set; }
        public long TotalSize { get; set; }
        public double Progress { get; set; }
        public int FilesRemaining { get; set; }
        public long SizeRemaining { get; set; }
        public string CurrentSourcePath { get; set; }
        public string CurrentTargetPath { get; set; }
        public int CurrentFile {  get; set; }
        public long CurrentFileSize { get; set; }

        public string TimeCrypt { get; set; }

        private static readonly object _lock = new object();
        public LogRealTime(string jsonPath) : base(jsonPath)
        {
            // Initialize the log with default values
            BackupName = "";
            Timestamp = DateTime.Now;
            State = "In Progress";
            TotalFiles = 0;
            TotalSize = 0;
            Progress = 0.0;
            FilesRemaining = 0;
            SizeRemaining = 0;
            CurrentSourcePath = "";
            CurrentTargetPath = "";
            CurrentFile = 0;
            CurrentFileSize = 0;
            TimeCrypt = "";
        }

        public void UpdateCurrentFileAndSize(long fileSize)
        {
            CurrentFile = CurrentFile +1;
            CurrentFileSize = fileSize;
            SizeRemaining = SizeRemaining - CurrentFileSize;

            if (SizeRemaining != 0)
            {
                Progress = 100 - (SizeRemaining * 100) / TotalSize;
            }
            else
            {
                Progress = 100;
                State = "Completed";
            }
            FilesRemaining = TotalFiles - CurrentFile;
        }
        public void CreateLog()
        {
            lock (_lock)
            {
                if (AppConstants.ExtensionType == ".json")
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                    string logEntry = JsonConvert.SerializeObject(this, settings);

                   
                    string completeFilePath = JsonPath + AppConstants.ExtensionType;

                    using (StreamWriter streamWriter = File.AppendText(completeFilePath))
                    {
                        streamWriter.WriteLine(logEntry);
                    }
                }
                else if (AppConstants.ExtensionType == ".xml")
                {
                    
                    string completeFilePath = JsonPath + AppConstants.ExtensionType;

                    using (StreamWriter streamWriter = File.AppendText(completeFilePath))
                    {
                        streamWriter.WriteLine("<LogEntry>");
                        streamWriter.WriteLine("  <BackupName>" + BackupName + "</BackupName>");
                        streamWriter.WriteLine("  <Timestamp>" + Timestamp + "</Timestamp>");
                        streamWriter.WriteLine("  <State>" + State + "</State>");
                        streamWriter.WriteLine("  <TotalFiles>" + TotalFiles + "</TotalFiles>");
                        streamWriter.WriteLine("  <TotalSize>" + TotalSize + "</TotalSize>");
                        streamWriter.WriteLine("  <Progress>" + Progress + "</Progress>");
                        streamWriter.WriteLine("  <FilesRemaining>" + FilesRemaining + "</FilesRemaining>");
                        streamWriter.WriteLine("  <SizeRemaining>" + SizeRemaining + "</SizeRemaining>");
                        streamWriter.WriteLine("  <CurrentSourcePath>" + CurrentSourcePath + "</CurrentSourcePath>");
                        streamWriter.WriteLine("  <CurrentTargetPath>" + CurrentTargetPath + "</CurrentTargetPath>");
                        streamWriter.WriteLine("  <CurrentFile>" + CurrentFile + "</CurrentFile>");
                        streamWriter.WriteLine("  <CurrentFileSize>" + CurrentFileSize + "</CurrentFileSize>");
                        streamWriter.WriteLine("  <TimeCrypt>" + TimeCrypt + "</TimeCrypt>");
                        streamWriter.WriteLine("</LogEntry>");
                    }
                }
            }
        }

            public void CalculateFolderSizeAndFileCount(string folderPath)
        {
            // Reset the log values
            TotalFiles = 0;
            TotalSize = 0;

            //file case
            if (!(Directory.Exists(folderPath)) && File.Exists(folderPath))
            {
                FileInfo fileInfo = new FileInfo(folderPath);
                TotalFiles = 1;
                SizeRemaining = fileInfo.Length;
                TotalSize = fileInfo.Length;
            }
            else
            {
                // Create a DirectoryInfo object
                DirectoryInfo dirInfo = new DirectoryInfo(folderPath);

                CalculateFolder(dirInfo);

                // Recursive method to calculate the size of all files in the folder
                void CalculateFolder(DirectoryInfo directory)
                {
                    try
                    {
                        // Count the number of files and calculate the total size
                        foreach (FileInfo file in directory.GetFiles())
                        {
                            TotalFiles++;
                            TotalSize += file.Length;
                        }

                        foreach (DirectoryInfo dir in directory.GetDirectories())
                        {
                            CalculateFolder(dir);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        // Manage the exception if the directory cannot be accessed
                        System.Console.WriteLine($"Cannot access {directory.FullName}: {ex.Message}");
                    }
                }

                SizeRemaining = TotalSize;
            }
            if (TotalSize == 0)
            {
                Progress = 100;
            }
        }

        public void CalculateFolderSizeAndFileCountDifferential(string sourcePath, string destinationPath)
        {
            TotalFiles = 0;
            TotalSize = 0;
            Progress = 0;

            if (!Directory.Exists(sourcePath) || !Directory.Exists(destinationPath))
            {
                throw new IOException("One or both of the paths do not exist.");
            }

            DirectoryInfo sourceDirInfo = new DirectoryInfo(sourcePath);
            DirectoryInfo destDirInfo = new DirectoryInfo(destinationPath);

            CalculateDifferential(sourceDirInfo, destDirInfo);

            void CalculateDifferential(DirectoryInfo sourceDir, DirectoryInfo destDir)
            {
                FileInfo[] sourceFiles = sourceDir.GetFiles();
                FileInfo[] destFiles = destDir.GetFiles();

                Dictionary<string, FileInfo> destFilesDict = destFiles.ToDictionary(f => f.Name);

                foreach (FileInfo sourceFile in sourceFiles)
                {
                    if (destFilesDict.TryGetValue(sourceFile.Name, out FileInfo destFile))
                    {
                        if (sourceFile.LastWriteTime != destFile.LastWriteTime)
                        {
                            TotalFiles++;
                            TotalSize += sourceFile.Length;
                        }
                    }
                    else
                    {
                        TotalFiles++;
                        TotalSize += sourceFile.Length;
                    }
                }

                DirectoryInfo[] sourceSubDirs = sourceDir.GetDirectories();
                foreach (DirectoryInfo subdir in sourceSubDirs)
                {
                    // Find the matching subdirectory in the destination
                    DirectoryInfo destSubDir = destDir.GetDirectories(subdir.Name).FirstOrDefault();
                    if (destSubDir != null)
                    {
                        CalculateDifferential(subdir, destSubDir);
                    }
                    else
                    {
                        CalculateFolder(subdir);
                    }
                }
            }
            void CalculateFolder(DirectoryInfo directory)
            {
                try
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        TotalFiles++;
                        TotalSize += file.Length;
                    }
                    foreach (DirectoryInfo dir in directory.GetDirectories())
                    {
                        CalculateFolder(dir);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Cannot access {directory.FullName}: {ex.Message}");
                }
            }
            SizeRemaining = TotalSize;
            if (TotalSize == 0)
            {
                Progress = 100;
            }
        }
    } 



}
