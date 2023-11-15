# Multi framework

Change the project to have  
`<TargetFrameworks>netstandard2.1</TargetFrameworks>`
instead of  
`<TargetFramework>netstandard2.1</TargetFramework>`

# Nullable

By the moment is enabled, what makes it impossible to target frameworks
with language level below 8.0, like net47 or netstandard2.0

It would be necessary to remove the nullability or make it conditional
 
# Links to alternatives

This alternative includes extensions for EF Core, Dapper, JSON.NET...

https://github.com/ardalis/SmartEnum

# Serialization

By default the serialization is done as an object including the Name, Value,
and any other additional properties included in the inherited class. And the
deserialization is not possible.

For `Syetm.Text.Json` you can use one of the three converters, to serialize
the whole object, or just the name or the value.

https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-7-0#registration-sample---converters-collection

Idea:
In the class itself, determine how you want to serialize it. if at all possible
(by reading the options?) check if there is the EnumStringConverter plugged in
the json serializer to respect its behavior. SO, the base class could have
a public static abstract property, like "serialization style" with these:
- Value
- Name
- All properties
- Only Value and key
- Value or Name if enumStringConverter

# technical debt
The serializer is inside the test project -- must be moved to the main library, or an
specific serialization project

