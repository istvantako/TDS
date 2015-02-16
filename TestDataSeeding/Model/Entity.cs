using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// Entity class, holds the values of the attributes defined in the corresponding EntityStructure.
    /// </summary>
    public class Entity
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
        /// Gets the dictionary of the attribute values, the keys are attribute names, the values are the corresponding
        /// attribute values.
        /// </summary>
        [YAXDictionary(EachPairName = "Attribute", KeyName = "AttributeName", ValueName = "Value",
                   SerializeKeyAs = YAXNodeTypes.Attribute,
                   SerializeValueAs = YAXNodeTypes.Attribute)]
        [YAXSerializeAs("AttributeValues")]
        public Dictionary<string, string> AttributeValues
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a new empty Entity.
        /// </summary>
        public Entity()
        {
            this.AttributeValues = new Dictionary<string, string>();
        }

        #region Overriden methods from Object
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

            // Return true, if the fields match, otherwise false.
            if (!Name.Equals(entity.Name))
            {
                return false;
            }

            foreach (var attribute in AttributeValues)
            {
                if (entity.AttributeValues.ContainsKey(attribute.Key))
                {
                    var value = entity.AttributeValues[attribute.Key];
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
            if (!Name.Equals(entity.Name))
            {
                return false;
            }

            foreach (var attribute in AttributeValues)
            {
                if (entity.AttributeValues.ContainsKey(attribute.Key))
                {
                    var value = entity.AttributeValues[attribute.Key];
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
                hash = (hash * 47) + Name.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Returns the string representation of this Entity.
        /// </summary>
        /// <returns>Returns the string representation of this Entity.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("[Entity: " + Name + "]\n");

            foreach (var attribute in AttributeValues)
            {
                stringBuilder.Append("  - " + attribute.Key + " => " + attribute.Value + "\n");
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
