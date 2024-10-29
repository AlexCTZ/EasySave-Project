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

namespace test_projet
{
    /// <summary>
    /// Logique d'interaction pour ModBackupJobWindow.xaml
    /// </summary>
    public partial class ModBackupJobWindow : Window
    {
        private MainWindow mainWindow;
        public BackupType DefaultBackupType { get; set; }
        public string Nom { get; set; }
        public string source { get; set; }
        public string dest { get; set; }
        public bool test { get; set; }
        public int _index { get; set; }

        public ModBackupJobWindow(MainWindow mainWindow,BackupJob job, int index,bool isreadonly = true)
        {
            InitializeComponent();
            
            DataContext = job;
            DefaultBackupType = job.Type;
            Nom = job.Name;
            source = job.SourceDirectory;
            dest = job.TargetDirectory;
            test = isreadonly;
            _index = index;
            // Définissez le DataContext sur cette fenêtre (c'est important pour le binding)
            DataContext = this;
            

            // Ajustez la ComboBox pour utiliser les valeurs de l'énumération BackupType
            BackupTypeComboBox.ItemsSource = Enum.GetValues(typeof(BackupType));

            //apdaute de la langue avec en accord avec la page principale
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
            BackupType backupType = (BackupType)BackupTypeComboBox.SelectedValue;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(sourceDirectory) && !string.IsNullOrWhiteSpace(targetDirectory) && !sourceDirectory.Equals(targetDirectory, StringComparison.OrdinalIgnoreCase))
            {
               

                
                mainWindow.viewModel.ModifyBackupJob(name,sourceDirectory,targetDirectory,backupType,_index);
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
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
        public void update_lang_backup()
        {
            ResourceDictionary dict = this.Resources.MergedDictionaries.FirstOrDefault();
            if (dict != null)
            {
                NameTag.Content = dict["NameTag"] as string;
                SourceTag.Content = dict["SourceTag"] as string;
                TargetTag.Content = dict["TargetTag"] as string;
                TypeTag.Content = dict["TypeTag"] as string;
            }

        }
    }
}
