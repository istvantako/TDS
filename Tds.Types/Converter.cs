using System;
using System.Globalization;

namespace Tds.Types
{
    public static class Converter
    {
        /// <summary>
        /// Converts a string to the desired type
        /// </summary>
        /// <param name="dataType">Data type</param>
        /// <param name="value">String value</param>
        /// <returns></returns>
        public static object ConvertFromString(DataType dataType, string value)
        {
            switch (dataType)
            {
                case DataType.Integer:
                    return int.Parse(value, CultureInfo.InvariantCulture);
                case DataType.Decimal:
                    return Decimal.Parse(value, CultureInfo.InvariantCulture);
                case DataType.String:
                    return value;
                default:
                    throw new ConverterException(dataType, value, "Unsupported data type.");
            }

            return null;
        }

        /// <summary>
        /// Converts objects of specific type to string.
        /// </summary>
        /// <param name="dataType">Data type of object</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static string ConvertToString(DataType dataType, object value)
        {
            switch (dataType)
            {
                case DataType.Integer:
                    return Convert.ToInt32(value).ToString(CultureInfo.InvariantCulture);
                case DataType.Decimal:
                    return Convert.ToDecimal(value).ToString(CultureInfo.InvariantCulture);
                case DataType.String:
                    return string.Format("'{0}'", value.ToString());
                default:
                    throw new ConverterException(dataType, value, "Unsupported data type.");
            }
        }
    }
}
