# NETStandard.Shim

Provides API consistency between the different .NET framework versions. Forget the `#if-#else-#endif` preprocessor hell!

Some goodies that this library brings:

* Reflection API consistency between .NET Standard 2.0 and earlier .NET Standard versions (1.0 - 1.5) and .NET Framework (v3.5 - v4.0)
* Exposed `System.Linq.ExpressionVisitor` in .NET Framework v3.5
* Provides the additional `Func<T..., TResult>` delegates defined in .NET Standard to .NET Framework v3.5 and v4.0
* Refers to third-party library LinqBridge for projects targeting .NET Framework 2.0 so that Linq Enumerable methods are available

## Purpose and Reasoning

The main goal of this project is to provide *working* fills for a couple of API gaps between earlier **.NET Standard** versions and the **.NET Frameworks**, but it also brings few of the new goodies to earlier .NET Framework versions.

Since the introduction of **.NET Standard**, it has become clear that the .NET world was undergoing a great effort of consolidation for the vast range of APIs and pursuing better platform coverage. Because the job was complicated, it lead to the creation of a few **.NET Standard** / **.NET Core** versions with increasing set of supported APIs with each release, having **.NET Standard 2.0** as the latest stable release (at the moment of creating this document). **.NET Standard 2.0** is now being way closer to the **.NET Framework**'s set of APIs than it's predecesors.

This small project aims to encourage following the standartization trend, by exposing a number of the .NET Standard 2.0 APIs to earlier versions of the **.NET Standard**, as well as to earlier versions of the **.NET Framework** itself. 

## A Quick Glossary

The naming of different .NET tools and frameworks that will be used in this document can be confusing to some folks (including me at first). Here is some quick reference to avoid ambiguity and confusion:

- **.NET Framework**  
  This is the original .NET framework, that is available as a Windows installation. It targets Windows exclusively. To a great extent, the Mono project provides an analogue for Linux and Mac OS, and Xamarin -- for the mobile operating systems (Androud and iOS).   

 - **.NET Core**
  A cross-platform open-source variation of the framework. This is the result of an effort to consistently target many platforms. It is available as a standalone package for the major OS-es.

 - **.NET Standard**
  This is the standardized version of the framework, with reduced (and at places slightly changed) APIs, which covers all the various platforms. This is not a version of the framework by itself, rather an *abstract* set of APIs that are guaranteed to work on all the platforms that can run .NET Core or .NET Framework (since certain versions). 
  It is intended to be the primary choice for developing .NET libraries, as this guarantees high degrees of portability.
