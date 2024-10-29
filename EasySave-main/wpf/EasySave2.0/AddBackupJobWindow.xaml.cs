using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace test_projet
{
    /// <summary>
    /// Logique d'interaction pour AddBackupJobWindow.xaml
    /// </summary>
    public partial class AddBackupJobWindow : Window
    {
        private MainWindow mainWindow;

        public AddBackupJobWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            LoadLanguageResources();
            update_lang_backup();
            this.mainWindow = mainWindow;
            
        }

        private void ValiderButton_Click(object sender, RoutedEventArgs e)
        {
            // Validation logique ici, assurez-vous que les champs sont correctement remplis

            string name = NameTextBox.Text;
            string sourceDirectory = SourceDirectoryTextBox.Text;
            string targetDirectory = TargetDirectoryTextBox.Text;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(sourceDirectory) && !string.IsNullOrWhiteSpace(targetDirectory) && !sourceDirectory.Equals(targetDirectory, StringComparison.OrdinalIgnoreCase))
            {
                // Convertir le ComboBoxItem en BackupType (Full ou Differential)
                BackupType backupType = (BackupType)Enum.Parse(typeof(BackupType), ((ComboBoxItem)BackupTypeComboBox.SelectedItem).Content.ToString());

                BackupJob newJob = new BackupJob
                {
                    Name = name,
                    SourceDirectory = sourceDirectory,
                    TargetDirectory = targetDirectory,
                    Type = backupType
                };

                mainWindow.viewModel.AddBackupJob(newJob);
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs. Vérifiez que la source n'es pas la même que la destination.");
            }
        }

        private void LoadLanguageResources()
        {
            string language = LanguageManager.CurrentLanguage;
            string resourceFile = $"Strings.{language}.xaml";

            // Nettoyer les anciens dictionnaires de ressources
            this.Resources.MergedDictionaries.Clear();

            // Charger le nouveau dictionnaire de ressources
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri(resourceFile, UriKind.Relative);
            this.Resources.MergedDictionaries.Add(dict);
        }


        /// <summary>
        /// Fonction qui sert a update la langue en fonction de ce qui été choisis sur la vue principale
        /// 
        /// on recupere le dictionnaire et on update les noms des elements de la page
        /// 
        /// si un nouveau bouton est ajouter dans ce cas il suffit de fait le lien et ajouter les langues dans le dictionnaire XML
        /// </summary>
        public void update_lang_backup() {
            ResourceDictionary dict = this.Resources.MergedDictionaries.FirstOrDefault();
            if(dict != null)
            {
                NameTag.Content = dict["NameTag"] as string;
                SourceTag.Content = dict["SourceTag"] as string;
                TargetTag.Content = dict["TargetTag"] as string;
                TypeTag.Content = dict["TypeTag"] as string;
            }
            
        }

        private void Select_Source(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                string selectedFolder = dialog.FileName;
                SourceDirectoryTextBox.Text = selectedFolder;
                TargetDirectoryTextBox.Focus();
            }
        }

        private void Select_Destination(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                string selectedFolder = dialog.FileName;
                TargetDirectoryTextBox.Text = selectedFolder;
                BackupTypeComboBox.Focus();
            }
        }
    }
}
