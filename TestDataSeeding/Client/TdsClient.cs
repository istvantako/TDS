using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

        private string structFileName;

        /// <summary>
        /// Constructs a new instance of TdsClient.
        /// </summary>
        public TdsClient(string defaultStoragePath, string structureFileName = Common.Consts.Structure.STRUCTURE_FILE_DEFAULT_NAME)
        {
            this.defaultStoragePath = defaultStoragePath;
            structFileName = structureFileName;

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

            entityManager.LoadEntities(entities, defaultStoragePath, Path.Combine(defaultStoragePath, structFileName), useLock);
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

            entityManager.LoadEntities(entities, path, Path.Combine(path, structFileName), useLock);
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

            entityManager.SaveEntities(entities, defaultStoragePath, Path.Combine(defaultStoragePath, structFileName), useLock, overwrite);
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

            entityManager.SaveEntities(entities, path, Path.Combine(path, structFileName), useLock, overwrite);
        }

        /// <summary>
        /// Loads the given entities from the default path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        public void LoadEntities(List<EntityWithKey> entities, bool useLock = false)
        {
            entityManager.LoadEntities(entities, defaultStoragePath, Path.Combine(defaultStoragePath, structFileName), useLock);
        }

        /// <summary>
        /// Loads the given entities from the given path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        /// <param name="path">The path.</param>
        public void LoadEntities(List<EntityWithKey> entities, string path, bool useLock = false)
        {
            entityManager.LoadEntities(entities, path, Path.Combine(path, structFileName), useLock);
        }

        /// <summary>
        /// Saves the given entities to the default path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        /// <param name="overwrite">If true, force overwrite, default is false.</param>
        public void SaveEntities(List<EntityWithKey> entities, bool useLock = false, bool overwrite = false)
        {
            entityManager.SaveEntities(entities, defaultStoragePath, Path.Combine(defaultStoragePath, structFileName), useLock, overwrite);
        }

        /// <summary>
        /// Saves the entities to the given path.
        /// </summary>
        /// <param name="entities">The list of entity identifiers.</param>
        /// <param name="path">The path.</param>
        /// <param name="overwrite">If true, force overwrite, default is false.</param>
        public void SaveEntities(List<EntityWithKey> entities, string path, bool useLock = false, bool overwrite = false)
        {
            entityManager.SaveEntities(entities, path, Path.Combine(path, structFileName), useLock, overwrite);
        }

        /// <summary>
        /// Generates the database structure to the default path.
        /// </summary>
        public void GenerateDatabaseStructure(bool overwrite = false)
        {
            serializedStorageStructureManager.SaveEntityStructures(dbStructureManager.GetDatabaseStructure(), defaultStoragePath, overwrite);
        }

        /// <summary>
        /// Generates the database structure to the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void GenerateDatabaseStructure(string path, bool overwrite = false)
        {
            serializedStorageStructureManager.SaveEntityStructures(dbStructureManager.GetDatabaseStructure(), path, overwrite);
        }

        /// <summary>
        /// Gets the entity structures from the default path.
        /// </summary>
        /// <returns>The entity structures from the default path.</returns>
        public EntityStructures GetEntityStructures()
        {
            return serializedStorageStructureManager.GetEntityStructures(Path.Combine(defaultStoragePath, structFileName));
        }

        /// <summary>
        /// Gets the entity structures from the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>the entity structures from the given path.</returns>
        public EntityStructures GetEntityStructures(string path)
        {
            return serializedStorageStructureManager.GetEntityStructures(Path.Combine(path, structFileName));
        }

        /// <summary>
        /// Saves the entity structures to the default path.
        /// </summary>
        /// <param name="entityStructures">The given entity structures.</param>
        public void SaveEntityStructures(EntityStructures entityStructures, bool overwrite = false)
        {
            serializedStorageStructureManager.SaveEntityStructures(entityStructures, defaultStoragePath, overwrite);
        }

        /// <summary>
        /// Saves the entity structures to the given path.
        /// </summary>
        /// <param name="entityStructures">The given entity structures.</param>
        /// <param name="path">The path.</param>
        public void SaveEntityStructures(EntityStructures entityStructures, string path, bool overwrite = false)
        {
            serializedStorageStructureManager.SaveEntityStructures(entityStructures, path, overwrite);
        }
    }
}