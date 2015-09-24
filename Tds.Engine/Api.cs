using System.Linq;

using Tds.Engine.Exceptions;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Interfaces.Metadata;
using Tds.Types;
using Tds.Engine.Core;
using System.Collections.Generic;

namespace Tds.Engine
{
    public class Api
    {
        #region Private fields
        private IMetadataWorkspace metadataWorkspace;
        private IRepository productionRepository;
        private IRepository backupRepository;
        #endregion

        #region Constructurs
        public Api(IMetadataProvider metadataProvider, IStorageProvider productionStorageProvider, IStorageProvider backupStorageProvider)
        {
            this.metadataWorkspace = metadataProvider.GetMetadataWorkspace();

            this.productionRepository = productionStorageProvider.GetRepository(metadataProvider);
            this.backupRepository = backupStorageProvider.GetRepository(metadataProvider);
        }
        #endregion

        #region Public methods
        public void Backup(string entityName, IEnumerable<string> keys)
        {
            var entityType = metadataWorkspace.GetEntityType(entityName);
            if (entityType == null)
            {
                throw new EntityTypeNotFoundException(entityName);
            }

            var backupTask = new MaintenanceTask(metadataWorkspace, productionRepository, backupRepository, new DependencyResolver());
            var entityKeyMembers = GetEntityKeys(entityType, keys.ToArray());

            backupTask.Save(entityName, entityKeyMembers);
        }

        public void Restore(string entityName, IEnumerable<string> keys)
        {
            var entityType = metadataWorkspace.GetEntityType(entityName);
            if (entityType == null)
            {
                throw new EntityTypeNotFoundException(entityName);
            }

            var backupTask = new MaintenanceTask(metadataWorkspace, backupRepository, productionRepository, new DependencyResolver());
            var entityKeyMembers = GetEntityKeys(entityType, keys.ToArray());

            backupTask.Save(entityName, entityKeyMembers);
        }
        #endregion

        #region Private fields
        private ICollection<EntityKey> GetEntityKeys(EntityType entityType, string[] keys)
        {
            var keyMembers = entityType.PrimaryKey;
            var properties = entityType.Properties;

            var result = new EntityKey[keyMembers.Count];
            
            var index = 0;
            foreach (var item in keyMembers.OrderBy(x => x.Sequence))
	        {
                result[index] = new EntityKey() 
                { 
                    Name = item.Name,
                    Value = Converter.ConvertFromString(properties[item.Name], keys[index])
                };

                index++;
	        }

            return result;
        }
        #endregion
    }
}
