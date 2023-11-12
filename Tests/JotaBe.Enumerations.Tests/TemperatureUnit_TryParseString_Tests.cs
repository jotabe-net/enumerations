using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{

    [TestClass]
    public class TemperatureUnit_TryParseString_Tests
    {
        [TestMethod]
        public void TryParseString_CorrectlyParsed()
        {
            var fahrenheit = "Fahrenheit";
            var parsed = TemperatureUnit.TryParse(fahrenheit, out var value);
            parsed.Should().BeTrue();
            value.Should().Be(TemperatureUnit.Fahrenheit);
        }

        [TestMethod]
        public void TryParseString_WrongCase_NotParsed()
        {
            var fahrenheit = "FAHRENHEIT";
            var parsed = TemperatureUnit.TryParse(fahrenheit, out var _);
            parsed.Should().BeFalse();
        }

        [TestMethod]
        public void TryParseStringCased_IgnoreCase_CorrectlyParsed()
        {
            var fahrenheit = "FAHRENHEIT";
            var parsed = TemperatureUnit.TryParse(fahrenheit, true, out var value);
            parsed.Should().BeTrue();
            value.Should().Be(TemperatureUnit.Fahrenheit);
        }

        [TestMethod]
        public void TryParseStringCased_DontIgnoreCase_CorrectlyParsed()
        {
            var fahrenheit = "Fahrenheit";
            var parsed = TemperatureUnit.TryParse(fahrenheit, false, out var value);
            parsed.Should().BeTrue();
            value.Should().Be(TemperatureUnit.Fahrenheit);
        }

        [TestMethod]
        public void ParseStringCased_DontIgnoreCase_NotParsed()
        {
            var fahrenheit = "FAHRENHEIT";
            var parsed = TemperatureUnit.TryParse(fahrenheit, false, out var value);
            parsed.Should().BeFalse();
        }
    }

}