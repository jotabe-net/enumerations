using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{

    [TestClass]
    public class TemperatureUnit_ParseSpan_Tests
    {
        [TestMethod]
        public void ParseSpan_CorrectlyParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'e', 'l', 'v', 'i', 'n' });
            var value = TemperatureUnit.Parse(kelvin);
            value.Should().Be(TemperatureUnit.Kelvin);
        }

        [TestMethod]
        public void ParseSpan_WrongCase_Throws()
        {
            var act = () =>
            {
                var kelvin = new Span<char>(new[] { 'K', 'E', 'L', 'V', 'I', 'N' });
                TemperatureUnit.Parse(kelvin);
            };
            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void ParseSpanCased_IgnoreCase_CorrectlyParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'E', 'L', 'V', 'I', 'N' });
            var value = TemperatureUnit.Parse(kelvin, true);
            value.Should().Be(TemperatureUnit.Kelvin);
        }

        [TestMethod]
        public void ParseSpanCased_DontIgnoreCase_CorrectlyParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'e', 'l', 'v', 'i', 'n' });
            var value = TemperatureUnit.Parse(kelvin, false);
            value.Should().Be(TemperatureUnit.Kelvin);
        }

        [TestMethod]
        public void ParseSpanCased_DontIgnoreCase_Throws()
        {
            var act = () =>
            {
                var kelvin = new Span<char>(new[] { 'K', 'E', 'L', 'V', 'I', 'N' });
                TemperatureUnit.Parse(kelvin, false);
            };
            act.Should().Throw<InvalidOperationException>();
        }
    }

}