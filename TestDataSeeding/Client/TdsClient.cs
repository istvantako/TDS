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

        public void LoadEntity(EntityWithKey entity, bool overwrite = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.LoadEntities(entities, defaultStoragePath, overwrite);
        }

        public void LoadEntity(EntityWithKey entity, string path, bool overwrite = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.LoadEntities(entities, path, overwrite);
        }

        public void SaveEntity(EntityWithKey entity, bool overwrite = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.SaveEntities(entities, defaultStoragePath, overwrite);
        }

        public void SaveEntity(EntityWithKey entity, string path, bool overwrite = false)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            entities.Add(entity);

            entityManager.SaveEntities(entities, path, overwrite);
        }

        public void LoadEntities(List<EntityWithKey> entities, bool overwrite = false)
        {
            entityManager.LoadEntities(entities, defaultStoragePath, overwrite);
        }

        public void LoadEntities(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            entityManager.LoadEntities(entities, path, overwrite);
        }

        public void SaveEntities(List<EntityWithKey> entities, bool overwrite = false)
        {
            entityManager.SaveEntities(entities, defaultStoragePath, overwrite);
        }

        public void SaveEntities(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            entityManager.SaveEntities(entities, path, overwrite);
        }

        public void GenerateDatabaseStructure()
        {
            serializedStorageStructureManager.SaveEntityStructures(dbStructureManager.GetDatabaseStructure(), defaultStoragePath);
        }

        public EntityStructures GetEntityStructures()
        {
            return serializedStorageStructureManager.GetEntityStructures(defaultStoragePath);
        }

        public EntityStructures GetEntityStructures(string path)
        {
            return serializedStorageStructureManager.GetEntityStructures(path);
        }

        public void SaveEntityStructures(EntityStructures entityStructures)
        {
            serializedStorageStructureManager.SaveEntityStructures(entityStructures, defaultStoragePath);
        }

        public void SaveEntityStructures(EntityStructures entityStructures, string path)
        {
            serializedStorageStructureManager.SaveEntityStructures(entityStructures, path);
        }
    }
}
