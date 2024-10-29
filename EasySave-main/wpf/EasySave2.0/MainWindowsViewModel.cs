using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace test_projet
{
    public class MainWindowsViewModel
    {  
        private MainWindow _mainWindow;
        public List<BackupJob> LaunchedSaves = new List<BackupJob>();
        public string logicielMetier = "EXCEL";



        public MainWindowsViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            BackupJobs = jsonSave.ReadSaveJson();
            

        }
        protected ILogs logger;

        public async Task ExecuteBackupJob(BackupJob job, string log, connexion _connexion)
        {
            LaunchedSaves.Add(job);
            

            if (Directory.GetFiles(job.SourceDirectory).Length == 0)
            {
                MessageBox.Show("Source Folder Empty", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(); // Créez une nouvelle instance
                Copieur Depl = new Copieur();
                Progress addProgressWindow = new Progress(Depl);
                addProgressWindow.Closed += (sender, e) =>
                {
                    addProgressWindow.copy.StopCopy();
                    //addProgressWindow.copy._connexion.Stop();
                    try
                    {
                        cancellationTokenSource.Cancel();
                    }
                    catch (ObjectDisposedException) { }
                };
                addProgressWindow.Show();
                addProgressWindow.changeName(job.Name);

                DateTime debut = DateTime.Now;

                try
                {
                    _mainWindow.NotifyClientBackupStarted(job);
                    await Depl.CopyFilesToDestination(job.Name, job.SourceDirectory, job.TargetDirectory, job.Type, addProgressWindow, cancellationTokenSource.Token, selectedExtensions, log, _connexion);
                }
                catch (OperationCanceledException)
                {
                   
                }
                finally
                {

                    cancellationTokenSource.Dispose(); // Disposez de l'instance après utilisation
                }

                DateTime end = DateTime.Now;
                logger.LogToDailyLogFile(job, debut, end);
                LaunchedSaves.Remove(job);
            }
        }
        public void LoadLog()
        {
            logger = LogsFactory.CreateLogs(LogsManager.CurrentLog);
        }

        
        private List<string> _selectedExtensions = new List<string>();
        public List<string> selectedExtensions
        {
            get { return _selectedExtensions; }
            set
            {
                if (_selectedExtensions != value)
                {
                    _selectedExtensions = value;
                    OnPropertyChanged(nameof(selectedExtensions));
                }
            }
        }

        private ObservableCollection<BackupJob> _backupJobs = new ObservableCollection<BackupJob>();
        public ObservableCollection<BackupJob> BackupJobs
        {
            get { return _backupJobs; }
            set
            {
                if (_backupJobs != value)
                {
                    _backupJobs = value;
                    OnPropertyChanged(nameof(BackupJobs));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RearangeJobs()
        {
            for (int i = 0; i < BackupJobs.Count; i++)
            {
                BackupJobs[i].Id = i + 1;
            }
        }

        public void AddOrRemoveExtension(string extension, bool? isChecked)
        {
            if (isChecked.HasValue && isChecked.Value)
            {
                // Si la CheckBox est cochée, ajouter l'extension à la liste
                if (selectedExtensions.Contains(extension))
                {
                    selectedExtensions.Add(extension);
                }
            }
            else
            {
                // Si la CheckBox est décochée, supprimer l'extension de la liste
                selectedExtensions.Remove(extension);
            }
        }
        public void ModifyBackupJob(string name, string source, string dest, BackupType type,int index)
        {
            BackupJobs[index].Name = name;
            BackupJobs[index].SourceDirectory = source;
            BackupJobs[index].TargetDirectory = dest;
            BackupJobs[index].Type = type;
            
            jsonSave.WriteSaveJson(BackupJobs);
        }

        public void AddBackupJob(BackupJob newJob)
        {
            int Idtest = BackupJobs.Count;
            newJob.Id = Idtest + 1;
            BackupJobs.Add(newJob);

            jsonSave.WriteSaveJson(BackupJobs);

        }
        public void AddLogBlock(string logiciel)
        {
            logicielMetier = logiciel;

        }
    }
}
