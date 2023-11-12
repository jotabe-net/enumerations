using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_InstanceEquals_Tests
    {
        [TestMethod]
        public void Equals_TrueCase()
        {
            var f1 = TemperatureUnit.Fahrenheit;
            var f2 = TemperatureUnit.Fahrenheit;
            f1.Equals(f2).Should().BeTrue();
        }

        [TestMethod]
        public void Equals_FalseCase()
        {
            var f = TemperatureUnit.Fahrenheit;
            var c = TemperatureUnit.Celsius;
            f.Equals(c).Should().BeFalse();
        }
    }


}