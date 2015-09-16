using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;
using Tds.Engine.Exceptions;
using Tds.Types;

namespace Tds.Engine.Tests
{
    [TestClass]
    public class ApiTests
    {
        /*[TestMethod]
        [ExpectedException(typeof(EntityStructureNotFoundException))]
        public void Backup_EntityDoesNotExist_ThrowsException()
        {
            // Arrange
            var entityName = "testEntity";

            var structureProvider = GetStructureProviderMock();

            var api = new Api(structureProvider);

            // Act
            api.Backup(entityName, "");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundInDatabaseException))]
        public void Backup_EntityWithOneKeyDoesntExist_ThrowsException()
        {
            // Arrange
            var entityName = "testEntity";
            var entityKeyName = "keyName";
            var entityKey = 10;

            #region Prepare structure provider
            IEntityStructure structure;
            var structureProvider = GetStructureProviderMock_GetEntityStructure(entityName, out structure, entityKeyName);
            #endregion

            var productionStorageProvider = Substitute.For<IStorageProvider>();

            var api = new Api(structureProvider, productionStorageProvider);

            // Act
            api.Backup(entityName, entityKey.ToString());

            // Assert
        }

        [TestMethod]
        public void Backup_EntityWithOneKeyWithNoDependencies_BackupEntity()
        {
            // Arrange
            var entityName = "testEntity";
            var entityKeyName = "keyName";
            var entityKey = 10;
            var entity = new IEntity()
            {
                Name = entityName,
                Properties = new Dictionary<string,object>() { { entityKeyName, entityKey } }
            };

            #region Prepare structure provider
            IEntityStructure structure;
            var structureProvider = GetStructureProviderMock_GetEntityStructure(entityName, out structure, entityKeyName);
            #endregion

            #region Prepare production storage provider
            var productionStorageProvider = Substitute.For<IStorageProvider>();
            productionStorageProvider
                .Read(entityName, Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 1 &&
                                                                        x.Any(y => y.Name == entityKeyName && 
                                                                                y.Value.Equals(entityKey))), structure)
                .Returns(entity);
            #endregion

            #region Prepare backup storage provider
            var backupStorageProvider = Substitute.For<IStorageProvider>();
            backupStorageProvider.
                Write(entity, Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 1 &&
                                                            x.Any(y => y.Name == entityKeyName &&
                                                                        y.Value.Equals(entityKey))), structure);
            #endregion

            var api = new Api(structureProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, entityKey.ToString());

            // Assert
            structureProvider.
                Received().
                GetEntityStructure(entityName);
            productionStorageProvider.
                Received().
                Read(entityName, Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 1 &&
                                                                        x.Any(y => y.Name == entityKeyName &&
                                                                                y.Value.Equals(entityKey))), structure);
            backupStorageProvider.
                Received().
                Write(entity,
                    Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 1 && 
                                                    x.Any(y => y.Name == entityKeyName && 
                                                            y.Value.Equals(entityKey))),
                    structure);
        }

        [TestMethod]
        public void Backup_EntityWithTwoKeysWithNoDependencies_BackupEntity()
        {
            // Arrange
            var entityName = "testEntity";
            var entityKeyName1 = "keyName1";
            var entityKeyName2 = "keyName2";
            var entityKey1 = 10;
            var entityKey2 = "turoo";

            var entity = new IEntity() { Name = entityName, Properties = new Dictionary<string, object>() };

            #region Prepare structure provider
            IEntityStructure structure;
            var structureProvider = GetStructureProviderMock_GetEntityStructure(entityName, out structure, 
                entityKeyName1, DataType.Integer, entityKeyName2, DataType.String);
            #endregion

            #region Prepare production storage provider
            var productionStorageProvider = Substitute.For<IStorageProvider>();
            productionStorageProvider
                .Read(entityName,
                        Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 2 &&
                                                                x.Any(y => y.Name == entityKeyName1 && y.Value.Equals(entityKey1)) &&
                                                                x.Any(y => y.Name == entityKeyName2 && y.Value.Equals(entityKey2))),
                        structure)
                .Returns(entity);
            #endregion

            #region Prepare backup storage provider
            var backupStorageProvider = Substitute.For<IStorageProvider>();
            backupStorageProvider
                .Write(entity,
                        Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 2 &&
                                                                x.Any(y => y.Name == entityKeyName1 && y.Value.Equals(entityKey1)) &&
                                                                x.Any(y => y.Name == entityKeyName2 && y.Value.Equals(entityKey2))),
                        structure);
            #endregion

            var api = new Api(structureProvider, productionStorageProvider, backupStorageProvider);

            // Act
            api.Backup(entityName, entityKey1.ToString(), entityKey2);

            // Assert
            productionStorageProvider.Received().Read(entityName,
                        Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 2 &&
                                                                x.Any(y => y.Name == entityKeyName1 && y.Value.Equals(entityKey1)) &&
                                                                x.Any(y => y.Name == entityKeyName2 && y.Value.Equals(entityKey2))),
                                            structure);

            backupStorageProvider.Received().Write(entity,
                        Arg.Is<IEnumerable<IEntityKey>>(x => x.Count() == 2 &&
                                                                x.Any(y => y.Name == entityKeyName1 && y.Value.Equals(entityKey1)) &&
                                                                x.Any(y => y.Name == entityKeyName2 && y.Value.Equals(entityKey2))),
                                            structure);
        }

        #region Private methods (helpers)
        private IStructureProvider GetStructureProviderMock()
        {
            return Substitute.For<IStructureProvider>();
        }

        private IStructureProvider GetStructureProviderMock_GetEntityStructure(string entityName, out IEntityStructure structure,
            string name, DataType type = DataType.Integer)
        {
            structure = new IEntityStructure()
            {
                Keys = new IKeyStructure[] 
                    { 
                        new IKeyStructure()
                        {
                            Sequence = 0,
                            Name = name,
                            Type = type
                        }
                    }
            };

            var mock = GetStructureProviderMock();
            mock.GetEntityStructure(entityName).Returns(structure);

            return mock;
        }

        private IStructureProvider GetStructureProviderMock_GetEntityStructure(string entityName, out IEntityStructure structure,
                    string name1, DataType type1,
                    string name2, DataType type2 = DataType.Integer)
        {
            structure = new IEntityStructure()
            {
                Keys = new IKeyStructure[] 
                    { 
                        new IKeyStructure()
                        {
                            Sequence = 0,
                            Name = name1,
                            Type = type1
                        },                        
                        new IKeyStructure()
                        {
                            Sequence = 1,
                            Name = name2,
                            Type = type2
                        }
                    }
            };

            var mock = GetStructureProviderMock();
            mock.GetEntityStructure(entityName).Returns(structure);

            return mock;
        }


        #endregion*/
    }
}
