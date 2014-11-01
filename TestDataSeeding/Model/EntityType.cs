using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// EntityType class, holds the structure of an entity (entity name, attribute names, primary key attributes, foreign keys).
    /// </summary>
    public class EntityType
    {
        /// <summary>
        /// The entity name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The attribute names and types.
        /// </summary>
        private Dictionary<string, string> attributes;

        /// <summary>
        /// Gets the dictionary of the attribute names, the keys are attribute names, the values are the corresponding
        /// attribute types.
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get
            {
                return attributes;
            }
        }

        /// <summary>
        /// The primary keys.
        /// </summary>
        private List<string> primaryKeys;

        /// <summary>
        /// Gets the list of the primary keys.
        /// </summary>
        public List<string> PrimaryKeys
        {
            get
            {
                return primaryKeys;
            }
        }

        /// <summary>
        /// The foreign keys.
        /// </summary>
        private Dictionary<string, Tuple<string, string>> foreignKeys;

        /// <summary>
        /// Gets the dictionary of the foreign keys (attribute names), the keys are attribute names, the values are 2-tuples,
        /// where the first item of the 2-tuple is the name of the referenced entity, while the second item is the
        /// referenced attribute name.
        /// </summary>
        public Dictionary<string, Tuple<string, string>> ForeignKeys
        {
            get
            {
                return foreignKeys;
            }
        }

        /// <summary>
        /// Constructs a new EntityType with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public EntityType(string name)
        {
            this.name = name;
            this.attributes = new Dictionary<string, string>();
            this.primaryKeys = new List<string>();
            this.foreignKeys = new Dictionary<string, Tuple<string, string>>();
        }

        /// <summary>
        /// Returns the string representation of this EntityType.
        /// </summary>
        /// <returns>Returns the string representation of this EntityType.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("[EntityType: " + name + "]\n");

            foreach (var attribute in attributes)
            {
                stringBuilder.Append("  - ");

                if (primaryKeys.Contains(attribute.Key))
                {
                    stringBuilder.Append("[PK] ");
                }

                stringBuilder.Append(attribute.Key + " (" + attribute.Value + ") ");

                if (foreignKeys.ContainsKey(attribute.Key))
                {
                    stringBuilder.Append("[FK references " + foreignKeys[attribute.Key].Item1 + "(" +
                        foreignKeys[attribute.Key].Item2 + ")" + "]");
                }

                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}
