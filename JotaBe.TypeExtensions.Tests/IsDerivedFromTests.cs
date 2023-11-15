using FluentAssertions;

namespace JotaBe.TypeExtensions.Tests
{
    [TestClass]
    public class IsDerivedFromTests
    {
        // Derived from unspecified types generic

        [TestMethod]
        public void CheckDirectlyDerived_FromGenericUnspecifiedTypes()
        {
            typeof(DerivedFromGenericIntString)
                .IsDerivedFrom(typeof(GenericBase<,>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckSecondLevelDerived_FromGenericUnspecifiedTypes()
        {
            typeof(DerivedFromDerivedFromGenericIntString)
                .IsDerivedFrom(typeof(GenericBase<,>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckObject_FromGenericUnspecifiedTypes()
        {
            typeof(object)
                .IsDerivedFrom(typeof(GenericBase<,>)).Should().BeFalse();
        }

        [TestMethod]
        public void CheckSameType_FromGenericUnspecifiedTypes()
        {
            typeof(GenericBase<,>)
                .IsDerivedFrom(typeof(GenericBase<,>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckSameTypeWithParams_FromGenericUnspecifiedTypes()
        {
            typeof(GenericBase<int, string>)
                .IsDerivedFrom(typeof(GenericBase<,>)).Should().BeTrue();
        }

        // Derived from specified types generic

        [TestMethod]
        public void CheckDirectlyDerived_FromGenericSpecifiedTypes()
        {
            typeof(DerivedFromGenericIntString)
                .IsDerivedFrom(typeof(GenericBase<int,string>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckSecondLevelDerived_FromGenericSpecifiedTypes()
        {
            typeof(DerivedFromDerivedFromGenericIntString)
                .IsDerivedFrom(typeof(GenericBase<int,string>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckObject_FromGenericSpecifiedTypes()
        {
            typeof(object)
                .IsDerivedFrom(typeof(GenericBase<int, string>)).Should().BeFalse();
        }

        [TestMethod]
        public void CheckSameType_FromGenericSspecifiedTypes()
        {
            typeof(GenericBase<int, string>)
                .IsDerivedFrom(typeof(GenericBase<int, string>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckDirectlyDerived_FromGenericSpecified_DifferentTypes()
        {
            typeof(DerivedFromGenericIntString)
                .IsDerivedFrom(typeof(GenericBase<int, char>)).Should().BeFalse();
        }

        [TestMethod]
        public void CheckSecondLevelDerived_FromGenericSpecified_DifferentTypes()
        {
            typeof(DerivedFromDerivedFromGenericIntString)
                .IsDerivedFrom(typeof(GenericBase<int, char>)).Should().BeFalse();
        }

    }

}