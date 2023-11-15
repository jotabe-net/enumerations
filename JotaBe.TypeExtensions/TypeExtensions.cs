using System;

namespace JotaBe.TypeExtensions
{
    public static class TypeExtensions
    {
        // TODO: this could cache the info to avoid repeating on each call

        /// <summary>
        /// Checks if the type is derived from the <paramref name="baseType"/>
        /// which can be a non generic type, or a generic type with or without
        /// specified parameter types.
        /// </summary>
        /// <param name="derived">Type to check if is derived from a base type</param>
        /// <param name="baseType">Type to check if is the base for the specified type
        /// which can be non generic or generic with or without parameter types,like
        /// <code>MyClass</code><code>MyClass{T,U}</code><code>MyClass{,}</code></param>
        /// <returns></returns>
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

        /// <summary>
        /// Verifies if the current class is derived from the specified generic class,
        /// and returns the generic type arguments of that base class.
        /// </summary>
        /// <param name="derived"></param>
        /// <param name="baseType">A generic base class, without specified parameters,
        /// like<code>MyClass{,}</code></param>
        /// <returns></returns>
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


