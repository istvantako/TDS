using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using TestDataSeeding.Client;
using TestDataSeeding.Model;

using Tds.Tests.Model;
using Tds.Tests.Model.Entities;

namespace Tds.Tests.SystemTests
{
    [TestClass]
    public class TdsClientSaveTests : SystemTestsBase
    {
        #region Test methods ------------------------------
        [TestMethod]
        public void SaveEntities_EntityAWithNoDependencies_XmlFileCreated()
        {
            // Arrange
            var a = GetA(1);
            var client = GetClientWithA();
            var entityToBeSaved = GetEntityWithKey(a);

            // Act
            client.SaveEntity(entityToBeSaved);

            // Assert
            Assert.IsTrue(File.Exists(a.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "A is not saved");
        }

        [TestMethod]
        public void SaveEntities_EntityAWithB_XmlFileCreated()
        {
            // Arrange
            var b = GetB(2, 1);
            var a = GetA(2, b);
            var client = GetClientWithAWithB();
            var entityToBeSaved = GetEntityWithKey(a);

            // Act
            client.SaveEntity(entityToBeSaved, true);

            // Assert
            Assert.IsTrue(File.Exists(a.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "A has not been saved.");
            Assert.IsTrue(File.Exists(b.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "B has not been saved.");
        }

        [TestMethod]
        public void SaveEntities_EntityAWithB_C_XmlFileCreated()
        {
            // Arrange
            var b = GetB(3, 1);
            var a = GetA(3, b);
            var c = GetC(3, 1, a);
            var client = GetClientWithAWithB_C();
            var entityToBeSaved = GetEntityWithKey(c);

            // Act
            client.SaveEntity(entityToBeSaved, true);

            // Assert
            Assert.IsTrue(File.Exists(a.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "A has not been saved.");
            Assert.IsTrue(File.Exists(b.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "B has not been saved.");
            Assert.IsTrue(File.Exists(c.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "C has not been saved.");
        }

        [TestMethod]
        public void SaveEntities_EntityAWithB_C_D_XmlFileCreated()
        {
            // Arrange
            var b = GetB(4, 1);
            var a = GetA(4, b);
            var d = GetD(4, 1);
            var c = GetC(4, 1, a, d);
            var client = GetClientWithAWithB_C_D();
            var entityToBeSaved = GetEntityWithKey(c);

            // Act
            client.SaveEntity(entityToBeSaved, true);

            // Assert
            Assert.IsTrue(File.Exists(a.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "A has not been saved.");
            Assert.IsTrue(File.Exists(b.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "B has not been saved.");
            Assert.IsTrue(File.Exists(c.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "C has not been saved.");
            Assert.IsTrue(File.Exists(d.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "D has not been saved.");
        }

        [TestMethod]
        public void SaveEntities_EntityAWithB_C_D_E_XmlFileCreated()
        {
            // Arrange
            var b = GetB(5, 1);
            var a = GetA(5, b);
            var d = GetD(5, 1);
            var c = GetC(5, 1, a, d);
            var e = GetE(b, d, 1);
            var client = GetClientWithAWithB_C_D_E();
            var entityToBeSaved = GetEntityWithKey(c);

            // Act
            client.SaveEntity(entityToBeSaved, true);

            // Assert
            Assert.IsTrue(File.Exists(a.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "A has not been saved.");
            Assert.IsTrue(File.Exists(b.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "B has not been saved.");
            Assert.IsTrue(File.Exists(c.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "C has not been saved.");
            Assert.IsTrue(File.Exists(d.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "D has not been saved.");
            Assert.IsTrue(File.Exists(e.GetExpectedFileName(TestSettings.Storage.EntitiesLocation)), "E has not been saved.");
        }
        #endregion ----------------------------------------
    }
}
