using ProjetDevSys;
using Microsoft.Extensions.Configuration;

using ProjetDevSys.Model;
using System.Globalization;
using System.Resources;
using System.Numerics;
using ProjetDevSys.VueModel;
using ProjetDevSys.Vue;
BackupFactory.LoadBackupsFromJson();

MenuPrincipal menuPrincipal = new MenuPrincipal();
menuPrincipal.PrincipalMenu();
