﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    /// <summary>
    /// Provides a set of initialization methods for instances of the <see cref="ImmutableList{T}"/> class.
    /// </summary>
    /// <seealso cref="ImmutableList{T}"/>
    /// <seealso cref="IImmutableList{T}"/>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static partial class ImmutableList
    {
        /// <summary>
        /// Creates an empty immutable list.
        /// </summary>
        /// <typeparam name="T">
        /// The type of items stored in the list.
        /// </typeparam>
        /// <returns>
        /// An empty immutable list.
        /// </returns>
        public static ImmutableList<T> Create<T>() => new ImmutableList<T>(new List<T>());
        
        /// <summary>
        /// Creates a new immutable collection prefilled with the specified item.
        /// </summary>
        /// <typeparam name="T">The type of items stored by the collection.</typeparam>
        /// <param name="item">The item to prepopulate.</param>
        /// <returns>The new immutable collection.</returns>
        public static ImmutableList<T> Create<T>(T item) => new ImmutableList<T>(new List<T>() { item });

        /// <summary>
        /// Creates a new immutable list that contains the specified <paramref name="items"/>.
        /// </summary>
        /// <param name="items">
        /// The items to add to the list.
        /// </param>
        /// <typeparam name="T">
        /// The type of items stored in the list.
        /// </typeparam>
        /// <returns>
        /// An immutable list that contains the specified <paramref name="items"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static ImmutableList<T> CreateRange<T>(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            return new ImmutableList<T>(new List<T>(items));
        }
        
        /// <summary>
        /// Creates a new immutable list that contains the specified <paramref name="items"/>.
        /// </summary>
        /// <param name="items">
        /// The items to add to the list.
        /// </param>
        /// <typeparam name="T">
        /// The type of items stored in the list.
        /// </typeparam>
        /// <returns>
        /// An immutable list that contains the specified <paramref name="items"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static ImmutableList<T> CreateRange<T>(params T[] items) => new ImmutableList<T>(new List<T>(items ?? new T[0]));

        /// <summary>Creates a new immutable list builder.</summary>
        /// <typeparam name="T">The type of items stored by the collection.</typeparam>
        /// <returns>The immutable collection builder.</returns>
        public static ImmutableList<T>.Builder CreateBuilder<T>() => Create<T>().ToBuilder();

        /// <summary>
        /// Enumerates a sequence exactly once and produces an immutable list of its contents.
        /// </summary>
        /// <typeparam name="TSource">The type of element in the sequence.</typeparam>
        /// <param name="source">The sequence to enumerate.</param>
        /// <returns>An immutable list.</returns>
        public static ImmutableList<TSource> ToImmutableList<TSource>(this IEnumerable<TSource> source) => source as ImmutableList<TSource> ?? ImmutableList<TSource>.Empty.AddRange(source);

        /// <summary>
        /// Replaces the first equal element in the list with the specified element.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="oldValue">The element to replace.</param>
        /// <param name="newValue">The element to replace the old element with.</param>
        /// <returns>The new list -- even if the value being replaced is equal to the new value for that position.</returns>
        /// <exception cref="ArgumentException">Thrown when the old value does not exist in the list.</exception>
        public static IImmutableList<T> Replace<T>(this IImmutableList<T> list, T oldValue, T newValue)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Replace(oldValue, newValue, EqualityComparer<T>.Default);
        }

        /// <summary>Removes the specified value from this list.</summary>
        /// <param name="list">The list to search.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>A new list with the element removed, or this list if the element is not in this list.</returns>
        public static IImmutableList<T> Remove<T>(this IImmutableList<T> list, T value)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Remove(value, EqualityComparer<T>.Default);
        }

        /// <summary>Removes the specified values from this list.</summary>
        /// <param name="list">The list to search.</param>
        /// <param name="items">The items to remove if matches are found in this list.</param>
        /// <returns>A new list with the elements removed.</returns>
        public static IImmutableList<T> RemoveRange<T>(this IImmutableList<T> list, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.RemoveRange(items, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the <see cref="IImmutableList{T}" />.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the range of
        /// elements in the <see cref="IImmutableList{T}" /> that extends from index
        /// to the last element, if found; otherwise, -1.
        /// </returns>
        public static int IndexOf<T>(this IImmutableList<T> list, T item)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.IndexOf(item, 0, list.Count, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the <see cref="IImmutableList{T}" />.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="equalityComparer">The equality comparer to use in the search.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the range of
        /// elements in the <see cref="IImmutableList{T}" /> that extends from index
        /// to the last element, if found; otherwise, -1.
        /// </returns>
        public static int IndexOf<T>(this IImmutableList<T> list, T item, IEqualityComparer<T> equalityComparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.IndexOf(item, 0, list.Count, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the range of elements in the <see cref="IImmutableList{T}" />
        /// that extends from the specified index to the last element.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="startIndex">
        /// The zero-based starting index of the search. 0 (zero) is valid in an empty
        /// list.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the range of
        /// elements in the <see cref="IImmutableList{T}" /> that extends from index
        /// to the last element, if found; otherwise, -1.
        /// </returns>
        public static int IndexOf<T>(this IImmutableList<T> list, T item, int startIndex)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.IndexOf(item, startIndex, list.Count - startIndex, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the range of elements in the <see cref="IImmutableList{T}" />
        /// that extends from the specified index to the last element.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="startIndex">
        /// The zero-based starting index of the search. 0 (zero) is valid in an empty
        /// list.
        /// </param>
        /// <param name="count">
        /// The number of elements in the section to search.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of item within the range of
        /// elements in the <see cref="IImmutableList{T}" /> that extends from index
        /// to the last element, if found; otherwise, -1.
        /// </returns>
        public static int IndexOf<T>(this IImmutableList<T> list, T item, int startIndex, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.IndexOf(item, startIndex, count, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// last occurrence within the entire <see cref="IImmutableList{T}" />.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of item within the entire the
        /// <see cref="IImmutableList{T}" />, if found; otherwise, -1.
        /// </returns>
        public static int LastIndexOf<T>(this IImmutableList<T> list, T item)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Count == 0 ? -1 : list.LastIndexOf(item, list.Count - 1, list.Count, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// last occurrence within the entire <see cref="IImmutableList{T}" />.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="equalityComparer">The equality comparer to use in the search.</param>
        /// <returns>
        /// The zero-based index of the last occurrence of item within the entire the
        /// <see cref="IImmutableList{T}" />, if found; otherwise, -1.
        /// </returns>
        public static int LastIndexOf<T>(this IImmutableList<T> list, T item, IEqualityComparer<T> equalityComparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Count == 0 ? -1 : list.LastIndexOf(item, list.Count - 1, list.Count, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// last occurrence within the range of elements in the <see cref="T:System.Collections.Immutable.IImmutableList`1" />
        /// that extends from the first element to the specified index.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Immutable.IImmutableList`1" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="startIndex">
        /// The zero-based starting index of the backward search.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of item within the range of elements
        /// in the <see cref="T:System.Collections.Immutable.IImmutableList`1" /> that extends from the first element
        /// to index, if found; otherwise, -1.
        /// </returns>
        public static int LastIndexOf<T>(this IImmutableList<T> list, T item, int startIndex)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.Count == 0 && startIndex == 0 ? -1 : list.LastIndexOf(item, startIndex, startIndex + 1, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// last occurrence within the range of elements in the <see cref="IImmutableList{T}" />
        /// that extends from the first element to the specified index.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="item">
        /// The object to locate in the <see cref="IImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="startIndex">
        /// The zero-based starting index of the backward search.
        /// </param>
        /// <param name="count">
        /// The number of elements in the section to search.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of item within the range of elements
        /// in the <see cref="IImmutableList{T}" /> that extends from the first element
        /// to index, if found; otherwise, -1.
        /// </returns>
        public static int LastIndexOf<T>(this IImmutableList<T> list, T item, int startIndex, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            return list.LastIndexOf(item, startIndex, count, EqualityComparer<T>.Default);
        }
    }
}