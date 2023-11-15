using FluentAssertions;

namespace JotaBe.TypeExtensions.Tests
{
    [TestClass]
    public class GetGenericBaseTypeArgumentsTests
    {
        // Derived from unspecified types generic

        [TestMethod]
        public void GetGenericBaseTypeArguments_FromGenericIntString()
        {
            var types = typeof(DerivedFromGenericIntString)
                .GetGenericBaseTypeArguments(typeof(GenericBase<,>));
            types.Should().BeEquivalentTo(new[] { typeof(int), typeof(string) });
        }

        [TestMethod]
        public void GetGenericBaseTypeArguments_SpecifiedTypes_FromGenericIntString()
        {
            var types = typeof(DerivedFromGenericIntString)
                .GetGenericBaseTypeArguments(typeof(GenericBase<int,string>));
            types.Should().BeEquivalentTo(new[] { typeof(int), typeof(string) });
        }

        [TestMethod]
        public void GetGenericBaseTypeArguments_SpecifiedIntChar_FromGenericIntString()
        {
            var types = typeof(DerivedFromGenericIntString)
                .GetGenericBaseTypeArguments(typeof(GenericBase<int, char>));
            types.Should().BeNull();
        }

        [TestMethod]
        public void GeObjectTypeArguments_FromGenericIntString()
        {
            var types = typeof(object)
                .GetGenericBaseTypeArguments(typeof(GenericBase<,>));
            types.Should().BeNull();
        }

        [TestMethod]
        public void GetGenericBaseTypeArguments_FromNonGenericBase()
        {
            var types = typeof(DerivedFromGenericIntString)
                .GetGenericBaseTypeArguments(typeof(object));
            types.Should().BeNull();
        }


        // TODO: base no generic, specified base with params

    }

}