using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TestDataSeeding.Logic;
using YAXLib;
using TestDataSeeding.Model;
using System.Xml.Linq;
using System.IO;

namespace TestDataSeeding.XmlDataAccess
{
    /// <summary>
    /// A basic implementation of the IXmlDataAccess interface.
    /// </summary>
    public class XmlHandler : IXmlDataAccess
    {
        public void SaveEntity(Entity entity, EntityStructure entityStructure, string path)
        {
            try
            {
                var serializer = new YAXSerializer(typeof(Entity));
                var xmlFileName = BuildFileName(entity, entityStructure, path);
                var writer = new XmlTextWriter(xmlFileName, null);
                serializer.Serialize(entity, writer);
                // Console.WriteLine(serializer.Serialize(entity));
                writer.Close();
            }
            catch (UnauthorizedAccessException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues, string path)
        {
            string xmlFileName = BuildFileName(entityStructure, primaryKeyValues, path);
            var deserializer = new YAXSerializer(typeof(Entity), YAXExceptionHandlingPolicies.ThrowErrorsOnly,
                YAXExceptionTypes.Warning);
            object deserializedObject = null;
            XElement xElement = XElement.Load(xmlFileName);

            try
            {
                deserializedObject = deserializer.Deserialize(xElement.ToString());

                if (deserializer.ParsingErrors.ContainsAnyError)
                {
                    Console.WriteLine("Succeeded to deserialize, but these problems also happened:");
                    Console.WriteLine(deserializer.ParsingErrors.ToString());
                }
            }
            catch (YAXException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return (deserializedObject as Entity);
        }

        public EntityStructures GetEntityStructures(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds an XML file name with <paramref name="path"/> and based on the name and key values of the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityStructure">The entity structure.</param>
        /// <param name="path">The path where the file is stored.</param>
        /// <returns>The XML file name with path.</returns>
        private string BuildFileName(Entity entity, EntityStructure entityStructure, string path)
        {
            StringBuilder builder = new StringBuilder(path + "\\" + entity.Name);

            foreach (var keyName in entityStructure.PrimaryKeys)
            {
                builder.Append("_" + entity.AttributeValues[keyName]);
            }

            builder.Append(".xml");

            return builder.ToString();
        }

        /// <summary>
        /// Builds an XML file name with <paramref name="path"/> and based on the name and key values of the entity.
        /// </summary>
        /// <param name="entityStructure">The given EntityStructure.</param>
        /// <param name="primaryKeyValues">The given primary key values.</param>
        /// <param name="path">The path where the file is stored.</param>
        /// <returns>The XML file name with path.</returns>
        private string BuildFileName(EntityStructure entityStructure, List<string> primaryKeyValues, string path)
        {
            StringBuilder builder = new StringBuilder(path + "\\" + entityStructure.Name);

            foreach (var keyValue in primaryKeyValues)
            {
                builder.Append("_" + keyValue);
            }

            builder.Append(".xml");

            return builder.ToString();
        }


        public XmlStatus getXmlStatus()
        {
            throw new NotImplementedException();
        }
    }
}