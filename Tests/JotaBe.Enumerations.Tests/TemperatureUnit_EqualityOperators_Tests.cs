using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_EqualityOperators_Tests
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

        [TestMethod]
        public void EqualOperator_TrueCase()
        {
            var f1 = TemperatureUnit.Fahrenheit;
            var f2 = TemperatureUnit.Fahrenheit;
            (f1 == f2).Should().BeTrue();
        }

        [TestMethod]
        public void EqualOperator_FalseCase()
        {
            var f = TemperatureUnit.Fahrenheit;
            var c = TemperatureUnit.Celsius;
            (f == c).Should().BeFalse();
        }

        [TestMethod]
        public void NotEqualOperator_FalseCase()
        {
            var f1 = TemperatureUnit.Fahrenheit;
            var f2 = TemperatureUnit.Fahrenheit;
            (f1 != f2).Should().BeFalse();
        }

        [TestMethod]
        public void NotEqualOperator_TrueCase()
        {
            var f = TemperatureUnit.Fahrenheit;
            var c = TemperatureUnit.Celsius;
            (f != c).Should().BeTrue();
        }
    }


}