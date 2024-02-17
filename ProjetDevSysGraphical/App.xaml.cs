using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            ThemeLoader.LoadTheme();
        }
    }

    public static class ThemeLoader
    {
        private static List<SolidColorBrush> ThemeBrush;

        static ThemeLoader()
        {
            Color BG;
            Color B1;
            Color B2;
            Color B3;
            Color B4;
            string theme = ProjetDevSys.AppConstants.Theme;

            switch (theme)
            {
                case "Legacy":
                    BG = (Color)ColorConverter.ConvertFromString("#000022");
                    B1 = (Color)ColorConverter.ConvertFromString("#FF1C3F5D");
                    B2 = (Color)ColorConverter.ConvertFromString("#0094C6");
                    B3 = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                    B4 = (Color)ColorConverter.ConvertFromString("#FF333333");
                    break;
                case "Moche":

                    BG = (Color)ColorConverter.ConvertFromString("#08415C");
                    B1 = (Color)ColorConverter.ConvertFromString("#CC2936");
                    B2 = (Color)ColorConverter.ConvertFromString("#EBBAB9");
                    B3 = (Color)ColorConverter.ConvertFromString("#388697");
                    B4 = (Color)ColorConverter.ConvertFromString("#B5FFE1");
                    break;
                default:
                    BG = (Color)ColorConverter.ConvertFromString("#463F3A");
                    B1 = (Color)ColorConverter.ConvertFromString("#8A817C");
                    B2 = (Color)ColorConverter.ConvertFromString("#BCB8B1");
                    B3 = (Color)ColorConverter.ConvertFromString("#F4F3EE");
                    B4 = (Color)ColorConverter.ConvertFromString("#E0AFA0");
                    break;
            }

            SolidColorBrush brushBG = new SolidColorBrush(BG);
            SolidColorBrush brush1= new SolidColorBrush(B1);
            SolidColorBrush brush2= new SolidColorBrush(B2);
            SolidColorBrush brush3= new SolidColorBrush(B3);
            SolidColorBrush brush4= new SolidColorBrush(B4);

            ThemeBrush = new List<SolidColorBrush>() { brushBG, brush1, brush2, brush3, brush4 };
        }
            public static void LoadTheme()
        {
            Application.Current.Resources["BrushBG"] = ThemeBrush[0];
            Application.Current.Resources["Brush1"] = ThemeBrush[1];
            Application.Current.Resources["Brush2"] = ThemeBrush[2];
            Application.Current.Resources["Brush3"] = ThemeBrush[3];
            Application.Current.Resources["Brush4"] = ThemeBrush[4];
        }
    }

}
