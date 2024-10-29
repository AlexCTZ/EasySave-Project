using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using Path = System.IO.Path;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;


namespace test_projet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public BackupJobView _test;
        public MainWindowsViewModel viewModel ;

        public event PropertyChangedEventHandler PropertyChanged;
        
        connexion _connexion = new connexion();

        public MainWindow()
        {
            InitializeComponent();
            LoadLanguageResources();
            UpdateUI();
            viewModel = new MainWindowsViewModel(this);
            viewModel.LoadLog();
            JobsListBox.ItemsSource = viewModel.BackupJobs;
            Closing += MainWindow_Closing;


        }

        public void NotifyClientBackupStarted(BackupJob job)
        {
            try
            {
                // Envoyer un message indiquant le début de la sauvegarde
                byte[] message = Encoding.UTF8.GetBytes("BackupStarted:"+job.Name);
                _connexion.client.Send(message);

            }
            catch (SocketException ex)
            {
                // Gérer les erreurs de socket ici
                Console.WriteLine($"Erreur lors de l'envoi de la notification de démarrage de la sauvegarde : {ex.Message}");
            }
        }


        private async void ExecuteBackupButton_Click(object sender, RoutedEventArgs e)
        {

            /*foreach (BackupJob job in JobsListBox.SelectedItems)
            {
                await ExecuteBackupJob(job);
            }*/
            /*foreach (BackupJob job in JobsListBox.SelectedItems)
            {
                await ExecuteBackupJob(job);
            }*/
            List<Task> backupTasks = new List<Task>();

            foreach (BackupJob job in JobsListBox.SelectedItems)
            {
                Task backupTask = viewModel.ExecuteBackupJob(job, viewModel.logicielMetier, _connexion);
                backupTasks.Add(backupTask);
            }

            // Attendre que toutes les sauvegardes soient terminées
            await Task.WhenAll(backupTasks);
        }

            private void AddBackupJobButton_Click(object sender, RoutedEventArgs e)
        {
            AddBackupJobWindow addBackupJobWindow = new AddBackupJobWindow(this);
            addBackupJobWindow.ShowDialog();
            JobsListBox.ItemsSource = viewModel.BackupJobs;
        }
        private async void ExecuteAllBackupsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        

            private void ViewBackupJobsButton_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    ModBackupJobWindow modBackupJobWindow = new ModBackupJobWindow(this, viewModel.BackupJobs[GetSelectedIndex()], GetSelectedIndex(),false);
                    modBackupJobWindow.ShowDialog();
        }
                catch (Exception except)
                {
                    MessageBox.Show($"message : {except.Message}", "ErrorInfo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        private void EditBackupJobButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (viewModel.LaunchedSaves.Contains(viewModel.BackupJobs[GetSelectedIndex()]))
                {
                    MessageBox.Show("Save launched Wait for the end", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else {
                    ModBackupJobWindow modBackupJobWindow = new ModBackupJobWindow(this, viewModel.BackupJobs[GetSelectedIndex()], GetSelectedIndex());
                    modBackupJobWindow.ShowDialog();
                    JobsListBox.ItemsSource = viewModel.BackupJobs;
                }
            }
            catch (Exception except)
            {
                MessageBox.Show($"message : {except.Message}", "ErrorInfo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            }

        private void DeleteBackupJobButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            viewModel.BackupJobs.RemoveAt(GetSelectedIndex());
            viewModel.RearangeJobs();
            jsonSave.WriteSaveJson(viewModel.BackupJobs);
        }
            catch (Exception except)
            {
                MessageBox.Show($"message : {except.Message}", "ErrorInfo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private int[] GetSelectedJobs()
        {
            var selectedJobs = new List<int>();
            foreach (var item in JobsListBox.SelectedItems)
            {
                if (item is ListBoxItem listBoxItem && listBoxItem.Content is BackupJob job)
                {
                    selectedJobs.Add(viewModel.BackupJobs.IndexOf(job));
                }
            }
            return selectedJobs.ToArray();
        }

        private int GetSelectedIndex()
        {
            BackupJob selectedJob = JobsListBox.SelectedItem as BackupJob;

            if (selectedJob != null)
            {
                return viewModel.BackupJobs.IndexOf(selectedJob);
            }

            return -1;
        }
        
        private void InitiateSetup_Click(object sender, RoutedEventArgs e)
        {
            ExtSelector ConfigWindow = new ExtSelector(this);
            ConfigWindow.ShowDialog();
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
        private void UpdateUI()
        {
            // Mettez à jour le contenu de tous les éléments d'interface utilisateur en fonction des nouvelles ressources
            // Par exemple :
            ResourceDictionary dict = this.Resources.MergedDictionaries.FirstOrDefault();

            if (dict != null)
            {
                // Mettez à jour le contenu de tous les éléments d'interface utilisateur en fonction des nouvelles ressources
                this.Title = dict["AppName"] as string;
                AddBackupJobButton.Content = dict["AddBackupJobButton"] as string;
                DeleteButton.Content = dict["DeleteButton"] as string;
                EditButton.Content = dict["EditButton"] as string;
                ExecAllButton.Content = dict["ExecAllButton"] as string;
                ExecBackupButton.Content = dict["ExecBackupButton"] as string;
                ViewButton.Content = dict["ViewButton"] as string;
                ListGridView.Columns[0].Header = dict["HeaderName"] as string;
                ListGridView.Columns[1].Header = dict["HeaderId"] as string;


                // Mettez à jour d'autres éléments d'interface utilisateur de la même manière
            }
            }

            /////////////////////////////////////////////////////////////////////////////////////////////

            //mesure de la latence avec ping vers serveur
            //modifier pour quand serveur cree
            public async Task<double> MeasureLatency(string host)
            {
                try
                {
                    var ping = new Ping();
                    var reply = await ping.SendPingAsync(host);
                    if (reply != null && reply.Status == IPStatus.Success)
                    {
                        return reply.RoundtripTime;
                    }
                }
                catch (PingException ex)
                {
                    Console.WriteLine($"Error measuring latency: {ex.Message}");
                }

                return -1; // Indicateur de mesure invalide
        }


            //////////////////////////////////////////////////////////////////////////////////////////////

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                string newLanguage = menuItem.Tag as string;
                if (!string.IsNullOrEmpty(newLanguage))
                {
                    LanguageManager.CurrentLanguage = newLanguage;
                    LoadLanguageResources();

                    // Rafraîchir les éléments d'interface utilisateur
                    UpdateUI();
                }
            }
        }


        private void InitiateLog_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                string newlog = menuItem.Tag as string;
                if (!string.IsNullOrEmpty(newlog))
                {
                    LogsManager.CurrentLog = newlog;
                    viewModel.LoadLog();
                }
            }
        }

        private CancellationTokenSource applicationClosingTokenSource;
        private async void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // Annuler l'événement de fermeture pour permettre un nettoyage personnalisé
            e.Cancel = true;

            // Créer un jeton pour signaler aux threads qu'ils doivent se terminer
            
            applicationClosingTokenSource = new CancellationTokenSource();
            if (viewModel.LaunchedSaves.Count != 0)
            {
                MessageBox.Show("Saves are currently running : Wait for completion", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Fermer proprement les threads ici
            else
            {
                // Fermer proprement les threads ici
                Environment.Exit(0);
            }



        }





    }
}
