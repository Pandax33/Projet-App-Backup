using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjetDevSys.VueModel;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GestionTask gestionTask = new GestionTask();
        private List<Button> deleteButtonList = new List<Button>();
        private List<Button> editButtonList = new List<Button>();
        private List<string> buttonNameList = new List<string>();
        private List<int> idToLaunch = new List<int>();

        public MainWindow()
        {
            InitializeComponent();
            GenerateGrid();
            ButtonInstance(stackPanel);
        }

        public void GenerateGrid()
        {
            dataGrid.Items.Clear();
            BackupGridViewModel backupGridViewModel = new BackupGridViewModel();
            var backups = backupGridViewModel.GetAllBackupsModel();
            foreach(var backup in backups) // backup type = Backup
            {
                dataGrid.Items.Add(new
                {
                    Propriete1 = backup.Name,
                    Propriete2 = backup.Source,
                    Propriete3 = backup.Destination,
                    Propriete4 = backup.Type
                });
                SetButtonName(backup.Name);
            }
        }

        public void ButtonInstance(StackPanel container)
        {
            int[] index = new int[dataGrid.Items.Count];
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                index[i] = i;

                // Create a horizontal StackPanel for each row in the DataGrid
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                // Create a button for Delete
                Button buttonDelete = new Button();
                buttonDelete.Width = 30;
                buttonDelete.Height = 30;
                buttonDelete.Margin = new Thickness(5, 0, 5, 0);
                buttonDelete.Background = Brushes.Red;
                buttonDelete.Click += new RoutedEventHandler(ButtonDelete_Click);

                // Create a button for Edit
                Button buttonEdit = new Button();
                buttonEdit.Width = 30;
                buttonEdit.Height = 30;
                buttonEdit.Margin = new Thickness(5, 0, 5, 0);
                buttonEdit.Background = Brushes.Blue;
                buttonEdit.Click += new RoutedEventHandler(ButtonEdit_Click);

                // Create a checkbox
                CheckBox checkBox = new CheckBox();
                checkBox.Width = 30;
                checkBox.Height = 30;
                checkBox.Margin = new Thickness(5, 0, 5, 0);
                checkBox.Click += new RoutedEventHandler(CheckBox_Click);

                // Add the buttons to the StackPanel
                panel.Children.Add(buttonDelete);
                panel.Children.Add(buttonEdit);
                panel.Children.Add(checkBox);

                // Add the StackPanel to the container
                container.Children.Add(panel);

                buttonDelete.Name = buttonNameList[i] + "_" + index[i].ToString() + "_" + "Delete";
                buttonEdit.Name = buttonNameList[i] + "_" + index[i].ToString() + "_" + "Edit";
                checkBox.Name = buttonNameList[i] + "_" + index[i].ToString() + "_" + "CheckBox";
                // Add the buttons to the list
                deleteButtonList.Add(buttonDelete);
                editButtonList.Add(buttonEdit);

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
            addTask.Show();
            Hide();
        }

        public void ButtonlaunchAllTasks_Click(object sender, RoutedEventArgs e)
        {
            RunSaveTask runSaveTask = new RunSaveTask();
            // Launch all tasks
            runSaveTask.RunMultipleTask(0, dataGrid.Items.Count);
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
                popUpWPF.Show();
                Hide();
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
                editTask.Show();
                Hide();
            }
        }
        #endregion

        private void MainWindow_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
