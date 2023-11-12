using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_GetName_Tests
    {
        [TestMethod]
        public void GetName_FromValueEquals_NameProperty()
        {
            var all = new[] { TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin };
            foreach (var value in all)
            {
                var name = value.Name;
                var nameFromValue = TemperatureUnit.GetName(value.Value);
                nameFromValue.Should().Be(name);
            }
        }

        [TestMethod]
        public void GetName_Existing_Match()
        {
            var fahrenheitName = "Fahrenheit";
            var name = TemperatureUnit.GetName('F');
            name.Should().Be(fahrenheitName);
        }

        [TestMethod]
        public void GetName_Nonexistent_ThrowsInvalidOperationException()
        {
            var act = () => TemperatureUnit.GetName('x');
            act.Should().Throw<InvalidOperationException>();
        }
    }


}