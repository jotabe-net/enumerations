using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_GetNames_Values_UnderlyingType_Tests
    {
        [TestMethod]
        public void GetNames_GetAllNames()
        {
            var allnames = new[] { "Celsius", "Fahrenheit", "Kelvin" };
            var names = TemperatureUnit.GetNames();
            names.Should().BeEquivalentTo(allnames);
        }

        [TestMethod]
        public void GetValues_GetsAll()
        {
            var allValues = new[]
            {
                TemperatureUnit.Celsius,
                TemperatureUnit.Fahrenheit,
                TemperatureUnit.Kelvin,
            };
            var values = TemperatureUnit.GetValues();
            values.Should().BeEquivalentTo(allValues);
        }

        [TestMethod]
        public void GetUnderlyingType_GetRightType()
        {
            var type = TemperatureUnit.GetUnderlyingType();
            type.Should().Be(typeof(char));
        }

        [TestMethod]
        public void GetValuesAsUnderlyingType_ReturnCorrectValues()
        {
            var values = new[] { 'C', 'F', 'K' };
            TemperatureUnit.GetValuesAsUnderlyingType();
        }
    }


}