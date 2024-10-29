using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_V1._0
{


    public interface ILanguageStrings
    {
        string create       { get; }
        string view         { get; }
        string edit         { get; }
        string del          { get; }
        string execone       { get; }
        string execall       { get; }
        string exit         { get; }
        public string selecedit   { get; }
        public string selecdel    { get; }
        public string selecexe    { get; }
        public string error       { get; }
        public string viewbc      { get; }
        public string editbc      { get; }
        public string editbcname  { get; }
        public string editbcsd    { get; }
        public string editbctd    { get; }
        public string editbctype  { get; }
        public string editval     { get; }
        public string indexerror  { get; }
        public string delval      { get; }
        public string execoneview { get; }
        public string newbcname   { get; }
        public string newbcsd     { get; }
        public string newbctd     { get; }
        public string newbctype { get; }
        public string newval { get; }
        public string errornbbc { get; }
        public string blockcl { get; }
        public string execval { get; }
        public string emptyvalerr   { get; }
        public string errortarget   { get; }
        public string errortype { get; }
    }
        class Langfr : ILanguageStrings
    {
        #region MenuFr
        public string create => "1. Créer une tâche de sauvegarde";
        public string view => "2. Voir les tâches de sauvegarde";
        public string edit => "3. Modifier une tâche de sauvegarde";
        public string del => "4. Supprimer une tâche de sauvegarde";
        public string execone => "5. Exécuter la sauvegarde";
        public string execall => "6. Exécuter toutes les sauvegardes";
        public string exit => "7. Quitter";
        #endregion MenuFr
        public string selecedit => "Entrez l'index de la tâche de sauvegarde à modifier :";
        public string selecdel => "Entrez l'index de la tâche de sauvegarde à supprimer :";
        public string selecexe => "Entrez l'index de la tâche de sauvegarde à exécuter :";
        public string error => "Entrée invalide. Veuillez réessayer.";
        public string viewbc => "Tâches de sauvegarde :";
        public string editbc => "Modification de la tâche de sauvegarde : ";
        public string editbcname => "Entrez le nouveau nom : ";
        public string editbcsd => "Entrez le nouveau répertoire source : ";
        public string editbctd => "Entrez le nouveau répertoire cible : ";
        public string editbctype => "Entrez le nouveau type (Complet/Différentiel) : ";
        public string editval => "Tâche de sauvegarde mise à jour avec succès.";
        public string indexerror => "Index invalide.";
        public string delval => "Tâche de sauvegarde supprimée avec succès.";
        public string execoneview => "Exécution de la tâche de sauvegarde : ";
        public string newbcname => "Entrez le nom : ";
        public string newbcsd => "Entrez le répertoire source : ";
        public string newbctd => "Entrez le répertoire cible : ";
        public string newbctype => "Entrez le type (Complet/Différentiel) : ";
        public string newval => "Sauvegarde créée avec succes";
        public string errornbbc => "Vous avez atteint le nombre max de tâches de sauvegardes, vous pouvez en modifier ou supprimer une existante";
        public string blockcl => "Attendez la fin de la tâche avant de fermer l'application.";
        public string execval => "execution de la tache de sauvegarde reussie";
        public string emptyvalerr => "Entrée invalide : entrer une chaine non-vide";
        public string errortarget => "Entrée invalide : entrer une chaine non-vide et differente de la source";
        public string errortype => "Entrée invalide : entrer soit 'Full' ou 'Differential'";

    }

    class LangAn : ILanguageStrings
    {
        #region 
        public string create  =>  "1. Create Backup Job";   
        public string view    =>    "2. View Backup Jobs";
        public string edit    =>    "3. Edit Backup Job";
        public string del     =>     "4. Delete Backup Job";
        public string execone => "5. Execute Backup";
        public string execall => "6. Execute All Backups";
        public string exit    =>    "7. Exit";
        #endregion MenuEn
        public string selecedit     => "Enter the index of the backup job to edit:";
        public string selecdel      => "Enter the index of the backup job to delete:";
        public string selecexe      => "Enter the index of the backup job to execute:";
        public string error         => "Invalid input. Please try again.";
        public string viewbc        => "Backup Jobs:";
        public string editbc        => "Editing Backup Job: ";
        public string editbcname    => "Enter new name: ";
        public string editbcsd      => "Enter new source directory: ";
        public string editbctd      => "Enter new target directory: ";
        public string editbctype    => "Enter new type (Full/Differential): ";
        public string editval       => "Backup Job updated successfully.";
        public string indexerror    => "Invalid index.";
        public string delval        => "Backup Job deleted successfully.";
        public string execoneview   => "Executing backup job: ";
        public string newbcname     => "Enter name: ";
        public string newbcsd       => "Enter source directory: ";
        public string newbctd       => "Enter target directory: ";
        public string newbctype     => "Enter type (Full/Differential): ";
        public string newval => "Backup Job created successfully.";
        public string errornbbc => "You have reached the max number of backup jobs, consider edit or delete to add nex backup job";
        public string blockcl => "Wait the end of the task before closing the application.";
        public string execval => "\nExecuted backup job successfully.";
        public string emptyvalerr => "Invalid input.Please enter a non-empty value.";
        public string errortarget => "Invalid input. The target must be non-empty and different from the source.";
        public string errortype   => "Invalid input. Please enter either 'Full' or 'Differential'.";

    };
}
