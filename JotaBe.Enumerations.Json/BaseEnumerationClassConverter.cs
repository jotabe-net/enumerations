using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using JotaBe.TypeExtensions;

namespace JotaBe.Enumerations.Json
{
    // TODO: test cases with attribute converter attribute ion property

    /// <summary>
    /// This converter controls how the <see cref="EnumerationClass{,}"/> is
    /// serialized (as whole object, name or value).<br/>
    /// This is an abstract base class, so you must use any of the derived classes,
    /// which just need to specify the serialization mode in the constructor.
    /// </summary>
    /// <remarks>
    /// As the class is generic it must use the factory pattern explained here
    /// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-7-0
    /// <br/>It supports naming policies for serialization and deserialization
    /// </remarks>
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
            Name,
            AsObject,
            RespectEnumMemberStringConverter
        }

        public override bool CanConvert(Type typeToConvert)
        {
            var convertible = typeToConvert.IsDerivedFrom(typeof(EnumerationClass<,>));
            return convertible;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            // This is only invoked if can convert is true, so we know that it derives from EnumerationClass<,>
            var typeArguments = typeToConvert.GetGenericBaseTypeArguments(typeof(EnumerationClass<,>));

            // The activator uses the internal enumeration class converter constructor, passing
            // the serialization options and the serialization mode
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(InternalEnumerationClassConverter<,>).MakeGenericType(typeArguments!),
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
                        return ReadFromFullObject(ref reader, typeToConvert, options);
                    case SerializationMode.Value:
                        return ReadFromValue(ref reader, typeToConvert, options);
                    case SerializationMode.Name:
                        return ReadFromName(ref reader, typeToConvert, options);
                    case SerializationMode.RespectEnumMemberStringConverter:
                        // TODO: respect enum member string converter
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
            private EnumerationClass<TEnumeration, TValue>? ReadFromFullObject(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // If the enum class is serialized as a full object, it must be an object and contain at least name and value
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException($"Expected an object but is {reader.TokenType}");
                }

                bool nameFound = false;
                string? name = null;
                bool valueFound = false;
                TValue value = default;

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

            /// <summary>
            /// This is used when the <see cref="EnumerationClass{,}" was serialized by its name property
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            /// <exception cref="JsonException"></exception>
            protected EnumerationClass<TEnumeration, TValue>? ReadFromName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException($"Expected an string value, but is {reader.TokenType}");
                }
                var name = reader.GetString();
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new JsonException("The property must have a string, but is empty or white space");
                }

                if (!EnumerationClass<TEnumeration, TValue>.TryParse(name, out var element))
                {
                    throw new JsonException($"Can't convert '{name}' to a value of {typeToConvert}");
                }

                return element;
            }

            /// <summary>
            /// This is used when the <see cref="EnumerationClass{,}"/> was serialized by its Value property
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            /// <exception cref="JsonException"></exception>
            protected EnumerationClass<TEnumeration, TValue>? ReadFromValue(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                TValue value = default(TValue);
                try
                {
                    value = (TValue)JsonSerializer.Deserialize(ref reader, typeof(TValue), options);
                }
                catch (Exception)
                {
                    throw new JsonException($"Cant' convert property");
                }

                if (value == null)
                {
                    throw new JsonException($"The property must have a value of type {typeof(TValue).Name} but is null");
                }

                if (!EnumerationClass<TEnumeration, TValue>.IsDefined(value))
                {
                    throw new JsonException($"Can't convert '{value}' to an enumeration class value");
                }

                var element = EnumerationClass<TEnumeration, TValue>.FromValue(value);
                return element;
            }


            public override void Write(Utf8JsonWriter writer, EnumerationClass<TEnumeration, TValue> value, JsonSerializerOptions options)
            {
                switch (_serializationMode)
                {
                    case SerializationMode.AsObject:
                        WriteAsObject(writer, value, options);
                        break;
                    case SerializationMode.Value:
                        WriteValue(writer, value, options);
                        break;
                    case SerializationMode.Name:
                        WriteName(writer, value, options);
                        break;
                    case SerializationMode.RespectEnumMemberStringConverter:
                        throw new NotImplementedException();
                    default:
                        throw new NotImplementedException($"Serialization mode {_serializationMode} not supported");
                }
            }

            protected void WriteAsObject(Utf8JsonWriter writer, EnumerationClass<TEnumeration, TValue> value, JsonSerializerOptions options)
            {
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

            protected void WriteValue(Utf8JsonWriter writer, EnumerationClass<TEnumeration, TValue> value, JsonSerializerOptions options)
            {
                // Use default serializer of TValue
                JsonSerializer.Serialize(writer, value.Value, typeof(TValue), options);
            }

            protected void WriteName(Utf8JsonWriter writer, EnumerationClass<TEnumeration, TValue> value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.Name);
            }

        }
    }
}


