using System.Linq;

using Tds.Engine.Exceptions;
using Tds.Interaces;
using Tds.Interaces.Database;
using Tds.Interaces.Structure;
using Tds.Types;

namespace Tds.Engine
{
    public class Api
    {
        #region Private fields
        private IStructureProvider _structureProvider;
        private IDatabaseProvider _databaseProvider;
        #endregion

        #region Constructurs
        public Api(IStructureProvider structureProvider, IDatabaseProvider databaseProvider, IFileStorageProvider fileStorageProvider)
        {
            _structureProvider = structureProvider;
            _databaseProvider = databaseProvider;
        }

        public Api(IStructureProvider structureProvider, IDatabaseProvider databaseProvider)
        {
            _structureProvider = structureProvider;
            _databaseProvider = databaseProvider;
        }

        public Api(IStructureProvider structureProvider)
        {
            _structureProvider = structureProvider;
        }
        #endregion

        #region Public methods
        public void Save(string entityName, params string[] keys)
        {
            // ==================================
            // Get entity structure
            // ==================================
            var entityStructure = _structureProvider.GetEntityStructure(entityName);
            if (entityStructure == null)
            {
                throw new EntityStructureNotFoundException(entityName);
            }

            // ==================================
            // Get entity from database
            // ==================================
            var entityKeys = GetEntityKeys(entityStructure.Keys, keys);
            var entityFromDatabase = _databaseProvider.GetEntities(entityName, entityKeys);
            if (entityFromDatabase == null) 
            {
                throw new EntityNotFoundInDatabaseException(entityName, 
                    entityKeys.ToDictionary(x => x.Name, x => x.Value.ToString()));
            }
        }
        #endregion

        #region Private fields
        private EntityKey[] GetEntityKeys(KeyStructure[] keysStructure, string[] keys)
        {
            var result = new EntityKey[keysStructure.Length];
            
            var index = 0;
            foreach (var item in keysStructure.OrderBy(x => x.Sequence))
	        {
                result[index] = new EntityKey() 
                { 
                    Name = item.Name,
                    Value = Converter.ConvertFromString(item.Type, keys[index])
                };

                index++;
	        }

            return result;
        }
        #endregion
    }
}
