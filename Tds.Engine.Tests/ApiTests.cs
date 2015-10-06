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
using log4net;

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

            var entities = SetUpSourceEntities();
            EnsureInDatabase(productionConnectionString, metadataWorkspace, entities);

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
        public void Backup_DrawingsIsPrincipalWithSimpleKey_BackupAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(productionConnectionString, metadataWorkspace, entities);

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
        public void Backup_DrawingsIsPrincipalWithFilter_BackupAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(productionConnectionString, metadataWorkspace, entities);
            entities = FilterDrawingsImages(entities);

            var entityName = "Drawings";
            var keys = new List<string>()
            {
                "1"
            };
            var entitiesToSkip = new List<string>()
            {
                "DrawingsImages"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, keys, entitiesToSkip);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(backupConnectionString, metadataWorkspace, entity), message);
            }
        }

        [TestMethod]
        public void Backup_SubDrawingsWithCompositeKeyIsDependent_BackupAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(productionConnectionString, metadataWorkspace, entities);

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
        public void Restore_DrawingsWithSimpleKeyToEmptyDatabase_RestoreAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(backupConnectionString, metadataWorkspace, entities);

            var entityName = "Drawings";
            var keys = new List<string>()
            {
                "1"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Restore(entityName, keys);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(productionConnectionString, metadataWorkspace, entity), message);
            }
        }

        [TestMethod]
        public void Restore_DrawingsWithSimpleKeyToDatabaseWithRemovedAndModifiedEntities_RestoreAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(backupConnectionString, metadataWorkspace, entities);

            var defectiveEntities = SetUpDefectiveEntities();
            EnsureInDatabase(productionConnectionString, metadataWorkspace, defectiveEntities);

            var entityName = "Drawings";
            var keys = new List<string>()
            {
                "1"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Restore(entityName, keys);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(productionConnectionString, metadataWorkspace, entity), message);
            }
        }

        [TestMethod]
        public void Restore_SubDrawingsWithCompositeKeyToDatabaseWithRemovedAndModifiedEntities_RestoreAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(backupConnectionString, metadataWorkspace, entities);

            var defectiveEntities = SetUpDefectiveEntities();
            EnsureInDatabase(productionConnectionString, metadataWorkspace, defectiveEntities);

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
            api.Restore(entityName, keys);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(productionConnectionString, metadataWorkspace, entity), message);
            }
        }

        [TestMethod]
        public void Restore_SubDrawingsWithCompositeKeyAndFilterToDefectiveDatabase_RestoreAllDependencies()
        {
            // Arrange
            var productionStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var backupStorageProvider = new SqlServerStorageProvider(backupConnectionString);
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            var entities = SetUpSourceEntities();
            EnsureInDatabase(backupConnectionString, metadataWorkspace, entities);
            entities = FilterDrawingsImages(entities);

            var defectiveEntities = FilterDrawingsImages(SetUpDefectiveEntities());
            EnsureInDatabase(productionConnectionString, metadataWorkspace, defectiveEntities);

            var entityName = "SubDrawings";
            var keys = new List<string>()
            {
                "1",
                "2",
                "300",
                "400",
                "1"
            };
            var entitiesToSkip = new List<string>()
            {
                "DrawingsImages"
            };

            var api = new Api(metadataProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Restore(entityName, keys, entitiesToSkip);

            // Assert
            foreach (var entity in entities)
            {
                string message = string.Format("Entity '{0}' could not be backed up.", entity.Name);
                Assert.IsTrue(CheckEntityExistsInSqlServerDb(productionConnectionString, metadataWorkspace, entity), message);
            }
        }

        #region Private methods (helpers)
        private void EnsureInDatabase(string connectionString, IMetadataWorkspace metadataWorkspace, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                InsertEntityIntoSqlServerDb(connectionString, metadataWorkspace, entity);

                if (!CheckEntityExistsInSqlServerDb(connectionString, metadataWorkspace, entity))
                {
                    string message = string.Format("DB INIT: Entity '{0}' could not be saved in the production database.", entity.Name);
                    throw new Exception(message);
                }
            }
        }

        private IEnumerable<Entity> SetUpSourceEntities()
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

            return entities;
        }

        private IEnumerable<Entity> SetUpDefectiveEntities()
        {
            List<Entity> entities = new List<Entity>();

            entities.Add(GetDrawing(1, "1st", 800, 600, 10));
            //entities.Add(GetDrawing(2, "2nd", 500, 200, 5));      // Missing.
            entities.Add(GetDrawing(3, "3rd", 480, 420, 2));        // Modified integer.
            entities.Add(GetDrawing(4, "modified", 320, 480, 4));   // Modified string.
            entities.Add(GetDrawing(5, "5th", 600, 800, 10));
            entities.Add(GetDrawing(7, "6th", 200, 500, 10));       // Modified primary key.

            //entities.Add(GetSubDrawing(1, 2, 300, 400, 1));       // Missing.
            entities.Add(GetSubDrawing(1, 4, 480, 320, 2));
            //entities.Add(GetSubDrawing(1, 3, 320, 280, 3));       // Missing.
            entities.Add(GetSubDrawing(2, 10, 400, 600, 2));        // Modified foreign key/primary key member.
            entities.Add(GetSubDrawing(2, 6, 100, 600, 3));
            entities.Add(GetSubDrawing(3, 4, 120, 120, 1));
            entities.Add(GetSubDrawing(3, 6, 180, 500, 2));         // Modified key member.
            entities.Add(GetSubDrawing(4, 5, 180, 320, 1));

            entities.Add(GetPixel(1, 1, 1, 1, 10));
            entities.Add(GetPixel(2, 3, 5, 10, 5));                 // Modified foreign key/primary key member.
            entities.Add(GetPixel(1, 1, 1, 6, 2));

            entities.Add(GetLine(1, 1, 800, 600, 1, 1, 10));
            //entities.Add(GetLine(1, 2, 480, 320, 2, 2, 5));       // Missing.
            entities.Add(GetLine(5, 5, 320, 280, 1, 5, 2));
            entities.Add(GetLine(1, 1, 800, 600, 1, 10, 10));       // Modified foreign key/primary key member.

            //entities.Add(GetImage(1, "1st.jpg"));                 // Missing.
            entities.Add(GetImage(2, "2nd.png"));
            entities.Add(GetImage(3, "modified"));                  // Modified.
            entities.Add(GetImage(5, "4th.jpg"));                   // Modified primary key.

            entities.Add(GetDrawingsImages(1, 1, 800, 600, 3));
            entities.Add(GetDrawingsImages(1, 5, 500, 200, 2));     // Modified foreign key.
            entities.Add(GetDrawingsImages(2, 2, 480, 320, 1));
            //entities.Add(GetDrawingsImages(2, 4, 320, 480, 3));   // Missing.
            entities.Add(GetDrawingsImages(4, 1, 600, 800, 1));
            entities.Add(GetDrawingsImages(4, 5, 200, 500, 2));     // Modified key memeber.

            return entities;
        }

        private IEnumerable<Entity> FilterDrawingsImages(IEnumerable<Entity> entities)
        {
            return entities.Where(entity => !entity.Name.Equals("DrawingsImages") && !entity.Name.Equals("Images"));
        }
        #endregion
    }
}
