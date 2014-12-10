using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataSeeding.Client
{
    /// <summary>
    /// Custom attribute (annotation) for loading an entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TdsLoadEntityAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new attribute with the given parameters identifying an entity.
        /// </summary>
        /// <param name="entityParams">The parameters identifying an entity: name of the table and primary key values.</param>
        public TdsLoadEntityAttribute(params string[] entityParams)
        {
            foreach (var param in entityParams)
            {
                Console.WriteLine(param);
            }
        }
    }
}
