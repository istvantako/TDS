using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// EntityForeignKey class, holds information about the referenced entity (entity (table) name and key (attribute) name).
    /// </summary>
    public class EntityForeignKey
    {
        /// <summary>
        /// The referenced entity (table) name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Gets the referenced entity (table) name.
        /// </summary>
        public string EntityName
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The referenced key name.
        /// </summary>
        private readonly string key;

        /// <summary>
        /// Gets the referenced key name.
        /// </summary>
        public string KeyName
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Constructs a new EntityForeignKey with <paramref name="name"/> and <paramref name="key"/>.
        /// </summary>
        /// <param name="name">The referenced entity name.</param>
        /// <param name="key">The referenced key name.</param>
        public EntityForeignKey(string name, string key)
        {
            this.name = name;
            this.key = key;
        }
    }
}
