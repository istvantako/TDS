using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDataSeeding.Model;

namespace TestDataSeeding.DbClient
{
    internal class MsSqlQueryBuilder
    {
        /// <summary>
        /// Creates the select query to retrieve the given entity.
        /// </summary>
        /// <param name="entityStructure">An entity structure.</param>
        /// <param name="primaryKeyValues">An string list containing the primary keys.</param>
        /// <returns>The SQL SELECT query string.</returns>
        public string CreateSelectQuery(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            //SELECT * FROM @Tablename WHERE @KeyAttribute1='@KeyValue1'//
            StringBuilder builder = new StringBuilder("SELECT * FROM ");
            builder.Append(entityStructure.Name)
                   .Append(" WHERE ")
                   .Append(entityStructure.PrimaryKeys[0])
                   .Append("='")
                   .Append(primaryKeyValues[0])
                   .Append("'");
            // AND @KeyAttributeX='@KeyValueX'//
            for (var i = 1; i < entityStructure.PrimaryKeys.Count; i++)
            {
                builder.Append(" AND ")
                       .Append(entityStructure.PrimaryKeys[i])
                       .Append("='")
                       .Append(primaryKeyValues[i])
                       .Append("'");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the select query to retrieve the given entities.
        /// </summary>
        /// <param name="entityName">The name of associative entity name.</param>
        /// <param name="keyValues">The dictionary with the given keys and values.</param>
        /// <returns>The SQL SELECT query string.</returns>
        public string CreateSelectQuery(string entityName, Dictionary<string, string> keyValues)
        {
            //SELECT * FROM @Tablename WHERE @KeyAttribute1='@KeyValue1'//
            StringBuilder builder = new StringBuilder("SELECT * FROM ");
            builder.Append(entityName)
                   .Append(" WHERE ")
                   .Append(keyValues.ElementAt(0).Key)
                   .Append("='")
                   .Append(keyValues.ElementAt(0).Value)
                   .Append("'");
            // AND @KeyAttributeX='@KeyValueX'//
            for (var i = 1; i < keyValues.Count; i++)
            {
                builder.Append(" AND ")
                       .Append(keyValues.ElementAt(i).Key)
                       .Append("='")
                       .Append(keyValues.ElementAt(i).Value)
                       .Append("'");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the update query which updates the given entity.
        /// </summary>
        /// <param name="entity">An entity.</param>
        /// <param name="entityStructure">The structure of the givem entity.</param>
        /// <returns>The SQL UPDATE query string.</returns>
        public string CreateUpdateQuery(Entity entity, EntityStructure entityStructure)
        {
            //we get the names of those attributes which are not PrimaryKeys
            List<string> nonPrimaryKeyAttributes = new List<string>();
            foreach (var item in entity.AttributeValues)
            {
                if (!entityStructure.isPrimaryKey(item.Key))
                {
                    nonPrimaryKeyAttributes.Add(item.Key);
                }
            }

            //UPDATE @Tablename SET @Attribute1='@Value1'//
            StringBuilder builder = new StringBuilder("UPDATE ");
            builder.Append(entity.Name)
                   .Append(" SET ")
                   .Append(nonPrimaryKeyAttributes[0])
                   .Append("='")
                   .Append(entity.AttributeValues[nonPrimaryKeyAttributes[0]])
                   .Append("'");
            //,@AttributeX='@ValueX'//
            for (var i = 1; i < nonPrimaryKeyAttributes.Count; i++)
            {
                builder.Append(", ")
                       .Append(nonPrimaryKeyAttributes[i])
                       .Append("='")
                       .Append(entity.AttributeValues[nonPrimaryKeyAttributes[i]])
                       .Append("'");
            }
            // WHERE @KeyAttribute1='@KeyValue1'//
            builder.Append(" WHERE ")
                   .Append(entityStructure.PrimaryKeys[0])
                   .Append("='")
                   .Append(entity.AttributeValues[entityStructure.PrimaryKeys[0]])
                   .Append("'");
            // AND @KeyAttributeX='@KeyValueX'//
            for (var i = 1; i < entityStructure.PrimaryKeys.Count; i++)
            {
                builder.Append(" AND ")
                       .Append(entityStructure.PrimaryKeys[i])
                       .Append("='")
                       .Append(entity.AttributeValues[entityStructure.PrimaryKeys[i]])
                       .Append("'");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the insert query to insert the given entity.
        /// </summary>
        /// <param name="entity">An entity.</param>
        /// <param name="entityStructure">The structure of the given entity.</param>
        /// <returns>The SQL INSERT query string.</returns>
        public string CreateInsertQuery(Entity entity, EntityStructure entityStructure)
        {
            //INSERT INTO @Tablename VALUES ('@Value1//
            StringBuilder builder = new StringBuilder("INSERT INTO ");
            builder.Append(entity.Name)
                   .Append(" VALUES ('")
                   .Append(entity.AttributeValues.ElementAt(0).Value);
            //','@ValueX//
            for (int i = 1; i < entity.AttributeValues.Count; i++)
            {
                builder.Append("', '")
                       .Append(entity.AttributeValues.ElementAt(i).Value);
            }
            //')//
            builder.Append("')");

            return builder.ToString();
        }
    }
}
