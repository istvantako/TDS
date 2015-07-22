using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tds.Types.Tests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void ConvertFromString_Integer_ReturnInt()
        {
            // Arrange
            var expected = 10;
            var text = expected.ToString(CultureInfo.InvariantCulture);

            // Act
            var actual = Converter.ConvertFromString(DataType.Integer, text);

            // Assert
            Assert.AreEqual(expected, actual, "Integer wasn't converted correctly");
        }

        [TestMethod]
        public void ConvertFromString_Decimal_ReturnInt()
        {
            // Arrange
            var expected = 10M;
            var text = expected.ToString(CultureInfo.InvariantCulture);

            // Act
            var actual = Converter.ConvertFromString(DataType.Decimal, text);

            // Assert
            Assert.AreEqual(expected, actual, "Decimal wasn't converted correctly");
        }

        [TestMethod]
        public void ConvertFromString_String_ReturnOriginal()
        {
            // Arrange
            var expected = "test";

            // Act
            var actual = Converter.ConvertFromString(DataType.String, expected);

            // Assert
            Assert.AreEqual(expected, actual, "String wasn't converted correctly");
        }

        [ExpectedException(typeof(ConverterException))]
        [TestMethod]
        public void ConvertFromString_Undefined_ThrowsException()
        {
            // Arrange
            var text = "test";

            // Act
            var actual = Converter.ConvertFromString(DataType.Undefined, text);

            // Assert
        }

        [TestMethod]
        public void ConvertToString_Integer_ConvertString()
        {
            // Arrange
            var i = 10;
            var expected = i.ToString(CultureInfo.InvariantCulture);

            // Act
            var actual = Converter.ConvertToString(DataType.Integer, i);

            // Assert
            Assert.AreEqual(expected, actual, "Integer not converted correctly into string.");
        }

        [TestMethod]
        public void ConvertToString_Decimal_ConvertString()
        {
            // Arrange
            var d = 10M;
            var expected = d.ToString(CultureInfo.InvariantCulture);

            // Act
            var actual = Converter.ConvertToString(DataType.Decimal, d);

            // Assert
            Assert.AreEqual(expected, actual, "Decimal not converted correctly into string.");
        }

        [TestMethod]
        public void ConvertToString_String_ConvertString()
        {
            // Arrange
            var expected = "test";

            // Act
            var actual = Converter.ConvertToString(DataType.String, expected);

            // Assert
            Assert.AreEqual(expected, actual, "String not converted correctly into string.");
        }
    }
}
