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
        /// The catalog for the saved associative entities.
        /// </summary>
        private EntityCatalog catalog;

        /// <summary>
        /// The visited entities during the recursive save/restore action.
        /// </summary>
        private List<EntityWithKey> visitedEntities;

        /// <summary>
        /// The stack of locked entities. The top of the stack is the current entity.
        /// A dependency can't reference a locked entity.
        /// </summary>
        private Stack<string> lockedEntities;

        /// <summary>
        /// Constructs a new EntityManager with a MS-SQL Server and XML serialized storage client.
        /// </summary>
        public EntityManager()
        {
            dbClient = new MsSqlClient();
            serializedStorageClient = new XmlStorageClient();
            visitedEntities = new List<EntityWithKey>();
            lockedEntities = new Stack<string>();
        }

        public void LoadEntities(List<EntityWithKey> entities, string path)
        {
            try
            {
                entityStructures = serializedStorageClient.GetEntityStructures(path);
                catalog = serializedStorageClient.GetCatalog(path);

                // Restore the entities and its dependencies.
                foreach (var entity in entities)
                {
                    EntityStructure entityStructure = entityStructures.Find(entity.EntityName);
                    if (entityStructure == null)
                    {
                        throw new TdsLogicException("The given entity/table name (" + entity.EntityName +") is not found.");
                    }

                    InnerLoadEntity(entity.EntityName, entity.PrimaryKeyValues, path);
                }

                dbClient.ExecuteTransaction();
            }
            catch (Exception exception)
            {
                visitedEntities.Clear();
                lockedEntities.Clear();

                throw exception;
            }

            visitedEntities.Clear();
            lockedEntities.Clear();
        }

        /// <summary>
        /// Restores the given entity and recursively its dependencies.
        /// </summary>
        /// <param name="entityName">The name of the given entity.</param>
        /// <param name="entityPrimaryKeyValues">The primary key values of the entity.</param>
        /// <param name="path">The storage path.</param>
        private void InnerLoadEntity(string entityName, List<string> entityPrimaryKeyValues, string path)
        {
            // Check if entity is locked, if not, lock it.
            if (IsLocked(entityName))
            {
                return;
            }
            lockedEntities.Push(entityName);
            bool locked = true;

            // Get the structure of the entity.
            EntityStructure entityStructure = entityStructures.Find(entityName);

            CheckPrimaryKeyCountsAreEqual(entityPrimaryKeyValues, entityStructure);

            // Return, if the current entity has already been updated in this batch.
            if (IsVisited(entityName, entityPrimaryKeyValues))
            {
                // If the entity was visited, unlock the entity and return.
                if (locked)
                {
                    lockedEntities.Pop();
                }

                return;
            }

            // Get the searched entity from the database and from the serialized storage.
            Entity entityFromDb = dbClient.GetEntity(entityStructure, entityPrimaryKeyValues);
            Entity entityFromSerializedStorage = serializedStorageClient.GetEntity(entityStructure, entityPrimaryKeyValues, path);

            try
            {
                visitedEntities.Add(new EntityWithKey(entityName, entityPrimaryKeyValues));

                // Create the list of dependencies and restore each dependency recursively.
                var dependencies = GetDependenciesAndKeys(entityFromSerializedStorage, entityStructure);

                foreach (var dependency in dependencies)
                {
                    InnerLoadEntity(dependency.Key, dependency.Value, path);
                }

                // Restore the current entity.
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

                // Restore the associative entities if the current entity is part of a many-to-many (associative) entity.
                LoadAssociativeEntities(entityFromSerializedStorage, entityStructure, path);

                // Unlock the entity.
                string entityPop = lockedEntities.Pop();
                if (!entityName.Equals(entityPop))
                {
                    throw new TdsLogicException("Entity unlock error.");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Restores the associative entities and their dependencies containing the given entity.
        /// </summary>
        /// <param name="entity">The given entity.</param>
        /// <param name="entityStructure">The structure of the given entity.</param>
        /// <param name="path">The storage path.</param>
        private void LoadAssociativeEntities(Entity entity, EntityStructure entityStructure, string path)
        {
            // Restore each associative entity and their dependencies.
            foreach (var associativeEntityName in entityStructure.BelongsToMany)
            {
                // Check if entity is locked, if not, lock it.
                if (IsLocked(associativeEntityName))
                {
                    continue;
                }
                lockedEntities.Push(associativeEntityName);

                // Get the referenced attributes of the entity from the associative entity.
                var associativeEntityStructure = entityStructures.Find(associativeEntityName);

                var keys = associativeEntityStructure.ForeignKeys.Where(attribute => attribute.Value.EntityName == entity.Name)
                                                                 .Select(attribute => attribute.Value.KeyName)
                                                                 .ToList();

                // Get the values of the referenced attributes.
                var keysAndValues = entity.AttributeValues.Where(attribute => keys.Contains(attribute.Key))
                                                          .ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

                // Get the associative entities.
                List<Entity> associativeEntities = new List<Entity>();
                var associativeEntitiesFileList = catalog.Find(associativeEntityName, keysAndValues);
                foreach (var filename in associativeEntitiesFileList)
                {
                    associativeEntities.Add(serializedStorageClient.GetEntity(filename));
                }

                // Restore each associative entity and its dependencies.
                foreach (var associativeEntity in associativeEntities)
                {
                    var associativeEntityPrimaryKeyValues =
                        associativeEntity.AttributeValues.Where(attribute => associativeEntityStructure.IsPrimaryKey(attribute.Key))
                                                         .Select(attribute => attribute.Value)
                                                         .ToList();

                    // Check if the entity has already been saved.
                    if (IsVisited(associativeEntity.Name, associativeEntityPrimaryKeyValues))
                    {
                        continue;
                    }

                    visitedEntities.Add(new EntityWithKey(associativeEntity.Name, associativeEntityPrimaryKeyValues));

                    // Create the list of dependencies and restore each dependency recursively.
                    var dependencies = GetDependenciesAndKeys(associativeEntity, associativeEntityStructure);

                    foreach (var dependency in dependencies)
                    {
                        if (!dependency.Key.Equals(entity.Name))
                        {
                            InnerLoadEntity(dependency.Key, dependency.Value, path);
                        }
                    }

                    // Restore the associative entity.
                    Entity entityFromDb = dbClient.GetEntity(associativeEntityStructure, associativeEntityPrimaryKeyValues);

                    if (entityFromDb != null)
                    {
                        // If the entity from the database is not equal to the serialized entity, restore the serialized entity
                        // in the database.
                        if (!associativeEntity.Equals(entityFromDb))
                        {
                            dbClient.UpdateWithTransaction(associativeEntity, associativeEntityStructure);
                        }
                    }
                    else
                    {
                        // The entity is missing, insert it into the database.
                        dbClient.InsertWithTransaction(associativeEntity, associativeEntityStructure);
                    }
                }

                // Unlock the entity.
                string entityPop = lockedEntities.Pop();
                if (!associativeEntityName.Equals(entityPop))
                {
                    throw new TdsLogicException("Entity unlock error.");
                }
            }
        }

        public void SaveEntities(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            try
            {
                entityStructures = serializedStorageClient.GetEntityStructures(path);
                catalog = serializedStorageClient.GetCatalog(path);
                serializedStorageClient.BeginTransaction();

                // Saves the entities and their dependencies.
                foreach (var entity in entities)
                {
                    EntityStructure entityStructure = entityStructures.Find(entity.EntityName);
                    if (entityStructure == null)
                    {
                        throw new TdsLogicException("The given entity/table name (" + entity.EntityName + ") is not found.");
                    }

                    InnerSaveEntity(entity.EntityName, entity.PrimaryKeyValues, path, overwrite);
                }

                serializedStorageClient.ExecuteTransaction();
                serializedStorageClient.SaveCatalog(catalog, path);
            }
            catch (Exception exception)
            {
                visitedEntities.Clear();
                lockedEntities.Clear();

                throw exception;
            }

            visitedEntities.Clear();
            lockedEntities.Clear();
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
            // Check if entity is locked, if not, lock it.
            if (IsLocked(entityName))
            {
                return;
            }
            lockedEntities.Push(entityName);
            bool locked = true;

            // Get the entity with the given name and primary key values from the database and its structure. 
            EntityStructure entityStructure = entityStructures.Find(entityName);

            CheckPrimaryKeyCountsAreEqual(entityPrimaryKeyValues, entityStructure);

            // Return, if the current entity has already been updated in this batch.
            if (IsVisited(entityName, entityPrimaryKeyValues))
            {
                // If the entity was visited, unlock the entity and return.
                if (locked)
                {
                    lockedEntities.Pop();
                }

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
                visitedEntities.Add(new EntityWithKey(entityName, entityPrimaryKeyValues));

                // Save the current entity.
                serializedStorageClient.SaveEntity(entityFromDb, entityStructure, path);

                // Create the list of dependencies and save each dependency recursively.
                var dependencies = GetDependenciesAndKeys(entityFromDb, entityStructure);

                foreach (var dependency in dependencies)
                {
                    InnerSaveEntity(dependency.Key, dependency.Value, path, overwrite);
                }

                // Save the associative entities if the current entity is part of a many-to-many (associative) entity.
                SaveAssociativeEntities(entityFromDb, entityStructure, path, overwrite);

                // Unlock the entity.
                string entityPop = lockedEntities.Pop();
                if (!entityName.Equals(entityPop))
                {
                    throw new TdsLogicException("Entity unlock error.");
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Saves the associative entities and their dependencies containing the given entity.
        /// </summary>
        /// <param name="entity">The given entity.</param>
        /// <param name="entityStructure">The structure of the given entity.</param>
        /// <param name="path">The storage path.</param>
        /// <param name="overwrite">If true, overwrite the already saved entities.</param>
        private void SaveAssociativeEntities(Entity entity, EntityStructure entityStructure, string path, bool overwrite)
        {
            // Save each associative entity and their dependencies.
            foreach (var associativeEntityName in entityStructure.BelongsToMany)
            {
                // Check if entity is locked, if not, lock it.
                if (IsLocked(associativeEntityName))
                {
                    continue;
                }
                lockedEntities.Push(associativeEntityName);

                // Get the referenced attributes of the entity from the associative entity.
                var associativeEntityStructure = entityStructures.Find(associativeEntityName);

                var keys = associativeEntityStructure.ForeignKeys.Where(attribute => attribute.Value.EntityName == entity.Name)
                                                                 .Select(attribute => attribute.Value.KeyName)
                                                                 .ToList();

                // Get the values of the referenced attributes.
                var keysAndValues = entity.AttributeValues.Where(attribute => keys.Contains(attribute.Key))
                                                          .ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

                // Get the associative entities.
                var associativeEntities = dbClient.GetAssociativeEntities(associativeEntityStructure, keysAndValues);

                // Save each associative entity and its dependencies.
                foreach (var associativeEntity in associativeEntities)
                {
                    var associativeEntityPrimaryKeyValues =
                        associativeEntity.AttributeValues.Where(attribute => associativeEntityStructure.IsPrimaryKey(attribute.Key))
                                                         .Select(attribute => attribute.Value)
                                                         .ToList();

                    // Check if the entity has already been visited.
                    if (IsVisited(associativeEntity.Name, associativeEntityPrimaryKeyValues))
                    {
                        continue;
                    }

                    visitedEntities.Add(new EntityWithKey(associativeEntity.Name, associativeEntityPrimaryKeyValues));

                    // Create the list of dependencies and save each dependency recursively.
                    var dependencies = GetDependenciesAndKeys(associativeEntity, associativeEntityStructure);

                    foreach (var dependency in dependencies)
                    {
                        if (!dependency.Key.Equals(entity.Name))
                        {
                            InnerSaveEntity(dependency.Key, dependency.Value, path, overwrite);
                        }
                    }

                    // Save the associative entity.
                    serializedStorageClient.SaveEntity(associativeEntity, associativeEntityStructure, path);
                    catalog.Add(associativeEntity, associativeEntityStructure,
                        serializedStorageClient.GetEntityFileName(associativeEntity.Name, associativeEntityPrimaryKeyValues, path));
                }

                // Unlock the entity.
                string entityPop = lockedEntities.Pop();
                if (!associativeEntityName.Equals(entityPop))
                {
                    throw new TdsLogicException("Entity unlock error.");
                }
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

        /// <summary>
        /// Returns true, if the entity is locked.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <returns>True, if the entity is locked.</returns>
        private bool IsLocked(string entityName)
        {
            return lockedEntities.Count(entity => entity.Equals(entityName)) > 0;
        }
    }
}
