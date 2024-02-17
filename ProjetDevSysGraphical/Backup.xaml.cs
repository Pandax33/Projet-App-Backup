using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour Backup.xaml
    /// </summary>
    public partial class Backup : UserControl
    {
        private GestionTask gestionTask = new GestionTask();
        private List<Button> deleteButtonList = new List<Button>();
        private List<Button> editButtonList = new List<Button>();
        private List<string> buttonNameList = new List<string>();
        private List<int> idToLaunch = new List<int>();
        public Backup()
        {
            InitializeComponent();
            GenerateGrid();
        }

        public void GenerateGrid()
        {
            grid.Children.Clear();
            BackupGridViewModel backupGridViewModel = new BackupGridViewModel();
            var backups = backupGridViewModel.GetAllBackupsModel();

            int i = 1; // keep track of the row index
            int index = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            TextBlock Name = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Name"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };

            Name.Margin = new Thickness(5);
            grid.Children.Add(Name);
            Grid.SetColumn(Name, 0);
            Grid.SetRow(Name, 0);

            TextBlock Source = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Source"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };
            Source.Margin = new Thickness(5);
            grid.Children.Add(Source);
            Grid.SetColumn(Source, 1);
            Grid.SetRow(Source, 0);

            TextBlock Destination = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Target"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };
            Destination.Margin = new Thickness(5);
            grid.Children.Add(Destination);
            Grid.SetColumn(Destination, 2);
            Grid.SetRow(Destination, 0);

            TextBlock Type = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Type"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };
            Type.Margin = new Thickness(5);
            grid.Children.Add(Type);
            Grid.SetColumn(Type, 3);
            Grid.SetRow(Type, 0);

            TextBlock Supprimer = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Delete"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };
            Supprimer.Margin = new Thickness(5);
            grid.Children.Add(Supprimer);
            Grid.SetColumn(Supprimer, 4);
            Grid.SetRow(Supprimer, 0);

            TextBlock Editer = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Edit"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };
            Editer.Margin = new Thickness(5);
            grid.Children.Add(Editer);
            Grid.SetColumn(Editer, 5);
            Grid.SetRow(Editer, 0);

            TextBlock Selectionner = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Select"),
                FontWeight = FontWeights.Bold, // Utilisation de SemiBold pour un effet gras moins intense
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4) // Ajout d'un peu d'espacement interne
            };
            Selectionner.Margin = new Thickness(5);
            grid.Children.Add(Selectionner);
            Grid.SetColumn(Selectionner, 6);
            Grid.SetRow(Selectionner, 0);

            foreach (var backup in backups)
            {
                var commonFontSize = (double)Application.Current.Resources["FontSizeGrid"];
                var commonFontFamily = (FontFamily)Application.Current.Resources["FontGrid"];
                var commonForeground = (SolidColorBrush)Application.Current.Resources["Brush5"];
                var commonMargin = new Thickness(5);
                var ButtonMargin = new Thickness(3);

                // Create TextBlocks with common styling and set them to bold
                TextBlock textBlock1 = new TextBlock
                {
                    Text = backup.Name,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.Bold // Set to bold
                };

                TextBlock textBlock2 = new TextBlock
                {
                    Text = backup.Source,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.SemiBold // Set to bold
                };

                TextBlock textBlock3 = new TextBlock
                {
                    Text = backup.Destination,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.SemiBold // Set to bold
                };

                TextBlock textBlock4 = new TextBlock
                {
                    Text = backup.Type,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.SemiBold // Set to bold
                };

                // Add TextBlocks to the grid and set their positions
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                grid.Children.Add(textBlock1);
                Grid.SetColumn(textBlock1, 0);
                Grid.SetRow(textBlock1, i);

                grid.Children.Add(textBlock2);
                Grid.SetColumn(textBlock2, 1);
                Grid.SetRow(textBlock2, i);

                grid.Children.Add(textBlock3);
                Grid.SetColumn(textBlock3, 2);
                Grid.SetRow(textBlock3, i);

                grid.Children.Add(textBlock4);
                Grid.SetColumn(textBlock4, 3);
                Grid.SetRow(textBlock4, i);

                // Continue with your code
                SetButtonName(backup.Name);

                Button buttonDelete = new Button
                {
                    Width = 30,
                    Height = 30,
                    Margin = ButtonMargin,
                    Content = new TextBlock
                    {
                        Text = "✖",
                        Foreground = Brushes.Red,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    Style = (Style)Application.Current.Resources["NavigationButtonStyle"], // Apply the common style
                    ToolTip = "Delete"
                };
                buttonDelete.Click += new RoutedEventHandler(ButtonDelete_Click);
                Grid.SetColumn(buttonDelete, 4);
                Grid.SetRow(buttonDelete, i);
                grid.Children.Add(buttonDelete);

                // Create a button for Edit
                Button buttonEdit = new Button
                {
                    Width = 30,
                    Height = 30,
                    Margin = ButtonMargin,
                    Content = new TextBlock
                    {
                        Text = "✏",
                        Foreground = Brushes.Blue,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    },
                    Style = (Style)Application.Current.Resources["NavigationButtonStyle"], // Apply the common style
                    ToolTip = "Edit"
                };
                buttonEdit.Click += new RoutedEventHandler(ButtonEdit_Click);
                Grid.SetColumn(buttonEdit, 5);
                Grid.SetRow(buttonEdit, i);
                grid.Children.Add(buttonEdit);

                // Create a checkbox
                CheckBox checkBox = new CheckBox();
                checkBox.Width = 30;
                checkBox.Height = 30;
                checkBox.Click += new RoutedEventHandler(CheckBox_Click);
                Grid.SetColumn(checkBox, 6);
                Grid.SetRow(checkBox, i);
                grid.Children.Add(checkBox);

                // Add the buttons to the StackPanel

                
                buttonDelete.Name = buttonNameList[index] + "_" + index.ToString() + "_" + "Delete";
                buttonEdit.Name = buttonNameList[index] + "_" + index.ToString() + "_" + "Edit";
                checkBox.Name = buttonNameList[index] + "_" + index.ToString() + "_" + "CheckBox";
                // Add the buttons to the list
                deleteButtonList.Add(buttonDelete);
                editButtonList.Add(buttonEdit);

                i++;
                index ++;
            }
        }


        private void SetButtonName(string buttonName)
        {
            buttonNameList.Add(buttonName);
        }

        #region ButtonClicks
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox clicked = sender as CheckBox;
            if (clicked != null)
            {
                // Extract the index from the button's name
                int index = int.Parse(clicked.Name.Split('_')[1]);

                if (clicked.IsChecked == true)
                {
                    // Add the index to the list if it is checked
                    idToLaunch.Add(index);
                }
                else
                {
                    // Delete the index from the list if it is unchecked
                    idToLaunch.Remove(index);
                }
            }
        }


        public void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a new task
            AddTask addTask = new AddTask();
            addTask.ShowDialog();
        }

        public void allTasksButton_Click(object sender, RoutedEventArgs e)
        {
            RunSaveTask runSaveTask = new RunSaveTask();
            // Launch all tasks
            runSaveTask.RunMultipleTask(0, grid.Children.Count / 4);
        }

        public void selectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            RunSaveTask runSaveTask = new RunSaveTask();
            MessageBox.Show(runSaveTask.RunTaskMultiple(idToLaunch.ToArray()), "Run", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Extract the index from the button's name
                string buttonName = clickedButton.Name;
                buttonName = buttonName.Split('_')[0];

                int index = int.Parse(clickedButton.Name.Split('_')[1]);

                // Call DeleteTask with the extracted index
                gestionTask.DeleteTask(index);

                MessageBox.Show(ResourceHelper.GetString("Task.DeleteInfo"), ResourceHelper.GetString("Task.Delete"), MessageBoxButton.OK, MessageBoxImage.Information);
                GenerateGrid();
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Extract the index from the button's name
                string buttonName = clickedButton.Name;
                buttonName = buttonName.Split('_')[0];

                int index = int.Parse(clickedButton.Name.Split('_')[1]);

                // Call EditTask view
                EditTask editTask = new EditTask(index, buttonName, null, null, null);
                editTask.ShowDialog();
            }
        }

        private void addTaskButton_Click_1(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new AddTask();
            addTask.ShowDialog();
        }
    }
}
#endregion