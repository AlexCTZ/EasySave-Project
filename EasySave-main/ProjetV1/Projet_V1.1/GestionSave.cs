using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace Projet_V1._0
{
    class BackupManager
    {
        public List<BackupJob> backupJobs;
        public ILanguageStrings chaines;
        public ILogs log;

        public BackupManager(List<BackupJob> recupjson)
        {
            backupJobs = recupjson;
        }

        public void AddBackupJob(BackupJob job)
        {
            backupJobs.Add(job);
            jsonSave.WriteSaveJson(backupJobs);
        }

        public void ExecuteBackup(int[] jobIndices)
        {
            foreach (var index in jobIndices)
            {
                if (index >= 0 && index < backupJobs.Count)
                {
                    ExecuteBackupJob(backupJobs[index]);
                }
            }
        }

        public void ExecuteAllBackups()
        {
            foreach (var job in backupJobs)
            {
                ExecuteBackupJob(job);
            }
        }

        public void ViewBackupJobs()
        {
            Console.WriteLine(chaines.viewbc);
            for (int i = 0; i < backupJobs.Count; i++)
            {
                Console.WriteLine($"{i}. {backupJobs[i].Name}");
            }
            Console.ReadKey();
            Console.Clear();
        }

        public void EditBackupJob(int index)
        {
            if (index >= 0 && index < backupJobs.Count)
            {
                BackupJob job = backupJobs[index];

                Console.WriteLine($"{chaines.editbc} : {job.Name}");
                job.Name = GetNonEmptyInput(chaines.editbcname);
                job.SourceDirectory = GetNonEmptyInput(chaines.editbcsd);
                job.TargetDirectory = GetTargetDirectory(chaines.editbctd, job.SourceDirectory);
                job.Type = GetBackupType(chaines.editbctype);
                Console.WriteLine($"{chaines.editbc} {job.Name}");
                Console.Write(chaines.editbcname);
                jsonSave.WriteSaveJson(backupJobs);

                Console.WriteLine(chaines.editval);
            }
            else
            {
                Console.WriteLine(chaines.indexerror);
            }
        }

        public void DeleteBackupJob(int index)
        {
            if (index >= 0 && index < backupJobs.Count)
            {
                backupJobs.RemoveAt(index);
                jsonSave.WriteSaveJson(backupJobs);
                Console.WriteLine(chaines.delval);
            }
            else
            {
                Console.WriteLine(chaines.indexerror);
            }
        }

        private void ExecuteBackupJob(BackupJob job)
        {
            DateTime debut = DateTime.Now;
            Console.WriteLine($"{chaines.execoneview} {job.Name}");
            // Logique de sauvegarde ici

            ProgressBar progressBar = new ProgressBar();
            CopyFilesToDestination(job.SourceDirectory, job.TargetDirectory, job.Type, progressBar);
            DateTime end = DateTime.Now;

            Console.WriteLine(chaines.execval);


            // Enregistrement dans le fichier log journalier
            log.LogToDailyLogFile(job,debut,end);
 
        }

        
    

        private void LogToRealTimeStateFile(BackupJob job)
        {
            // Logique d'écriture dans le fichier d'état en temps réel en format JSON
            /*string stateEntry = JsonSerializer.Serialize(new
            {
                JobName = job.Name,
                Timestamp = DateTime.Now,
                State = "Actif", // Vous devrez ajuster cela en fonction de l'état réel du travail
                                 // Ajoutez d'autres informations nécessaires
            });

            File.WriteAllText("real_time_state.json", stateEntry + Environment.NewLine);*/
        }
        static void CopyFilesToDestination(string sourceDirectory, string destinationDirectory, BackupType type, ProgressBar progressBar)
        {
            try
            {
                // Create the destination directory if it doesn't exist
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                string[] files = Directory.GetFiles(sourceDirectory);
                int totalFiles = files.Length;
                int completedFiles = 0;

                Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                    file =>
                    {
                        string fileName = Path.GetFileName(file);
                        string destFile = Path.Combine(destinationDirectory, fileName);

                    // Recursively copy files and subfolders in subdirectories
                    if (type == BackupType.Full)
                        {
                            File.Copy(file, destFile, true);
                        }
                        else if (type == BackupType.Differential)
                        {
                            if (!File.Exists(destFile) || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(destFile))
                            {
                                File.Copy(file, destFile, true);
                            }
                        }

                    // Increment after updating the progress bar
                    Interlocked.Increment(ref completedFiles);
                        progressBar.Report((double)completedFiles / totalFiles);
                    });

                // Copy subfolders
                string[] subdirectories = Directory.GetDirectories(sourceDirectory);
                foreach (string subdirectory in subdirectories)
                {
                    string subdirectoryName = Path.GetFileName(subdirectory);
                    string destSubdirectory = Path.Combine(destinationDirectory, subdirectoryName);

                    // Create the destination subfolder if it doesn't exist
                    Directory.CreateDirectory(destSubdirectory);

                    // Recursively copy files and subfolders in subdirectories
                    CopyFilesToDestination(subdirectory, destSubdirectory, type, progressBar);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private string GetNonEmptyInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine(chaines.emptyvalerr);
                }
            }
        }
        private string GetTargetDirectory(string prompt, string source)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value) && !value.Equals(source, StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }
                else
                {
                    Console.WriteLine(chaines.errortarget);
                }
            }
        }

        private BackupType GetBackupType(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (Enum.TryParse(Console.ReadLine(), true, out BackupType type) && Enum.IsDefined(typeof(BackupType), type))
                {
                    return type;
                }
                else
                {
                    Console.WriteLine(chaines.errortype);
                }
            }
        }

        public void CreateBackupJob()
        {
            BackupJob newJob = new BackupJob();

            newJob.Name = GetNonEmptyInput(chaines.newbcname);
            newJob.SourceDirectory = GetNonEmptyInput(chaines.newbcsd);
            newJob.TargetDirectory = GetTargetDirectory(chaines.newbctd, newJob.SourceDirectory);
            newJob.Type = GetBackupType(chaines.newbctype);

            this.AddBackupJob(newJob);
            Console.WriteLine(chaines.newval);
            
        }
    }


}
