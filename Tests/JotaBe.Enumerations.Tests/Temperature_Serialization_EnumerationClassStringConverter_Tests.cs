using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class Temperature_Serialization_EnumerationClassStringConverter_Tests
    {
        // TODO: negative cases

        [TestMethod]
        public void JsonSerializer_Serialize_With_EnumerationClassStringConverter_NoNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var expected = """
                {
                  "Value": 27.3,
                  "Unit": "Celsius"
                }
                """;
            serialized.Should().Be(expected);
        }

        [TestMethod]
        public void JsonSerializer_Serialize_With_EnumerationClassStringConverter_CamelCaseNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var expected = """
                {
                  "value": 27.3,
                  "unit": "Celsius"
                }
                """;
            serialized.Should().Be(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_With_EnumerationClassStringConverter_NoNamingPolicy()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": "Celsius"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_With_EnumerationClassStringConverter_CamelCaseNamingPolicy()
        {
            var json = """
                {
                  "value": 27.3,
                  "unit": "Celsius"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

    }
}


