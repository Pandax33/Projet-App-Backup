using ProjetDevSys;
using Microsoft.Extensions.Configuration;

using ProjetDevSys.Model;
using System.Globalization;
using System.Resources;
using System.Numerics;

CultureInfo ci = new CultureInfo(AppConstants.Langage);
CultureInfo.CurrentUICulture = ci;
BackupFactory.LoadBackupsFromJson();
Backup test = BackupFactory.CreateBackup(
    "Paul",
    @"C:\Users\leanb\Pictures\test",
    @"C:\Users\leanb\Documents\test",
    "A"
);


// Utiliser une chaîne de ressource
Console.WriteLine(ResourceHelper.GetString("HelloMessage"));
IEnumerable<Backup> allBackups = BackupFactory.GetAllBackups();
foreach (Backup backup in allBackups)
{
    Console.WriteLine($"Save: {backup.Name}, Source: {backup.Source}, Destination: {backup.Destination}, Type: {backup.Type}");
}

BackupJob backupJob = new BackupJob(test);

backupJob.Save();



