using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// Entity identifier.
    /// Holds the name of the entity and the primary key values.
    /// </summary>
    public class EntityWithKey
    {
        /// <summary>
        /// The name of the entity.
        /// </summary>
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// The primary key values.
        /// </summary>
        public List<string> PrimaryKeyValues
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public EntityWithKey()
        {
        }

        /// <summary>
        /// Constructs a new entity identifier with the given values.
        /// </summary>
        /// <param name="entityName">The name of the entity.</param>
        /// <param name="primaryKeyValues">The primary key values.</param>
        public EntityWithKey(string entityName, List<string> primaryKeyValues)
        {
            EntityName = entityName;
            PrimaryKeyValues = primaryKeyValues;
        }

        /// <summary>
        /// Returns true, if the given entity identifiers are equal.
        /// </summary>
        /// <param name="entityName">The given entity name.</param>
        /// <param name="entityPrimaryKeyValues">The given primary key values.</param>
        /// <returns>True, if the given entity identifiers are equal.</returns>
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

        /// <summary>
        /// Returns the string representation of this Entity.
        /// </summary>
        /// <returns>Returns the string representation of this Entity.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("[" + EntityName + "]\n");
            stringBuilder.Append("Attributes: \n");

            foreach (var keyValue in PrimaryKeyValues)
            {
                stringBuilder.Append("  - '" + keyValue + "'\n");
            }

            return stringBuilder.ToString();
        }
    }
}
