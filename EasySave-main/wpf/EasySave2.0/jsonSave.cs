using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;

static class jsonSave
{
    static public void WriteSaveJson(ObservableCollection<BackupJob> liste)
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
    static public ObservableCollection<BackupJob> ReadSaveJson()
    {
        if (File.Exists("Save.json"))
        {
            try
            {
                // Lire tout le contenu du fichier en tant que chaîne JSON
                string json = File.ReadAllText("Save.json");

                // Désérialiser la chaîne JSON en une liste d'objets BackupJob
                ObservableCollection<BackupJob> liste = JsonSerializer.Deserialize<ObservableCollection<BackupJob>>(json);

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
            return new ObservableCollection<BackupJob>(); // Gérer le cas où le fichier n'existe pas

        }
    }
}