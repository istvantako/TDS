using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Client;
using TestDataSeeding.SerializedStorage;
using TestDataSeeding.DbClient;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// A basic implementation of IEntityManager.
    /// </summary>
    public class EntityManager : IEntityManager
    {
        /// <summary>
        /// The database client.
        /// </summary>
        private IDbClient dbClient;

        /// <summary>
        /// The serialized storage client.
        /// </summary>
        private ISerializedStorageClient serializedStorageClient;

        /// <summary>
        /// The entity structures.
        /// </summary>
        private EntityStructures entityStructures;

        /// <summary>
        /// The active storage path.
        /// </summary>
        private string activeStoragePath;

        /// <summary>
        /// Constructs a new EntityManager with a MS-SQL Server and XML serialized storage client.
        /// </summary>
        /// <param name="path">The storage path.</param>
        public EntityManager(string path)
        {
            dbClient = new MsSqlClient();
            serializedStorageClient = new XmlStorageClient();
            entityStructures = serializedStorageClient.GetEntityStructures(path);
            activeStoragePath = path;
        }

        public void LoadEntity(string entityName, List<string> entityPrimaryKeyValues, string path)
        {
            if (!activeStoragePath.Equals(path))
            {
                activeStoragePath = path;
                entityStructures = serializedStorageClient.GetEntityStructures(path);
            }

            EntityStructure entityStructure = entityStructures.Find(entityName);

            Entity entityFromDb = dbClient.GetEntity(entityStructure, entityPrimaryKeyValues);
            Entity entityFromSerializedStorage = serializedStorageClient.GetEntity(entityStructure, entityPrimaryKeyValues, path);

            try
            {
                // If the entity from the database is not equal to the serialized entity, restore the serialized entity
                // to the database.
                if (!entityFromSerializedStorage.Equals(entityFromDb))
                {
                    dbClient.SaveEntity(entityFromSerializedStorage, entityStructure);
                }
                // QUESTION: where to put the recursive call, in the if statement or in the outer code block?

                // Restore each dependent entity recursively.
                var dependencies = CreateDependencies(entityFromSerializedStorage, entityStructure);

                foreach (var dependency in dependencies)
                {
                    LoadEntity(dependency.Key, dependency.Value, path);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void SaveEntity(string entityName, List<string> entityPrimaryKeyValues, string path)
        {
            if (!activeStoragePath.Equals(path))
            {
                activeStoragePath = path;
                entityStructures = serializedStorageClient.GetEntityStructures(path);
            }

            // Get the entity with the given name and primary key values from the database and its structure. 
            EntityStructure entityStructure = entityStructures.Find(entityName);
            Entity entityFromDb = dbClient.GetEntity(entityStructure, entityPrimaryKeyValues);

            try
            {
                // Save the current entity.
                serializedStorageClient.SaveEntity(entityFromDb, entityStructure, path);

                // Save each dependent entity recursively.
                var dependencies = CreateDependencies(entityFromDb, entityStructure);

                foreach (var dependency in dependencies)
                {
                    SaveEntity(dependency.Key, dependency.Value, path);
                }
                
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Creates a Dictionary with the dependencies. The keys are the entity names, the values are list of the primary
        /// key values.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <returns>A Dictionary with the dependencies.</returns>
        private Dictionary<string, List<string>> CreateDependencies(Entity entity, EntityStructure entityStructure)
        {
            Dictionary<string, EntityForeignKey> foreignKeys = entityStructure.ForeignKeys;
            Dictionary<string, List<string>> dependencies = new Dictionary<string, List<string>>();

            // Get the dependent entity names from the foreign keys.
            var entityNames = foreignKeys.GroupBy(foreignKey => foreignKey.Value.EntityName)
                                         .Select(group => group.First().Value.EntityName)
                                         .ToList();

            // Get the primary key values for each entity (in the order in which they appear in the EntityStructure).
            foreach (var entityName in entityNames)
            {
                dependencies[entityName] = new List<string>();

                foreach (var primaryKey in entityStructures.Find(entityName).PrimaryKeys)
                {
                    var attributeName = foreignKeys.Where(foreignKey =>
                                                        foreignKey.Value.EntityName == entityName
                                                        && foreignKey.Value.KeyName == primaryKey)
                                                   .Select(foreignKey => foreignKey.Key)
                                                   .First();
                    dependencies[entityName].Add(entity.AttributeValues[attributeName]);
                }
            }

            return dependencies;
        }
    }
}
