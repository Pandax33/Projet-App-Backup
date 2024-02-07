using System;
using System.Collections.Generic;


namespace ProjetDevSys.Model
{
    internal static class BackupFactory
    {
        private static int _nextId = 0; // Compteur pour l'auto-incrémentation des ID
        private static readonly Dictionary<int, Backup> _backups = new Dictionary<int, Backup>(); // Stocke les relations ID-Backup

        // Méthode de la Factory pour créer une instance de Backup
        public static Backup CreateBackup(string save, string destination, string source, string type)
        {
            int id = _nextId++; // Utilisez l'ID actuel et incrémente pour le prochain usage

            var backup = new Backup
            {
                ID = id,
                Name = save,
                Destination = destination,
                Source = source,
                Type = type
            };

            // Ajoute le Backup créé à la liste (ou au dictionnaire) pour le suivi
            _backups.Add(backup.ID, backup);

            return backup;
        }

        // Méthode pour récupérer un Backup par son ID
        public static Backup GetBackupById(int id)
        {
            if (_backups.ContainsKey(id))
            {
                return _backups[id];
            }

            return null; // Ou gérer autrement si l'ID n'existe pas
        }

        // Optionnellement, fournir une méthode pour lister tous les Backups
        public static IEnumerable<Backup> GetAllBackups()
        {
            return _backups.Values;
        }
    }
}