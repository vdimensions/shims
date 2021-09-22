# NETStandard.Shim

This project is an attempt to provide API consistency between the different .NET framework versions and to avoid the `#if-#else-#endif` preprocessor hell.

This library refers the .NET reference source repository to obtain implementations of features that are missing in earlier framework vesions. See the License section for more information

Some of the goodies that this library brings:

* _[in progress]_ Reflection API consistency between .NET Standard 2.0 and earlier .NET Standard versions (1.0 - 1.5) and .NET Framework (v3.5 - v4.0)
* Exposed `System.Linq.ExpressionVisitor` as a public inheritable class in .NET Framework v3.5
* Exposes Linq Enumerable methods in `net20`
* Enables support for the `System.Tuple` types in `net20`, `net35` and `net40`.

## Purpose and Reasoning

Since the introduction of **.NET Standard**, it has become clear that the .NET world was undergoing a great effort of consolidation for the vast range of APIs and pursuing better platform coverage. Because the job was complicated, it lead to the creation of a few **.NET Standard** / **.NET Core** versions with increasing set of supported APIs with each release, having **.NET Standard 2.0** as the latest stable release (at the moment of creating this document). **.NET Standard 2.0** is now being way closer to the **.NET Framework**'s set of APIs than it's predecesors.

[NETStandard.Shim](https://github.com/vdimensions/netstandard.shim) aims to encourage following the standartization trend, by exposing a number of the .NET Standard 2.0 APIs to earlier versions of the **.NET Standard**, as well as to earlier versions of the **.NET Framework** itself. The main goal of this project is to provide *working* polyfills for a couple of API gaps between the current **.NET Standard** version, earlier **.NET Standard** versions and **.NET Framework** versions not conforming to the standard completely. It also brings few of the new .NET goodies to earlier .NET Framework versions. 

## A Quick Glossary

The naming of different .NET tools and frameworks that will be used in this document can be confusing to some folks (including me at first). Here is some quick reference to avoid ambiguity and confusion:

- **.NET Framework**  
  This is the original .NET framework, that is available as a Windows installation. It targets Windows exclusively. To a great extent, the Mono project provides an analogue for Linux and Mac OS, and Xamarin -- for the mobile operating systems (Androud and iOS).  

 - **.NET Core**
  A cross-platform open-source variation of the framework. This is the result of an effort to consistently target many platforms. It is available as a standalone package for the major OS-es.

 - **.NET Standard**
  This is the standardized version of the framework, with reduced (and at places slightly changed) APIs, which covers all the various platforms. This is not a version of the framework by itself, rather an *abstract* set of APIs that are guaranteed to work on all the platforms that can run .NET Core or .NET Framework (since certain versions). 
  It is intended to be the primary choice for developing .NET libraries, as this guarantees high degrees of portability.

## Licensing

This project is licensed under the [MIT](./LICENSE) license for maximum compatbility with the [.NET reference source](https://github.com/microsoft/referencesource) codebase. Some implementations refer to that codedbase directly (it is referenced as a submodule). Please, refer to the [`submodules/referencesource/LICENSE.txt`](https://github.com/microsoft/referencesource/blob/master/LICENSE.txt) and [`submodules/referencesource/PATENTS.TXT`](https://github.com/microsoft/referencesource/blob/master/PATENTS.TXT) for information about the respective license and patents.

Some of the features are implemented in popular 3rd party libraries:

 - [LinqBridge](https://www.nuget.org/packages/LinqBridge) which is released under the [BSD 3-Clause License](https://github.com/atifaziz/LINQBridge/blob/master/COPYING.txt)  
 - [NetLegacySupport.Tuple](https://www.nuget.org/packages/NetLegacySupport.Tuple) that uses the [MIT License](https://github.com/SaladLab/NetLegacySupport/blob/master/LICENSE)  
