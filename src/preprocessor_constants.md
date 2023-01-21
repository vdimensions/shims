# Preprocessor Constants

The project uses preprocessor constants to make certain shims and polyfills available for different TFM builds. We have used naming convention for these constants as follows, which is based on the [established understanding](https://dzone.com/articles/what-difference-between-shim) for the terms _shim_ and _polyfill_ in the software development community:

- __Polyfills__:  Start with `POLYFILL_`. Represent extension method classes that cannot be named as the original implementation they are substituting, because it exists, but is incomplete. In other words, these are used to fill partial functionalities that are missing in earlier TFMs.  
This naming is somewhat consistent with the ide behind the term _polyfill_ in software development, as it is a form of "filling the gaps" in the apis.

- __Shims__: Start with `SHIM_`. Represent complete (re)implentations of types or extension method classes that are not existent for the TFM.  
This namming is also consistent with the general understanding of a _shim_, being a complete implementation of a missing block of functionality.  
