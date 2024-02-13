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
using ProjetDevSys.Model;
using ProjetDevSys.VueModel;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GestionTask gestionTask = new GestionTask();


        public MainWindow()
        {
            InitializeComponent();
            GenerateGrid();
        }

        public void ButtonInstance()
        {
            foreach (var item in dataGrid.Items)
            {
                // Create a button for each row in the DataGrid
                Button button = new Button();
                button.Content = "Delete";
                button.Click += new RoutedEventHandler(ButtonDelete_Click);

                // Create a click event for each row in the DataGrid
                CheckBox checkBox = new CheckBox();
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.VerticalAlignment = VerticalAlignment.Bottom;
                checkBox.Margin = new Thickness(635, 0, 0, 305);
            }
        }

        public void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new AddTask();
            addTask.Show();
            Hide();
        }

        public void ButtonlaunchAllTasks_Click(object sender, RoutedEventArgs e)
        {
            // Launch all tasks
            foreach (ItemCollection item in dataGrid.Items)
            {
                // gestionTask.CreateTask("fileName", "sourcePath", "destinationPath", "backupType");
            }
        }

        public void ButtonlaunchSelectedTasks_Click(object sender, RoutedEventArgs e)
        {
            // Launch selected tasks
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            gestionTask.DeleteTask(dataGrid.SelectedIndex);
        }

        public void GenerateGrid()
        {
            BackupGridViewModel backupGridViewModel = new BackupGridViewModel();
            // Obtenez la liste des sauvegardes depuis le ViewModel
            var backups = backupGridViewModel.GetAllBackupsModel();
            // Assurez-vous que la DataGrid est vide
            dataGrid.Items.Clear();
            // Ajoutez les sauvegardes à la DataGrid
            foreach (var backup in backups)
            {
                dataGrid.Items.Add(new 
                {
                    Propriete1 = backup.Name,
                    Propriete2 = backup.Source,
                    Propriete3 = backup.Destination,
                    Propriete4 = backup.Type
                });
            }
        }
    }
}