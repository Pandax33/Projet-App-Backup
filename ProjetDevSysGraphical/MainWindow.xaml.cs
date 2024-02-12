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
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
        public void ButtonlaunchAllTasks_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
        public void ButtonlaunchSelectedTasks_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void ButtonInstance()
        {
            foreach (var item in dataGrid.Items)
            {
                // Create a button for each row in the DataGrid
                Button button = new Button();
                button.Content = "Delete";

                // Create a click event for each row in the DataGrid
                CheckBox checkBox = new CheckBox();
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.VerticalAlignment = VerticalAlignment.Bottom;
                // checkBox.Margin = Datagrid.row.position;
            }
        }
    }
}