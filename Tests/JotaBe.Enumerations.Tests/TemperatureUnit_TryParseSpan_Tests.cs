using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{

    [TestClass]
    public class TemperatureUnit_TryParseSpan_Tests
    {
        [TestMethod]
        public void TryParseSpan_CorrectlyParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'e', 'l', 'v', 'i', 'n' });
            var parsed = TemperatureUnit.TryParse(kelvin, out var value);
            parsed.Should().BeTrue();
            value.Should().Be(TemperatureUnit.Kelvin);
        }

        [TestMethod]
        public void TryParseSpan_WrongCase_NotParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'E', 'L', 'V', 'I', 'N' });
            var parsed = TemperatureUnit.TryParse(kelvin, out var _);
            parsed.Should().BeFalse();
        }

        [TestMethod]
        public void TryParseSpanCased_IgnoreCase_CorrectlyParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'E', 'L', 'V', 'I', 'N' });
            var parsed = TemperatureUnit.TryParse(kelvin, true, out var value);
            parsed.Should().BeTrue();
            value.Should().Be(TemperatureUnit.Kelvin);
        }

        [TestMethod]
        public void TryParseSpanCased_DontIgnoreCase_CorrectlyParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'e', 'l', 'v', 'i', 'n' });
            var parsed = TemperatureUnit.TryParse(kelvin, false, out var value);
            parsed.Should().BeTrue();
            value.Should().Be(TemperatureUnit.Kelvin);
        }

        [TestMethod]
        public void TryParseSpanCased_DontIgnoreCase_NotParsed()
        {
            var kelvin = new Span<char>(new[] { 'K', 'E', 'L', 'V', 'I', 'N' });
            var parsed = TemperatureUnit.TryParse(kelvin, false, out var _);
            parsed.Should().BeFalse();
        }
    }

}