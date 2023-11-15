using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using JotaBe.TypeExtensions;

namespace JotaBe.Enumerations.Tests
{
    /// <summary>
    /// This converter writes the enum with <see cref="EnumerationClass{TEnumeration, TValue}.Name"/>
    /// and <see cref="EnumerationClass{TEnumeration, TValue}.Value"/> separated.
    /// </summary>
    public class EnumerationClassConverter : JsonConverterFactory
    {
        // TODO: the factory should not have parameters beacuse can't be used in jsonserializer attribute
        // TODO: so it's possible to create a base class, and derive whole objec + value + name, by default value??

        // As it's used for generic classes, must use the factory pattern explained here
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-7-0

        public override bool CanConvert(Type typeToConvert)
        {
            var convertible = typeToConvert.IsDerivedFrom(typeof(EnumerationClass<,>));
            //var convertible2 = typeToConvert.IsDerivedFrom(typeof(EnumerationClass<TemperatureUnit,char>)); 
            return convertible;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            // This is only invoked if can convert is true, so we know that it derives from EnumerationClass<,>
            var typeArguments = typeToConvert.GetGenericBaseTypeArguments(typeof(EnumerationClass<,>));
            var TEnumeration = typeArguments![0];
            var TValue = typeArguments[1];

            // TODO: make this activator pass a second argument to choose if incluse value / name or full object
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(InternalEnumerationClassConverter<,>).MakeGenericType(typeArguments),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null
                )!;

            return converter;
        }

        private enum EnumerationMode
        {
            Value,
            Key,
            AllValues,
            // RespectEnumMemberStringConverter
        }

        private class InternalEnumerationClassConverter<TEnumeration, TValue>
            : JsonConverter<EnumerationClass<TEnumeration, TValue>>
            where TEnumeration : EnumerationClass<TEnumeration, TValue>
        {
            public InternalEnumerationClassConverter(JsonSerializerOptions options)
            {
                // TODO: could try to find if there are enum converter for string or not, and apply it, or use some other option??
                //var valueConverter = options.GetConverter(typeof(TEnumeration));

                _valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
                _valueType = typeof(TValue);
            }

            JsonConverter<TValue> _valueConverter;
            Type _valueType;

            const string NamePropertyName = nameof(EnumerationClass<TEnumeration, TValue>.Name);
            const string ValuePropertyName = nameof(EnumerationClass<TEnumeration, TValue>.Value);

            public override EnumerationClass<TEnumeration, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException($"Expected an object but is {reader.TokenType}");
                }

                bool nameFound = false;
                string? name = null;
                bool valueFound = false;
                TValue? value = default;

                // TODO: at some point, the reader finishes...
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException("Expected property name");
                    }
                    // For the generic serializer, it could be name, or value, and be case sensitive or insensitive
                    var propertyName = reader.GetString();
                    if (propertyName == null)
                    {
                        throw new JsonException("Expected property name, but it's empty");
                    }
                    if (IsProperty(propertyName, NamePropertyName))
                    {
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException($"The value for property {NamePropertyName} should be a string but is {reader.TokenType}");
                        }
                        nameFound = true;
                        name = reader.GetString()!; // TODO: verify is not null or empty
                    }
                    else if (IsProperty(propertyName, ValuePropertyName))
                    {
                        reader.Read();
                        valueFound = true;
                        value = _valueConverter.Read(ref reader, _valueType, options);
                        // TODO: verify is not null
                    }
                    else
                    {
                        // Skip value of other properties. It's not needed
                        reader.Read();
                    }
                }

                if (!nameFound)
                {
                    throw new JsonException($"Missing {NamePropertyName}");
                }

                if (!valueFound)
                {
                    throw new JsonException($"Missing {ValuePropertyName}");
                }

                var element = EnumerationClass<TEnumeration, TValue>.FromValue(value);
                if (element.Name != name)
                {
                    throw new JsonException($"The value {value} doesn't match the name {name}, should be {element.Name}");
                }
                return element;
            }

            /// <summary>
            /// Currently .NET doesn't support property policies to deserialize, and properties
            /// could be camel-cased. So it's necessary to check the property name in case
            /// sensitive and case insensitive way. First case sensitive for performance reasons.
            /// </summary>
            /// <param name="jsonPropertyName"></param>
            /// <param name="propertyName"></param>
            /// <returns></returns>
            private bool IsProperty(string jsonPropertyName, string propertyName)
            {
                return propertyName.Equals(jsonPropertyName, StringComparison.Ordinal)
                    || propertyName.Equals(jsonPropertyName, StringComparison.OrdinalIgnoreCase);
            }

            public override void Write(Utf8JsonWriter writer, EnumerationClass<TEnumeration, TValue> value, JsonSerializerOptions options)
            {
                //JsonSerializer.Serialize(writer, value, options);
                writer.WriteStartObject();

                // Write name
                var propertyName = nameof(value.Name);
                propertyName = options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
                writer.WriteString(propertyName, value.Name);

                // Write value
                propertyName = nameof(value.Value);
                propertyName = options.PropertyNamingPolicy?.ConvertName(propertyName) ?? propertyName;
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value.Value, options);
                writer.WriteEndObject();
            }
        }

    }
}


