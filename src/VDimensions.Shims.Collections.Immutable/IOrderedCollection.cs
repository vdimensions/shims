using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    /// <summary>
    /// Describes an ordered collection of elements.
    /// </summary>
    /// <typeparam name="T">
    /// The type of element in the collection.
    /// </typeparam>
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    #if NETSTANDARD || NET40_OR_NEWER
    internal interface IOrderedCollection<out T>
    #else
    internal interface IOrderedCollection<T>
    #endif
        : IEnumerable<T>
        , IEnumerable
    {
        /// <summary>Gets the number of elements in the collection.</summary>
        int Count { get; }

        /// <summary>Gets the element in the collection at a given index.</summary>
        T this[int index] { get; }
    }
}
