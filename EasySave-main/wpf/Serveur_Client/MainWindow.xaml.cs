using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.ObjectModel;

namespace Serveur_Client
{
    public partial class MainWindow : Window
    {
        private Socket server;
        private Socket client;
        private CancellationTokenSource cancellationTokenSource;
        Thread serverThread;
        private ObservableCollection<SaveInProgress> savesInProgress;

        public MainWindow()
        {
            InitializeComponent();
            serverThread = new Thread(Launch);
            serverThread.Start();
            Closing += Window_Closing;
            savesInProgress = new ObservableCollection<SaveInProgress>();
            SaveDataGrid.ItemsSource = savesInProgress;
        }

        private void Launch()
        {
            try
            {
                client = SeConnecter();

                cancellationTokenSource = new CancellationTokenSource();

                Thread progressThread = new Thread(() => ListenForProgress(cancellationTokenSource.Token));
                progressThread.Start();
            }
            catch
            {
                MessageBox.Show("error : No connection to server", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (client != null)
                {
                    RestartThread();
                }

            }
        }

        private static Socket SeConnecter()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            int port = 12345;

            IPEndPoint serverEndpoint = new IPEndPoint(iPAddress, port);
            clientSocket.Connect(serverEndpoint);

            Console.WriteLine("Connecté au serveur.");

            return clientSocket;
        }

        private void ListenForProgress(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested && client.Connected)
                {
                    byte[] buffer = new byte[128];
                    int bytesRead = client.Receive(buffer);

                    if (bytesRead == 0)
                    {
                        // connexion fermée par le serveur, sortie de while
                        break;
                    }
                    /*else if (bytesRead == 4)
                    {
                        int progress = BitConverter.ToInt32(buffer, 0);

                        // Update the progress value in the corresponding SaveInProgress item
                        Dispatcher.Invoke(() =>
                        {
                            if (savesInProgress.Any())
                            {
                                savesInProgress[0].Progress = progress; // Assuming you want to update the first item, adjust as needed
                            }
                        });
                    }*/
                    /*else if (bytesRead == "BackupStarted".Length)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        HandleServerMessage(message);
                    }*/
                    else
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        HandleServerMessage(message);
                    }
                }
            }
            catch (SocketException ex)
            {
                // Gérer la fermeture de la connexion ici
                Console.WriteLine($"La connexion a été fermée par le serveur. Message d'erreur : {ex.Message}");
            }
            finally
            {
                // Fermer proprement la connexion et le token lorsque le thread se termine
                client?.Close();
                cancellationTokenSource?.Cancel();
            }
        }
        private void HandleServerMessage(string message)
        {
            string[] parts = message.Split(':');
            switch (parts[0])
            { 
                case "BackupStarted":
                    MessageBox.Show("New save launched: " + parts[1], "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                    Dispatcher.Invoke(() =>
                    {
                        AddRow(parts[1]);
                    });
                    break;

                case "ProgressUpdate":
                    string saveName = parts[1];
                    string[] pr = parts;
                    if (parts[2] != null)
                    {
                        int progress = int.Parse(parts[2]);
                        Dispatcher.Invoke(() =>
                        {
                            var saveToUpdate = savesInProgress.FirstOrDefault(s => s.SelectedSave == saveName);
                            if (saveToUpdate != null)
                            {
                                saveToUpdate.Progress = progress;
                            }
                        });
                        if (progress == 100)
                        {
                            RemoveRow(saveName);
                        }
                    }
                    
                    break;
            }
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {

                // Annuler la tâche asynchrone côté client lors de la fermeture de l'application
               
                cancellationTokenSource?.Cancel();
                client?.Close();
                
            }
            catch
            {

            }
        }

        private void RestartThread()
        {
            
            cancellationTokenSource?.Cancel();
            serverThread = new Thread(Launch);
            serverThread.Start();
        }

        private void AddRow(string name)
        {
            SaveInProgress newSave = new SaveInProgress
            {
                BackupAction = "Start backup...",
                SelectedSave = name,
                Progress = 0
            };
            savesInProgress.Add(newSave);
        }

        private void RemoveRow(string saveName)
        {

            var saveToUpdate = savesInProgress.FirstOrDefault(s => s.SelectedSave == saveName);
            Dispatcher.Invoke(() => 
                savesInProgress.Remove((SaveInProgress)SaveDataGrid.SelectedItem)
            );
            
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }        
        private void Pause_Click(object sender, RoutedEventArgs e)
        {

        }       
        private void Stop_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
