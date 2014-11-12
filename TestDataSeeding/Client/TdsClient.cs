using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Logic;

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
        /// Constructs a new instance of TdsClient.
        /// </summary>
        public TdsClient()
        {
            entityManager = new EntityManager(ConfigurationManager.AppSettings["TdsStoragePath"]);
        }

        /// <summary>
        /// Loads a saved entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// into the database, from the default storage path given in the AppConfig.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        public void LoadEntity(string entityName, List<string> entityPrimaryKeyValues)
        {
            entityManager.LoadEntity(entityName, entityPrimaryKeyValues, ConfigurationManager.AppSettings["TdsStoragePath"]);
        }

        /// <summary>
        /// Loads a saved entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// into the database, from the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <param name="path">The path where the entity is stored.</param>
        public void LoadEntity(string entityName, List<string> entityPrimaryKeyValues, string path)
        {
            entityManager.LoadEntity(entityName, entityPrimaryKeyValues, path);
        }

        /// <summary>
        /// Saves an entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// to the default storage path given in the AppConfig.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        public void SaveEntity(string entityName, List<string> entityPrimaryKeyValues)
        {
            entityManager.SaveEntity(entityName, entityPrimaryKeyValues, ConfigurationManager.AppSettings["TdsStoragePath"]);
        }

        /// <summary>
        /// Saves an entity, identified by <paramref name="entityName"/> and <paramref name="entityPrimaryKeyValues"/>,
        /// to the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="entityPrimaryKeyValues">A list with the pramary key values that identifies the entity.</param>
        /// <param name="path">The path where the entity will be stored.</param>
        public void SaveEntity(string entityName, List<string> entityPrimaryKeyValues, string path)
        {
            entityManager.SaveEntity(entityName, entityPrimaryKeyValues, path);
        }
    }
}
