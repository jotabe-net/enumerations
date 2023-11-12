using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{

    [TestClass]
    public class TemperatureUnit_ParseString_Tests
    {
        [TestMethod]
        public void ParseString_CorrectlyParsed()
        {
            var fahrenheit = "Fahrenheit";
            TemperatureUnit.Parse(fahrenheit)
                .Should().Be(TemperatureUnit.Fahrenheit);
        }

        [TestMethod]
        public void ParseString_WrongCase_Throws()
        {
            var fahrenheit = "FAHRENHEIT";
            var act = () => TemperatureUnit.Parse(fahrenheit);
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void ParseStringCased_IgnoreCase_CorrectlyParsed()
        {
            var fahrenheit = "FAHRENHEIT";
            TemperatureUnit.Parse(fahrenheit, true)
                .Should().Be(TemperatureUnit.Fahrenheit);
        }

        [TestMethod]
        public void ParseStringCased_DontIgnoreCase_CorrectlyParsed()
        {
            var fahrenheit = "Fahrenheit";
            var value = TemperatureUnit.Parse(fahrenheit, false);
            value.Should().Be(TemperatureUnit.Fahrenheit);
        }

        [TestMethod]
        public void ParseStringCased_DontIgnoreCase_Throws()
        {
            var fahrenheit = "FAHRENHEIT";
            var act = () => TemperatureUnit.Parse(fahrenheit, false);
            act.Should().Throw<InvalidOperationException>();
        }
    }

}