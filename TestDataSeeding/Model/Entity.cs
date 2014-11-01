using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// Entity class, holds the values of the attributes defined in the corresponding EntityType.
    /// </summary>
    public class Entity
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
        /// The attribute values.
        /// </summary>
        private Dictionary<string, string> attributeValues;

        /// <summary>
        /// Gets the dictionary of the attribute values, the keys are attribute names, the values are the corresponding
        /// attribute values.
        /// </summary>
        public Dictionary<string, string> AttributeValues
        {
            get
            {
                return attributeValues;
            }
        }

        /// <summary>
        /// Constructs a new Entity with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the entity.</param>
        public Entity(string name)
        {
            this.name = name;
            this.attributeValues = new Dictionary<string, string>();
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True, if the two object instances are equal, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null, return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Entity, return false.
            Entity entity = obj as Entity;
            if ((System.Object) entity == null)
            {
                return false;
            }

            // Return true, if the fields match.
            return (this.name.Equals(entity.name) && this.attributeValues.Equals(entity.attributeValues));
        }

        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="entity">The object to compare.</param>
        /// <returns>True, if the two object instances are equal, otherwise false.</returns>
        public bool Equals(Entity entity)
        {
            // If parameter is null, return false.
            if ((object) entity == null)
            {
                return false;
            }

            // Return true, if the fields match, otherwise false.
            if (!name.Equals(entity.name))
            {
                return false;
            }

            foreach (var attribute in attributeValues)
            {
                if (entity.attributeValues.ContainsKey(attribute.Key))
                {
                    var value = entity.attributeValues[attribute.Key];
                    if (!attribute.Value.Equals(value))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Serves as a hash function for this Entity.
        /// </summary>
        /// <returns>Returns the hash code for this Entity.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 269;
                hash = (hash * 47) + name.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the string representation of this Entity.
        /// </summary>
        /// <returns>Returns the string representation of this Entity.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("[Entity: " + name + "]\n");

            foreach (var attribute in attributeValues)
            {
                stringBuilder.Append("  - " + attribute.Key + " => " + attribute.Value + "\n");
            }

            return stringBuilder.ToString();
        }
    }
}
