using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ProjetDevSys.Model;

namespace ProjetDevSys.VueModel
{
    public class ConfigViewModel
    {
        public string EditerConfig(string JsonPath, string Langage, string JsonPathRealTime, string JsonPathSave)
        {
            Config.JsonPath = JsonPath;
            Config.Langage = Langage;
            Config.JsonPathRealTime = JsonPathRealTime;
            Config.JsonPathSave = JsonPathSave;

            Config.EditConfig();


            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerJsonPath(string JsonPath)
        {
            Config.JsonPath = JsonPath;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerLangage(string Langage)
        {
            Config.Langage = Langage;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerJsonPathRealTime(string JsonPathRealTime)
        {
            Config.JsonPathRealTime = JsonPathRealTime;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditerJsonPathSave(string JsonPathSave)
        {
            Config.JsonPathSave = JsonPathSave;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditExtensionType(string ExtensionType)
        {
            Config.ExtensionType = ExtensionType;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditExtensionListCrypt(string ExtensionCrypt)
        {

            if (!ExtensionCrypt.StartsWith("."))
            {
                ExtensionCrypt = "." + ExtensionCrypt;
            }
            Config.ExtensionListCrypt.Add(ExtensionCrypt);
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string removeExtensionListCrypt(int ExtensionListCrypt)
        {
            Config.ExtensionListCrypt.RemoveAt(ExtensionListCrypt);

            Config.EditConfig();

            return ResourceHelper.GetString("ConfigViewModelExtensionRemoved");
        }
        public string ChangeExtensionListCrypt(List<string> ExtensionListCrypt)
        {
            Config.ExtensionListCrypt = ExtensionListCrypt;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditExtensionListPriority(string ExtensionPriority)
        {
            if (!ExtensionPriority.StartsWith("."))
            {
                ExtensionPriority = "." + ExtensionPriority;
            }
            Config.ExtensionListPriority.Add(ExtensionPriority);
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string removeExtensionListPriority(int ExtensionListPriority)
        {
            Config.ExtensionListPriority.RemoveAt(ExtensionListPriority);

            Config.EditConfig();

            return ResourceHelper.GetString("ConfigViewModelExtensionRemoved");
        }
        public string ChangeExtensionListPriority(List<string> ExtensionListPriority)
        {
            Config.ExtensionListPriority = ExtensionListPriority;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditCryptPath(string CryptPath)
        {
            Config.CryptPath = CryptPath;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditBlockerProcess(string process)
        {
            Config.BlockerProcess.Add(process);
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }
        public string RemoveBlockerProcess(int process)
        {
            Config.BlockerProcess.RemoveAt(process);
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }
        public string ChangeBlockerProcessList(List<string> blockerProcess)
        {
            Config.BlockerProcess = blockerProcess;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditFileSize(long FileSize, string FileSizeUnit)
        {
            switch (FileSizeUnit)
            {
                case "Octet":
                    Config.FileSize = FileSize * 1;
                    break;

                case "Ko":
                    Config.FileSize = FileSize * 1000;
                    break;

                case "Mo":
                    Config.FileSize = FileSize * 1000000;
                    break;

                case "Go":
                    Config.FileSize = FileSize * 1000000000;
                    break;
            }
            Config.FileSizeUnit = FileSizeUnit;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public string EditServerStatus(bool IsServOn)
        {
            Config.IsServOn = IsServOn;
            Config.EditConfig();
            return ResourceHelper.GetString("ConfigViewModel1");
        }

        public bool verifInputLanguage(string input)
        {
            if (input == "fr" || input == "en")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool verifInputExtension(string input)
        {
            if (input == ".json" || input == ".xml")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool verifCryptPath(string input)
        {
            bool fileExists = File.Exists(input);

            bool isExe = Path.GetExtension(input).Equals(".exe", StringComparison.OrdinalIgnoreCase);

            return fileExists && isExe;
        }

        public bool IsExtensionValid(string extension)
        {
            if (!extension.StartsWith(".") || extension.Length < 2)
            {
                return false;
            }

            for (int i = 1; i < extension.Length; i++)
            {
                if (!char.IsLetter(extension[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public string resetConfig()
        {
            Config.ResetSetting();
            return ResourceHelper.GetString("ConfigViewModel25");
        }

        public bool verifDeleteExtensionList(int index)
        {
            if (index >= 0 && index < AppConstants.ExtensionListCrypt.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool verifDeleteExtensionListPriority(int index)
        {
            if (index >= 0 && index < AppConstants.ExtensionListPriority.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EditTheme(string theme)
        {
            Config.Theme = theme;
            Config.EditConfig();
        }

    }
}
