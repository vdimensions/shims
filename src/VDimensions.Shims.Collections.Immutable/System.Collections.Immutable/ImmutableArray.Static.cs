using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Immutable
{
    /// <summary>
    /// A set of initialization methods for instances of <see cref="ImmutableArray{T}" />.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class ImmutableArray
    {
        /// <summary>
        /// Creates an empty <see cref="ImmutableArray{T}" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <returns>
        /// An empty array.
        /// </returns>
        public static ImmutableArray<T> Create<T>() => ImmutableArray<T>.Empty;

        /// <summary>
        /// Creates an <see cref="ImmutableArray{T}" /> with the specified element as its only member.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="item">
        /// The element to store in the array.
        /// </param>
        /// <returns>
        /// A 1-element array.
        /// </returns>
        public static ImmutableArray<T> Create<T>(T item) => new ImmutableArray<T>(new [] { item });

        /// <summary>
        /// Creates an <see cref="ImmutableArray{T}" /> with the specified elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="item1">
        /// The first element to store in the array.
        /// </param>
        /// <param name="item2">
        /// The second element to store in the array.
        /// </param>
        /// <returns>
        /// A 2-element array.
        /// </returns>
        public static ImmutableArray<T> Create<T>(T item1, T item2) => new ImmutableArray<T>(new [] { item1, item2 });

        /// <summary>
        /// Creates an <see cref="ImmutableArray{T}" /> with the specified elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="item1">
        /// The first element to store in the array.
        /// </param>
        /// <param name="item2">
        /// The second element to store in the array.
        /// </param>
        /// <param name="item3">
        /// The third element to store in the array.
        /// </param>
        /// <returns>
        /// A 3-element array.
        /// </returns>
        public static ImmutableArray<T> Create<T>(T item1, T item2, T item3) 
            => new ImmutableArray<T>(new [] { item1, item2, item3 });

        /// <summary>
        /// Creates an <see cref="ImmutableArray{T}" /> with the specified elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="item1">
        /// The first element to store in the array.
        /// </param>
        /// <param name="item2">
        /// The second element to store in the array.
        /// </param>
        /// <param name="item3">
        /// The third element to store in the array.
        /// </param>
        /// <param name="item4">
        /// The fourth element to store in the array.
        /// </param>
        /// <returns>
        /// A 4-element array.
        /// </returns>
        public static ImmutableArray<T> Create<T>(T item1, T item2, T item3, T item4) 
            => new ImmutableArray<T>(new [] { item1, item2, item3, item4 });

        /// <summary>
        /// Creates an <see cref="ImmutableArray{T}" /> populated with the contents of the specified sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="items">
        /// The elements to store in the array.
        /// </param>
        /// <returns>
        /// An immutable array.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static ImmutableArray<T> CreateRange<T>(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException((nameof(items)));
            }
            if (items is IImmutableArray immutableArray)
            {
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return new ImmutableArray<T>((T[]) immutableArray.Array);
            }
            if (!ImmutableExtensions.TryGetCount(items, out var count))
            {
                return new ImmutableArray<T>(Enumerable.ToArray(items));
            }
            return count == 0 ? Create<T>() : new ImmutableArray<T>(ImmutableExtensions.ToArray(items, count));
        }

        /// <summary>
        /// Creates an empty <see cref="ImmutableArray{T}" />.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="items">
        /// The elements to store in the array.
        /// </param>
        /// <returns>
        /// An immutable array.
        /// </returns>
        public static ImmutableArray<T> Create<T>(params T[] items) 
            => items == null ? Create<T>() : CreateDefensiveCopy(items);

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The array to initialize the array with. A defensive copy is made.
        /// </param>
        /// <param name="start">
        /// The index of the first element in the source array to include in the resulting array.
        /// </param>
        /// <param name="length">
        /// The number of elements from the source array to include in the resulting array.
        /// </param>
        /// <remarks>
        /// This overload allows helper methods or custom builder classes to efficiently avoid paying a redundant
        /// tax for copying an array when the new array is a segment of an existing array.
        /// </remarks>
        public static ImmutableArray<T> Create<T>(T[] items, int start, int length)
        {
            if (items == null)
            {
                throw new ArgumentNullException((nameof(items)));
            }
            if (start < 0 || start > items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || start + length > items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (length == 0)
            {
                return Create<T>();
            }
            var items1 = new T[length];
            for (var index = 0; index < length; ++index)
            {
                items1[index] = items[start + index];
            }
            return new ImmutableArray<T>(items1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The array to initialize the array with. The selected array segment may be copied into a new array.
        /// </param>
        /// <param name="start">
        /// The index of the first element in the source array to include in the resulting array.
        /// </param>
        /// <param name="length">
        /// The number of elements from the source array to include in the resulting array.
        /// </param>
        /// <remarks>
        /// This overload allows helper methods or custom builder classes to efficiently avoid paying a redundant
        /// tax for copying an array when the new array is a segment of an existing array.
        /// </remarks>
        public static ImmutableArray<T> Create<T>(ImmutableArray<T> items, int start, int length)
        {
            if (start < 0 || start > items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || start + length > items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            
            if (length == 0)
            {
                return Create<T>();
            }

            if (start == 0 && length == items.Length)
            {
                return items;
            }
            var objArray = new T[length];
            {
                Array.Copy(items.array, start, objArray, 0, length);
            }
            return new ImmutableArray<T>(objArray);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The source array to initialize the resulting array with.
        /// </param>
        /// <param name="selector">
        /// The function to apply to each element from the source array.
        /// </param>
        /// <remarks>
        /// This overload allows efficient creation of an <see cref="ImmutableArray{T}" /> based on an existing
        /// <see cref="ImmutableArray{T}" />, where a mapping function needs to be applied to each element from the
        /// source array.
        /// </remarks>
        public static ImmutableArray<TResult> CreateRange<TSource, TResult>(
            ImmutableArray<TSource> items,
            Func<TSource, TResult> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException((nameof(selector)));
            }
            
            var length = items.Length;
            if (length == 0)
            {
                return Create<TResult>();
            }
            var items1 = new TResult[length];
            for (var index = 0; index < length; ++index)
            {
                items1[index] = selector(items[index]);
            }
            return new ImmutableArray<TResult>(items1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The source array to initialize the resulting array with.
        /// </param>
        /// <param name="start">
        /// The index of the first element in the source array to include in the resulting array.
        /// </param>
        /// <param name="length">
        /// The number of elements from the source array to include in the resulting array.
        /// </param>
        /// <param name="selector">
        /// The function to apply to each element from the source array included in the resulting array.
        /// </param>
        /// <remarks>
        /// This overload allows efficient creation of an <see cref="ImmutableArray{T}" /> based on a slice of an
        /// existing <see cref="ImmutableArray{T}" />, where a mapping function needs to be applied to each element from
        /// the source array included in the resulting array.
        /// </remarks>
        public static ImmutableArray<TResult> CreateRange<TSource, TResult>(
            ImmutableArray<TSource> items,
            int start,
            int length,
            Func<TSource, TResult> selector)
        {
            var currentLength = items.Length;
            if (start < 0 || start > currentLength)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || start + length > currentLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            
            if (length == 0)
            {
                return Create<TResult>();
            }
            var items1 = new TResult[length];
            for (var index = 0; index < length; ++index)
            {
                items1[index] = selector(items[index + start]);
            }
            return new ImmutableArray<TResult>(items1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The source array to initialize the resulting array with.
        /// </param>
        /// <param name="selector">
        /// The function to apply to each element from the source array.
        /// </param>
        /// <param name="arg">
        /// An argument to be passed to the selector mapping function.
        /// </param>
        /// <remarks>
        /// This overload allows efficient creation of an <see cref="ImmutableArray{T}" /> based on an existing
        /// <see cref="ImmutableArray{T}" />, where a mapping function needs to be applied to each element from the
        /// source array.
        /// </remarks>
        public static ImmutableArray<TResult> CreateRange<TSource, TArg, TResult>(
            ImmutableArray<TSource> items,
            Func<TSource, TArg, TResult> selector,
            TArg arg)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            
            var length = items.Length;
            if (length == 0)
            {
                return Create<TResult>();
            }
            var items1 = new TResult[length];
            for (var index = 0; index < length; ++index)
            {
                items1[index] = selector(items[index], arg);
            }
            return new ImmutableArray<TResult>(items1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The source array to initialize the resulting array with.
        /// </param>
        /// <param name="start">
        /// The index of the first element in the source array to include in the resulting array.
        /// </param>
        /// <param name="length">
        /// The number of elements from the source array to include in the resulting array.
        /// </param>
        /// <param name="selector">
        /// The function to apply to each element from the source array included in the resulting array.
        /// </param>
        /// <param name="arg">
        /// An argument to be passed to the selector mapping function.
        /// </param>
        /// <remarks>
        /// This overload allows efficient creation of an <see cref="ImmutableArray{T}" /> based on a slice of an
        /// existing <see cref="ImmutableArray{T}" />, where a mapping function needs to be applied to each element from
        /// the source array included in the resulting array.
        /// </remarks>
        public static ImmutableArray<TResult> CreateRange<TSource, TArg, TResult>(
            ImmutableArray<TSource> items,
            int start,
            int length,
            Func<TSource, TArg, TResult> selector,
            TArg arg)
        {
            var currentLength = items.Length;
            if (start < 0 || start > currentLength)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || start + length > currentLength)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            
            if (length == 0)
            {
                return Create<TResult>();
            }
            var items1 = new TResult[length];
            for (var index = 0; index < length; ++index)
            {
                items1[index] = selector(items[index + start], arg);
            }
            return new ImmutableArray<TResult>(items1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}.Builder" /> class.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements stored in the array.
        /// </typeparam>
        /// <returns>
        /// A new builder.
        /// </returns>
        public static ImmutableArray<T>.Builder CreateBuilder<T>() => Create<T>().ToBuilder();

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}.Builder" /> class.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements stored in the array.
        /// </typeparam>
        /// <param name="initialCapacity">
        /// The size of the initial array backing the builder.
        /// </param>
        /// <returns>
        /// A new builder.
        /// </returns>
        public static ImmutableArray<T>.Builder CreateBuilder<T>(int initialCapacity) 
            => new ImmutableArray<T>.Builder(initialCapacity);

        /// <summary>
        /// Enumerates a sequence exactly once and produces an immutable array of its contents.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of element in the sequence.
        /// </typeparam>
        /// <param name="items">
        /// The sequence to enumerate.
        /// </param>
        /// <returns>
        /// An immutable array.
        /// </returns>
        public static ImmutableArray<TSource> ToImmutableArray<TSource>(this IEnumerable<TSource> items) 
            => items is ImmutableArray<TSource> immutableArray ? immutableArray : CreateRange(items);

        /// <summary>
        /// Searches an entire one-dimensional sorted <see cref="ImmutableArray{T}" /> for a specific element, using the
        /// <see cref="IComparable{T}" /> generic interface implemented by each element of the
        /// <see cref="ImmutableArray{T}" /> and by the specified object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="array">
        /// The sorted, one-dimensional array to search.
        /// </param>
        /// <param name="value">
        /// The object to search for.
        /// </param>
        /// <returns>
        /// <para>
        /// The index of the specified <paramref name="value" /> in the specified array, if <paramref name="value" /> is
        /// found.
        /// </para>
        /// <para>
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in
        /// array, a negative number which is the bitwise complement of the index of the first element that is larger
        /// than <paramref name="value" />.
        /// </para>
        /// <para>
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements
        /// in array, a negative number which is the bitwise complement of (the index of the last element plus 1).
        /// </para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="value" /> does not implement the <see cref="IComparable{T}" /> generic interface, and
        /// the search encounters an element that does not implement the <see cref="IComparable{T}" /> generic
        /// interface.
        /// </exception>
        public static int BinarySearch<T>(this ImmutableArray<T> array, T value) 
            => Array.BinarySearch(array.array, value);

        /// <summary>
        /// Searches an entire one-dimensional sorted <see cref="ImmutableArray{T}" /> for a value using the specified
        /// <see cref="IComparer{T}" /> generic interface.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="array">
        /// The sorted, one-dimensional array to search.
        /// </param>
        /// <param name="value">
        /// The object to search for.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}" /> implementation to use when comparing elements; or <c>null</c> to use the
        /// <see cref="IComparable{T}" /> implementation of each
        /// element.
        /// </param>
        /// <returns>
        /// <para>
        /// The index of the specified <paramref name="value" /> in the specified <paramref name="array"/>, if
        /// <paramref name="value" /> is found.
        /// </para>
        /// <para>
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in
        /// <paramref name="array"/>, a negative number which is the bitwise complement of the index of the first
        /// element that is larger than <paramref name="value" />.
        /// </para>
        /// <para>
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements
        /// in <paramref name="array"/> a negative number which is the bitwise complement of (the index of the last
        /// element plus 1).
        /// </para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="comparer" /> is <c>null</c>, <paramref name="value" /> does not implement the
        /// <see cref="IComparable{T}" /> generic interface, and the search encounters an element that does not
        /// implement the <see cref="IComparable{T}" /> generic interface.
        /// </exception>
        public static int BinarySearch<T>(this ImmutableArray<T> array, T value, IComparer<T> comparer) 
            => Array.BinarySearch(array.array, value, comparer);

        /// <summary>
        /// Searches a range of elements in a one-dimensional sorted <see cref="ImmutableArray{T}" /> for a value, using
        /// the <see cref="IComparable{T}" /> generic interface implemented by each element of the
        /// <see cref="ImmutableArray{T}" /> and by the specified value.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="array">
        /// The sorted, one-dimensional array to search.
        /// </param>
        /// <param name="index">
        /// The starting index of the range to search.
        /// </param>
        /// <param name="length">
        /// The length of the range to search.
        /// </param>
        /// <param name="value">
        /// The object to search for.
        /// </param>
        /// <returns>
        /// The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if
        /// <paramref name="value" /> is found. If <paramref name="value" /> is not found and <paramref name="value" />
        /// is less than one or more elements in <paramref name="array" />, a negative number which is the bitwise
        /// complement of the index of the first element that is larger than <paramref name="value" />.
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements
        /// in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last
        /// element plus 1).
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="value" /> does not implement the <see cref="T:System.IComparable`1" /> generic interface,
        /// and the search encounters an element that does not implement the <see cref="T:System.IComparable`1" />
        /// generic interface.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in
        /// <paramref name="array" />.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <para>
        /// <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
        /// </para>-or-
        /// <para>
        /// <paramref name="length" /> is less than zero.
        /// </para>
        /// </exception>
        public static int BinarySearch<T>(this ImmutableArray<T> array, int index, int length, T value) 
            => Array.BinarySearch(array.array, index, length, value);

        /// <summary>
        /// Searches a range of elements in a one-dimensional sorted <see cref="ImmutableArray{T}" /> for a value, using
        /// the specified <see cref="IComparer{T}" /> generic interface.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element stored in the array.
        /// </typeparam>
        /// <param name="array">
        /// The sorted, one-dimensional array to search.
        /// </param>
        /// <param name="index">
        /// The starting index of the range to search.
        /// </param>
        /// <param name="length">
        /// The length of the range to search.
        /// </param>
        /// <param name="value">
        /// The object to search for.
        /// </param>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}" /> implementation to use when comparing
        /// elements; or null to use the <see cref="IComparable{T}" /> implementation of each
        /// element.
        /// </param>
        /// <returns>
        /// The index of the specified <paramref name="value" /> in the specified <paramref name="array" />, if
        /// <paramref name="value" /> is found.
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is less than one or more elements in
        /// <paramref name="array" />, a negative number which is the bitwise complement of the index of the first
        /// element that is larger than <paramref name="value" />.
        /// If <paramref name="value" /> is not found and <paramref name="value" /> is greater than any of the elements
        /// in <paramref name="array" />, a negative number which is the bitwise complement of (the index of the last
        /// element plus 1).
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="comparer" /> is <c>null</c>, <paramref name="value" /> does not implement the
        /// <see cref="IComparable{T}" /> generic
        /// interface, and the search encounters an element that does not implement the
        /// <see cref="IComparable" /> generic interface.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para>
        /// <paramref name="index" /> and <paramref name="length" /> do not specify a valid range in
        /// <paramref name="array" />.
        /// </para>
        /// -or-
        /// <para>
        /// <paramref name="comparer" /> is <c>null</c>, and <paramref name="value" /> is of a type that is not
        /// compatible with the elements of <paramref name="array" />.
        /// </para>
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <para>
        /// <paramref name="index" /> is less than the lower bound of <paramref name="array" />.
        /// </para>
        /// -or-
        /// <para>
        /// <paramref name="length" /> is less than zero.
        /// </para>
        /// </exception>
        public static int BinarySearch<T>(
            this ImmutableArray<T> array,
            int index,
            int length,
            T value,
            IComparer<T> comparer)
        {
            return Array.BinarySearch(array.array, index, length, value, comparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct.
        /// </summary>
        /// <param name="items">
        /// The array from which to copy.
        /// </param>
        internal static ImmutableArray<T> CreateDefensiveCopy<T>(T[] items)
        {
            if (items.Length == 0)
            {
                return ImmutableArray<T>.Empty;
            }
            var objArray = new T[items.Length];
            Array.Copy(items, 0, objArray, 0, items.Length);
            return new ImmutableArray<T>(objArray);
        }
    }
}