using ProjetDevSys;
using Microsoft.Extensions.Configuration;

using ProjetDevSys.Model;

Backup test = BackupFactory.CreateBackup(
    "Goatjo.jpg",
    @"C:\Users\leanb\Pictures\test",
    @"C:\Users\leanb\Documents\test",
    "A"
);

var allBackups = BackupFactory.GetAllBackups();
foreach (var backup in allBackups)
{
    Console.WriteLine($"ID: {backup.ID}, Save: {backup.Name}, Source: {backup.Source}, Destination: {backup.Destination}, Type: {backup.Type}");
}

BackupJob backupJob = new BackupJob(test);

backupJob.Save();



