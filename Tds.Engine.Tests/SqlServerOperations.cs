using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tds.Interfaces;
using Tds.Interfaces.Model;
using Tds.Types;

namespace Tds.Engine.Tests
{
    class SqlServerOperations
    {
        public static bool CheckEntityExistsInDatabase(string connectionString, IMetadataWorkspace metadataWorkspace, Entity entity)
        {
            var properties = new string[entity.Properties.Count];
            var entityType = metadataWorkspace.GetEntityType(entity.Name);

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = string.Format("{0} = {1}", property.Key, Converter.ConvertToString(entityType.Properties[property.Key], property.Value));
                index++;
            }

            var query = string.Format("SELECT COUNT({0}) FROM {1} WHERE {2};",
                "*",
                entity.Name,
                string.Join(" AND ", properties));

            var entityExists = true;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    int count = int.Parse(command.ExecuteScalar().ToString());
                    if (count != 1)
                    {
                        entityExists = false;
                    }
                }
            }

            return entityExists;
        }

        public static bool InsertEntityInDatabase(string connectionString, IMetadataWorkspace metadataWorkspace, Entity entity)
        {
            var properties = new string[entity.Properties.Count];
            var header = new string[entity.Properties.Count];
            var entityType = metadataWorkspace.GetEntityType(entity.Name);

            int index = 0;
            foreach (var property in entity.Properties)
            {
                properties[index] = Converter.ConvertToString(entityType.Properties[property.Key], property.Value);
                header[index] = property.Key;
                index++;
            }

            var query = string.Format("INSERT INTO {0} ({1}) VALUES ({2});",
                entity.Name,
                string.Join(", ", header),
                string.Join(", ", properties));

            var entityExists = false;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (var command = new SqlCommand(query, connection))
                {
                    if (!command.ExecuteReader().HasRows)
                    {
                            entityExists = true;
                    }
                }
            }

            return entityExists;
        }

        public static void TruncateDatabase(string connectionString, IEnumerable<string> entities)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var entity in entities)
                {
                    var query = string.Format("TRUNCATE TABLE {0};", entity);
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
