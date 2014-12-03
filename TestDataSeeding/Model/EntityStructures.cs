using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace TestDataSeeding.Model
{
    /// <summary>
    /// Holds a collection of EntityStructure objects.
    /// </summary>
    public class EntityStructures
    {
        /// <summary>
        /// The list of entity structures.
        /// </summary>
        [YAXCollection(YAXCollectionSerializationTypes.Serially, SeparateBy = "\n ")]
        [YAXSerializeAs("Structures")]
        public List<EntityStructure> Structures
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a new EntityStructures object.
        /// </summary>
       
        public EntityStructures()
        {
            Structures = new List<EntityStructure>();
        }

        /// <summary>
        /// Adds an <paramref name="entityStructure"/> to the end of this collection, if there is no EntityStructure object
        /// with the same name in the collection.
        /// </summary>
        /// <param name="entityStructure">The EntityStructure object to be added.</param>
        public void Add(EntityStructure entityStructure)
        {
            if (!Structures.Exists(structure => structure.Name == entityStructure.Name))
            {
                Structures.Add(entityStructure);
            }
        }

        /// <summary>
        /// Searches for an EntityStructure identified by <paramref name="entityName"/>.
        /// </summary>
        /// <param name="entityName">The name of the searched EntityStructure.</param>
        /// <returns>Returns the first matchig EntityStructure identified by <paramref name="entityName"/>.</returns>
        public EntityStructure Find(string entityName)
        {
            return Structures.Find(entityStructure => entityStructure.Name == entityName);
        }

        /// <summary>
        /// Searches for an EntityStructure identified by the name of the <paramref name="entity"/>.
        /// </summary>
        /// <param name="entity">The Entity object.</param>
        /// <returns>Returns the first matchig EntityStructure identified by the name of the <paramref name="entity"/>.</returns>
        public EntityStructure Find(Entity entity)
        {
            return Structures.Find(entityStructure => entityStructure.Name == entity.Name);
        }
    }
}
