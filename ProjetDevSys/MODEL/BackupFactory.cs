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

        public static IEnumerable<Backup> GetBackupsInRange(int startNumber, int endNumber)
        {
            // Calculer le nombre d'éléments à prendre après avoir sauté startNumber éléments
            int count = endNumber - startNumber + 1;

            // Vérifier que le calcul de count est positif, sinon retourner un enumerable vide
            if (count > 0)
            {
                return _backups.Values.Skip(startNumber).Take(count);
            }
            else
            {
                return Enumerable.Empty<Backup>(); // Retourne une collection vide si la plage est invalide
            }
        }

        public static Backup GetBackupByIndex(int index)
        {
            // Convertit les valeurs du dictionnaire en liste et tente de récupérer l'élément à l'index spécifié
            var backupList = _backups.Values.ToList();
            if (index >= 0 && index < backupList.Count)
            {
                return backupList[index];
            }
            else
            {
                return null; // ou gérer l'erreur comme désiré
            }
        }
    }
}
