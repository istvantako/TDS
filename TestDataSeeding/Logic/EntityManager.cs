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
    internal class EntityManager : IEntityManager
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
        /// The visited entities during the recursive save/restore action.
        /// </summary>
        private List<EntityWithKey> visitedEntities;

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
            visitedEntities = new List<EntityWithKey>();
        }

        public void LoadEntity(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            SetActiveStoragePath(path);

            try
            {
                // Restore the entities and its dependencies.
                foreach (var entity in entities)
                {
                    InnerLoadEntity(entity.EntityName, entity.PrimaryKeyValues, path, overwrite);
                }

                dbClient.ExecuteTransaction();
            }
            catch (Exception exception)
            {
                visitedEntities.Clear();
                throw exception;
            }

            visitedEntities.Clear();
        }

        /// <summary>
        /// Restores the given entity and recursively its dependencies.
        /// </summary>
        /// <param name="entityName">The name of the given entity.</param>
        /// <param name="entityPrimaryKeyValues">The primary key values of the entity.</param>
        /// <param name="path">The storage path.</param>
        /// <param name="overwrite">If true, overwrite the already saved entities.</param>
        private void InnerLoadEntity(string entityName, List<string> entityPrimaryKeyValues, string path, bool overwrite)
        {
            // Get the structure of the entity.
            EntityStructure entityStructure = entityStructures.Find(entityName);

            CheckPrimaryKeyCountsAreEqual(entityPrimaryKeyValues, entityStructure);

            // Return, if the current entity has already been updated in this batch.
            if (IsVisited(entityName, entityPrimaryKeyValues))
            {
                return;
            }

            // Get the searched entity from the database and from the serialized storage.
            Entity entityFromDb = dbClient.GetEntity(entityStructure, entityPrimaryKeyValues);
            Entity entityFromSerializedStorage = serializedStorageClient.GetEntity(entityStructure, entityPrimaryKeyValues, path);

            try
            {
                // If the entity exists in the database, check if it's equal to the serialized entity.
                if (entityFromDb != null)
                {
                    // If the entity from the database is not equal to the serialized entity, restore the serialized entity
                    // in the database.
                    if (!entityFromSerializedStorage.Equals(entityFromDb))
                    {
                        dbClient.UpdateWithTransaction(entityFromSerializedStorage, entityStructure);
                    }
                }
                else
                {
                    // The entity is missing, insert it into the database.
                    dbClient.InsertWithTransaction(entityFromSerializedStorage, entityStructure);
                }

                visitedEntities.Add(new EntityWithKey(entityName, entityPrimaryKeyValues));

                // Create the list of dependencies and restore each dependency recursively.
                var dependencies = GetDependenciesAndKeys(entityFromSerializedStorage, entityStructure);

                foreach (var dependency in dependencies)
                {
                    InnerLoadEntity(dependency.Key, dependency.Value, path, overwrite);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void InnerLoadAssociativeEntity(Entity entity, EntityStructure entityStructure, string path, bool overwrite)
        {
        }

        public void SaveEntity(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            SetActiveStoragePath(path);

            try
            {
                // Saves the entities and their dependencies.
                foreach (var entity in entities)
                {
                    InnerSaveEntity(entity.EntityName, entity.PrimaryKeyValues, path, overwrite);
                }
            }
            catch (Exception exception)
            {
                visitedEntities.Clear();
                throw exception;
            }

            visitedEntities.Clear();
        }

        /// <summary>
        /// Saves the given entity and recursively its dependencies.
        /// </summary>
        /// <param name="entityName">The name of the given entity.</param>
        /// <param name="entityPrimaryKeyValues">The primary key values of the entity.</param>
        /// <param name="path">The storage path.</param>
        /// <param name="overwrite">If true, overwrite the already saved entities.</param>
        private void InnerSaveEntity(string entityName, List<string> entityPrimaryKeyValues, string path, bool overwrite)
        {
            // Get the entity with the given name and primary key values from the database and its structure. 
            EntityStructure entityStructure = entityStructures.Find(entityName);

            CheckPrimaryKeyCountsAreEqual(entityPrimaryKeyValues, entityStructure);

            // Return, if the current entity has already been updated in this batch.
            if (IsVisited(entityName, entityPrimaryKeyValues))
            {
                return;
            }

            // Check if the entity has already been saved on the specified path.
            if (!overwrite && serializedStorageClient.IsSaved(entityName, entityPrimaryKeyValues, path))
            {
                throw new EntityAlreadySavedException("The entity has already been saved.");
            }

            Entity entityFromDb = dbClient.GetEntity(entityStructure, entityPrimaryKeyValues);

            // If there is no entity with the given primary key values, abort the saving process.
            if (entityFromDb == null)
            {
                throw new TdsLogicException("Entity not found in the database.");
            }

            try
            {
                // Save the current entity.
                serializedStorageClient.SaveEntity(entityFromDb, entityStructure, path);
                visitedEntities.Add(new EntityWithKey(entityName, entityPrimaryKeyValues));

                // Create the list of dependencies and save each dependency recursively.
                var dependencies = GetDependenciesAndKeys(entityFromDb, entityStructure);

                foreach (var dependency in dependencies)
                {
                    InnerSaveEntity(dependency.Key, dependency.Value, path, overwrite);
                }

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void InnerSaveAssociativeEntity(Entity entity, EntityStructure entityStructure, string path, bool overwrite)
        {
            foreach (var associativeEntity in entityStructure.BelongsToMany)
            {

            }
        }

        /// <summary>
        /// Creates a Dictionary with the dependencies. The keys are the entity names, the values are list of the primary
        /// key values.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The structure of the entity.</param>
        /// <returns>A Dictionary with the dependencies.</returns>
        private Dictionary<string, List<string>> GetDependenciesAndKeys(Entity entity, EntityStructure entityStructure)
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

        /// <summary>
        /// Refresh the the entity structures collection, if the active path has changed.
        /// </summary>
        /// <param name="path">The given storage path.</param>
        private void SetActiveStoragePath(string path)
        {
            if (!activeStoragePath.Equals(path))
            {
                activeStoragePath = path;
                entityStructures = serializedStorageClient.GetEntityStructures(path);
            }
        }

        /// <summary>
        /// Check if the number of the provided primary key values matches the number of primary key components.
        /// </summary>
        /// <param name="entityPrimaryKeyValues">The given primary key values.</param>
        /// <param name="entityStructure">The entity structure which holds the primary key definitions.</param>
        private void CheckPrimaryKeyCountsAreEqual(List<string> entityPrimaryKeyValues, EntityStructure entityStructure)
        {
            if (entityPrimaryKeyValues.Count != entityStructure.PrimaryKeys.Count)
            {
                throw new TdsLogicException("Incorrect number of primary key values.");
            }
        }

        /// <summary>
        /// Returns true, if the given entity has already been visited in this batch.
        /// </summary>
        /// <param name="entityName">The name of the given entity.</param>
        /// <param name="entityPrimaryKeyValues">The primary key values of the given entity.</param>
        /// <returns>True, if the given entity has already been visited in this batch.</returns>
        private bool IsVisited(string entityName, List<string> entityPrimaryKeyValues)
        {
            return visitedEntities.Count(entityWithKey => entityWithKey.IsEqual(entityName, entityPrimaryKeyValues)) > 0;
        }
    }
}
