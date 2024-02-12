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

        public MainWindow()
        {
            InitializeComponent();
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
            // Add a new task
            // gestionTask.CreateTask("fileName", "sourcePath", "destinationPath", "backupType");
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
    }
}