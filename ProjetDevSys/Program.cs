using ProjetDevSys;
using Microsoft.Extensions.Configuration;

using ProjetDevSys.Model;
using System.Globalization;
using System.Resources;

CultureInfo ci = new CultureInfo(AppConstants.Langage);
CultureInfo.CurrentUICulture = ci;

Backup test = BackupFactory.CreateBackup(
    "Goatjo.jpg",
    @"C:\Users\leanb\Pictures\test",
    @"C:\Users\leanb\Documents\test",
    "A"
);


// Utiliser une chaîne de ressource
Console.WriteLine(ResourceHelper.GetString("HelloMessage"));
var allBackups = BackupFactory.GetAllBackups();
foreach (var backup in allBackups)
{
    Console.WriteLine($"ID: {backup.ID}, Save: {backup.Name}, Source: {backup.Source}, Destination: {backup.Destination}, Type: {backup.Type}");
}

BackupJob backupJob = new BackupJob(test);

backupJob.Save();



