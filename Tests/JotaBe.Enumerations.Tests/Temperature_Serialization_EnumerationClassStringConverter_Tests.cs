using FluentAssertions;
using JotaBe.Enumerations.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class Temperature_Serialization_EnumerationClassStringConverter_Tests
    {
        [TestMethod]
        public void JsonSerializer_Serialize_NoNamingPolicy()
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
        public void JsonSerializer_Serialize_CamelCaseNamingPolicy()
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
        public void JsonSerializer_Deserialize_NoNamingPolicy()
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
        public void JsonSerializer_Deserialize_NoNamingPolicy_UnknownUnit()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": "UnknownUnit"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>().WithMessage("Can't convert 'UnknownUnit' to a value of JotaBe.Enumerations.Tests.TemperatureUnit");
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy_WrongType()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": true
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>().WithMessage("Expected an string value, but is *");
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy_Empty()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": ""
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>().WithMessage("The property must have a string, but is empty or white space");
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_NoNamingPolicy_Whitespace()
        {
            var json = """
                {
                  "Value": 27.3,
                  "Unit": "  "
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var act = () => JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            act.Should().Throw<JsonException>().WithMessage("The property must have a string, but is empty or white space");
        }

        // TODO: ideally, this should throw !! a null unit is not allowed
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
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);
            var expected = new Temperature(27.3, null);
            deserialized.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void JsonSerializer_Deserialize_CamelCaseNamingPolicy()
        {
            var json = """
                {
                  "value": 27.3,
                  "unit": "Celsius"
                }
                """;

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassStringConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }

    }
}


