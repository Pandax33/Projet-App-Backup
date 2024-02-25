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

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : UserControl
    {
        public Accueil()
        {
            InitializeComponent();
            UpdateGridWithBackupProgress();
            ProjetDevSys.AppConstants.BackupProgressUpdated += AppConstants_BackupProgressUpdated;
        }

        ~Accueil()
        {
            ProjetDevSys.AppConstants.BackupProgressUpdated -= AppConstants_BackupProgressUpdated;
        }
        private void AppConstants_BackupProgressUpdated(string backupName, double progress)
        {
            // Assurez-vous que l'opération se déroule sur le thread de l'UI
            Dispatcher.Invoke(() =>
            {
                // Mettre à jour l'interface utilisateur ici
                UpdateGridWithBackupProgress();
            });
        }
        public void UpdateGridWithBackupProgress()
        {
            // Clear existing rows and content
            BackupsGrid.RowDefinitions.Clear();
            BackupsGrid.Children.Clear();

            int row = 0;
            foreach (var backup in ProjetDevSys.AppConstants.backupProgress)
            {
                BackupsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                // Nom de la sauvegarde
                var nameLabel = new TextBlock { Text = $"Nom de la backup : {backup.Key}" };
                Grid.SetRow(nameLabel, row);
                Grid.SetColumn(nameLabel, 0);
                BackupsGrid.Children.Add(nameLabel);

                // Barre de progression
                var progressBar = new ProgressBar
                {
                    Value = backup.Value,
                    Maximum = 100,
                    Minimum = 0,
                    Width = 200
                };
                Grid.SetRow(progressBar, row);
                Grid.SetColumn(progressBar, 1);
                BackupsGrid.Children.Add(progressBar);

                // Bouton de pause
                var pauseButton = new Button
                {
                    Content = "Pause",
                    Tag = backup.Key // Utilisez le Tag pour stocker le nom de la sauvegarde
                };
                pauseButton.Click += PauseButton_Click; // Abonnez-vous à l'événement Click
                Grid.SetRow(pauseButton, row);
                Grid.SetColumn(pauseButton, 2);
                BackupsGrid.Children.Add(pauseButton);

                // Bouton de reprise
                var repriseButton = new Button
                {
                    Content = "Reprise",
                    Tag = backup.Key // Utilisez le Tag pour stocker le nom de la sauvegarde
                };
                repriseButton.Click += RepriseButton_Click; // Abonnez-vous à l'événement Click
                Grid.SetRow(repriseButton, row);
                Grid.SetColumn(repriseButton, 3);
                BackupsGrid.Children.Add(repriseButton);

                // Bouton Stop
                var stopButton = new Button
                {
                    Content = "Stop",
                    Tag = backup.Key // Utilisez le Tag pour stocker le nom de la sauvegarde
                };
                stopButton.Click += StopButton_Click; // Abonnez-vous à l'événement Click
                Grid.SetRow(stopButton, row);
                Grid.SetColumn(stopButton, 4); // Assurez-vous que c'est la bonne colonne
                BackupsGrid.Children.Add(stopButton);

                row++;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ProjetDevSys.AppConstants.PauseBackup(button.Tag.ToString());
            
        }

        private void RepriseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ProjetDevSys.AppConstants.ResumeBackup(button.Tag.ToString());

        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var backupName = button.Tag.ToString();
            ProjetDevSys.AppConstants.StopBackup(backupName); // Implémentez cette méthode pour arrêter la sauvegarde
        }
    }
}
