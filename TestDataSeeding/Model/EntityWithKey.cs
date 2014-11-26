using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Model
{
    public class EntityWithKey
    {
        public string EntityName
        {
            get;
            set;
        }

        public List<string> PrimaryKeyValues
        {
            get;
            set;
        }

        public EntityWithKey()
        {
        }

        public EntityWithKey(string entityName, List<string> primaryKeyValues)
        {
            EntityName = entityName;
            PrimaryKeyValues = primaryKeyValues;
        }

        public bool IsEqual(string entityName, List<string> entityPrimaryKeyValues)
        {
            if (!EntityName.Equals(entityName))
            {
                return false;
            }

            for (int i = 0; i < PrimaryKeyValues.Count; i++)
            {
                if (!PrimaryKeyValues.ElementAt(i).Equals(entityPrimaryKeyValues.ElementAt(i)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
