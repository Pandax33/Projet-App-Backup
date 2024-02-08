using ProjetDevSys.MODEL; // Assurez-vous que cet espace de noms est correct
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ProjetDevSys.Model
{
    internal static class BackupFactory
    {
        private static readonly Dictionary<string, Backup> _backups = new Dictionary<string, Backup>();
        private static readonly JsonManager _jsonManager = new JsonManager(AppConstants.JsonSave);

        // Méthode mise à jour pour créer une instance de Backup
        public static Backup CreateBackup(string save, string destination, string source, string type)
        {
            // Vérifie si un backup avec le même nom existe déjà
            if (_backups.ContainsKey(save))
            {
                // Vous pouvez lever une exception ou simplement retourner null/le backup existant
                throw new ArgumentException($"Un backup nommé '{save}' existe déjà.");
            }

            Backup backup = new Backup
            {
                Name = save,
                Destination = destination,
                Source = source,
                Type = type
            };

            // Utilise le nom comme clé pour le dictionnaire
            _backups.Add(save, backup);
            SaveBackupsToJson();

            return backup;
        }

        // Méthode mise à jour pour récupérer un Backup par son nom
        public static Backup GetBackupByName(string name)
        {
            if (_backups.TryGetValue(name, out Backup backup))
            {
                return backup;
            }

            return null;
        }

        // Les autres méthodes restent plus ou moins inchangées...

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

        public static void DisplayFirstFiveBackupNames(int Number)
        {
            // Prend les 5 premiers éléments du dictionnaire _backups
            IEnumerable<Backup> firstFiveBackups = _backups.Values.Take(Number);

            foreach (Backup backup in firstFiveBackups)
            {
                Console.WriteLine(backup.Name);
            }
        }
    }
}
