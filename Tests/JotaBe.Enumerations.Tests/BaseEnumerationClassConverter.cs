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
    public abstract class BaseEnumerationClassConverter : JsonConverterFactory
    {
        protected BaseEnumerationClassConverter(SerializationMode mode)
        {
            _serializationMode = mode;
        }

        protected readonly SerializationMode _serializationMode;

        protected enum SerializationMode
        {
            Value,
            Key,
            AsObject,
            RespectEnumMemberStringConverter
        }

        // TODO: the factory should not have parameters because can't be used in json serializer attribute
        // TODO: so it's possible to create a base class, and derive whole object + value + name, by default value??

        // As it's used for generic classes, must use the factory pattern explained here
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-7-0
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization?pivots=dotnet-8-0

        // on naming policies:
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/customize-properties?pivots=dotnet-8-0#use-a-built-in-naming-policy

        // use in immutable types
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability

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
                args: new object[] { options, _serializationMode },
                culture: null
                )!;

            return converter;
        }

        private class InternalEnumerationClassConverter<TEnumeration, TValue>
            : JsonConverter<EnumerationClass<TEnumeration, TValue>>
            where TEnumeration : EnumerationClass<TEnumeration, TValue>
        {
            public InternalEnumerationClassConverter(JsonSerializerOptions options, SerializationMode mode)
            {
                // TODO: could try to find if there are enum converter for string or not, and apply it, or use some other option??
                //var valueConverter = options.GetConverter(typeof(TEnumeration));

                _valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
                _valueType = typeof(TValue);
                _serializationOptions = options;
                _serializationMode = mode;
            }

            private readonly JsonConverter<TValue> _valueConverter;
            private readonly Type _valueType;
            private readonly JsonSerializerOptions _serializationOptions;
            private readonly SerializationMode _serializationMode;

            const string NamePropertyName = nameof(EnumerationClass<TEnumeration, TValue>.Name);
            const string ValuePropertyName = nameof(EnumerationClass<TEnumeration, TValue>.Value);

            public override EnumerationClass<TEnumeration, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                switch (_serializationMode)
                {
                    case SerializationMode.AsObject:
                        return ReadFullObject(ref reader, typeToConvert, options);
                    case SerializationMode.Value:
                    case SerializationMode.Key:
                    case SerializationMode.RespectEnumMemberStringConverter:
                        throw new NotImplementedException();
                    default:
                        throw new NotImplementedException($"Serialization mode {_serializationMode} not supported");
                }
            }

            /// <summary>
            /// To use when the <see cref="JsonConverter{T}"/> serializes the whole object, it
            /// checks that the beginning is an start object, that it contains at least the 
            /// <see cref="EnumerationClass{TEnumeration, TValue}.Name"/> and
            /// <see cref="EnumerationClass{TEnumeration, TValue}.Value"/> properties of
            /// the base class, and there values are consistent, and returns the corresponding
            /// value. If any of this fails throws <see cref="JsonException"/> with the right message
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            /// <exception cref="JsonException"></exception>
            private EnumerationClass<TEnumeration, TValue>? ReadFullObject(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // If the enum class is serialized as a full object, it must be an object and contain at least name and value
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException($"Expected an object but is {reader.TokenType}");
                }

                bool nameFound = false;
                string? name = null;
                bool valueFound = false;
                TValue? value = default;

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

                // Verifies that the name and value are consistent
                var element = EnumerationClass<TEnumeration, TValue>.FromValue(value!);
                if (element.Name != name)
                {
                    throw new JsonException($"The value {value} doesn't match the name {name}, should be {element.Name}");
                }
                return element;
            }



            /// <summary>
            /// True if the property name read from the JSON matches the deserialized property
            /// name, after casing it with the naming policy, if existing.
            /// </summary>
            /// <param name="jsonPropertyName">Property name as read from the serialized JSON</param>
            /// <param name="deserializedPropertyName">Original property name</param>
            /// <returns></returns>
            private bool IsProperty(string jsonPropertyName, string deserializedPropertyName)
            {
                // TODO: should explain that this is the way to go in the examples of converters, not using a IgnoreCase !!
                if (_serializationOptions.PropertyNamingPolicy == null)
                {
                    return deserializedPropertyName.Equals(jsonPropertyName, StringComparison.Ordinal);
                }
                var casedPropertyName = _serializationOptions.PropertyNamingPolicy.ConvertName(deserializedPropertyName);
                return casedPropertyName.Equals(jsonPropertyName, StringComparison.Ordinal);
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


