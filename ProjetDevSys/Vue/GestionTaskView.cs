using ProjetDevSys.Model;
using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSys.Vue
{
    public class GestionTaskView
    {
        public string SelectGestionTaskView()
        {
            Console.WriteLine("Voulez vous effectuez une Ajouter(1) ou Supprimer(2) ou Editer une tache(3) ?");
            string input = Console.ReadLine();
            GestionTask gestionTask = new GestionTask();
            switch (input)
            {
                case "1":
                    Console.WriteLine("Entrez le nom du fichier, le chemin source, le chemin de destination, et le type de backup séparés par des virgules:");
                    string[] createParams = Console.ReadLine().Split(',');
                    if (createParams.Length == 4)
                    {
                        bool created = gestionTask.CreateTask(createParams[0], createParams[1], createParams[2], createParams[3]);
                        return(created ? "Backup créé avec succès." : "Échec de la création du backup.");
                    }
                    else
                    {
                        return("Paramètres incorrects.");
                    }
                case "2":
                    int index = 0;
                    IEnumerable<Backup> Backuplist = BackupFactory.GetAllBackups();
                    foreach (Backup backup in Backuplist)
                    {
                        Console.WriteLine($"ID : {index} Save: {backup.Name}");
                        index = index + 1;
                    }
                    Console.WriteLine("Entrez l'identifiant du backup à supprimer:");
                    string deleteIdInput = Console.ReadLine();
                    if (int.TryParse(deleteIdInput, out int deleteId))
                    {
                        // Modifier pour appeler DeleteTask sur BackupFactory
                        string result = gestionTask.DeleteTask(deleteId);
                        return(result);
                    }
                    else
                    {
                        return("Identifiant invalide.");
                    }
                case "3":
                    int index2 = 0;
                    IEnumerable<Backup> Backuplist2 = BackupFactory.GetAllBackups();
                    foreach (Backup backup in Backuplist2)
                    {
                        Console.WriteLine($"ID : {index2} Save: {backup.Name}");
                        index2 = index2 + 1;
                    }
                    Console.WriteLine("Entrez l'id du backup à éditer, suivi par le nouveau chemin de destination, le nouveau chemin source, et le nouveau type de backup, séparés par des virgules:");
                    string[] editParams = Console.ReadLine().Split(',');
                    if (editParams.Length == 4)
                    {
                        if (int.TryParse(editParams[0], out int id))
                        {
                            string result = gestionTask.EditTask(id, editParams[1], editParams[2], editParams[3]);
                            return(result);
                        }
                        else
                        {
                            return("Identifiant invalide.");
                        }
                    }
                    else
                    {
                        return("Paramètres incorrects.");
                    }
                default:
                    return("Option invalide.");
            }
        }
    }
}
