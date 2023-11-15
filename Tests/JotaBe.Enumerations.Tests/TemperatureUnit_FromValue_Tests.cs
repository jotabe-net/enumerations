using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_FromValue_Tests
    {
        [TestMethod]
        public void FromValue_GetRightElement()
        {
            var value = TemperatureUnit.FromValue('C');
            value.Should().Be(TemperatureUnit.Celsius);
        }

        [TestMethod]
        public void FromValue_Nonexistent_ThrowsInvalidOperationException()
        {
            var act = () => TemperatureUnit.FromValue('X');
            act.Should().Throw<InvalidOperationException>();
        }
    }


}