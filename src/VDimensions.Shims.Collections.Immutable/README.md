# VDimensions.Shims.Collections.Immutable

## Target Frameworks

* __Unity Engine WebGL__  
* .NETFramework v2.0 (net20)
* .NETFramework v3.5 (net35)
* .NETFramework v4.0 (net40)

## Description

This is a backport of the [System.Collections.Immutable](https://www.nuget.org/packages/System.Collections.Immutable) package which is avalilable for .NETFramework v4.5, .NETStandard and above. `VDimensions.Shims.Collections.Immutable` exposes identical _public_ APIs to the ones from the `System.Collections.Immutable`, which will enable backporting existing code depenent on immutable collections to older .NET Framework versions without issues.  

### Differences from the orignal implementation

It must be noted, that instead of backporting the source code of the original `System.Collections.Immutable` library (which seems to be possible), this implementation uses the built-in collections already in the .NET Framework under the hood. The primary reason behind this decision is that we want to be able to _properly_ support immutable collections inside the __Unity Game Engine__ (which is our primary area of interest), and be able to produce __WebGL__ builds. __Unity__, as of now, is already compliant with .NETStandard and can use the original System.Collections.Immutable package as it is. However, the package is unsuitable for WebGL builds, because Microsoft are making use of __thread-locals__ in the implementation of the immutable collections, and that code gets __stripped__ by the Unity WebGL compiler in a way that breaks the behaviour of those collections. The result is that the collections exhibit very erratic state and cause application crashes.

Our re-implementation __deliberately avoids__ usage of thead-locals, so that the code will work for Unity WebGL builds with no issues. Important _impacts_ from this decision need to be made known to the consumers of our library, however. The orignal implementation from Microsoft promises better performance in concurrent uses of the immutable collections, especially when immutable collections builders are being used (mutable mode). This is achieved elegantly with thread-locals to prevent unnecessary exclusive locks. Since our implementation takes a different approach, we must compensate this with  __exclusive locks__ to provide the same thread-safety guarantees. Therefore, in multi-threaded scenarios where immutable builders are being used, the original implementation will perform better.


## Dependencies

None.