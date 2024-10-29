using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace test_projet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "YourUniqueMutexName"; // Remplacez cela par un nom unique pour votre application

            mutex = new Mutex(true, mutexName, out bool createdNew);

            if (!createdNew)
            {
                // Une autre instance de l'application est déjà en cours d'exécution
                MessageBox.Show("L'application est déjà en cours d'exécution.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Current.Shutdown();
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex.ReleaseMutex(); // Libérer le mutex lorsque l'application se ferme
            mutex.Dispose();

            base.OnExit(e);
        }
    }
}
