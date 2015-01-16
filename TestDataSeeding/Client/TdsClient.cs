using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.DbClient;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;
using TestDataSeeding.SerializedStorage;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Test Data Seeding Client class.
    /// </summary>
    public class TdsClient
    {
        /// <summary>
        /// The entity manager.
        /// </summary>
        private IEntityManager entityManager;

        /// <summary>
        /// The database structure manager.
        /// </summary>
        private IDbStructureManager dbStructureManager;

        /// <summary>
        /// The serialized storage entity structure manager.
        /// </summary>
        private ISerializedStorageStructureManager serializedStorageStructureManager;

        /// <summary>
        /// The default storage path, used when no path is specified in the requests.
        /// </summary>
        private string defaultStoragePath;

        /// <summary>
        /// Constructs a new instance of TdsClient.
        /// </summary>
        public TdsClient(string defaultStoragePath)
        {
            this.defaultStoragePath = defaultStoragePath;

            dbStructureManager = new MsSqlClient();
            serializedStorageStructureManager = new XmlStorageClient();
            entityManager = new EntityManager();
        }

        /// <summary>
        /// Loads an entity from the default path.
        /// </summary>
        /// <param name="entity">The entity identifier.</param>
        public void LoadEntity(EntityWithKey entity, bool useLock = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.LoadEntities(entities, defaultStoragePath, useLock);
        }

        /// <summary>
        /// Loads an entity from the given path.
        /// </summary>
        /// <param name="entity">The entity identifier.</param>
        /// <param name="path">The path.</param>
        public void LoadEntity(EntityWithKey entity, string path, bool useLock = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.LoadEntities(entities, path, useLock);
        }

        /// <summary>
        /// Saves an entity to the default path.
        /// </summary>
        /// <param name="entity">The entity identifier.</param>
        /// <param name="overwrite">If true, force overwrite, default is false.</param>
        public void SaveEntity(EntityWithKey entity, bool useLock = false, bool overwrite = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.SaveEntities(entities, defaultStoragePath, useLock, overwrite);
        }

        /// <summary>
        /// Saves an entity to the given path.
        /// </summary>
        /// <param name="entity">The given entity identifier.</param>
        /// <param name="path">The path.</param>
        /// <param name="overwrite">If true, force overwrite, default is false.</param>
        public void SaveEntity(EntityWithKey entity, string path, bool useLock = false, bool overwrite = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.SaveEntities(entities, path, useLock, overwrite);
        }

        /// <summary>
        /// Loads the given entities from the default path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        public void LoadEntities(List<EntityWithKey> entities, bool useLock = false)
        {
            entityManager.LoadEntities(entities, defaultStoragePath, useLock);
        }

        /// <summary>
        /// Loads the given entities from the given path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        /// <param name="path">The path.</param>
        public void LoadEntities(List<EntityWithKey> entities, string path, bool useLock = false)
        {
            entityManager.LoadEntities(entities, path, useLock);
        }

        /// <summary>
        /// Saves the given entities to the default path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        /// <param name="overwrite">If true, force overwrite, default is false.</param>
        public void SaveEntities(List<EntityWithKey> entities, bool useLock = false, bool overwrite = false)
        {
            entityManager.SaveEntities(entities, defaultStoragePath, useLock, overwrite);
        }

        /// <summary>
        /// Saves the entities to the given path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        /// <param name="path">The path.</param>
        /// <param name="overwrite">If true, force overwrite, default is false.</param>
        public void SaveEntities(List<EntityWithKey> entities, string path, bool useLock = false, bool overwrite = false)
        {
            entityManager.SaveEntities(entities, path, useLock, overwrite);
        }

        /// <summary>
        /// Generates the database structure to the default path.
        /// </summary>
        public void GenerateDatabaseStructure()
        {
            serializedStorageStructureManager.SaveEntityStructures(dbStructureManager.GetDatabaseStructure(), defaultStoragePath);
        }

        /// <summary>
        /// Generates the database structure to the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void GenerateDatabaseStructure(string path)
        {
            serializedStorageStructureManager.SaveEntityStructures(dbStructureManager.GetDatabaseStructure(), path);
        }

        /// <summary>
        /// Gets the entity structures from the default path.
        /// </summary>
        /// <returns>The entity structures from the default path.</returns>
        public EntityStructures GetEntityStructures()
        {
            return serializedStorageStructureManager.GetEntityStructures(defaultStoragePath);
        }

        /// <summary>
        /// Gets the entity structures from the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>the entity structures from the given path.</returns>
        public EntityStructures GetEntityStructures(string path)
        {
            return serializedStorageStructureManager.GetEntityStructures(path);
        }

        /// <summary>
        /// Saves the entity structures to the default path.
        /// </summary>
        /// <param name="entityStructures">The given entity structures.</param>
        public void SaveEntityStructures(EntityStructures entityStructures)
        {
            serializedStorageStructureManager.SaveEntityStructures(entityStructures, defaultStoragePath);
        }

        /// <summary>
        /// Saves the entity structures to the given path.
        /// </summary>
        /// <param name="entityStructures">The given entity structures.</param>
        /// <param name="path">The path.</param>
        public void SaveEntityStructures(EntityStructures entityStructures, string path)
        {
            serializedStorageStructureManager.SaveEntityStructures(entityStructures, path);
        }
    }
}
