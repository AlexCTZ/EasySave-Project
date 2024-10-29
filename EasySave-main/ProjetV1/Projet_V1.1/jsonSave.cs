using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Projet_V1._0
{
    static class jsonSave
    {
        static public void WriteSaveJson(List<BackupJob> liste)
        {
            using (FileStream fichier = new FileStream("Save.json", FileMode.Create))
            {
                string json = JsonSerializer.Serialize(liste, new JsonSerializerOptions { WriteIndented = true });
                using (StreamWriter writer = new StreamWriter(fichier))
                {
                    // Écrire la chaîne JSON dans le fichier
                    writer.Write(json);
                }

            }
        }
        static public List<BackupJob> ReadSaveJson()
        {
            if (File.Exists("Save.json"))
            {
                try
                {
                    // Lire tout le contenu du fichier en tant que chaîne JSON
                    string json = File.ReadAllText("Save.json");

                    // Désérialiser la chaîne JSON en une liste d'objets BackupJob
                    List<BackupJob> liste = JsonSerializer.Deserialize<List<BackupJob>>(json);

                    return liste;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier JSON : {ex.Message}");
                    return null; // Gérer l'erreur selon vos besoins
                }
            }
            else
            {
                Console.WriteLine("Le fichier JSON n'existe pas.");
                return new List<BackupJob>(); // Gérer le cas où le fichier n'existe pas
                
            }
        }
    }
}

