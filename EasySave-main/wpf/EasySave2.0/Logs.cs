using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml;
using test_projet;

namespace test_projet
{
    public interface ILogs
    {
        public abstract void LogToDailyLogFile(BackupJob job, DateTime debut, DateTime fin);
    }

    public class jsonLog : ILogs
    {
        public void LogToDailyLogFile(BackupJob job, DateTime debut, DateTime fin)
        {
            try
            {

                // Calcul du temps de transfert (en ms)
                TimeSpan transferTime = fin - debut;

                // Créez un objet anonyme avec les informations de sauvegarde
                string logFilePath = "log.json";

                // Vérifiez si le fichier journal JSON existe
                if (!File.Exists(logFilePath))
                {
                    // Si le fichier n'existe pas, on le crée avec un tableau JSON vide
                    try
                    {
                        File.WriteAllText(logFilePath, "[]");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la création du fichier journal JSON : {ex.Message}");
                        return;
                    }
                }

                string currentContent = File.ReadAllText(logFilePath);

                // Désérialise le contenu actuel ou crée un tableau JSON vide si le contenu n'est pas valide JSON
                var backupArray = JsonSerializer.Deserialize<object[]>(currentContent) ?? new object[0];

                var logInfo = new
                {
                    Name = job.Name,
                    FileSource = job.SourceDirectory,
                    FileTarget = job.TargetDirectory,
                    //FileSize = Convert.ToString(Calculs.FormatTaille(Calculs.CalculerTailleDossier(job.SourceDirectory))),
                    FileTransferTime = Convert.ToString(Calculs.FormatTimeSpan(transferTime)),
                    Date = Convert.ToString(debut)
                };
                var newArray = new object[backupArray.Length + 1];
                Array.Copy(backupArray, newArray, backupArray.Length);
                newArray[backupArray.Length] = logInfo;

                // Sérialise le tableau mis à jour en tant que JSON avec une mise en forme
                string updatedContent = JsonSerializer.Serialize(newArray, new JsonSerializerOptions { WriteIndented = true });

                // Écrit le contenu mis à jour dans le fichier
                File.WriteAllText(logFilePath, updatedContent);

                Console.WriteLine("Information enregistrée dans le journal JSON avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'enregistrement dans le journal JSON : {ex.Message}");
            }
        }
    }


    public class XmlLog : ILogs
    {
        public void LogToDailyLogFile(BackupJob job, DateTime debut, DateTime fin)
        {
            try
            {
                // Calcul du temps de transfert (en ms)
                TimeSpan transferTime = fin - debut;

                // Chemin du fichier journal XML
                string logFilePath = "log.xml";

                // Vérifiez si le fichier journal XML existe
                if (!File.Exists(logFilePath))
                {
                    // Si le fichier n'existe pas, on le crée avec une structure XML vide
                    try
                    {
                        using (XmlWriter writer = XmlWriter.Create(logFilePath))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("BackupLogs");
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors de la création du fichier journal XML : {ex.Message}");
                        return;
                    }
                }

                // Charge le document XML existant
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(logFilePath);

                // Crée un nouvel élément XML avec les informations de sauvegarde
                var logElement = xmlDoc.CreateElement("BackupLog");

                AppendXmlElement(xmlDoc, logElement, "name", job.Name);
                AppendXmlElement(xmlDoc, logElement, "fileSource", job.SourceDirectory);
                AppendXmlElement(xmlDoc, logElement, "fileTarget", job.TargetDirectory);
                //AppendXmlElement(xmlDoc, logElement, "fileSize", Convert.ToString(Calculs.FormatTaille(Calculs.CalculerTailleDossier(job.SourceDirectory))));
                AppendXmlElement(xmlDoc, logElement, "fileTransferTime", Convert.ToString(Calculs.FormatTimeSpan(transferTime)));
                AppendXmlElement(xmlDoc, logElement, "date", Convert.ToString(debut));

                // Ajoute le nouvel élément au document XML
                xmlDoc.DocumentElement?.AppendChild(logElement);

                // Sauvegarde le document XML mis à jour
                xmlDoc.Save(logFilePath);

                Console.WriteLine("Information enregistrée dans le journal XML avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'enregistrement dans le journal XML : {ex.Message}");
            }
        }

        // Fonction utilitaire pour ajouter un élément XML au parent avec le contenu spécifié
        private void AppendXmlElement(XmlDocument xmlDoc, XmlElement parent, string elementName, string content)
        {
            var element = xmlDoc.CreateElement(elementName);
            element.InnerText = content;
            parent.AppendChild(element);
        }
    }
}

public class LogsManager
{
    public static string CurrentLog { get; set; } = "json"; // par défaut, le français

    public static void ToggleLanguage()
    {
        CurrentLog = (CurrentLog == "json") ? "xml" : "json";
    }
}

public static class LogsFactory
{
    public static ILogs CreateLogs(string logType)
    {
        switch (logType)
        {
            case "json":
                return new jsonLog();
            case "xml":
                return new XmlLog();
            // Ajoutez d'autres types de journaux au besoin
            default:
                throw new ArgumentException("Invalid log type");
        }
    }
}