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
        internal string CreateSelectQuery(EntityStructure entityStructure, List<string> primaryKeyValues)
        {
            //SELECT @Attribute1, @AttributeN//
            StringBuilder builder = new StringBuilder("SELECT ");

            string separator = "";
            for (var i = 0; i < entityStructure.Attributes.Count; i++)
            {
                string attribute = entityStructure.Attributes.ElementAt(i).Key;
                string type = entityStructure.Attributes.ElementAt(i).Value;

                builder.Append(separator)
                       .Append(ConvertAttribute(attribute,type));
                separator = ", ";
            }

            // FROM @TableName WHERE @KeyAttribute1 = @KeyValue1 AND @KeyAttributeN = @KeyValueN//
            builder.Append(" FROM ")
                   .Append(entityStructure.Name)
                   .Append(" WHERE ");

            separator = "";
            for (var i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                string attribute = entityStructure.PrimaryKeys[i];
                string type = entityStructure.Attributes[entityStructure.PrimaryKeys[i]];

                builder.Append(separator)
                       .Append('"').Append(attribute).Append('"').Append(" = ")
                       .Append(ConvertValue(primaryKeyValues[i], type));
                separator = " AND ";
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the select query to retrieve the given entities.
        /// </summary>
        /// <param name="entityStructure">An entity structure.</param>
        /// <param name="keyValues">The dictionary with the given keys and values.</param>
        /// <returns>The SQL SELECT query string.</returns>
        internal string CreateSelectQuery(EntityStructure entityStructure, Dictionary<string, string> keyValues)
        {
            //SELECT @Attribute1, @AttributeN//
            StringBuilder builder = new StringBuilder("SELECT ");

            string separator = "";
            for (var i = 0; i < entityStructure.Attributes.Count; i++)
            {
                string attribute = entityStructure.Attributes.ElementAt(i).Key;
                string type = entityStructure.Attributes.ElementAt(i).Value;

                builder.Append(separator)
                       .Append(ConvertAttribute(attribute, type));
                separator = ", ";
            }
            // FROM @TableName WHERE @KeyAttribute1 = @KeyValue1 AND @KeyAttributeN = @KeyValueN//
            builder.Append(" FROM ")
                   .Append(entityStructure.Name)
                   .Append(" WHERE ");

            separator = "";
            for (var i = 0; i < keyValues.Count; i++)
            {
                string attribute = keyValues.ElementAt(i).Key;
                string type = entityStructure.Attributes[keyValues.ElementAt(i).Key];

                builder.Append(separator)
                       .Append('"').Append(attribute).Append('"').Append(" = ")
                       .Append(ConvertValue(keyValues.ElementAt(i).Value, type));
                separator = " AND ";
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the update query which updates the given entity.
        /// </summary>
        /// <param name="entity">An entity.</param>
        /// <param name="entityStructure">The structure of the given entity.</param>
        /// <returns>The SQL UPDATE query string.</returns>
        internal string CreateUpdateQuery(Entity entity, EntityStructure entityStructure)
        {
            //we get the names of those attributes which are not PrimaryKeys
            var nonPrimaryKeyAttributes = entityStructure.Attributes.Where(attribute => !entityStructure.IsPrimaryKey(attribute.Key));

            //UPDATE @Tablename SET @Attribute1 = @Value1 , @AttributeN = @ValueN//
            StringBuilder builder = new StringBuilder("UPDATE ");
            builder.Append(entity.Name)
                   .Append(" SET ");

            string separator = "";
            foreach (var item in nonPrimaryKeyAttributes)
            {
                string value = entity.AttributeValues[item.Key];
                string type = item.Value;

                builder.Append(separator)
                       .Append('"')
                       .Append(item.Key)
                       .Append('"')
                       .Append(" = ")
                       .Append(ConvertValue(value,type));
                separator = ", ";
            }
            // WHERE @KeyAttribute1 = @KeyValue1 AND @KeyAttributeN = @KeyValueN//
            builder.Append(" WHERE ");

            separator = "";
            for (var i = 0; i < entityStructure.PrimaryKeys.Count; i++)
            {
                string attribute = entityStructure.PrimaryKeys[i];
                string value = entity.AttributeValues[entityStructure.PrimaryKeys[i]];
                string type = entityStructure.Attributes[entityStructure.PrimaryKeys[i]];
                
                builder.Append(separator)
                       .Append('"').Append(attribute).Append('"').Append(" = ")
                       .Append(ConvertValue(value,type));

                separator = " AND ";
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the insert query to insert the given entity.
        /// </summary>
        /// <param name="entity">An entity.</param>
        /// <param name="entityStructure">The structure of the given entity.</param>
        /// <returns>The SQL INSERT query string.</returns>
        internal string CreateInsertQuery(Entity entity, EntityStructure entityStructure)
        {
            //INSERT INTO @Tablename VALUES (@Value1, @ValueN)//
            StringBuilder builder = new StringBuilder("INSERT INTO ");
            builder.Append(entity.Name)
                   .Append(" VALUES (");
            
            string separator = "";
            for (int i = 0; i < entity.AttributeValues.Count; i++)
            {
                string value = entity.AttributeValues.ElementAt(i).Value;
                string type = entityStructure.Attributes[entity.AttributeValues.ElementAt(i).Key];

                builder.Append(separator)
                       .Append(ConvertValue(value,type));

                separator=", ";
            }
            
            builder.Append(")");

            return builder.ToString();
        }

        /// <summary>
        /// Prepares the <paramref name="attribute"/> for a SELECT command.
        /// </summary>
        /// <param name="attribute">The attribute to be handled.</param>
        /// <param name="type">Type of the <paramref name="attribute"/>.</param>
        /// <returns>The attribte string for the SELECT <paramref name="attribute"/> command. </returns>
        private string ConvertAttribute(string attribute, string type)
        {
            StringBuilder builder = new StringBuilder();

            string[] datetimes = { "date", "time", "datetime", "datetime2", "smalldatetime", "datetimeoffset" };
            string[] binaries = { "binary", "varbinary" };
            string image = "image";
            string timestamp = "timestamp";

            //CONVERT(VARCHAR(34), "@DatetimeAttribute", 126) AS "@DatetimeAttribute"
            if (datetimes.Contains(type))
            {
                builder.Append("CONVERT(VARCHAR(34), ").Append('"').Append(attribute).Append('"')
                       .Append(", 126) AS ").Append('"').Append(attribute).Append('"');
                return builder.ToString();
            }
            //CONVERT(VARCHAR(MAX), "@BinaryAttribute", 1) AS "@BinaryAttribute"
            else if (binaries.Contains(type))
            {
                builder.Append("CONVERT(VARCHAR(MAX), ").Append('"').Append(attribute).Append('"')
                       .Append(", 1) AS ").Append('"').Append(attribute).Append('"');
                return builder.ToString();
            }
            //CONVERT(VARCHAR(MAX), CONVERT(VARBINARY(MAX), "@ImageAttribute", 1), 1) AS "@ImageAttribute"
            else if (type == image)
            {
                builder.Append("CONVERT(VARCHAR(MAX), CONVERT(VARBINARY(MAX), ")
                       .Append('"').Append(attribute).Append('"').Append(", 1), 1) AS ")
                       .Append('"').Append(attribute).Append('"');
                return builder.ToString();
            }
            //'null' AS "@TimestampAttribute"
            else if (type == timestamp)
            {
                builder.Append("'null' AS ").Append('"').Append(attribute).Append('"');
                return builder.ToString();
            }
            //"@OtherAttribute"
            else
            {
                builder.Append('"').Append(attribute).Append('"');
                return builder.ToString();
            }
        }

        /// <summary>
        /// Prepares the <paramref name="value"/> for an insert command or where statement.
        /// </summary>
        /// <param name="value">The value to be handled.</param>
        /// <param name="type">Type of the <paramref name="value"/>.</param>
        /// <returns>The value string for the Insert <paramref name="value"/> command or where statement.</returns>
        private string ConvertValue(string value, string type)
        {
            if (value == "") return "null";

            string[] strings = { "char", "varchar", "text", "sql_variant", "bit", "xml",
                                 "geography", "geometry", "hierarchyid", "uniqueidentifier" };
            string[] nstrings = { "nchar", "nvarchar", "ntext" };
            string[] datetimes = { "date", "time", "datetime", "datetime2", "smalldatetime", "datetimeoffset" };

            StringBuilder builder = new StringBuilder();

            //'stringValue'// -also escapes the sql quote signs(')
            if (strings.Contains(type))
            {
                builder.Append("'")
                   .Append(value.Replace("'", "''"))
                   .Append("'");
                return builder.ToString();
            }
            //N'stringValue'// -also escapes the sql quote signs(')
            else if (nstrings.Contains(type))
            {
                builder.Append("N'")
                   .Append(value.Replace("'", "''"))
                   .Append("'");
                return builder.ToString();
            }
            //'datetimeValue'//
            else if (datetimes.Contains(type))
            {
                builder.Append("'")
                   .Append(value)
                   .Append("'");
                return builder.ToString();
            }
            //numericValue//
            else
            {
                return value;
            }
        }
    }
}
