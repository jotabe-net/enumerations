using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace JotaBe.Enumerations
{
    // TODO: if order may be of importance, either make the value IComparable, or allow an int for defining the order
    // TODO: document all 

    /// <summary>
    /// Can be used as a replacement for C# enum, to allow including
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class EnumerationClass<TEnumeration,TValue>
        where TEnumeration : EnumerationClass<TEnumeration,TValue>
        //where TValue : IEquatable<TValue>
    {
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of the item
        /// </summary>
        public TValue Value { get; }

        protected EnumerationClass(string name, TValue value)
        {
            Name = name;
            Value = value;
        }

        private static List<TEnumeration> Elements
        {
            get 
            {
                EnsureElements();
                return _cachedElements;
            }
        }
        private static object _cachedElementsLock = new object();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static List<TEnumeration> _cachedElements;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Initializes the list of elements with double lock pattern, to support concurrency
        /// </summary>
        static void EnsureElements()
        {
            if (_cachedElements == null)
            {
                lock (_cachedElementsLock)
                {
                    if (_cachedElements == null)
                    {
                        var enumerationType = typeof(TEnumeration);
                        var fields = enumerationType.GetFields(BindingFlags.Public | BindingFlags.Static); // | BindingFlags.DeclaredOnly);
                        _cachedElements = fields.Select(field => field.GetValue(null)).Cast<TEnumeration>().ToList()!;
                    }
                }
            }
        }

        public bool Equals(TEnumeration? other)
        {
            return other != null && other.Value!.Equals(Value);
        }

        public static TEnumeration FromValue(TValue value)
        {
            var element = Elements.First(e => e.Value!.Equals(value));
            return element;
        }

        /// <summary>
        /// Get the value with the specified name. Alternatively, can use any of the <code>ParseXxx</code>
        /// methods.
        /// </summary>
        /// <param name="name">Name of the enumeration value</param>
        /// <returns></returns>
        public static TEnumeration FromName(string name)
        {
            var element = Elements.First(e => e.Name == name);
            return element;
        }

        public static TEnumeration[] GetValues()
        {
            return Elements.ToArray();
        }

        public static string GetName(TValue value)
        {
            return Elements.First(e => e.Value!.Equals(value)).Name;
        }

        public static string[] GetNames()
        {
            return Elements.Select(e => e.Name).ToArray();
        }

        public static Type GetUnderlyingType() => typeof (TValue);

        public static TValue[] GetValuesAsUnderlyingType()
        {
            var values = GetValues().Select(v => v.Value).ToArray();
            return values;
        }

        public static bool IsDefined(TValue value)
        {
            return Elements.Exists(e => e.Value!.Equals(value));
        }

        public static TEnumeration Parse(string name)
        {
            return Elements.First(e => e.Name == name);
        }

        public static TEnumeration Parse(ReadOnlySpan<char> name)
        {
            foreach (var e in Elements)
            {
                if (MemoryExtensions.Equals(name, e.Name, StringComparison.Ordinal))
                {
                    return e;
                }
            }
            throw new InvalidOperationException();
        }

        public static TEnumeration Parse(string name, bool ignoreCase)
        {
            return ignoreCase
                ? Elements.First(e => e.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                : Parse(name);
        }

        public static TEnumeration Parse(ReadOnlySpan<char> name, bool ignoreCase)
        {
            var comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            foreach (var e in Elements)
            {
                if (MemoryExtensions.Equals(name, e.Name, comparison))
                {
                    return e;
                }
            }
            throw new InvalidOperationException();
        }

        public static bool TryParse(string name, out TEnumeration? value)
        {
            var exists = Elements.Exists(e => e.Name == name);
            value = exists
                ? Elements.First(e => e.Name == name)
                : null;
            return exists;
        }

        public static bool TryParse(ReadOnlySpan<char> name, out TEnumeration? value)
        {
            bool exists = false;
            value = null;
            foreach (var e in Elements)
            {
                if (MemoryExtensions.Equals(name, e.Name, StringComparison.Ordinal))
                {
                    exists = true;
                    value = e;
                }
            }
            return exists;
        }

        public static bool TryParse(string name, bool ignoreCase, out TEnumeration? value)
        {
            var comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            var exists = Elements.Exists(e => e.Name.Equals(name, comparison));
            value = exists
                ? Elements.First(e => e.Name.Equals(name, comparison))
                : null;
            return exists;
        }

        public static bool TryParse(ReadOnlySpan<char> name, bool ignoreCase, out TEnumeration? value)
        {
            var exists = false;
            value = null;
            var comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            foreach (var e in Elements)
            {
                if (MemoryExtensions.Equals(name, e.Name, comparison))
                {
                    exists = true;
                    value = e;
                }
            }
            return exists;
        }

        private string DebuggerDisplay => $"{Value}: {Name}";
    }
}