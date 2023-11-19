using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class Temperature_Serialization_EnumerationClassAsObjectConverter_Tests
    {
        [TestMethod]
        public void JsonSerializer_Serialize_With_EnumerationClassAsObjectConverter_NoNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassAsObjectConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var expected = """
                {
                  "Value": 27.3,
                  "Unit": {
                    "Name": "Celsius",
                    "Value": "C"
                  }
                }
                """;
            serialized.Should().Be(expected);
        }

        [TestMethod]
        public void JsonSerializer_Serialize_With_EnumerationClassAsObjectConverter_CamelCaseNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassAsObjectConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var expected = """
                {
                  "value": 27.3,
                  "unit": {
                    "name": "Celsius",
                    "value": "C"
                  }
                }
                """;
            serialized.Should().Be(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_With_EnumerationClassAsObjectConverter_NoNamingPolicy()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": {
                    "Name": "Celsius",
                    "Value": "C"
                  }
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassAsObjectConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_With_EnumerationClassAsObjectConverter_CamelCaseNamingPolicy()
        {
            var json = """
                {
                  "value": 27.3,
                  "unit": {
                    "name": "Celsius",
                    "value": "C"
                  }
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassAsObjectConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

    }
}


