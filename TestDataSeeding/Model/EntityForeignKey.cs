using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// EntityForeignKey class, holds information about the referenced entity (entity (table) name and key (attribute) name).
    /// </summary>
    public class EntityForeignKey
    {
        /// <summary>
        /// Gets or sets the referenced entity (table) name.
        /// </summary>
        [YAXSerializeAs("Name")]
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the referenced key name.
        /// </summary>
        [YAXSerializeAs("AttributeName")]
        public string KeyName
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs an empty EntityForeignKey object.
        /// </summary>
        public EntityForeignKey()
        {
        }

        /// <summary>
        /// Constructs a new EntityForeignKey with <paramref name="name"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="name">The referenced entity name.</param>
        /// <param name="key">The referenced key name.</param>
        public EntityForeignKey(string name, string key)
        {
            EntityName = name;
            KeyName = key;
        }
    }
}
