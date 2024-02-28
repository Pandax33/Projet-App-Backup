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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjetDevSysGraphical.VueModel;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Logique d'interaction pour Backup.xaml
    /// </summary>
    public partial class BackupView : UserControl
    {
        private GestionTask gestionTask = new GestionTask();
        private List<Button> deleteButtonList = new List<Button>();
        private List<Button> editButtonList = new List<Button>();
        private List<string> buttonNameList = new List<string>();
        private List<int> idToLaunch = new List<int>();

        private int? shiftSelected = null;
        public BackupView()
        {
            InitializeComponent();
            GenerateGrid(); 
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            selectedTasksButton.Content += $" : 0"; //prepare the button with empty value
        }

        public void GenerateGrid()
        {
            grid.Children.Clear();
            BackupGridViewModel backupGridViewModel = new BackupGridViewModel();
            var backups = backupGridViewModel.GetAllBackupsModel();

            int i = 1; // keep track of the row index
            int index = 0;
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock ID = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.ID"),
                FontWeight = FontWeights.Bold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            ID.Margin = new Thickness(5);
            grid.Children.Add(ID);
            Grid.SetColumn(ID, 0);
            Grid.SetRow(ID, 0);

            TextBlock Name = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Name"),
                FontWeight = FontWeights.Bold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            Name.Margin = new Thickness(5);
            grid.Children.Add(Name);
            Grid.SetColumn(Name, 1);
            Grid.SetRow(Name, 0);

            TextBlock Source = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Source"),
                FontWeight = FontWeights.SemiBold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Source.Margin = new Thickness(5);
            grid.Children.Add(Source);
            Grid.SetColumn(Source, 2);
            Grid.SetRow(Source, 0);

            TextBlock Destination = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Target"),
                FontWeight = FontWeights.Bold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Destination.Margin = new Thickness(5);
            grid.Children.Add(Destination);
            Grid.SetColumn(Destination, 3);
            Grid.SetRow(Destination, 0);

            TextBlock Type = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Type"),
                FontWeight = FontWeights.Bold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Type.Margin = new Thickness(5);
            grid.Children.Add(Type);
            Grid.SetColumn(Type, 4);
            Grid.SetRow(Type, 0);

            TextBlock Supprimer = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Delete"),
                FontWeight = FontWeights.Bold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Supprimer.Margin = new Thickness(5);
            grid.Children.Add(Supprimer);
            Grid.SetColumn(Supprimer, 5);
            Grid.SetRow(Supprimer, 0);

            TextBlock Editer = new TextBlock
            {
                Text = ResourceHelper.GetString("Task.Grid.Edit"),
                FontWeight = FontWeights.Bold, // Utilisation de Bold pour mise en valeur
                FontSize = (double)Application.Current.Resources["FontSizeTittle"], // Taille de police ajustée pour l'équilibre depuis les ressources
                Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"], // Couleur du texte depuis les ressources
                FontFamily = (FontFamily)Application.Current.Resources["FontGrid"], // Police de caractère depuis les ressources
                TextWrapping = TextWrapping.Wrap, // Activation du retour à la ligne automatique
                Padding = new Thickness(2, 4, 2, 4), // Ajout d'un peu d'espacement interne
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Editer.Margin = new Thickness(5);
            grid.Children.Add(Editer);
            Grid.SetColumn(Editer, 6);
            Grid.SetRow(Editer, 0);


            foreach (var backup in backups)
            {
                var commonFontSize = (double)Application.Current.Resources["FontSizeGrid"];
                var commonFontFamily = (FontFamily)Application.Current.Resources["FontGrid"];
                var commonForeground = (SolidColorBrush)Application.Current.Resources["Brush5"];
                var commonMargin = new Thickness(5,10,5,5);
                var ButtonMargin = new Thickness(3,3,3,10);

                //Add highlighters
                if (i < backups.Count()+1)
                {
                    Border highlighter = new Border
                    {
                        Name = $"HighlighterBackup{i}",
                        Height = 50,
                        Background = (SolidColorBrush)Application.Current.Resources["Brush1"],
                        Margin = new Thickness(0, 5, 0, 5),
                        VerticalAlignment = VerticalAlignment.Bottom,
                        CornerRadius = new CornerRadius(10),
                    };

                    grid.Children.Add(highlighter);
                    Grid.SetColumn(highlighter, 0);
                    Grid.SetRow(highlighter, i);
                    Grid.SetColumnSpan(highlighter, 8);
                };

                // Create TextBlocks with common styling and set them to bold
                //ID
                TextBlock textBlockID = new TextBlock
                {
                    Text = $"#{i} ",
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.Bold, // Set to bold
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                //Name
                TextBlock textBlock1 = new TextBlock
                {
                    Text = backup.Name,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.Bold, // Set to bold
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                //Source
                TextBlock textBlock2 = new TextBlock
                {
                    Text = backup.Source,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.Medium, // Set to bold
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                //Target
                TextBlock textBlock3 = new TextBlock
                {
                    Text = backup.Destination,
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.Medium, // Set to bold
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                //Type
                TextBlock textBlock4 = new TextBlock
                {
                    Text = GetTypeBackupName(backup.Type),
                    Margin = commonMargin,
                    FontSize = commonFontSize,
                    FontFamily = commonFontFamily,
                    Foreground = commonForeground,
                    FontWeight = FontWeights.Medium, // Set to bold
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                // Add TextBlocks to the grid and set their positions
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                grid.Children.Add(textBlockID);
                Grid.SetColumn(textBlockID, 0);
                Grid.SetRow(textBlockID, i);

                grid.Children.Add(textBlock1);
                Grid.SetColumn(textBlock1, 1);
                Grid.SetRow(textBlock1, i);

                grid.Children.Add(textBlock2);
                Grid.SetColumn(textBlock2, 2);
                Grid.SetRow(textBlock2, i);

                grid.Children.Add(textBlock3);
                Grid.SetColumn(textBlock3, 3);
                Grid.SetRow(textBlock3, i);

                grid.Children.Add(textBlock4);
                Grid.SetColumn(textBlock4, 4);
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
                        Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"],
                        //TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                    },
                    Style = (Style)Application.Current.Resources["NavigationButtonStyle"], // Apply the common style
                    ToolTip = "Delete"
                };
                buttonDelete.Style = AppConstants.GridButtonStyle();
                buttonDelete.Click += new RoutedEventHandler(ButtonDelete_Click);
                Grid.SetColumn(buttonDelete, 5);
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
                        Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"],
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                    },
                    ToolTip = "Edit"
                };
                buttonEdit.Style = AppConstants.GridButtonStyle();
                buttonEdit.Click += new RoutedEventHandler(ButtonEdit_Click);
                Grid.SetColumn(buttonEdit, 6);
                Grid.SetRow(buttonEdit, i);
                grid.Children.Add(buttonEdit);

                // Add the buttons to the StackPanel
                buttonDelete.Name = buttonNameList[index] + "_" + index.ToString() + "_" + "Delete";
                buttonEdit.Name = buttonNameList[index] + "_" + index.ToString() + "_" + "Edit";
                // Add the buttons to the list
                deleteButtonList.Add(buttonDelete);
                editButtonList.Add(buttonEdit);

                i++;
                index ++;
            }
        }

        private string GetTypeBackupName(string type)
        {
            switch(type)
            {
                case "A": return ResourceHelper.GetString("Task.Type1");
                case "B": return ResourceHelper.GetString("Task.Type2");
                default: return "Error";
            }
        }

        private void SetButtonName(string buttonName)
        {
            buttonNameList.Add(buttonName);
        }

        #region ButtonClicks
        public void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Add a new task
            AddTask addTask = new AddTask();
            addTask.ShowDialog();
        }

        public async void allTasksButton_Click(object sender, RoutedEventArgs e)
        {

            SynchronizationContext context = SynchronizationContext.Current;
            int taskCount = grid.Children.Count / 4;
            int[] idList = Enumerable.Range(0, taskCount).ToArray();
            try
            {
                BackupManager.AddBackupToQueue(idList);
                ProcessWatcherViewModel processWatcherViewModel = new ProcessWatcherViewModel();
                if (processWatcherViewModel.Blockerprocess() == true)
                {
                    MessageBox.Show(ResourceHelper.GetString("RunTaskView4"), ResourceHelper.GetString("RunTaskView5"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = new Accueil();
            }
        }


        public async void selectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
    
            SynchronizationContext context = SynchronizationContext.Current;
            BackupManager.SetSynchronizationContext(context);
            int[] idList = idToLaunch.ToArray();

            try
            {
                BackupManager.AddBackupToQueue(idList);
                ProcessWatcherViewModel processWatcherViewModel = new ProcessWatcherViewModel();
                if (processWatcherViewModel.Blockerprocess() == true)
                {
                    MessageBox.Show(ResourceHelper.GetString("RunTaskView4"), ResourceHelper.GetString("RunTaskView5"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, affichez un message
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.contentControl.Content = new Accueil();
            }

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

                //MessageBox.Show(ResourceHelper.GetString("Task.DeleteInfo"), ResourceHelper.GetString("Task.Delete"), MessageBoxButton.OK, MessageBoxImage.Information);
                GenerateGrid();
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            // Extract the index from the button's name
            string buttonName = clickedButton.Name;
            buttonName = buttonName.Split('_')[0];

            BackupGridViewModel backupGridViewModel = new BackupGridViewModel();
            var backups = backupGridViewModel.GetAllBackupsModel();
            foreach (var backup in backups)
            {
                if (backup.Name == buttonName)
                {
                    if (clickedButton != null)
                    {
                        int index = int.Parse(clickedButton.Name.Split('_')[1]);

                        // Call EditTask view
                        EditTask editTask = new EditTask(index, buttonName, backup.Source, backup.Destination, backup.Type);
                        editTask.ShowDialog();
                        return;
                    }
                }
            }
        }

        private void addTaskButton_Click_1(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new AddTask();
            addTask.ShowDialog();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Get as Grid
            Grid grid = sender as Grid;
            if (grid == null) return;

            //Get position
            var clickPosition = e.GetPosition(grid);

            //link row
            int rowIndex = 0;
            double accumulatedHeight = 0.0;
            foreach (var rowDefinition in grid.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= clickPosition.Y)
                    break;
                rowIndex++;
            }

            RowSelector(rowIndex, Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));

            selectedTasksButton.Content = ResourceHelper.GetString("Task.LaunchSelect") + $" : {idToLaunch.Count}";

        }
        #endregion

        private void RowSelector(int rowIndex, bool shiftDown=false)
        {
            if (shiftDown)
            {
                if (!shiftSelected.HasValue)
                {
                    //If first selected
                    shiftSelected = rowIndex;
                }
                else
                {
                    //Second selection
                    if (shiftSelected > rowIndex)
                    {
                        int temp = (int)shiftSelected; //shiftSelected is int? so it can be null. Cast required.
                        shiftSelected = rowIndex;
                        rowIndex = temp;
                    }

                    for (int i = (int)shiftSelected; i <= rowIndex; i++)
                    {
                        RowSelector(i); 
                    }

                    //reset
                    shiftSelected = null;
                }
            }
            else
            {
                //reset shift
                shiftSelected = null;
                if (rowIndex < grid.Children.Count)
                {
                    var highlighter = grid.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => Grid.GetRow(b) == rowIndex);
                    if (highlighter == null) return;
                    var currentBrush = highlighter.Background as SolidColorBrush;

                    var brushBase = (SolidColorBrush)Application.Current.Resources["Brush1"];
                    var brushHighlight = (SolidColorBrush)Application.Current.Resources["Brush2"];

                    if (currentBrush != null && currentBrush.Color == brushBase.Color)
                    {
                        highlighter.Background = brushHighlight;
                        if (!idToLaunch.Contains(rowIndex-1)) //Don't forget the offset : Graphically starts at index 1. idToLaunch starts at index 0.
                        {
                            idToLaunch.Add(rowIndex-1);
                        }
                    }
                    else
                    {
                        highlighter.Background = brushBase;
                        if (idToLaunch.Contains(rowIndex-1)) //Don't forget the offset : Graphically starts at index 1. idToLaunch starts at index 0.
                        {
                            idToLaunch.Remove(rowIndex-1);
                        }
                    }
                }
            }
        }
    }
}