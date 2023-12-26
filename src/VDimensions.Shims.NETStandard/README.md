# VDimensions.Shims.NETStandard

## Target Frameworks

All.

## Description

Some of the features that this library brings:

* _[in progress]_ Reflection API consistency between .NET Standard 2.0 and earlier .NET Standard versions (1.0 - 1.5) and .NET Framework (v3.5 - v4.0)
* <strike>Exposed `System.Linq.ExpressionVisitor` as a public inheritable class in .NET Framework v3.5</strike>
* Exposes Linq Enumerable extension methods in `net20`.  
  __Note__, that when used in `net20`, these methods can only be called as static methods, since the `net20` compiler does not support the `this` keyword in extension method definitions.
* Enables support for the `System.Tuple` types in `net20`, `net35` and `net40`
* Exposes the `System.Concurrent.ConcurrentDictionary<,>` type for use in .NETStandard 1.0
## Dependencies

* [LinqBridge](https://www.nuget.org/packages/LinqBridge/)
* [NetLegacySupport.Tuple](https://www.nuget.org/packages/NetLegacySupport.Tuple/)
* [Portable.ConcurrentDictionary](https://www.nuget.org/packages/Portable.ConcurrentDictionary/)
* [System.Threading.Tasks.Unofficial](https://www.nuget.org/packages/System.Threading.Tasks.Unofficial/)