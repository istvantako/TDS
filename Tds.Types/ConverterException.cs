using System;

namespace Tds.Types
{
    public class ConverterException : Exception
    {
        public ConverterException(DataType dataType, object value, string message)
            : base(string.Format("Converting value {0} to the TDS type {1} failed: \"{2}\"", 
                    value, dataType, message))
        {}
    }
}
