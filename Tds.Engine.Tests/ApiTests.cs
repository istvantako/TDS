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
        #region Test initialization -----------------------
        [ClassInitialize]
        public void Initialize()
        {
            // We need to make sure the directories used for testing.
            if (!Directory.Exists(TestSettings.Storage.XmlMetadataManualLocation))
            {
                Directory.CreateDirectory(TestSettings.Storage.XmlMetadataManualLocation);
            }
        }
        #endregion ----------------------------------------

        [TestMethod]
        [ExpectedException(typeof(EntityTypeNotFoundException))]
        public void Backup_EntityDoesNotExist_ThrowsException()
        {
            // Arrange
            var productionConnectionString = ConfigurationManager.ConnectionStrings["DrawingsProductionContext"].ConnectionString;
            var backupConnectionString = ConfigurationManager.ConnectionStrings["DrawingsBackupContext"].ConnectionString;
            var drawingsXmlMetadataLocation = TestSettings.Storage.XmlMetadataManualLocation + @"\Drawings.xml";

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
            var productionConnectionString = ConfigurationManager.ConnectionStrings["DrawingsProductionContext"].ConnectionString;
            var backupConnectionString = ConfigurationManager.ConnectionStrings["DrawingsBackupContext"].ConnectionString;
            var drawingsXmlMetadataLocation = TestSettings.Storage.XmlMetadataManualLocation + @"\Drawings.xml";

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

        }

        [TestMethod]
        public void Backup_SubDrawingsIsDependentAllDependencies_BackupAllEntities()
        {

        }

        [TestMethod]
        public void Restore_DrawingsIsPrincipalWithAllDependencies_RestoreAllEntities()
        {

        }

        [TestMethod]
        public void Restore_SubDrawingsIsDependentWithAllDependencies_RestoreAllEntities()
        {

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
