using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataSeeding.Model;

namespace TestDataSeeding.Logic
{
    /// <summary>
    /// Logger for the EntityManager.
    /// </summary>
    internal class EntityManagerLogger
    {
        /// <summary>
        /// Action delimiter in the log file.
        /// </summary>
        internal void StartNewLog()
        {
            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["EntityManagerLogPath"], true);
                file.WriteLine("\n\nNEW ACTION STARTED: " + DateTime.Now + "\n");
                file.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Logs the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="structure">The structure of the entity.</param>
        internal void LogEntity(Entity entity, EntityStructure structure)
        {
            StringBuilder stringBuilder = new StringBuilder("[Entity: " + entity.Name + "]\n");

            foreach (var attribute in entity.AttributeValues)
            {
                if (structure.IsPrimaryKey(attribute.Key))
                {
                    stringBuilder.Append("  - [PK] " + attribute.Key + " => " + attribute.Value + "\n");
                }
                else if (structure.IsForeignKey(attribute.Key))
                {
                    stringBuilder.Append("  - " + attribute.Key + " => " + attribute.Value + " [FK references " + structure.ForeignKeys[attribute.Key].EntityName + "(" +
                        structure.ForeignKeys[attribute.Key].KeyName + ")" + "]\n");
                }
                else
                {
                    stringBuilder.Append("  - " + attribute.Key + " => " + attribute.Value + "\n");
                }
            }

            string info = stringBuilder.ToString();

            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["EntityManagerLogPath"], true);
                file.WriteLine(info);
                file.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="info">The message.</param>
        internal void LogInfo(string info)
        {
            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["EntityManagerLogPath"], true);
                file.WriteLine(info);
                file.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Logs an already visited entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="keyValues">The keys of the entity.</param>
        internal void LogIsVisited(string entity, List<string> keyValues)
        {
            StringBuilder builder = new StringBuilder("Already visited: " + entity + " [");
            foreach (var key in keyValues)
            {
                builder.Append(key + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append("]\n");

            string info = builder.ToString();

            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["EntityManagerLogPath"], true);
                file.WriteLine(info);
                file.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Logs an entity in the visited phase.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="keyValues">The keys of the entity.</param>
        internal void LogVisited(string entity, List<string> keyValues)
        {
            StringBuilder builder = new StringBuilder("Is now visited: " + entity + " [");
            foreach (var key in keyValues)
            {
                builder.Append(key + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append("]\n");

            string info = builder.ToString();

            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["EntityManagerLogPath"], true);
                file.WriteLine(info);
                file.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Logs the dependencies.
        /// </summary>
        /// <param name="dependencies">The dependencies.</param>
        internal void LogDependencies(Dictionary<string, List<string>> dependencies)
        {
            StringBuilder builder = new StringBuilder("Dependencies:\n");

            foreach (var keyValue in dependencies)
            {
                builder.Append("  - " + keyValue.Key + " [");
                foreach (var keys in keyValue.Value)
                {
                    builder.Append(keys + ", ");
                }
                builder.Remove(builder.Length - 2, 2);
                builder.Append("]\n");
            }

            string info = builder.Append("\n").ToString();

            try
            {
                StreamWriter file = new StreamWriter(ConfigurationManager.AppSettings["EntityManagerLogPath"], true);
                file.WriteLine(info);
                file.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
