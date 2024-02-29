using ProjetDevSys.MODEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ProjetDevSys.Model
{
    public static class BackupFactory
    {
        public static readonly Dictionary<string, Backup> _backups = new Dictionary<string, Backup>();
        private static readonly JsonManager _jsonManager = new JsonManager(AppConstants.JsonSave);

        // Update method to create a backup
        public static Backup CreateBackup(string save, string source,string destination, string type)
        {
            // Check if the backup already exists
            if (_backups.ContainsKey(save))
            {
                // You can throw an exception or return null
                throw new ArgumentException(ResourceHelper.GetString("BackupFactory1"));
            }

            Backup backup = new Backup
            {
                Name = save,
                Destination = destination,
                Source = source,
                Type = type
            };

            // use the name as a key to store the backup in the dictionary
            _backups.Add(save, backup);
            SaveBackupsToJson();

            return backup;
        }

        // Update Method to get a backup by name
        public static Backup GetBackupByName(string name)
        {
            if (_backups.TryGetValue(name, out Backup backup))
            {
                return backup;
            }

            return null;
        }

        private static void SaveBackupsToJson()
        {
            _jsonManager.Serialize(_backups);
        }

        public static void LoadBackupsFromJson()
        {
            try
            {
                Dictionary < string, Backup> loadedBackups = _jsonManager.Deserialize<Dictionary<string, Backup>>();
                if (loadedBackups != null)
                {
                    _backups.Clear();
                    foreach (var item in loadedBackups)
                    {
                        _backups.Add(item.Key, item.Value);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IEnumerable<Backup> GetAllBackups()
        {
            return _backups.Values;
        }

        public static bool DeleteBackup(string name)
        {
            if (_backups.ContainsKey(name))
            {
                _backups.Remove(name);
                // Update the JSON file
                SaveBackupsToJson();
                return true;
            }
            else
            {
                return false; // Return false if the backup was not found
            }
        }

        public static bool EditBackup(string name, string newDestination, string newSource, string newType)
        {
            // Check if the backup exists
            if (_backups.TryGetValue(name, out Backup backup))
            {
                // Update the backup properties
                backup.Destination = newDestination;
                backup.Source = newSource;
                backup.Type = newType;

                // Save the changes to the JSON file
                SaveBackupsToJson();

                return true; // Return true if the backup was found and updated
            }
            else
            {
                return false; // Return false if the backup was not found
            }
        }

        public static IEnumerable<Backup> GetBackupsInRange(int startNumber, int endNumber)
        {
            // Calculate the number of elements to take from the dictionary
            int count = endNumber - startNumber + 1;

            // Check if the range is valid
            if (count > 0)
            {
                return _backups.Values.Skip(startNumber).Take(count);
            }
            else
            {
                return Enumerable.Empty<Backup>(); // Return an empty collection if the range is invalid
            }
        }

        public static Backup GetBackupByIndex(int index)
        {
            // Convert the dictionary values to a list
            List<Backup> backupList = _backups.Values.ToList();
            if (index >= 0 && index < backupList.Count)
            {
                return backupList[index];
            }
            else
            {
                return null;
            }
        }
    }
}
