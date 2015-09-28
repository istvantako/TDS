using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;
using Tds.MetadataProviders.Xml;
using Tds.StorageProviders.SqlServer;
using Tds.Types;
using System.IO;

namespace Tds.Engine.Tests
{
    [TestClass]
    public class ApiTests
    {
        #region Private fields ----------------------------

        private static string productionConnectionString;

        private static string backupConnectionString;

        private static string drawingsXmlMetadataLocation;

        #endregion ----------------------------------------

        #region Test initialization -----------------------

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            productionConnectionString = ConfigurationManager.ConnectionStrings["DrawingsProductionContext"].ConnectionString;
            backupConnectionString = ConfigurationManager.ConnectionStrings["DrawingsBackupContext"].ConnectionString;
            drawingsXmlMetadataLocation = TestSettings.Storage.XmlMetadataManualLocation + @"\Drawings.xml";
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var entities = new List<string>()
            {
                "Drawings",
                "Images",
                "DrawingsImages",
                "SubDrawings",
                "Pixels",
                "Lines"
            };

            SqlServerOperations.TruncateDatabase(productionConnectionString, entities);
            SqlServerOperations.TruncateDatabase(backupConnectionString, entities);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #endregion ----------------------------------------

        [TestMethod]
        [ExpectedException(typeof(EntityTypeNotFoundException))]
        public void Backup_EntityDoesNotExist_ThrowsException()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);

            var entityName = "NotExistingEntity";
            var keys = new List<string>();

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundInDatabaseException))]
        public void Backup_EntityWithOneKeyDoesntExist_ThrowsException()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);

            var entityName = "Drawings";
            var keys = new List<string>()
            {
                "1000"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys);

            // Assert
        }

        [TestMethod]
        public void Backup_DrawingsIsPrincipalWithAllDependencies_BackupAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);

            // Act
            var d = new Entity();

            SqlServerOperations.InsertEntityInDatabase(productionConnectionString, metadataProvider.GetMetadataWorkspace(), d);

            // Assert
            Assert.IsTrue(SqlServerOperations.CheckEntityExistsInDatabase(productionConnectionString, metadataProvider.GetMetadataWorkspace(), d), "Entity not saved!");
        }

        [TestMethod]
        public void Backup_SubDrawingsIsDependentAllDependencies_BackupAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);

            // Act

            // Assert
        }

        [TestMethod]
        public void Restore_DrawingsIsPrincipalWithAllDependencies_RestoreAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);

            // Act

            // Assert
        }

        [TestMethod]
        public void Restore_SubDrawingsIsDependentWithAllDependencies_RestoreAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);

            // Act

            // Assert
        }

        /*[TestMethod]
        public void Backup_EntityWithOneKeyWithNoDependencies_BackupEntity()
        {
            // Arrange

            // Act

            // Assert
        }

        [TestMethod]
        public void Backup_EntityWithTwoKeysWithNoDependencies_BackupEntity()
        {
            // Arrange

            // Act

            // Assert
        }*/

        #region Private methods (helpers)
        #endregion
    }
}
