using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Tds.Engine.Tests;
using Tds.StorageProviders.SqlServer;
using Tds.Interfaces.Metadata;
using Tds.Types;
using Tds.MetadataProviders.Xml;
using Tds.Interfaces.Model;
using System.Collections.Generic;

namespace Tds.StorageProviders.Tests
{
    [TestClass]
    public class SqlServerStorageProviderTest
    {
        /*[TestMethod]
        public void GetSubDrawingEntityWithCompositeKeyFromDatabase_EntityExists_AssertsTrue()
        {
            // Arrange
            var productionConnectionString = ConfigurationManager.ConnectionStrings["DrawingsProductionContext"].ConnectionString;
            var sqlServerStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var drawingsXmlMetadataLocation = TestSettings.Storage.XmlMetadataManualLocation + @"\Drawings.xml";
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var sqlServerRepository = sqlServerStorageProvider.GetRepository(metadataProvider);

            var keys = new EntityKey[2];
            keys[0] = new EntityKey()
            {
                Name = "MainDrawingId",
                Value = 19
            };
            keys[1] = new EntityKey()
            {
                Name = "SubDrawingId",
                Value = 21
            };

            // Act
            sqlServerRepository.Read("SubDrawings", keys);

            // Assert
        }*/

        [TestMethod]
        public void GetDrawingEntityWithSimpleKeyFromDatabase_EntityExists_AssertsTrue()
        {
            // Arrange
            var productionConnectionString = ConfigurationManager.ConnectionStrings["DrawingsProductionContext"].ConnectionString;
            var sqlServerStorageProvider = new SqlServerStorageProvider(productionConnectionString);
            var drawingsXmlMetadataLocation = TestSettings.Storage.XmlMetadataManualLocation + @"\Drawings.xml";
            var metadataProvider = new XmlMetadataProvider(drawingsXmlMetadataLocation);
            var sqlServerRepository = sqlServerStorageProvider.GetRepository(metadataProvider);

            var keys = new EntityKey[1];
            keys[0] = new EntityKey()
            {
                Name = "Id",
                Value = 30
            };

            // Act
            sqlServerRepository.Read("Drawings", keys);

            // Assert
        }
    }
}
