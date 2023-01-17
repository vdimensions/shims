#if FX_READONLY_COLLECTION
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a strongly-typed read-only collection of values.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the collection.
    /// </typeparam>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    #if NET40_OR_NEWER
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    #else
    public interface IReadOnlyCollection<T> : IEnumerable<T>
    #endif
    {
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        int Count { get; }
    }
}
#endif