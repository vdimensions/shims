This project is being renamed to VDimensions.Shims 
--------------------------------------------------
Make sure you update your remote url
* __GitHub__: https://github.com/vdimensions/shims
* __GitLab__: https://gitlab.com/vdimensions/frameworks/shims/vdimensions.shims

===
# NETStandard.Shim

This project is an attempt to provide API consistency between the different .NET framework versions and to avoid the `#if-#else-#endif` preprocessor hell.

This library refers the .NET reference source repository and 3rd party libraries to obtain implementations for features that are missing in earlier framework releases. See the [Licensing](#licensing) section for more information.

Some of the features that this library brings:

* _[in progress]_ Reflection API consistency between .NET Standard 2.0 and earlier .NET Standard versions (1.0 - 1.5) and .NET Framework (v3.5 - v4.0)
* Exposed `System.Linq.ExpressionVisitor` as a public inheritable class in .NET Framework v3.5
* Exposes Linq Enumerable methods in `net20`
* Enables support for the `System.Tuple` types in `net20`, `net35` and `net40`
* Exposes the `System.Concurrent.ConcurrentDictionary<,>` type for use in .NETStandard 1.0, 1.1 and 1.2

## Purpose and Reasoning

Since the introduction of **.NET Standard**, it has become clear that the .NET world was undergoing a great effort of consolidation for the vast range of APIs and pursuing better platform coverage. Because the job was complicated, it lead to the creation of a few **.NET Standard** / **.NET Core** versions with increasing set of supported APIs with each release, having **.NET Standard 2.0** as the latest stable release (at the moment of creating this document). **.NET Standard 2.0** is now being way closer to the **.NET Framework**'s set of APIs than it's predecesors.

[NETStandard.Shim](https://github.com/vdimensions/netstandard.shim) aims to encourage following the standartization trend, by exposing a number of the .NET Standard 2.0 APIs to earlier versions of the **.NET Standard**, as well as to earlier versions of the **.NET Framework** itself. The main goal of this project is to provide *working* polyfills for a couple of API gaps between the current **.NET Standard** version, earlier **.NET Standard** versions and **.NET Framework** versions not conforming to the standard completely. It also brings few of the new .NET goodies to earlier .NET Framework versions. 

## A Quick Glossary

The naming of different .NET tools and frameworks that will be used in this document can be confusing to some folks (including me at first). Here is some quick reference to avoid ambiguity and confusion:

* **.NET Framework**  
  This is the original .NET framework, that is available as a Windows installation. It targets Windows exclusively and different versions of it are pre-installed in the Windows OS. To a great extent, the Mono project provides an analogue for Linux and Mac OS, and Xamarin -- for the mobile operating systems (Androud and iOS). Framework monikers use the format `netXX`, where XX is the version of the framework, for example `net20`, `net451` and etc.  

* **.NET Standard**  
  This is the standardized version of the framework, with reduced (and at places slightly changed) APIs, which covers all the various platforms. This is not a version of the framework by itself, rather an *abstract* set of APIs that are guaranteed to work on all the platforms that can run .NET Core or .NET Framework (since certain versions).  Framework monikers use the format `netstandardX.X` where `X.X` represents the particular standard version, for example `netstandard2.0`.  
  `.NETStandard` is intended to be the primary choice for developing libraries, because it guarantees higher portability.  

* **.NET Core**  
  A cross-platform open-source variation of the framework. This is the result of an effort to consistently target many platforms, and thus is cut off from its windows-only roots that we see in `.NET Framework`. That is why targeting this framework may make certain apis typical to `.NET Framework` unavailable. `.NET Core` is available as a standalone package for the major OS-es. Framework monikers use the format `netcoreapp`.  

* **Unified .NET Platform**  
  This version represents the merging of the __.NET Standard__ and the __.NET Core__ concepts, and is also a cross-platform .NET version.  
  The first release that represents this unifucation is __.NET 5__, which uses the moniker `net5.0`, or `net5.0-XX` where `XX` is the target plafrom (e.g. `windows`) for platform-specific builds. Future versions (e.g. `net6.0`) of .NET will abide to this naming format and will always be compatible with `netstandard2.1`.

## Licensing

This project is licensed under the [MIT](./LICENSE) license for maximum compatbility with the [.NET reference source](https://github.com/microsoft/referencesource) codebase, which is also referenced as a submodule. Please, refer to the [`submodules/referencesource/LICENSE.txt`](https://github.com/microsoft/referencesource/blob/master/LICENSE.txt) and [`submodules/referencesource/PATENTS.TXT`](https://github.com/microsoft/referencesource/blob/master/PATENTS.TXT) for information about the respective license and patents.

Over the years, popular 3rd party libraries have attempted to address the challenge for consistent .NET apis across different .NET versions. The work invested in those projects should be respected. We are doing our best to re-use those libraries in this project as "reinventing the whell" is rarely a productive way of work. However, most of the existing polyfill projects were written by different people addressing different concerns in different times. This was an uncoordinated effort, and when these libraries are put together in a single project (this project), they don't always play nice with each other.  

Below is a list of 3rd party libraries that have been chosen as depedencies of this project. The reasons for these choices were that they play nice (enough) with each other, and also, their licenses allow combining them together.

 - [LinqBridge](https://www.nuget.org/packages/LinqBridge/), licensed under the [BSD 3-Clause License](https://github.com/atifaziz/LINQBridge/blob/master/COPYING.txt)  
   This library has remained stable over the years and a useful extension for compatibility with net35
 - [NetLegacySupport.Tuple](https://www.nuget.org/packages/NetLegacySupport.Tuple/), licensed under the [MIT License](https://github.com/SaladLab/NetLegacySupport/blob/master/LICENSE)  
 - [Portable.ConcurrentDictionary](https://www.nuget.org/packages/Portable.ConcurrentDictionary/), licensed under [MIT License](https://raw.githubusercontent.com/StefH/Portable.ConcurrentDictionary/master/LICENSE)
 - [System.Threading.Tasks.Unofficial](https://www.nuget.org/packages/System.Threading.Tasks.Unofficial/). Various licenses available:
   - Apache-2.0 License
   - GPL-MIT License
   - LGPL-2.0 License
   - MIT License
   - MS-PL License  

   We favored this library over the official TPL library from Microsoft, because its API surface was richer and closer to a complete polyfill.
