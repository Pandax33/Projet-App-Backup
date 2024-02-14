using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using ProjetDevSys;
using ProjetDevSys.VueModel;
using ProjetDevSys.Vue;


[TestClass]
public class MyTests
{
    [TestMethod]
    public void TestVerifJson()
    {
        Assert.IsFalse(AppConstants.VerifJson("C://impossible.json"));
    }

    [TestMethod]
    public void TestEditerLangage()
    {
        ConfigViewModel configViewModel = new ConfigViewModel();
        Assert.AreEqual(ResourceHelper.GetString("ConfigViewModel1"), configViewModel.EditerLangage("en"));
        Assert.AreEqual(ResourceHelper.GetString("ConfigViewModel1"), configViewModel.EditerLangage("esp"));
        Assert.AreEqual(ResourceHelper.GetString("ConfigViewModel1"), configViewModel.EditerLangage("fr"));
    }

    [TestMethod]
    public void TestVerifExist()
    {
        Assert.IsFalse(AppConstants.VerifExist("C://impossible.json"));
        Assert.IsTrue(AppConstants.VerifExist("C://Users/alexa/AppData/Roaming"));
    }

    [TestMethod]
    public void TestVerifPath()
    {
        Assert.IsTrue(AppConstants.VerifPath("éèéè_*_*"));
        Assert.IsTrue(AppConstants.VerifPath("C://Users/alexa/AppData/Roaming"));
    }

    [TestMethod]
    public void TestCreateTask()
    {
        GestionTask gestionTask = new GestionTask();
        Assert.IsTrue(gestionTask.CreateTask("test", "C://", "C://", "A"));
        Assert.IsFalse(gestionTask.CreateTask("test2", "C://", "C://", "D"));
    }
    public void TestEditNewDestination()
    {
        GestionTask gestionTask = new GestionTask();
        Assert.AreEqual(ResourceHelper.GetString("ConfigViewModel1"), gestionTask.EditNewDestination(0, "C://"));

    }

    public void TestEditNewType()
    {
        GestionTask gestionTask = new GestionTask();
        Assert.AreEqual(ResourceHelper.GetString("ConfigViewModel1"), gestionTask.EditNewType(0, "C://"));

    }

    [TestMethod]
    public void TestDeleteTask()
    {
        GestionTask gestionTask = new GestionTask();
        Assert.AreEqual("Suppression terminée.", gestionTask.DeleteTask(0));
        Assert.AreEqual("test", gestionTask.DeleteTask(-1));
    }
}
