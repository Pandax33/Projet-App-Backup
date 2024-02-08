using ProjetDevSys;
using Microsoft.Extensions.Configuration;

using ProjetDevSys.Model;
using System.Globalization;
using System.Resources;
using System.Numerics;
using ProjetDevSys.VueModel;
using ProjetDevSys.Vue;
BackupFactory.LoadBackupsFromJson();

RunTaskView runTaskView = new RunTaskView();
GestionTaskView gestionTaskView = new GestionTaskView();
//Console.WriteLine(runTaskView.SelectTaskView());
gestionTaskView.SelectGestionTaskView();

