using ProjetDevSys;
using Microsoft.Extensions.Configuration;

using ProjetDevSys.Model;
using System.Globalization;
using System.Resources;
using System.Numerics;
using ProjetDevSys.VueModel;
using ProjetDevSys.Vue;
using ProjetDevSys;


CultureInfo ci = new CultureInfo(AppConstants.Langage);
CultureInfo.CurrentUICulture = ci;
BackupFactory.LoadBackupsFromJson();
Config.EditConfig();

MenuPrincipal menuPrincipal = new MenuPrincipal();
menuPrincipal.PrincipalMenu();
