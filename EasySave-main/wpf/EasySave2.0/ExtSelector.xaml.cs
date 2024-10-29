    using System;
    using System.Collections.Generic;
using System.ComponentModel;
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
        /// Logique d'interaction pour ExtSelector.xaml
        /// </summary>
        public partial class ExtSelector : Window , INotifyPropertyChanged
    {
            private MainWindow mainWindow;
            public ExtSelector(MainWindow mainWindow)
            {
                InitializeComponent();
                this.mainWindow = mainWindow;
            
            lstSelectedExtensions.ItemsSource = mainWindow.viewModel.selectedExtensions;
            foreach (UIElement element in truc.Children)
            {
                if (element is CheckBox checkBox)
                {
                    if (mainWindow.viewModel.selectedExtensions.Contains(checkBox.Content))
                    {
                        checkBox.IsChecked = true;
                    }
                    else
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Confirmer_Click(object sender, RoutedEventArgs e)
            {
                // Liste pour stocker les extensions sélectionnées


                // Vérifier chaque CheckBox et ajouter/supprimer de la liste
                mainWindow.viewModel.AddOrRemoveExtension(".txt", chkTxt.IsChecked);
                mainWindow.viewModel.AddOrRemoveExtension(".doc", chkDoc.IsChecked);
                mainWindow.viewModel.AddOrRemoveExtension(".pdf", chkPdf.IsChecked);
                mainWindow.viewModel.AddOrRemoveExtension(".csv", chkCsv.IsChecked);
            mainWindow.viewModel.AddLogBlock(Log_met.Text);
                this.Close();


                // Mettre à jour la ListBox avec les extensions sélectionnées

            }

        
        }
    }
