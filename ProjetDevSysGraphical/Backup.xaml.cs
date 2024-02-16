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

            int i = 0; // keep track of the row index

            foreach (var backup in backups)
            {
                TextBlock textBlock1 = new TextBlock { Text = backup.Name };
                TextBlock textBlock2 = new TextBlock { Text = backup.Source };
                TextBlock textBlock3 = new TextBlock { Text = backup.Destination };
                TextBlock textBlock4 = new TextBlock { Text = backup.Type };

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

                SetButtonName(backup.Name);

                // Create a button for Delete
                Button buttonDelete = new Button();
                buttonDelete.Width = 30;
                buttonDelete.Height = 30;
                buttonDelete.Background = Brushes.Red;
                buttonDelete.Click += new RoutedEventHandler(ButtonDelete_Click);
                Grid.SetColumn(buttonDelete, 4);
                Grid.SetRow(buttonDelete, i);

                // Create a button for Edit
                Button buttonEdit = new Button();
                buttonEdit.Width = 30;
                buttonEdit.Height = 30;
                buttonEdit.Background = Brushes.Blue;
                buttonEdit.Click += new RoutedEventHandler(ButtonEdit_Click);
                Grid.SetColumn(buttonEdit, 5);
                Grid.SetRow(buttonEdit, i);

                // Create a checkbox
                CheckBox checkBox = new CheckBox();
                checkBox.Width = 30;
                checkBox.Height = 30;
                checkBox.Click += new RoutedEventHandler(CheckBox_Click);
                Grid.SetColumn(checkBox, 6);
                Grid.SetRow(checkBox, i);

                // Add the buttons to the StackPanel
                grid.Children.Add(buttonDelete);
                grid.Children.Add(buttonEdit);
                grid.Children.Add(checkBox);

                buttonDelete.Name = buttonNameList[i] + "_" + i.ToString() + "_" + "Delete";
                buttonEdit.Name = buttonNameList[i] + "_" + i.ToString() + "_" + "Edit";
                checkBox.Name = buttonNameList[i] + "_" + i.ToString() + "_" + "CheckBox";
                // Add the buttons to the list
                deleteButtonList.Add(buttonDelete);
                editButtonList.Add(buttonEdit);

                i++;
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


        public void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Add a new task
            AddTask addTask = new AddTask();
            addTask.ShowDialog();
        }

        public void ButtonlaunchAllTasks_Click(object sender, RoutedEventArgs e)
        {
            RunSaveTask runSaveTask = new RunSaveTask();
            // Launch all tasks
            runSaveTask.RunMultipleTask(0, grid.Children.Count / 4);
        }

        public void ButtonlaunchSelectedTasks_Click(object sender, RoutedEventArgs e)
        {
            RunSaveTask runSaveTask = new RunSaveTask();
            runSaveTask.RunTaskMultiple(idToLaunch.ToArray());
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
                PopUpWPF popUpWPF = new PopUpWPF(buttonName + " " + "Task deleted successfully");
                popUpWPF.ShowDialog();
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
    }
}
#endregion