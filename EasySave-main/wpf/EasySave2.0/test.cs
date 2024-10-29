using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Path = System.IO.Path;

namespace test_projet
{
    public class Copieur
    {
        //connexion _connexion = new connexion();
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
        public bool pause = true;
        public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public async Task CopyFilesToDestination(string saveName, string sourceDirectory, string destinationDirectory, BackupType type, Progress progressWindow, CancellationToken cancellationToken, List<string> selectedext, string log, connexion _connexion)
        {
            try
            {
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                string[] files = Directory.GetFiles(sourceDirectory);
                int totalFiles = files.Length;
                int completedFiles = 0;
                //attention le path est régler sur le projet !
                string keyFilePath = "..\\..\\..\\BinKey";
                byte[] xorKey = ChiffrementXOR.LoadXorKey(keyFilePath);

                TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
                cancellationTokenSource = new CancellationTokenSource();
                await Task.Run(() =>
                {
                    while (IsExcelRunning(log))
                    {

                        // Mettre en pause le traitement
                        MessageBox.Show("Logiciel metier detecté Pause", "Indic", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Vous pouvez ajouter ici un mécanisme pour attendre ou demander à l'utilisateur de reprendre.
                    }
                    Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                        file =>
                        {
                            
                            try
                            {
                                pauseEvent.Wait(cancellationToken);
                                cancellationToken.ThrowIfCancellationRequested();



                                string fileName = Path.GetFileName(file);
                                string destFile = Path.Combine(destinationDirectory, fileName);

                                if (type == BackupType.Full || (type == BackupType.Differential && (!File.Exists(destFile) || File.GetLastWriteTimeUtc(file) > File.GetLastWriteTimeUtc(destFile))))
                                {

                                    File.Copy(file, destFile, true);
                                    if (selectedext.Contains(Path.GetExtension(destFile)))
                                    {
                                        // Appel à ChiffrementXOR.EncryptFile si l'extension est présente
                                        ChiffrementXOR.EncryptFile(destFile, xorKey);
                                    }
                                }
                                _connexion.SendProgress((int)Math.Round(100.0 / totalFiles), saveName);
                                progressWindow.Dispatcher.Invoke(() =>
                                {
                                    double fileProgressRatio = 100.0 / totalFiles;
                                    int roundedProgress = (int)Math.Round(fileProgressRatio);
                                    progressWindow.IncrementProgress(roundedProgress);
                                });

                            }
                            catch (OperationCanceledException)
                            {
                                // Handle cancellation
                                return;
                            }
                            catch (Exception ex)
                            {
                                // Log the error, but continue with the next file
                                Console.WriteLine($"Error copying file: {ex.Message}");
                            }
                            finally
                            {
                                if (Interlocked.Decrement(ref totalFiles) == 0)
                                {
                                    progressWindow.Dispatcher.Invoke(() =>
                                    {
                                        progressWindow.Finished();
                                    });
                                    completionSource.SetResult(true);
                                    progressWindow.finished = true;
                                }
                            }
                        });
                }, cancellationTokenSource.Token);

                completionSource.Task.Wait();

                string[] subdirectories = Directory.GetDirectories(sourceDirectory);
                foreach (string subdirectory in subdirectories)
                {
                    string subdirectoryName = Path.GetFileName(subdirectory);
                    string destSubdirectory = Path.Combine(destinationDirectory, subdirectoryName);

                    Directory.CreateDirectory(destSubdirectory);

                    await CopyFilesToDestination(saveName, subdirectory, destSubdirectory, type, progressWindow, cancellationToken, selectedext,log, _connexion);
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        //verif si excel est lancé
        static bool IsExcelRunning(string log)
        {
            Process[] processes = Process.GetProcessesByName(log);

            // Vérifie si un processus Excel est en cours d'exécution
            return processes.Length > 0;
        }
        public void StopCopy()
        {
            cancellationTokenSource.Cancel();
        }
    }
}
