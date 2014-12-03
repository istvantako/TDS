using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// EntityStructure class, holds the structure of an entity (entity (table) name, attribute names, primary key attributes,
    /// foreign keys).
    /// </summary>
    public class EntityStructure
    {
        /// <summary>
        /// Gets the entity (table) name.
        /// </summary>
        [YAXAttributeForClass()]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dictionary of the attribute names, the keys are attribute names, the values are the corresponding
        /// attribute types.
        /// </summary>
        [YAXDictionary(EachPairName = "Attribute", KeyName = "AttributeName", ValueName = "Type",
                   SerializeKeyAs = YAXNodeTypes.Attribute,
                   SerializeValueAs = YAXNodeTypes.Attribute)]
        [YAXSerializeAs("Attributes")]
        public Dictionary<string, string> Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of the primary keys.
        /// </summary>
        [YAXCollection(YAXCollectionSerializationTypes.Serially, SeparateBy = ", ")]
        [YAXSerializeAs("PrimaryKeys")]
        public List<string> PrimaryKeys
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dictionary of the foreign keys (attribute names), the keys are attribute names, the values are
        /// EntityForeignKey objects, holding the referenced entity (table) name and the referenced key (attribute) name.
        /// </summary>
        [YAXDictionary(EachPairName = "ForeignKey", KeyName = "AttributeName", ValueName = "Target",
                   SerializeKeyAs = YAXNodeTypes.Attribute,
                   SerializeValueAs = YAXNodeTypes.Attribute)]
        [YAXSerializeAs("ForeignKeys")]
        public Dictionary<string, EntityForeignKey> ForeignKeys
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of the associative entity names, which contain this entity.
        /// </summary>
        [YAXCollection(YAXCollectionSerializationTypes.Serially, SeparateBy = ", ")]
        [YAXSerializeAs("BelongsToMany")]
        public List<string> BelongsToMany
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a new empty EntityStructure.
        /// </summary>
        public EntityStructure()
        {
            Attributes = new Dictionary<string, string>();
            PrimaryKeys = new List<string>();
            ForeignKeys = new Dictionary<string, EntityForeignKey>();
            BelongsToMany = new List<string>();
        }

        /// <summary>
        /// Constructs a new EntityType with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public EntityStructure(string name)
        {
            Name = name;
            Attributes = new Dictionary<string, string>();
            PrimaryKeys = new List<string>();
            ForeignKeys = new Dictionary<string, EntityForeignKey>();
            BelongsToMany = new List<string>();
        }

        /// <summary>
        /// Indicates whether the given <paramref name="attribute"/> is (part of the) primary key.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <returns>True, if the attribute is (part of the) primary key, otherwise false.</returns>
        public bool IsPrimaryKey(string attribute)
        {
            return PrimaryKeys.Exists(key => key.Equals(attribute));
        }

        /// <summary>
        /// Indicates whether the given <paramref name="attribute"/> is a foreign key.
        /// </summary>
        /// <param name="attribute">The attribute name.</param>
        /// <returns>True, if the attribute is a foreign key, otherwise false.</returns>
        public bool IsForeignKey(string attribute)
        {
            return ForeignKeys.ContainsKey(attribute);
        }

        /// <summary>
        /// Returns the string representation of this EntityType.
        /// </summary>
        /// <returns>Returns the string representation of this EntityType.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("[EntityStructure: " + Name + "]\n");

            foreach (var attribute in Attributes)
            {
                stringBuilder.Append("  - ");

                if (PrimaryKeys.Contains(attribute.Key))
                {
                    stringBuilder.Append("[PK] ");
                }

                stringBuilder.Append(attribute.Key + " (" + attribute.Value + ") ");

                if (ForeignKeys.ContainsKey(attribute.Key))
                {
                    stringBuilder.Append("[FK references " + ForeignKeys[attribute.Key].EntityName + "(" +
                        ForeignKeys[attribute.Key].KeyName + ")" + "]");
                }

                stringBuilder.Append("\n");
            }

            if (BelongsToMany.Any())
            {
                stringBuilder.Append("  + BelongsToMany (");
                foreach (var item in BelongsToMany)
                {
                    stringBuilder.Append(item).Append(" ");
                }
                stringBuilder.Append(")\n");
            }

            return stringBuilder.ToString();
        }
    }
}
