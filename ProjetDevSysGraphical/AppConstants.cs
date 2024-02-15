using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using ProjetDevSys;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevSysGraphical
{
    public static class AppConstants
    {
        public static string OpenFolderDialog()
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    return dialog.FileName;
                }
            }
            return "";
        }
        public static string OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
        public static string GetLanguage()
        {
            string langage = ProjetDevSys.AppConstants.Langage.Substring(0,2); //take only the primary part
            switch (langage)
            {
                case "en":
                    return "English";
                case "fr":
                    return "Francais";
                default:
                    return null;
            }
        }
    }
}
