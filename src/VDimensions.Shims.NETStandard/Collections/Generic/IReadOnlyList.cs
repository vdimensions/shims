#if SHIM_READONLY_COLLECTIONS
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a read-only collection of elements that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the read-only list.
    /// </typeparam>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    #if NET40_OR_NEWER
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>
    #else
    public interface IReadOnlyList<T> : IReadOnlyCollection<T>
    #endif
    {
        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get.
        /// </param>
        /// <returns>
        /// The element at the specified index in the read-only list.
        /// </returns>
        T this[int index] { get; }
    }
}
#endif