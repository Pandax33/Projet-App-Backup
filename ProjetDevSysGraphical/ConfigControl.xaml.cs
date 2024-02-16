using ProjetDevSys.VueModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour ConfigControl.xaml
    /// </summary>
    public partial class ConfigControl : UserControl
    {
        public ObservableCollection<string> CryptoExtensions { get; set; }
        public ObservableCollection<string> blockerProcesses { get; set; }
        public ConfigControl()
        {
            InitializeComponent();
            Refresh();
        }

        private void pathLogDailyExplorer_Click(object sender, RoutedEventArgs e)
        {
            logDailyEntry.Text = AppConstants.OpenFolderDialog();
        }

        private void pathLogRTExplorer_Click(object sender, RoutedEventArgs e)
        {
            logRTEntry.Text = AppConstants.OpenFolderDialog();
        }

        private void pathSaveBackupExplorer_Click(object sender, RoutedEventArgs e)
        {
            pathSaveBackupEntry.Text = AppConstants.OpenFileDialog();
        }

        private void cryptoPathExplorer_Click(object sender, RoutedEventArgs e)
        {
            cryptoPathEntry.Text = AppConstants.OpenFileDialog();
        }

        private void languageSelector_GotFocus(object sender, RoutedEventArgs e)
        {
            languageSelector.IsDropDownOpen = true;
        }
        private void logExtensionSelector_GotFocus(object sender, RoutedEventArgs e)
        {
            logExtensionSelector.IsDropDownOpen = true;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset all settings ?", "Confirm Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ProjetDevSys.VueModel.ConfigViewModel config = new ConfigViewModel();
                config.resetConfig();
                Refresh();
            }
        }

        private void cryptoExtensionsAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cryptoExtensionsEntry.Text))
            {
                CryptoExtensions.Add(cryptoExtensionsEntry.Text);
                cryptoExtensionsEntry.Clear();
                cryptoExtensionsUpdate();
            }
        }

        private void cryptoExtensionsRemove_Click(object sender, RoutedEventArgs e)
        {
            if (cryptoExtensionsListView.SelectedItem != null)
            {
                CryptoExtensions.Remove((cryptoExtensionsListView.SelectedItem as dynamic).extension);
                cryptoExtensionsUpdate();
            }
        }

        private void cryptoExtensionsUpdate()
        {
            cryptoExtensionsListView.Items.Clear();
            foreach (string extensions in CryptoExtensions)
            {
                cryptoExtensionsListView.Items.Add(new { extension = extensions });
            }
        }
        private void blockerAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(blockerEntry.Text))
            {
                blockerProcesses.Add(blockerEntry.Text);
                blockerEntry.Clear();
                blockerUpdate();
            }
        }

        private void blockerRemove_Click(object sender, RoutedEventArgs e)
        {
            if (blockerListView.SelectedItem != null)
            {
                blockerProcesses.Remove((blockerListView.SelectedItem as dynamic).process);
                blockerUpdate();
            }
        }

        private void blockerUpdate()
        {
            blockerListView.Items.Clear();
            foreach (string processes in blockerProcesses)
            {
                blockerListView.Items.Add(new { process = processes });
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            //add verifications and error messages !

            ProjetDevSys.VueModel.ConfigViewModel configViewModel = new ConfigViewModel();

            //Paths
            if (!String.IsNullOrWhiteSpace(pathSaveBackupEntry.Text)) configViewModel.EditerJsonPathSave(pathSaveBackupEntry.Text);
            if (!String.IsNullOrWhiteSpace(logDailyEntry.Text)) configViewModel.EditerJsonPath(logDailyEntry.Text);
            if (!String.IsNullOrWhiteSpace(logRTEntry.Text)) configViewModel.EditerJsonPathRealTime(logRTEntry.Text);

            //extensions
            if (logExtensionSelector.Text != null) configViewModel.EditExtensionType(logExtensionSelector.Text);

            //language
            if (languageSelector.Text != null) configViewModel.EditerLangage(GetLangageCulture(languageSelector.Text));

            //crypto
            if (CryptoExtensions != null) configViewModel.ChangeExtensionListCrypt(new List<string>(CryptoExtensions));
            if (!String.IsNullOrWhiteSpace(cryptoPathEntry.Text)) configViewModel.EditCryptPath(cryptoPathEntry.Text);

            //blocker Process
            if (blockerProcesses != null) configViewModel.ChangeBlockerProcessList(new List<string>(blockerProcesses));

            //refresh content
            Refresh();
            //MainWindow.ReloadWindow();
        }

        private void Refresh()
        {
            //paths
            pathSaveBackupEntry.Text = ProjetDevSys.AppConstants.JsonSave;
            logDailyEntry.Text = ProjetDevSys.AppConstants.LogFilePath;
            logRTEntry.Text = ProjetDevSys.AppConstants.LogFilePathRealTime;
            //extensions
            string extension = ProjetDevSys.AppConstants.ExtensionType;
            logExtensionSelector.SelectedItem = extension;
            logExtensionSelector.Text = extension;
            //language
            string language = GetLanguage();
            languageSelector.SelectedItem = Language;
            languageSelector.Text = language;
            //crypto
            cryptoPathEntry.Text = ProjetDevSys.AppConstants.CryptPath;
            CryptoExtensions = new ObservableCollection<string>();
            if (ProjetDevSys.AppConstants.ExtensionListCrypt != null)
            {
                foreach (string extensions in ProjetDevSys.AppConstants.ExtensionListCrypt) CryptoExtensions.Add(extensions);
            }
            cryptoExtensionsUpdate();

            //blocker
            blockerProcesses = new ObservableCollection<string>();
            if (ProjetDevSys.AppConstants.BlockerProcess != null)
            {
                foreach (string processes in ProjetDevSys.AppConstants.BlockerProcess) blockerProcesses.Add(processes);
            }
            blockerUpdate();
        }
        public static string GetLangageCulture(string langage)
        {
            switch (langage)
            {
                case "Francais":
                    return "fr";
                default:
                    return "en";
            }
        }
        public static string GetLanguage()
        {
            string langage = ProjetDevSys.AppConstants.Langage.Substring(0, 2); //take only the primary part
            switch (langage)
            {
                case "fr":
                    return "Francais";
                default:
                    return "English";
            }
        }
    }
}
