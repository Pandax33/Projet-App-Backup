using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;

using ProjetDevSys;

namespace ProjetDevSysGraphical
{
    /// <summary>
    /// Logique d'interaction pour Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        public Config()
        {
            //pathSaveBackupEntry.Text = ProjetDevSys.AppConstants.JsonSave;
            InitializeComponent();
        }

        private void pathLogDailyExplorer_Click(object sender, RoutedEventArgs e)
        {
            logDailyEntry.Text =  AppConstants.OpenFolderDialog();
        }

        private void pathLogRTExplorer_Click(object sender, RoutedEventArgs e)
        {
            logRTEntry.Text = AppConstants.OpenFolderDialog();
        }

        private void pathSaveBackupExplorer_Click(object sender, RoutedEventArgs e)
        {
            pathSaveBackupEntry.Text = AppConstants.OpenFileDialog();
        }

        private void pathCryptoExplorer_Click(object sender, RoutedEventArgs e)
        {
            pathCryptoEntry.Text = AppConstants.OpenFileDialog();
        }

        private void languageSelector_GotFocus(object sender, RoutedEventArgs e)
        {
            languageSelector.IsDropDownOpen = true;
        }
        private void logExtensionSelector_GotFocus(object sender, RoutedEventArgs e)
        {
            logExtensionSelector.IsDropDownOpen = true;
        }
    }
}
