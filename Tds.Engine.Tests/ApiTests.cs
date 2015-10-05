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
using System;

namespace Tds.Engine.Tests
{
    [TestClass]
    public class ApiTests : ApiTestsBase
    {
        #region Test initialization/cleanup ---------------
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

            queries.Clear();

            TruncateSqlServerDb(productionConnectionString, entities);
            TruncateSqlServerDb(backupConnectionString, entities);
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
        [ExpectedException(typeof(MissingKeyMemberException))]
        public void Backup_SubDrawingsMissingKey_ThrowsException()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();
            var entities = SetUpSourceDbContext(metadataWorkspace);

            var entityName = "SubDrawings";
            var keys = new List<string>()
            {
                "1",
                "2"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys);
        }

        [TestMethod]
        public void Backup_DrawingsIsPrincipalWithAllDependencies_BackupAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();
            var entities = SetUpSourceDbContext(metadataWorkspace);

            var entityName = "Drawings";
            var keys = new List<string>()
            {
                "1"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(backupConnectionString, metadataWorkspace, entity), message);
            }
        }

        [TestMethod]
        public void Backup_DrawingsIsPrincipalWithFilter_BackupAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();
            var entities = SetUpSourceDbContext(metadataWorkspace);

            var entityName = "Drawings";
            var keys = new List<string>()
            {
                "1"
            };
            var entitiesToSkip = new List<string>()
            {
                "DrawingsImages"
            };
            entities = FilterDrawingsImages(entities);

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(backupConnectionString, metadataWorkspace, entity), message);
            }
        }

        [TestMethod]
        public void Backup_SubDrawingsIsDependentAllDependencies_BackupAllEntities()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();
            var entities = SetUpSourceDbContext(metadataWorkspace);

            var entityName = "SubDrawings";
            var keys = new List<string>()
            {
                "1",
                "2",
                "300",
                "400",
                "1"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(backupConnectionString, metadataWorkspace, entity), message);
            }
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
        private IEnumerable<Entity> SetUpSourceDbContext(IMetadataWorkspace metadataWorkspace)
        {
            List<Entity> entities = new List<Entity>();

            entities.Add(GetDrawing(1, "1st", 800, 600, 10));
            entities.Add(GetDrawing(2, "2nd", 500, 200, 5));
            entities.Add(GetDrawing(3, "3rd", 480, 320, 2));
            entities.Add(GetDrawing(4, "4th", 320, 480, 4));
            entities.Add(GetDrawing(5, "5th", 600, 800, 10));
            entities.Add(GetDrawing(6, "6th", 200, 500, 10));

            entities.Add(GetSubDrawing(1, 2, 300, 400, 1));
            entities.Add(GetSubDrawing(1, 4, 480, 320, 2));
            entities.Add(GetSubDrawing(1, 3, 320, 280, 3));
            entities.Add(GetSubDrawing(2, 5, 400, 600, 2));
            entities.Add(GetSubDrawing(2, 6, 100, 600, 3));
            entities.Add(GetSubDrawing(3, 4, 120, 120, 1));
            entities.Add(GetSubDrawing(3, 6, 180, 180, 2));
            entities.Add(GetSubDrawing(4, 5, 180, 320, 1));

            entities.Add(GetPixel(1, 1, 1, 1, 10));
            entities.Add(GetPixel(2, 3, 5, 3, 5));
            entities.Add(GetPixel(1, 1, 1, 6, 2));

            entities.Add(GetLine(1, 1, 800, 600, 1, 1, 10));
            entities.Add(GetLine(1, 2, 480, 320, 2, 2, 5));
            entities.Add(GetLine(5, 5, 320, 280, 1, 5, 2));
            entities.Add(GetLine(1, 1, 800, 600, 1, 5, 10));

            entities.Add(GetImage(1, "1st.jpg"));
            entities.Add(GetImage(2, "2nd.png"));
            entities.Add(GetImage(3, "3rd.bmp"));
            entities.Add(GetImage(4, "4th.jpg"));

            entities.Add(GetDrawingsImages(1, 1, 800, 600, 3));
            entities.Add(GetDrawingsImages(1, 3, 500, 200, 2));
            entities.Add(GetDrawingsImages(2, 2, 480, 320, 1));
            entities.Add(GetDrawingsImages(2, 4, 320, 480, 3));
            entities.Add(GetDrawingsImages(4, 1, 600, 800, 1));
            entities.Add(GetDrawingsImages(4, 4, 200, 500, 2));

            foreach (var entity in entities)
            {
                InsertEntityIntoSqlServerDb(productionConnectionString, metadataWorkspace, entity);

                if (!CheckEntityExistsInSqlServerDb(productionConnectionString, metadataWorkspace, entity))
                {
                    string message = string.Format("DB INIT: Entity '{0}' could not be saved in the production database.", entity.Name);
                    throw new Exception(message);
                }
            }

            return entities;
        }

        private IEnumerable<Entity> FilterDrawingsImages(IEnumerable<Entity> entities)
        {
            return entities.Where(entity => !entity.Name.Equals("DrawingsImages") || !entity.Name.Equals("Images"));
        }
        #endregion
    }
}
