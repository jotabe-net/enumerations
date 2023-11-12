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

