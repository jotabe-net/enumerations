using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_FromName_Tests
    {
        [TestMethod]
        public void FromName_GetRightElement()
        {
            var value = TemperatureUnit.FromName("Celsius");
            value.Should().Be(TemperatureUnit.Celsius);
        }

        [TestMethod]
        public void FromName_Nonexistent_ThrowsInvalidOperationException()
        {
            var act = () => TemperatureUnit.FromName("non-existent");
            act.Should().Throw<InvalidOperationException>();
        }
    }


}