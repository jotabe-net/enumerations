namespace JotaBe.Enumerations.Tests
{
    public static class TypeExtensions
    {
        // TODO: this could cache the info to avoid repeating on each call

        public static bool IsDerivedFrom(this Type derived, Type baseType)
        {
            // This is true if baseType is like Dictionary<,>, not like Dictionary<string,int>
            var baseIsGenericWithoutParmas = baseType.IsGenericType && baseType.ContainsGenericParameters;

            // Checks if the derived class, or its parent, grandparent... is the base type
            var toCheck = derived;
            while (toCheck != null)
            {
                if (baseIsGenericWithoutParmas && toCheck.IsGenericType)
                {
                    var toCompare = toCheck.GetGenericTypeDefinition();
                    if (toCompare == baseType)
                    {
                        return true;
                    }
                }
                else
                {
                    if (toCheck == baseType)
                    {
                        return true;
                    }
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static Type[]? GetGenericBaseTypeArguments(this Type derived, Type baseType)
        {
            if (!baseType.IsGenericType)
            {
                return null;
            }

            // This is true if baseType is like Dictionary<,>, not like Dictionary<string,int>
            var baseIsGenericWithoutParmas = baseType.IsGenericType && baseType.ContainsGenericParameters;

            // Checks if the derived class, or its parent, grandparent... is the base type
            var toCheck = derived;
            while (toCheck != null && toCheck != typeof(object))
            {
                if (baseIsGenericWithoutParmas && toCheck.IsGenericType)
                {
                    var toCompare = toCheck.GetGenericTypeDefinition();
                    if (toCompare == baseType)
                    {
                        return toCheck.GenericTypeArguments;
                    }
                }
                else
                {
                    if (toCheck == baseType)
                    {
                        return toCheck.GenericTypeArguments;
                    }
                }
                toCheck = toCheck.BaseType;
            }
            return null;
        }
    }
}


