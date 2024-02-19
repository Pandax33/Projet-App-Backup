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
            // Clear existing rows
            BackupsGrid.RowDefinitions.Clear();
            BackupsGrid.Children.Clear();

            int row = 0;
            foreach (var backup in ProjetDevSys.AppConstants.backupProgress)
            {
                BackupsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var nameLabel = new TextBlock { Text = $"Nom de la backup : {backup.Key}" };
                Grid.SetRow(nameLabel, row);
                Grid.SetColumn(nameLabel, 0);

                var progressBar = new ProgressBar
                {
                    Value = backup.Value,
                    Maximum = 100,
                    Minimum = 0,
                    Width = 200
                };
                Grid.SetRow(progressBar, row);
                Grid.SetColumn(progressBar, 1);

                BackupsGrid.Children.Add(nameLabel);
                BackupsGrid.Children.Add(progressBar);

                row++;
            }
        }
    }
}
