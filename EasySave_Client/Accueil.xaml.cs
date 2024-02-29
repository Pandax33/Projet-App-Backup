using EasySave_Client;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static Accueil CurrentInstance { get; private set; }
        public Accueil()
        {
            InitializeComponent();
            GenerateGrid();
            CurrentInstance = this;

        }
      
        public void GenerateGrid()
        {
            // Clear existing rows and content
            BackupsGrid.RowDefinitions.Clear();
            BackupsGrid.Children.Clear();

            int row = 0;
            if (BackupsGrid == null)
            {
                Console.WriteLine("BackupsGrid est null.");
                return;
            }
            foreach (var backup in AppConstants.backupProgress)
            {
                BackupsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                //Add highlighters
                if (row <= AppConstants.backupProgress.Count())
                {
                    Border border = new Border
                    {
                        Name = $"BorderBackup{row}",
                        Height = 50,
                        Background = (SolidColorBrush)Application.Current.Resources["Brush2"],
                        Margin = new Thickness(0, 5, 0, 5),
                        VerticalAlignment = VerticalAlignment.Bottom,
                        CornerRadius = new CornerRadius(10),
                    };

                    BackupsGrid.Children.Add(border);
                    Grid.SetColumn(border, 0);
                    Grid.SetRow(border, row);
                    Grid.SetColumnSpan(border, 8);
                };

                Thickness commonMargin = new Thickness(5, 10, 5, 5);

                TextBlock nameLabel = new TextBlock { 
                    Text = backup.Key,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = commonMargin,
                    FontSize = (double)Application.Current.Resources["FontSizeGrid"],
                    FontFamily = (FontFamily)Application.Current.Resources["FontTitle"],
                    Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"],
                    FontWeight = FontWeights.Bold,
                };
                Grid.SetRow(nameLabel, row);
                Grid.SetColumn(nameLabel, 0);
                BackupsGrid.Children.Add(nameLabel);


                ProgressBar progressBar = new ProgressBar
                {
                    Name = $"ProgressBar_{backup.Key}",
                    Value = backup.Value,
                    Maximum = 100,
                    Minimum = 0,
                    Width = 200,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = commonMargin,
                };
                Grid.SetRow(progressBar, row);
                Grid.SetColumn(progressBar, 1);
                BackupsGrid.Children.Add(progressBar);

                Button toggleButton = new Button
                {
                    Tag = backup.Key,
                    Width = 30,
                    Height = 30,
                    Margin = commonMargin,
                    Content = new TextBlock
                    {
                        Text = "▶",
                        Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"],
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                    },
                };

                toggleButton.Style = AppConstants.GridButtonStyle();
                bool isPaused = false; //initial state (logic-wise)

                toggleButton.Click += (sender, e) =>
                {
                    if (isPaused)
                    {
                        toggleButton.Content = "▶";
                        RepriseButton_Click(sender, e); 
                    }
                    else
                    {
                        toggleButton.Content = "⏸";
                        PauseButton_Click(sender, e);
                    }
                    isPaused = !isPaused;
                };

                Grid.SetRow(toggleButton, row);
                Grid.SetColumn(toggleButton, 2); 
                BackupsGrid.Children.Add(toggleButton);

                Button stopButton = new Button
                {
                    Tag = backup.Key,
                    Width = 30,
                    Height = 30,
                    Margin = commonMargin,
                    Content = new TextBlock
                    {
                        Text = "■",
                        Foreground = (SolidColorBrush)Application.Current.Resources["Brush3"],
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Top,
                    },
                };
                stopButton.Style = AppConstants.GridButtonStyle();
                stopButton.Click += StopButton_Click; 
                Grid.SetRow(stopButton, row);
                Grid.SetColumn(stopButton, 4);
                BackupsGrid.Children.Add(stopButton);

                row++;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ClientSocket.PauseBackup(button.Tag.ToString());
            
        }

        private void RepriseButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ClientSocket.ResumeBackup(button.Tag.ToString());

        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            string backupName = button.Tag.ToString();
            ClientSocket.StopBackup(backupName);
        }

    }
    class ProgressBarDash : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(0, 0, -(double)value, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
