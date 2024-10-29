using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace test_projet
{
    public partial class Progress : Window
    {
        public Copieur copy;
        public bool finished = false;
        public bool ispaused = false;

        public Progress(Copieur copy)
        {
            InitializeComponent();
            this.copy = copy;
            copy.pauseEvent.Reset(); // Pause the operation
            copy.pause = true;
        }

        public void IncrementProgress(int increment)
        {
            // To avoid the progress bar from updating when it's finished
            if (!finished)
            {
                ExecutionProgressBar.Value = increment;
            }
        }

        public void Finished()
        {
            ExecutionProgressBar.Value = 100;
            var okButton = new Button { Content = "OK" };
            okButton.Click += (sender, e) => Close();
            Grid.SetRow(okButton, 2);
            Grid.SetColumn(okButton, 0);
            ((Grid)Content).Children.Add(okButton);
            action.Content = "Done executing backup: ";
        }

        public void changeName(string name)
        {
            selected_save.Content = name;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (copy.pause)
            {
                copy.pauseEvent.Set(); // Resume the operation
                copy.pause = false;
                Pause.Content = "Pause";
                action.Content = "Executing backup : ";
            }
            else
            {
                copy.pauseEvent.Reset(); // Pause the operation
                copy.pause = true;
                Pause.Content = "Resume";
                action.Content = "Paused backup : ";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Pause_Click(null, null);
            copy.StopCopy();
            action.Content = "Stopped backup : ";
            Close();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            copy.pauseEvent.Set(); // Start the operation
            copy.pause = false;
            action.Content = "Executing backup : ";
        }
    }
}
