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
                var xmlFileName = BuildFileName(entity, entityStructure, path);
                Serialize<Entity>(entity, xmlFileName);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public Entity GetEntity(EntityStructure entityStructure, List<string> primaryKeyValues, string path)
        {
            string xmlFileName = BuildFileName(entityStructure, primaryKeyValues, path);
            Entity deserializedObject = null;
            try
            {
                deserializedObject= Deserialize<Entity>(xmlFileName);
            }
            catch (YAXException exception)
            {
                throw exception;
            }

            return (deserializedObject as Entity);
        }

        private T Deserialize<T>(string xmlFilePath)
        {
            var deserializer = new YAXSerializer(typeof(T), YAXExceptionHandlingPolicies.ThrowErrorsOnly,
               YAXExceptionTypes.Warning);
            object deserializedObject = null;
            XElement xElement = XElement.Load(xmlFilePath);
            deserializedObject = deserializer.Deserialize(xElement.ToString());
            if (deserializer.ParsingErrors.ContainsAnyError)
            {
                Console.WriteLine("Succeeded to deserialize, but these problems also happened:");
                Console.WriteLine(deserializer.ParsingErrors.ToString());
            }
            return ((T) deserializedObject );
        }


        public EntityStructures GetEntityStructures(string path)
        {
            string[] structureFilePaths;
            EntityStructures entityStructures = new EntityStructures();
            try
            {
                structureFilePaths = Directory.GetFiles(path, "*.xml");

                foreach (var filePath in structureFilePaths)
                {
                    try
                    {
                        entityStructures.Add(GetEntityStructure(filePath));
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
            }
            catch (DirectoryNotFoundException exception)
            {
                throw exception;
            }

            return entityStructures;

        }


        private EntityStructure GetEntityStructure(string XmlPath)
        {
            EntityStructure deserializedObject;
            try
            {
                deserializedObject = Deserialize<EntityStructure>(XmlPath);
            }
            catch (YAXException exception)
            {
                throw exception;
            }

            return (deserializedObject );
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

        /// <summary>
        /// ideiglenesen bentvan, hogy lehessen kimenteni strukturat ne kelljen kezzel megirni
        /// </summary>
        /// <param name="entityStructure"></param>
        /// <param name="path"></param>

        public void SaveStructure(EntityStructure entityStructure, string path, string xmlName)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            try
            {
                Serialize<EntityStructure>(entityStructure, path + "\\" + xmlName);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void Serialize<T>(T entity, String xmlFileName)
        {
            var serializer = new YAXSerializer(typeof(T));
            var writer = new XmlTextWriter(xmlFileName, null);
            writer.Formatting = Formatting.Indented;
            serializer.Serialize(entity, writer);
            writer.Close();
        }
    }
}