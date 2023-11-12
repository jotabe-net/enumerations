using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JotaBe.Enumerations.Tests
{
    [TestClass]
    public class TemperatureUnit_IsDefined_Tests
    {
        [TestMethod]
        public void IsDefined_TrueCase()
        {
            TemperatureUnit.IsDefined('K')
                .Should().BeTrue();
        }

        [TestMethod]
        public void IsDefined_FalseCase()
        {
            TemperatureUnit.IsDefined('X')
                .Should().BeFalse();
        }
    }


}