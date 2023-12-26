#if SHIM_SET
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// Provides the base interface for the abstraction of sets.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the set.
    /// </typeparam>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    public interface ISet<T> 
        : ICollection<T>
        , IEnumerable<T>
        , IEnumerable
    {
        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>
        /// <see langword="true" /> if the element is added to the set; <see langword="false" /> if the element is already in the set.
        /// </returns>
        new bool Add(T item);

        /// <summary>Removes all elements in the specified collection from the current set.</summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        void ExceptWith(IEnumerable<T> other);

        /// <summary>Modifies the current set so that it contains only elements that are also in a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        void IntersectWith(IEnumerable<T> other);

        /// <summary>Determines whether the current set is a proper (strict) subset of a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the current set is a proper subset of <paramref name="other" />; otherwise, <see langword="false" />.</returns>
        bool IsProperSubsetOf(IEnumerable<T> other);

        /// <summary>Determines whether the current set is a proper (strict) superset of a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the current set is a proper superset of <paramref name="other" />; otherwise, <see langword="false" />.</returns>
        bool IsProperSupersetOf(IEnumerable<T> other);

        /// <summary>Determines whether a set is a subset of a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the current set is a subset of <paramref name="other" />; otherwise, <see langword="false" />.</returns>
        bool IsSubsetOf(IEnumerable<T> other);

        /// <summary>Determines whether the current set is a superset of a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the current set is a superset of <paramref name="other" />; otherwise, <see langword="false" />.</returns>
        bool IsSupersetOf(IEnumerable<T> other);

        /// <summary>Determines whether the current set overlaps with the specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the current set and <paramref name="other" /> share at least one common element; otherwise, <see langword="false" />.</returns>
        bool Overlaps(IEnumerable<T> other);

        /// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        /// <returns>
        /// <see langword="true" /> if the current set is equal to <paramref name="other" />; otherwise, <see langword="false" />.</returns>
        bool SetEquals(IEnumerable<T> other);

        /// <summary>Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        void SymmetricExceptWith(IEnumerable<T> other);

        /// <summary>Modifies the current set so that it contains all elements that are present in the current set, in the specified collection, or in both.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is <see langword="null" />.</exception>
        void UnionWith(IEnumerable<T> other);
    }
}
#endif