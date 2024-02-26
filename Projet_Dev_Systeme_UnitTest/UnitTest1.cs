using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using ProjetDevSys;
using ProjetDevSys.VueModel;
using ProjetDevSys.Vue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using ProjetDevSys.Model;


[TestClass]
public class MyTests

// Config ViewModel
{
    ConfigViewModel configViewModel = new ConfigViewModel();
    GestionTask gestionTask = new GestionTask();
    RunSaveTask runSaveTask = new RunSaveTask();

    string folderPath;
    string ConfigPath;

    string BackFilePathIn1;
    string BackFilePathIn2;
    string BackFilePathIn3;

    string BackFilePathOut;

    string name;
    string source;
    string target;
    string type;

    public MyTests()
    {
        folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySaveGP5");
        ConfigPath = Path.Combine(folderPath, "appsettings.json");

        BackFilePathIn1 = Path.Combine(folderPath, "test1.txt");
        BackFilePathIn2 = Path.Combine(folderPath, "test2.txt");
        BackFilePathIn3 = Path.Combine(folderPath, "test3.txt");

        BackFilePathOut = Path.Combine(folderPath, "outputTest");

        name = "unitTest31415";
        source = "C:/";
        target = "C:/Downloads";
        type = "A";
    }

    [TestMethod]
    public void TestEditerConfig()
    {
        string jsonContent = File.ReadAllText(ConfigPath);

        JObject jsonObject = JObject.Parse(jsonContent);

        // parse data
        string jsonPath = (string)jsonObject["Logging"]["JsonPath"];
        string langage = (string)jsonObject["Langage"]["Langage"];
        string jsonPathRealTime = (string)jsonObject["RealTimeLogging"]["JsonPathRealTime"];
        string jsonPathSave = (string)jsonObject["LoadSave"]["JsonPathSave"];

        // data that will be used to test
        string expected1 = folderPath + "/JsonPathUT.json";
        string expected2 = "le";
        string expected3 = folderPath + "/JsonPathRealTimeUT.json";
        string expected4 = folderPath + "/JsonPathSaveUT.json";

        // change the config to a test config
        configViewModel.EditerConfig(expected1, expected2, expected3, expected4);

        string NewjsonContent = File.ReadAllText(ConfigPath);
        JObject NewjsonObject = JObject.Parse(NewjsonContent);

        // collect new data
        string NewjsonPath = (string)NewjsonObject["Logging"]["JsonPath"];
        string Newlangage = (string)NewjsonObject["Langage"]["Langage"];
        string NewjsonPathRealTime = (string)NewjsonObject["RealTimeLogging"]["JsonPathRealTime"];
        string NewjsonPathSave = (string)NewjsonObject["LoadSave"]["JsonPathSave"];

        // repair the config
        configViewModel.EditerConfig(jsonPath, langage, jsonPathRealTime, jsonPathSave);

        // test data == expected data ?
        Assert.AreEqual(NewjsonPath, expected1);
        Assert.AreEqual(Newlangage, expected2);
        Assert.AreEqual(NewjsonPathRealTime, expected3);
        Assert.AreEqual(NewjsonPathSave, expected4);

    }

    [TestMethod]
    public void TestEditExtensionType()
    {
        string expectedExtension = ".cryptoSafe";
        configViewModel.EditExtensionType(expectedExtension);

        string NewjsonContent = File.ReadAllText(ConfigPath);
        JObject NewjsonObject = JObject.Parse(NewjsonContent);

        string NewjsonExtension = (string)NewjsonObject["LogType"]["ExtensionType"];

        configViewModel.EditExtensionType(".json");

        Assert.AreEqual(NewjsonExtension, expectedExtension);
    }

    public void TestverifInputLanguage()
    {
        Assert.IsFalse(configViewModel.verifInputLanguage("leandro"));
        Assert.IsTrue(configViewModel.verifInputLanguage("fr"));
        Assert.IsTrue(configViewModel.verifInputLanguage("en"));
    }

    [TestMethod]
    public void TestInputExtension()
    {
        Assert.IsFalse(configViewModel.verifInputExtension("leandro"));
        Assert.IsTrue(configViewModel.verifInputExtension(".json"));
        Assert.IsTrue(configViewModel.verifInputExtension(".xml"));
    }

    // Gestion task

    [TestMethod]
    public void TestCreateTask()
    {

        Assert.AreEqual(gestionTask.CreateTask(name, source, target, type), ResourceHelper.GetString("GestionTaskView3"));
        //Assert.ThrowsException<ArgumentException>(() => gestionTask.CreateTask(name, source, target, type));
        //Assert.ThrowsException<ArgumentException>(() => gestionTask.CreateTask("", source, target, type));

        // remove the traces
        BackupFactory.DeleteBackup(name);
    }

    [TestMethod]
    public void TestDeleteTask()
    {
        gestionTask.CreateTask(name, source, target, type);

        string BackPath = Path.Combine(folderPath, "Backlist.json");
        BackupFactory.DeleteBackup(name);


        string NewjsonContent = File.ReadAllText(BackPath);
        JObject NewjsonObject = JObject.Parse(NewjsonContent);

        Assert.IsFalse(NewjsonObject.ContainsKey(name));
    }

    [TestMethod]
    public void TestEditTask()
    {

        /*gestionTask.CreateTask(name, source, target, type);

        string BackPath = Path.Combine(folderPath, "Backlist.json");

        Assert.AreEqual(gestionTask.EditTask(0, target, "C:\\Users\\alexa\\AppData\\Roaming\\EasySaveGP5", "B"), ResourceHelper.GetString("GestionTask3"));
        Assert.AreEqual(gestionTask.EditTask(1000, target, "C:\\Users\\alexa\\AppData\\Roaming\\EasySaveGP5", "B"), ResourceHelper.GetString("GestionTask4"));

        BackupFactory.DeleteBackup(name);
        */
    }

    [TestMethod]
    public void TestVerifyId()
    {

        gestionTask.CreateTask(name, source, target, type);
        Assert.IsTrue(gestionTask.VerifyId(0));
        Assert.IsFalse(gestionTask.VerifyId(1000));

        BackupFactory.DeleteBackup(name);
    }

    [TestMethod]
    public void TestVerifInputBackupType()
    {
        Assert.IsTrue(gestionTask.verifInputBackupType("A"));
        Assert.IsTrue(gestionTask.verifInputBackupType("B"));
        Assert.IsFalse(gestionTask.verifInputBackupType("DDDDDDDDDDDDDDDDDDDD"));
    }

    [TestMethod]
    public void TestVerifSource()
    {
        Assert.IsTrue(gestionTask.VerifSource("C://"));
        Assert.IsFalse(gestionTask.VerifSource("C:/ImposibleToFOundThisPath.impossible"));
    }

    // RunSaveTask

    [TestMethod]
    public void TestRunTask()
    {
        gestionTask.CreateTask(name, source, target, type);
    }

    [TestMethod]
    public void TestRunMultipleTask()
    {
        File.WriteAllText(BackFilePathIn1, "this is a test.");
        File.WriteAllText(BackFilePathIn2, "this is a test.");
        File.WriteAllText(BackFilePathIn3, "this is a test.");

        // create output directory
        Directory.CreateDirectory(Path.Combine(folderPath, "outputTest"));

        // create some test backup
        gestionTask.CreateTask("UnitTestBackup1", BackFilePathIn1, BackFilePathOut, "A");
        gestionTask.CreateTask("UnitTestBackup2", BackFilePathIn2, BackFilePathOut, "B");
        gestionTask.CreateTask("UnitTestBackup3", BackFilePathIn3, BackFilePathOut, "A");

        // run tasks
        runSaveTask.RunMultipleTask(0, 2);

        // check of files in the folder
        string[] filesList = Directory.GetFiles(Path.Combine(folderPath, "outputTest"));

        string[] expectedNames = { "test1.txt", "test2.txt", "test3.txt" };

        Directory.Delete(Path.Combine(folderPath, "outputTest"), true);

        // remove traces
        File.Delete(BackFilePathIn1);
        File.Delete(BackFilePathIn2);
        File.Delete(BackFilePathIn3);

        BackupFactory.DeleteBackup("UnitTestBackup1");
        BackupFactory.DeleteBackup("UnitTestBackup2");
        BackupFactory.DeleteBackup("UnitTestBackup3");

        Assert.AreEqual(filesList.Length, expectedNames.Length);
    }

    [TestMethod]
    public void TestRunTaskMultiple()
    {

        File.WriteAllText(BackFilePathIn1, "this is a test.");
        File.WriteAllText(BackFilePathIn2, "this is a test.");
        File.WriteAllText(BackFilePathIn3, "this is a test.");

        // create output directory
        Directory.CreateDirectory(Path.Combine(folderPath, "outputTest"));

        // create some test backup
        gestionTask.CreateTask("UnitTestBackup1", BackFilePathIn1, BackFilePathOut, "A");
        gestionTask.CreateTask("UnitTestBackup2", BackFilePathIn2, BackFilePathOut, "B");
        gestionTask.CreateTask("UnitTestBackup3", BackFilePathIn3, BackFilePathOut, "A");

        // run tasks
        int[] orderToRun = new int[] { 2, 0, 1 };
        runSaveTask.RunTaskMultiple(orderToRun);

        // check of files in the folder
        string[] filesList = Directory.GetFiles(Path.Combine(folderPath, "outputTest"));

        string[] expectedNames = { "test1.txt", "test2.txt", "test3.txt" };

        Assert.AreEqual(filesList.Length, expectedNames.Length);
        // remove traces
        Directory.Delete(Path.Combine(folderPath, "outputTest"), true);

        File.Delete(BackFilePathIn1);
        File.Delete(BackFilePathIn2);
        File.Delete(BackFilePathIn3);

        BackupFactory.DeleteBackup("UnitTestBackup1");
        BackupFactory.DeleteBackup("UnitTestBackup2");
        BackupFactory.DeleteBackup("UnitTestBackup3");
    }

    [TestMethod]
    public void TestVerifyIdBackUp()
    {
        gestionTask.CreateTask("UnitTestBackup3", "C://", "C://Users", "A");
        Assert.IsTrue(gestionTask.VerifyId(0));
        Assert.IsFalse(gestionTask.VerifyId(999999999));

        BackupFactory.DeleteBackup("UnitTestBackup3");
    }

    [TestMethod]
    public void TestVerifyContinueId()
    {
        gestionTask.CreateTask("UnitTestBackup1", BackFilePathIn1, BackFilePathOut, "A");
        gestionTask.CreateTask("UnitTestBackup2", BackFilePathIn2, BackFilePathOut, "B");
        gestionTask.CreateTask("UnitTestBackup3", BackFilePathIn3, BackFilePathOut, "A");

        // all the tests

        Assert.IsFalse(runSaveTask.VerifyContinueId("0,1,2"));
        Assert.IsTrue(runSaveTask.VerifyContinueId("0,2"));
        Assert.IsFalse(runSaveTask.VerifyContinueId("2,0"));

        BackupFactory.DeleteBackup("UnitTestBackup1");
        BackupFactory.DeleteBackup("UnitTestBackup2");
        BackupFactory.DeleteBackup("UnitTestBackup3");

    }

}