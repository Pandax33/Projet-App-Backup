using ProjetDevSys.MODEL;
using ProjetDevSysGraphical.VueModel;
using ProjetDevSysGraphical.Watcher;
using System.Configuration;
using System.Data;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Threading;
using ProjetDevSys.VueModel;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public ProcessWatcher processWatcher;
        private static Mutex mutex = null;
        public BackupCompletionWatcher backupCompletionWatcher;
        private volatile bool serverRunning = true;
        protected override void OnStartup(StartupEventArgs e)
        {

            const string mutexName = "StartMutex";

            // Tentative de création d'un Mutex.
            bool createdNew;
            mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show(ResourceHelper.GetString("StartupPopup1"));
                Application.Current.Shutdown();
                return;
            }
            SynchronizationContext context = SynchronizationContext.Current;
            BackupManager.SetSynchronizationContext(context);
            backupCompletionWatcher = new BackupCompletionWatcher(context);
            processWatcher = new ProcessWatcher(context);
            base.OnStartup(e);
            backupCompletionWatcher.StartWatching();
            processWatcher.StartWatching();
            if (ProjetDevSys.AppConstants.IsServOn)
            {
                ProjetDevSys.AppConstants.serverThread = new Thread(ProjetDevSys.AppConstants.StartServer) { IsBackground = true };
                ProjetDevSys.AppConstants.serverThread.Start();
            }
            ThemeLoader.LoadTheme();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            processWatcher.StopWatching();
            backupCompletionWatcher.StopWatching();
            Server.serverRunning = false;

            if (ProjetDevSys.AppConstants.serverSocket != null)
            {
                ProjetDevSys.AppConstants.serverSocket.Close();
            }
            if (mutex != null)
            {
                mutex.ReleaseMutex();
            }
            base.OnExit(e);
        }

    }

    public static class ThemeLoader
    {
        private static List<SolidColorBrush> ThemeBrush;
        private static List<FontFamily> ThemeFont;


        
        private static void themeSelector()
        {
            Color BG;
            Color B1;
            Color B2;
            Color B3;
            Color B4;
            Color B5;
            FontFamily FontTittle;
            FontFamily FontButton;
            FontFamily FontBase;
            FontFamily FontGrid;
            string theme = ProjetDevSys.AppConstants.Theme;

            //B1 = Background GRID + Bouton
            //B2 = Background Barre de navigation
            //B3 = Font Color Bouton
            //B4 = Border Color bouton Navigation
            //B5 = Font Color Grid
            switch (theme)
            {
                case "Legacy":
                    BG = (Color)ColorConverter.ConvertFromString("#463F3A");
                    B1 = (Color)ColorConverter.ConvertFromString("#8A817C");
                    B2 = (Color)ColorConverter.ConvertFromString("#BCB8B1");
                    B3 = (Color)ColorConverter.ConvertFromString("#F4F3EE");
                    B4 = (Color)ColorConverter.ConvertFromString("#E0AFA0");
                    B5 = (Color)ColorConverter.ConvertFromString("#F4F3EE");
                    FontTittle = new FontFamily("Roboto");
                    FontButton = new FontFamily("Roboto");
                    FontGrid = new FontFamily("Roboto");
                    FontBase= new FontFamily("Roboto");
                    break;
                case "Moche":

                    BG = (Color)ColorConverter.ConvertFromString("#08415C");
                    B1 = (Color)ColorConverter.ConvertFromString("#CC2936");
                    B2 = (Color)ColorConverter.ConvertFromString("#EBBAB9");
                    B3 = (Color)ColorConverter.ConvertFromString("#000000");
                    B4 = (Color)ColorConverter.ConvertFromString("#B5FFE1");
                    B5 = (Color)ColorConverter.ConvertFromString("#353839");
                    FontTittle = new FontFamily("Comic Sans MS");
                    FontButton = new FontFamily("Comic Sans MS");
                    FontGrid = new FontFamily("Comic Sans MS");
                    FontBase = new FontFamily("Comic Sans MS");
                    break;
                default:
                    BG = (Color)ColorConverter.ConvertFromString("#252627");
                    B1 = (Color)ColorConverter.ConvertFromString("#D3D4D9");
                    B2 = (Color)ColorConverter.ConvertFromString("#4B88A2");
                    B3 = (Color)ColorConverter.ConvertFromString("#353839");
                    B4 = (Color)ColorConverter.ConvertFromString("#ADD8E6");
                    B5 = (Color)ColorConverter.ConvertFromString("#353839");
                    FontTittle = new FontFamily("Roboto");
                    FontButton = new FontFamily("Roboto");
                    FontGrid = new FontFamily("Roboto");
                    FontBase = new FontFamily("Roboto");
                    break;
                case "Leandro":
                    BG = (Color)ColorConverter.ConvertFromString("#4B88A2"); // Platinum #4B88A2
                    B1 = (Color)ColorConverter.ConvertFromString("#D3D4D9"); // Air Force Blue
                    B2 = (Color)ColorConverter.ConvertFromString("#B80A21"); // Fire Brick
                    B3 = (Color)ColorConverter.ConvertFromString("#252627"); // Eerie Black
                    B4 = (Color)ColorConverter.ConvertFromString("#FFF9FB"); // Snow
                    B5 = (Color)ColorConverter.ConvertFromString("#353839");
                    FontTittle = new FontFamily("Roboto");
                    FontButton = new FontFamily("Roboto");
                    FontGrid = new FontFamily("Roboto");
                    FontBase = new FontFamily("Roboto");
                    break;
                case "Raimon":
                    BG = (Color)ColorConverter.ConvertFromString("#3d6ca6"); // blue
                    B1 = (Color)ColorConverter.ConvertFromString("#0d161f"); // dark
                    B2 = (Color)ColorConverter.ConvertFromString("#2E517B"); // deep blue
                    B3 = (Color)ColorConverter.ConvertFromString("#F0E6A8"); // cream
                    B4 = (Color)ColorConverter.ConvertFromString("#e9c557"); // golden
                    B5 = (Color)ColorConverter.ConvertFromString("#e9c557"); //golden
                    FontTittle = new FontFamily("Roboto");
                    FontButton = new FontFamily("Roboto");
                    FontGrid = new FontFamily("Roboto");
                    FontBase = new FontFamily("Roboto");
                    break;
                case "Portugal":
                    BG = (Color)ColorConverter.ConvertFromString("#046A38"); // green
                    B1 = (Color)ColorConverter.ConvertFromString("#DA291C"); // red
                    B2 = (Color)ColorConverter.ConvertFromString("#FFE900"); // yellow
                    B3 = (Color)ColorConverter.ConvertFromString("#002D72"); // blue
                    B4 = (Color)ColorConverter.ConvertFromString("#FFFFFF"); // white
                    B5 = (Color)ColorConverter.ConvertFromString("#000000"); //black
                    FontTittle = new FontFamily("Roboto");
                    FontButton = new FontFamily("Roboto");
                    FontGrid = new FontFamily("Roboto");
                    FontBase = new FontFamily("Roboto");
                    break;
                    case "Dark":
                    BG = (Color)ColorConverter.ConvertFromString("#000000"); // black
                    B1 = (Color)ColorConverter.ConvertFromString("#1C1C1C"); // dark
                    B2 = (Color)ColorConverter.ConvertFromString("#2E2E2E"); // deep dark
                    B3 = (Color)ColorConverter.ConvertFromString("#FFFFFF"); // white
                    B4 = (Color)ColorConverter.ConvertFromString("#FFFFFF"); // white
                    B5 = (Color)ColorConverter.ConvertFromString("#FFFFFF"); //white
                    FontTittle = new FontFamily("Roboto");
                    FontButton = new FontFamily("Roboto");
                    FontGrid = new FontFamily("Roboto");
                    FontBase = new FontFamily("Roboto");
                    break;
            }

            SolidColorBrush brushBG = new SolidColorBrush(BG);
            SolidColorBrush brush1 = new SolidColorBrush(B1);
            SolidColorBrush brush2 = new SolidColorBrush(B2);
            SolidColorBrush brush3 = new SolidColorBrush(B3);
            SolidColorBrush brush4 = new SolidColorBrush(B4);
            SolidColorBrush brush5 = new SolidColorBrush(B5);
            Application.Current.Resources["FontTitle"] = FontTittle;
            Application.Current.Resources["FontButton"] = FontButton;
            Application.Current.Resources["FontBase"] = FontBase;


            ThemeBrush = new List<SolidColorBrush>() { brushBG, brush1, brush2, brush3, brush4, brush5 };
            ThemeFont = new List<FontFamily>() { FontTittle, FontButton, FontGrid, };
        }

        public static void LoadTheme()
        {
            themeSelector();
            Application.Current.Resources["BrushBG"] = ThemeBrush[0];
            Application.Current.Resources["Brush1"] = ThemeBrush[1];
            Application.Current.Resources["Brush2"] = ThemeBrush[2];
            Application.Current.Resources["Brush3"] = ThemeBrush[3];
            Application.Current.Resources["Brush4"] = ThemeBrush[4];
            Application.Current.Resources["Brush5"] = ThemeBrush[5];
            Application.Current.Resources["FontTittle"] = ThemeFont[0];
            Application.Current.Resources["FontButton"] = ThemeFont[1];
            Application.Current.Resources["FontGrid"] = ThemeFont[2];
            Application.Current.Resources["FontSizeTittle"] = 16.0;
            Application.Current.Resources["FontSizeNavigationButton"] = 16.0;
            Application.Current.Resources["FontSizeButton"] = 14.0;
            Application.Current.Resources["FontSizeGrid"] = 14.0;
        }
    }

}
