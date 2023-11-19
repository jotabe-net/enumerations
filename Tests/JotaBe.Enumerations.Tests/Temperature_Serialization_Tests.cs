using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class Temperature_Serialization_Tests
    {
        [TestMethod]
        public void JsonSerializer_Serialize_NoConverter()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
        }

        [TestMethod]
        public void JsonSerializer_Serialize_With_EnumerationClassAsObjectConverter_NoNamingPolicy()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.Converters.Add(new EnumerationClassAsObjectConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
            var serializedName = """
                "Name": "Celsius"
                """;
            var serializedValue = """
                "Value": 27.3
                """;
            serialized.Should().Contain(serializedName, 1.TimesExactly());
            serialized.Should().Contain(serializedValue, 1.TimesExactly());
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
            var serializedName = """
                "name": "Celsius"
                """;
            var serializedValue = """
                "value": 27.3
                """;
            serialized.Should().Contain(serializedName, 1.TimesExactly());
            serialized.Should().Contain(serializedValue, 1.TimesExactly());

        }

        [TestMethod]
        public void JsonSerializer_DeserializeWithoutConverter_Throws()
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

            Action act = () => JsonSerializer.Deserialize<Temperature>(json);
            act.Should().Throw<NotSupportedException>();
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


