using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_StaticEquals_Tests
    {
        [TestMethod]
        public void StaticEquals_TrueCase()
        {
            var f1 = TemperatureUnit.Fahrenheit;
            var f2 = TemperatureUnit.Fahrenheit;
            Equals(f1, f2).Should().BeTrue();
        }

        [TestMethod]
        public void StaticEquals_FalseCase()
        {
            var f = TemperatureUnit.Fahrenheit;
            var c = TemperatureUnit.Celsius;
            f.Equals(c).Should().BeFalse();
        }
    }


}