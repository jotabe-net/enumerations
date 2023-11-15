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
            typeof(DerivedFromGeneric)
                .IsDerivedFrom(typeof(GenericBase<,>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckSecondLevelDerived_FromGenericUnspecifiedTypes()
        {
            typeof(DerivedFromDerivedFromGeneric)
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
        public void CheckDirectlyDerived_FromGenericSspecifiedTypes()
        {
            typeof(DerivedFromGeneric)
                .IsDerivedFrom(typeof(GenericBase<int,string>)).Should().BeTrue();
        }

        [TestMethod]
        public void CheckSecondLevelDerived_FromGenericSpecifiedTypes()
        {
            typeof(DerivedFromDerivedFromGeneric)
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
            typeof(DerivedFromGeneric)
                .IsDerivedFrom(typeof(GenericBase<int, char>)).Should().BeFalse();
        }

        [TestMethod]
        public void CheckSecondLevelDerived_FromGenericSpecified_DifferentTypes()
        {
            typeof(DerivedFromDerivedFromGeneric)
                .IsDerivedFrom(typeof(GenericBase<int, char>)).Should().BeFalse();
        }

    }

    public class GenericBase<TKey, TValue>
        where TKey : notnull
    {
        public GenericBase()
        {
            TheDictionary = new Dictionary<TKey, TValue>();
        }

        public Dictionary<TKey, TValue> TheDictionary { get; set; }
    }

    public class DerivedFromGeneric : GenericBase<int, string>
    {

    }

    public class DerivedFromDerivedFromGeneric : DerivedFromGeneric
    {

    }


}