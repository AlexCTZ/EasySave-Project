using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace test_projet
{
    static class Calculs
    {
        public static long CalculerTailleDossier(string cheminDossier)
        {
            // Créer une instance de DirectoryInfo pour le dossier spécifié
            DirectoryInfo infoDossier = new DirectoryInfo(cheminDossier);

            // Récupérer la taille du dossier en appelant la méthode GetSize
            long tailleDossier = GetSize(infoDossier);

            return tailleDossier;
        }

        static long GetSize(DirectoryInfo directoryInfo)
        {
            // Calculer la taille du dossier en additionnant les tailles de tous les fichiers dans le dossier
            long taille = 0;

            foreach (var fichier in directoryInfo.GetFiles())
            {
                taille += fichier.Length;
            }

            // Récupérer la taille de tous les sous-dossiers en appelant récursivement la fonction GetSize
            foreach (var sousDossier in directoryInfo.GetDirectories())
            {
                taille += GetSize(sousDossier);
            }

            return taille;
        }
        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            // Utilisation des propriétés TotalMinutes, Seconds et Milliseconds de TimeSpan
            int minutes = (int)timeSpan.TotalMinutes;
            int secondes = timeSpan.Seconds;
            int millisecondes = timeSpan.Milliseconds;

            // Formatage de la représentation
            string representation = $"{minutes:D2}:{secondes:D2}:{millisecondes:D3}";

            return representation;
        }
        public static string FormatTaille(long tailleEnOctets)
        {
            // Convertir la taille en octets en une représentation lisible (Ko, Mo, Go, etc.)
            string[] suffixes = { "Octets", "Ko", "Mo", "Go", "To" };
            int indice = 0;

            double taille = tailleEnOctets;

            while (taille >= 1024 && indice < suffixes.Length - 1)
            {
                taille /= 1024;
                indice++;
            }

            return $"{taille:N2} {suffixes[indice]}";
        }
    }
}

