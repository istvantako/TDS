using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

using Tds.Engine;
using Tds.Interaces;
using Tds.Interaces.Database;
using Tds.Interaces.Structure;
using Tds.Engine.Exceptions;
using Tds.Types;

namespace Tds.Engine.Tests
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        [ExpectedException(typeof(EntityStructureNotFoundException))]
        public void Save_EntityDoesNotExist_ThrowsException()
        {
            // Arrange
            var entityName = "testEntity";

            var structureProvider = Substitute.For<IStructureProvider>();

            var api = new Api(structureProvider);

            // Act
            api.Save(entityName, "");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundInDatabaseException))]
        public void Save_EntityWithOneKeyDoesntExist_ThrowsException()
        {
            // Arrange
            var entityName = "testEntity";
            var entityKeyName = "keyName";
            var entityKey = 10;

            #region Prepare structure provider
            var structureProvider = Substitute.For<IStructureProvider>();
            structureProvider.GetEntityStructure(entityName).Returns(new EntityStructure()
            {
                Keys = new KeyStructure[] 
                    { 
                        new KeyStructure()
                        {
                            Sequence = 0,
                            Name = entityKeyName,
                            Type = DataType.Integer
                        }
                    }
            });
            #endregion

            var databaseProvider = Substitute.For<IDatabaseProvider>();

            var api = new Api(structureProvider, databaseProvider);

            // Act
            api.Save(entityName, entityKey.ToString());

            // Assert
        }

        [TestMethod]
        public void Save_EntityWithOneKeyWithNoDependencies_SavesEntity()
        {
            // Arrange
            var entityName = "testEntity";
            var entityKeyName = "keyName";
            var entityKey = 10;
            var entity = new Entity()
            {
                Name = entityName,
                Properties = new Dictionary<string,object>() { { entityKeyName, entityKey } }
            };
            var keys = new KeyStructure[] 
                    { 
                        new KeyStructure()
                        {
                            Sequence = 0,
                            Name = entityKeyName,
                            Type = DataType.Integer
                        }
                    };

            #region Prepare structure provider
            var structureProvider = Substitute.For<IStructureProvider>();
            var entityStructure = new EntityStructure()
            {
                Keys = keys
            };
            structureProvider.GetEntityStructure(entityName).Returns(entityStructure);
            #endregion

            #region Prepare database provider
            var databaseProvider = Substitute.For<IDatabaseProvider>();
            databaseProvider
                .GetEntities(entityName, Arg.Is<EntityKey>(x => x.Name == entityKeyName && x.Value.Equals(entityKey)))
                .Returns(new List<Entity>() { new Entity() { Name = entityName } });
            #endregion

            #region Prepare file storage provider
            var fileStorageProvider = Substitute.For<IFileStorageProvider>();
            fileStorageProvider.
                Get(entityName, Arg.Is<List<object>>(x => x.Count == 1 && x[0] == entityKey.ToString())).
                Returns(new Entity());
            #endregion

            var api = new Api(structureProvider, databaseProvider, fileStorageProvider);

            // Act
            api.Save(entityName, entityKey.ToString());

            // Assert
            structureProvider.
                Received().
                GetEntityStructure(entityName);
            databaseProvider.
                Received().
                GetEntities(entityName, Arg.Is<EntityKey>(x => x.Name == entityKeyName && x.Value.Equals(entityKey)));
            fileStorageProvider.
                Received().
                Save(entity,
                    Arg.Is<List<EntityKey>>(x => x.Count == 1 && x[0].Name == entityKeyName && x[0].Value.Equals(entityKey)),
                    entityStructure);
        }

        [TestMethod]
        public void Save_EntityWithTwoKeysWithNoDependencies_SavesEntity()
        {
            // Arrange
            var entityName = "testEntity";
            var entityKeyName1 = "keyName1";
            var entityKeyName2 = "keyName2";
            var entityKey1 = 10;
            var entityKey2 = "turoo";

            #region Prepare structure provider
            var structureProvider = Substitute.For<IStructureProvider>();
            structureProvider.GetEntityStructure(entityName).Returns(new EntityStructure()
            {
                Keys = new KeyStructure[] 
                    { 
                        new KeyStructure()
                        {
                            Sequence = 0,
                            Name = entityKeyName1,
                            Type = DataType.Integer
                        },
                        new KeyStructure()
                        {
                            Sequence = 1,
                            Name = entityKeyName2,
                            Type = DataType.String
                        }
                    }
            });
            #endregion

            #region Prepare database provider
            var databaseProvider = Substitute.For<IDatabaseProvider>();
            databaseProvider
                .GetEntities(entityName,
                                Arg.Is<EntityKey>(x => x.Name == entityKeyName1 && x.Value.Equals(entityKey1)),
                                Arg.Is<EntityKey>(x => x.Name == entityKeyName2 && x.Value.Equals(entityKey2)))
                .Returns(new List<Entity>() { new Entity() { Name = entityName } });
            #endregion

            var api = new Api(structureProvider, databaseProvider);

            // Act
            api.Save(entityName, entityKey1.ToString(), entityKey2);

            // Assert
            databaseProvider.Received().GetEntities(entityName,
                Arg.Is<EntityKey>(x => x.Name == entityKeyName1 && x.Value.Equals(entityKey1)),
                Arg.Is<EntityKey>(x => x.Name == entityKeyName2 && x.Value.Equals(entityKey2)));
        }
    }
}
