using FluentAssertions;
using JotaBe.Enumerations.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class Temperature_Serialization_EnumerationClassValueConverter_Tests
    {
        [TestMethod]
        public void JsonSerializer_Serialize_NoNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var expected = """
                {
                  "Value": 27.3,
                  "Unit": "C"
                }
                """;
            serialized.Should().Be(expected);
        }

        [TestMethod]
        public void JsonSerializer_Serialize_CamelCaseNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var expected = """
                {
                  "value": 27.3,
                  "unit": "C"
                }
                """;
            serialized.Should().Be(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": "C"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy_UnknownUnitValue()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": "X"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>().WithMessage("Can't convert 'X' to an enumeration class value");
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy_WrongType()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": 22
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>();
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy_Null()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": null
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>().WithMessage("The property must have a value, but is null");
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_CamelCaseNamingPolicy()
        {
            var json = """
                {
                  "value": 27.3,
                  "unit": "C"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassValueConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

    }
}


