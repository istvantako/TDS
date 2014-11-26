using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Logic;
using TestDataSeeding.Model;

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
        /// The default storage path, used when no path is specified in the requests.
        /// </summary>
        private string defaultStoragePath;

        /// <summary>
        /// Constructs a new instance of TdsClient.
        /// </summary>
        public TdsClient(string defaultStoragePath)
        {
            this.defaultStoragePath = defaultStoragePath;
            entityManager = new EntityManager(defaultStoragePath);
        }

        public void LoadEntity(List<EntityWithKey> entities, bool overwrite = false)
        {
            entityManager.LoadEntity(entities, defaultStoragePath, overwrite);
        }

        public void LoadEntity(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            entityManager.LoadEntity(entities, path, overwrite);
        }

        public void SaveEntity(List<EntityWithKey> entities, bool overwrite = false)
        {
            entityManager.SaveEntity(entities, defaultStoragePath, overwrite);
        }

        public void SaveEntity(List<EntityWithKey> entities, string path, bool overwrite = false)
        {
            entityManager.SaveEntity(entities, path, overwrite);
        }
    }
}
