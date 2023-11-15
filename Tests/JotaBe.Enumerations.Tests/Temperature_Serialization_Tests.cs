using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class Temperature_Serialization_Tests
    {
        [TestMethod]
        public void JsonSerializer_Serialize()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
        }

        [TestMethod]
        public void JsonSerializer_SerializeWithConverter()
        {
            var temperature = new Temperature(27.3, TemperatureUnit.Celsius);

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassConverter());
            var serialized = JsonSerializer.Serialize(temperature, serializerOptions);
            Console.WriteLine(serialized);
        }

        [TestMethod]
        public void JsonSerializer_DeserializeWithConverter()
        {
            var json = @"
 {
  ""value"": 27.3,
  ""unit"": {
    ""name"": ""Celsius"",
    ""value"": ""C""
  }
}";

            var serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;
            serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            serializerOptions.Converters.Add(new EnumerationClassConverter());
            var deserialized = JsonSerializer.Deserialize<Temperature>(json, serializerOptions);

            var expected = new Temperature(27.3, TemperatureUnit.Celsius);
            deserialized.Should().BeEquivalentTo(expected);
        }



    }
}


