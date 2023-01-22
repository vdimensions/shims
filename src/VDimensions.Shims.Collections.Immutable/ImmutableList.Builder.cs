using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace System.Collections.Immutable
{
    partial class ImmutableList<T>
    {
        /// <summary>
        /// A list that mutates with little or no memory allocations,
        /// can produce and/or build on immutable list instances very efficiently.
        /// </summary>
        /// <remarks>
        /// <para>
        /// While <see cref="System.Collections.Immutable.ImmutableList{T}.AddRange(System.Collections.Generic.IEnumerable{T})" />
        /// and other bulk change methods
        /// already provide fast bulk change operations on the collection, this class allows
        /// multiple combinations of changes to be made to a set with equal efficiency.
        /// </para>
        /// <para>
        /// Instance members of this class are <em>not</em> thread-safe.
        /// </para>
        /// </remarks>
        [DebuggerDisplay("Count = {Count}")]
        [DebuggerTypeProxy(typeof (ImmutableList<>.Builder.DebuggerProxy))]
        [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
        [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
        public sealed partial class Builder 
            : IList<T>
            , ICollection<T>
            , IEnumerable<T>
            , IEnumerable
            , IList
            , ICollection
            , IOrderedCollection<T>
            , IImmutableListQueries<T>
            , IReadOnlyList<T>
            , IReadOnlyCollection<T>
        {
            private readonly List<T> _impl;
            private ImmutableList<T> _immutable;
            private int _version;
            private object _syncRoot;

            internal Builder(ImmutableList<T> immutableList)
            {
                if (immutableList == null)
                {
                    throw new ArgumentNullException(nameof(immutableList));
                }
                _impl = new List<T>(immutableList._impl);
                _immutable = immutableList;
            }

            /// <summary>Gets the number of elements in this list.</summary>
            public int Count => _impl.Count;

            /// <summary>
            /// Gets a value indicating whether this instance is read-only.
            /// </summary>
            /// <value>Always <c>false</c>.</value>
            bool ICollection<T>.IsReadOnly => false;

            /// <summary>
            /// Gets the current version of the contents of this builder.
            /// </summary>
            internal int Version => _version;

            internal TResult UpdateVersion<TResult>(in TResult value)
            {
                ++_version;
                _immutable = null;
                return value;
            }

            /// <summary>
            /// Gets or sets the value for a given index into the list.
            /// </summary>
            /// <param name="index">The index of the desired element.</param>
            /// <returns>The value at the specified index.</returns>
            public T this[int index]
            {
                get => _impl[index];
                set => UpdateVersion(_impl[index] = value);
            }

            /// <summary>Gets the element in the collection at a given index.</summary>
            T IOrderedCollection<T>.this[int index] => this[index];

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public int IndexOf(T item) => _impl.IndexOf(item);

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public void Insert(int index, T item)
            {
                _impl.Insert(index, item);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public void RemoveAt(int index)
            {
                _impl.RemoveAt(index);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public void Add(T item)
            {
                _impl.Add(item);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public void Clear()
            {
                _impl.Clear();
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public bool Contains(T item) => _impl.Contains(item);

            /// <summary>
            /// See <see cref="T:System.Collections.Generic.IList`1" />
            /// </summary>
            public bool Remove(T item)
            {
                var index = IndexOf(item);
                if (index < 0)
                {
                    return false;
                }

                var count = _impl.Count;
                _impl.RemoveAt(index);
                return UpdateVersion(_impl.Count < count);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            public List<T>.Enumerator GetEnumerator() => _impl.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => _impl.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator() => _impl.GetEnumerator();

            /// <summary>
            /// Performs the specified action on each element of the list.
            /// </summary>
            /// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the list.</param>
            public void ForEach(Action<T> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                for (var i = 0; i < _impl.Count; i++)
                {
                    action(_impl[i]);
                }
            }

            /// <summary>
            /// Copies the entire ImmutableList&lt;T&gt; to a compatible one-dimensional
            /// array, starting at the beginning of the target array.
            /// </summary>
            /// <param name="array">
            /// The one-dimensional System.Array that is the destination of the elements
            /// copied from ImmutableList&lt;T&gt;. The System.Array must have
            /// zero-based indexing.
            /// </param>
            public void CopyTo(T[] array)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }

                if (array.Length < Count)
                {
                    throw new ArgumentException(nameof(array));
                }
                _impl.CopyTo(array);
            }

            /// <summary>
            /// Copies the entire ImmutableList&lt;T&gt; to a compatible one-dimensional
            /// array, starting at the specified index of the target array.
            /// </summary>
            /// <param name="array">
            /// The one-dimensional System.Array that is the destination of the elements
            /// copied from ImmutableList&lt;T&gt;. The System.Array must have
            /// zero-based indexing.
            /// </param>
            /// <param name="arrayIndex">
            /// The zero-based index in array at which copying begins.
            /// </param>
            public void CopyTo(T[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
  
                if (array.Length < (arrayIndex + Count))
                {
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }
                _impl.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// Copies a range of elements from the ImmutableList&lt;T&gt; to
            /// a compatible one-dimensional array, starting at the specified index of the
            /// target array.
            /// </summary>
            /// <param name="index">
            /// The zero-based index in the source ImmutableList&lt;T&gt; at
            /// which copying begins.
            /// </param>
            /// <param name="array">
            /// The one-dimensional System.Array that is the destination of the elements
            /// copied from ImmutableList&lt;T&gt;. The System.Array must have
            /// zero-based indexing.
            /// </param>
            /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
            /// <param name="count">The number of elements to copy.</param>
            public void CopyTo(int index, T[] array, int arrayIndex, int count) => _impl.CopyTo(index, array, arrayIndex, count);

            /// <summary>
            /// Creates a shallow copy of a range of elements in the source ImmutableList&lt;T&gt;.
            /// </summary>
            /// <param name="index">
            /// The zero-based ImmutableList&lt;T&gt; index at which the range
            /// starts.
            /// </param>
            /// <param name="count">The number of elements in the range.</param>
            /// <returns>
            /// A shallow copy of a range of elements in the source ImmutableList&lt;T&gt;.
            /// </returns>
            public ImmutableList<T> GetRange(int index, int count)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (count < 0 || index + count > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                return new ImmutableList<T>(_impl.GetRange(index, count));
            }

            /// <summary>
            /// Converts the elements in the current ImmutableList&lt;T&gt; to
            /// another type, and returns a list containing the converted elements.
            /// </summary>
            /// <param name="converter">
            /// A System.Converter&lt;TInput,TOutput&gt; delegate that converts each element from
            /// one type to another type.
            /// </param>
            /// <typeparam name="TOutput">
            /// The type of the elements of the target array.
            /// </typeparam>
            /// <returns>
            /// A ImmutableList&lt;T&gt; of the target type containing the converted
            /// elements from the current ImmutableList&lt;T&gt;.
            /// </returns>
            public ImmutableList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter)
            {
                if (converter == null)
                {
                    throw new ArgumentNullException(nameof(converter));
                }
                Converter <T, TOutput> converterDelegate = converter.Invoke;
                return new ImmutableList<TOutput>(_impl.ConvertAll(converterDelegate));
            }

            /// <summary>
            /// Determines whether the ImmutableList&lt;T&gt; contains elements
            /// that match the conditions defined by the specified predicate.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements
            /// to search for.
            /// </param>
            /// <returns>
            /// true if the ImmutableList&lt;T&gt; contains one or more elements
            /// that match the conditions defined by the specified predicate; otherwise,
            /// false.
            /// </returns>
            public bool Exists(Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return _impl.Exists(match);
            }

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the first occurrence within the entire ImmutableList&lt;T&gt;.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the element
            /// to search for.
            /// </param>
            /// <returns>
            /// The first element that matches the conditions defined by the specified predicate,
            /// if found; otherwise, the default value for type T.
            /// </returns>
            public T Find(Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return _impl.Find(match);
            }

            /// <summary>
            /// Retrieves all the elements that match the conditions defined by the specified
            /// predicate.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements
            /// to search for.
            /// </param>
            /// <returns>
            /// A ImmutableList&lt;T&gt; containing all the elements that match
            /// the conditions defined by the specified predicate, if found; otherwise, an
            /// empty ImmutableList&lt;T&gt;.
            /// </returns>
            public ImmutableList<T> FindAll(Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return new ImmutableList<T>(_impl.FindAll(match));
            }

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the zero-based index of the first occurrence within
            /// the entire ImmutableList&lt;T&gt;.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the element
            /// to search for.
            /// </param>
            /// <returns>
            /// The zero-based index of the first occurrence of an element that matches the
            /// conditions defined by match, if found; otherwise, -1.
            /// </returns>
            public int FindIndex(Predicate<T> match) => Helper.FindIndex(_impl, match);

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the zero-based index of the first occurrence within
            /// the range of elements in the ImmutableList&lt;T&gt; that extends
            /// from the specified index to the last element.
            /// </summary>
            /// <param name="startIndex">The zero-based starting index of the search.</param>
            /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
            /// <returns>
            /// The zero-based index of the first occurrence of an element that matches the
            /// conditions defined by match, if found; otherwise, -1.
            /// </returns>
            public int FindIndex(int startIndex, Predicate<T> match) => Helper.FindIndex(_impl, startIndex, match);

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the zero-based index of the first occurrence within
            /// the range of elements in the ImmutableList&lt;T&gt; that starts
            /// at the specified index and contains the specified number of elements.
            /// </summary>
            /// <param name="startIndex">The zero-based starting index of the search.</param>
            /// <param name="count">The number of elements in the section to search.</param>
            /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element to search for.</param>
            /// <returns>
            /// The zero-based index of the first occurrence of an element that matches the
            /// conditions defined by match, if found; otherwise, -1.
            /// </returns>
            public int FindIndex(int startIndex, int count, Predicate<T> match) => Helper.FindIndex(_impl, startIndex, count, match);

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the last occurrence within the entire ImmutableList&lt;T&gt;.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the element
            /// to search for.
            /// </param>
            /// <returns>
            /// The last element that matches the conditions defined by the specified predicate,
            /// if found; otherwise, the default value for type T.
            /// </returns>
            public T FindLast(Predicate<T> match) => Helper.FindLast(_impl, match);

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the zero-based index of the last occurrence within
            /// the entire ImmutableList&lt;T&gt;.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the element
            /// to search for.
            /// </param>
            /// <returns>
            /// The zero-based index of the last occurrence of an element that matches the
            /// conditions defined by match, if found; otherwise, -1.
            /// </returns>
            public int FindLastIndex(Predicate<T> match) => Helper.FindLastIndex(_impl, match);

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the zero-based index of the last occurrence within
            /// the range of elements in the ImmutableList&lt;T&gt; that extends
            /// from the first element to the specified index.
            /// </summary>
            /// <param name="startIndex">The zero-based starting index of the backward search.</param>
            /// <param name="match">The System.Predicate&lt;T&gt; delegate that defines the conditions of the element
            /// to search for.</param>
            /// <returns>
            /// The zero-based index of the last occurrence of an element that matches the
            /// conditions defined by match, if found; otherwise, -1.
            /// </returns>
            public int FindLastIndex(int startIndex, Predicate<T> match) => Helper.FindLastIndex(_impl, startIndex, match);

            /// <summary>
            /// Searches for an element that matches the conditions defined by the specified
            /// predicate, and returns the zero-based index of the last occurrence within
            /// the range of elements in the ImmutableList&lt;T&gt; that contains
            /// the specified number of elements and ends at the specified index.
            /// </summary>
            /// <param name="startIndex">The zero-based starting index of the backward search.</param>
            /// <param name="count">The number of elements in the section to search.</param>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the element
            /// to search for.
            /// </param>
            /// <returns>
            /// The zero-based index of the last occurrence of an element that matches the
            /// conditions defined by match, if found; otherwise, -1.
            /// </returns>
            public int FindLastIndex(int startIndex, int count, Predicate<T> match) => Helper.FindLastIndex(_impl, startIndex, count, match);

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// first occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that extends from the specified index to the last element.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <param name="index">
            /// The zero-based starting index of the search. 0 (zero) is valid in an empty
            /// list.
            /// </param>
            /// <returns>
            /// The zero-based index of the first occurrence of item within the range of
            /// elements in the ImmutableList&lt;T&gt; that extends from index
            /// to the last element, if found; otherwise, -1.
            /// </returns>
            public int IndexOf(T item, int index) => IndexOf(item, index, Count - index, EqualityComparer<T>.Default);

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// first occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that starts at the specified index and contains the specified number of elements.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <param name="index">
            /// The zero-based starting index of the search. 0 (zero) is valid in an empty
            /// list.
            /// </param>
            /// <param name="count">
            /// The number of elements in the section to search.
            /// </param>
            /// <returns>
            /// The zero-based index of the first occurrence of item within the range of
            /// elements in the ImmutableList&lt;T&gt; that starts at index and
            /// contains count number of elements, if found; otherwise, -1.
            /// </returns>
            public int IndexOf(T item, int index, int count) => IndexOf(item, index, count, EqualityComparer<T>.Default);

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// first occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that starts at the specified index and contains the specified number of elements.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <param name="index">
            /// The zero-based starting index of the search. 0 (zero) is valid in an empty
            /// list.
            /// </param>
            /// <param name="count">
            /// The number of elements in the section to search.
            /// </param>
            /// <param name="equalityComparer">
            /// The equality comparer to use in the search.
            /// If <c>null</c>, <see cref="P:System.Collections.Generic.EqualityComparer`1.Default" /> is used.
            /// </param>
            /// <returns>
            /// The zero-based index of the first occurrence of item within the range of
            /// elements in the ImmutableList&lt;T&gt; that starts at index and
            /// contains count number of elements, if found; otherwise, -1.
            /// </returns>
            public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
            {
                return Helper.IndexOf(_impl, item, index, count, equalityComparer);
            }

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// last occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that contains the specified number of elements and ends at the specified
            /// index.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <returns>
            /// The zero-based index of the last occurrence of item within the range of elements
            /// in the ImmutableList&lt;T&gt; that contains count number of elements
            /// and ends at index, if found; otherwise, -1.
            /// </returns>
            public int LastIndexOf(T item) => Count == 0 ? -1 : LastIndexOf(item, Count - 1, Count, EqualityComparer<T>.Default);

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// last occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that contains the specified number of elements and ends at the specified
            /// index.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <param name="startIndex">The zero-based starting index of the backward search.</param>
            /// <returns>
            /// The zero-based index of the last occurrence of item within the range of elements
            /// in the ImmutableList&lt;T&gt; that contains count number of elements
            /// and ends at index, if found; otherwise, -1.
            /// </returns>
            public int LastIndexOf(T item, int startIndex)
            {
                return Count == 0 && startIndex == 0
                    ? -1
                    : LastIndexOf(item, startIndex, startIndex + 1, EqualityComparer<T>.Default);
            }

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// last occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that contains the specified number of elements and ends at the specified
            /// index.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <param name="startIndex">The zero-based starting index of the backward search.</param>
            /// <param name="count">The number of elements in the section to search.</param>
            /// <returns>
            /// The zero-based index of the last occurrence of item within the range of elements
            /// in the ImmutableList&lt;T&gt; that contains count number of elements
            /// and ends at index, if found; otherwise, -1.
            /// </returns>
            public int LastIndexOf(T item, int startIndex, int count) => LastIndexOf(item, startIndex, count, EqualityComparer<T>.Default);

            /// <summary>
            /// Searches for the specified object and returns the zero-based index of the
            /// last occurrence within the range of elements in the ImmutableList&lt;T&gt;
            /// that contains the specified number of elements and ends at the specified
            /// index.
            /// </summary>
            /// <param name="item">
            /// The object to locate in the ImmutableList&lt;T&gt;. The value
            /// can be null for reference types.
            /// </param>
            /// <param name="startIndex">The zero-based starting index of the backward search.</param>
            /// <param name="count">The number of elements in the section to search.</param>
            /// <param name="equalityComparer">The equality comparer to use in the search.</param>
            /// <returns>
            /// The zero-based index of the last occurrence of item within the range of elements
            /// in the ImmutableList&lt;T&gt; that contains count number of elements
            /// and ends at index, if found; otherwise, -1.
            /// </returns>
            public int LastIndexOf(
                T item,
                int startIndex,
                int count,
                IEqualityComparer<T> equalityComparer)
            {
                return Helper.LastIndexOf(_impl, item, startIndex, count, equalityComparer);
            }

            /// <summary>
            /// Determines whether every element in the ImmutableList&lt;T&gt;
            /// matches the conditions defined by the specified predicate.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions to check against
            /// the elements.
            /// </param>
            /// <returns>
            /// true if every element in the ImmutableList&lt;T&gt; matches the
            /// conditions defined by the specified predicate; otherwise, false. If the list
            /// has no elements, the return value is true.
            /// </returns>
            public bool TrueForAll([ValidatedNotNull] Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return _impl.TrueForAll(match);
            }

            /// <summary>
            /// Adds the elements of a sequence to the end of this collection.
            /// </summary>
            /// <param name="items">
            /// The sequence whose elements should be appended to this collection.
            /// The sequence itself cannot be null, but it can contain elements that are
            /// null, if type <typeparamref name="T" /> is a reference type.
            /// </param>
            public void AddRange([ValidatedNotNull] IEnumerable<T> items)
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                
                _impl.AddRange(items);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Inserts the elements of a collection into the ImmutableList&lt;T&gt;
            /// at the specified index.
            /// </summary>
            /// <param name="index">
            /// The zero-based index at which the new elements should be inserted.
            /// </param>
            /// <param name="items">
            /// The collection whose elements should be inserted into the ImmutableList&lt;T&gt;.
            /// The collection itself cannot be null, but it can contain elements that are
            /// null, if type T is a reference type.
            /// </param>
            public void InsertRange(int index, IEnumerable<T> items)
            {
                if (index < 0 || index > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                _impl.InsertRange(index, items);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Removes all the elements that match the conditions defined by the specified
            /// predicate.
            /// </summary>
            /// <param name="match">
            /// The System.Predicate&lt;T&gt; delegate that defines the conditions of the elements
            /// to remove.
            /// </param>
            /// <returns>
            /// The number of elements removed from the ImmutableList&lt;T&gt;
            /// </returns>
            public int RemoveAll(Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return UpdateVersion(_impl.RemoveAll(match));
            }

            /// <summary>
            /// Reverses the order of the elements in the entire ImmutableList&lt;T&gt;.
            /// </summary>
            public void Reverse()
            {
                _impl.Reverse();
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Reverses the order of the elements in the specified range.
            /// </summary>
            /// <param name="index">The zero-based starting index of the range to reverse.</param>
            /// <param name="count">The number of elements in the range to reverse.</param>
            public void Reverse(int index, int count)
            {
                Helper.Reverse(_impl, index, count);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Sorts the elements in the entire ImmutableList&lt;T&gt; using
            /// the default comparer.
            /// </summary>
            public void Sort()
            {
                _impl.Sort();
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Sorts the elements in the entire ImmutableList&lt;T&gt; using
            /// the specified System.Comparison&lt;T&gt;.
            /// </summary>
            /// <param name="comparison">
            /// The <see cref="System.Comparison{T}" /> to use when comparing elements.
            /// </param>
            /// <exception cref="System.ArgumentNullException"><paramref name="comparison" /> is null.</exception>
            public void Sort(Comparison<T> comparison)
            {
                if (comparison == null)
                {
                    throw new ArgumentNullException(nameof(comparison));
                }
                _impl.Sort(comparison);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Sorts the elements in the entire ImmutableList&lt;T&gt; using
            /// the specified comparer.
            /// </summary>
            /// <param name="comparer">
            /// The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing
            /// elements, or null to use <see cref="P:System.Collections.Generic.Comparer`1.Default" />.
            /// </param>
            public void Sort(IComparer<T> comparer)
            {
                _impl.Sort(comparer);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Sorts the elements in a range of elements in ImmutableList&lt;T&gt;
            /// using the specified comparer.
            /// </summary>
            /// <param name="index">
            /// The zero-based starting index of the range to sort.
            /// </param>
            /// <param name="count">The length of the range to sort.</param>
            /// <param name="comparer">
            /// The <see cref="System.Collections.Generic.IComparer{T}" /> implementation to use when comparing
            /// elements, or null to use <see cref="System.Collections.Generic.Comparer{T}.Default" />.
            /// </param>
            public void Sort(int index, int count, IComparer<T> comparer)
            {
                Helper.Sort(_impl, index, count, comparer ?? Comparer<T>.Default);
                UpdateVersion<object>(null);
            }

            /// <summary>
            /// Searches the entire sorted System.Collections.Generic.List&lt;T&gt; for an element
            /// using the default comparer and returns the zero-based index of the element.
            /// </summary>
            /// <param name="item">The object to locate. The value can be null for reference types.</param>
            /// <returns>
            /// The zero-based index of item in the sorted System.Collections.Generic.List&lt;T&gt;,
            /// if item is found; otherwise, a negative number that is the bitwise complement
            /// of the index of the next element that is larger than item or, if there is
            /// no larger element, the bitwise complement of System.Collections.Generic.List&lt;T&gt;.Count.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default cannot
            /// find an implementation of the System.IComparable&lt;T&gt; generic interface or
            /// the System.IComparable interface for type T.
            /// </exception>
            public int BinarySearch(T item) => BinarySearch(item, null);

            /// <summary>
            ///  Searches the entire sorted System.Collections.Generic.List&lt;T&gt; for an element
            ///  using the specified comparer and returns the zero-based index of the element.
            /// </summary>
            /// <param name="item">The object to locate. The value can be null for reference types.</param>
            /// <param name="comparer">
            /// The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing
            /// elements.-or-null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.
            /// </param>
            /// <returns>
            /// The zero-based index of item in the sorted System.Collections.Generic.List&lt;T&gt;,
            /// if item is found; otherwise, a negative number that is the bitwise complement
            /// of the index of the next element that is larger than item or, if there is
            /// no larger element, the bitwise complement of System.Collections.Generic.List&lt;T&gt;.Count.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// comparer is null, and the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default
            /// cannot find an implementation of the System.IComparable&lt;T&gt; generic interface
            /// or the System.IComparable interface for type T.
            /// </exception>
            public int BinarySearch(T item, IComparer<T> comparer) => BinarySearch(0, Count, item, comparer);

            /// <summary>
            /// Searches a range of elements in the sorted System.Collections.Generic.List&lt;T&gt;
            /// for an element using the specified comparer and returns the zero-based index
            /// of the element.
            /// </summary>
            /// <param name="index">The zero-based starting index of the range to search.</param>
            /// <param name="count"> The length of the range to search.</param>
            /// <param name="item">The object to locate. The value can be null for reference types.</param>
            /// <param name="comparer">
            /// The System.Collections.Generic.IComparer&lt;T&gt; implementation to use when comparing
            /// elements, or null to use the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default.
            /// </param>
            /// <returns>
            /// The zero-based index of item in the sorted System.Collections.Generic.List&lt;T&gt;,
            /// if item is found; otherwise, a negative number that is the bitwise complement
            /// of the index of the next element that is larger than item or, if there is
            /// no larger element, the bitwise complement of System.Collections.Generic.List&lt;T&gt;.Count.
            /// </returns>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            /// index is less than 0.-or-count is less than 0.
            /// </exception>
            /// <exception cref="T:System.ArgumentException">
            /// index and count do not denote a valid range in the System.Collections.Generic.List&lt;T&gt;.
            /// </exception>
            /// <exception cref="T:System.InvalidOperationException">
            /// comparer is null, and the default comparer System.Collections.Generic.Comparer&lt;T&gt;.Default
            /// cannot find an implementation of the System.IComparable&lt;T&gt; generic interface
            /// or the System.IComparable interface for type T.
            /// </exception>
            public int BinarySearch(int index, int count, T item, IComparer<T> comparer) => _impl.BinarySearch(index, count, item, comparer);

            /// <summary>
            /// Creates an immutable list based on the contents of this instance.
            /// </summary>
            /// <returns>An immutable list.</returns>
            /// <remarks>
            /// This method is an O(n) operation, and approaches O(1) time as the number of
            /// actual mutations to the set since the last call to this method approaches 0.
            /// </remarks>
            public ImmutableList<T> ToImmutable()
            {
                return _immutable ?? (_immutable = new ImmutableList<T>(_impl));
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <param name="value">The object to add to the <see cref="T:System.Collections.IList" />.</param>
            /// <returns>
            /// The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection,
            /// </returns>
            int IList.Add(object value)
            {
                Add((T) value);
                return Count - 1;
            }

            /// <summary>Clears this instance.</summary>
            void IList.Clear() => Clear();

            /// <summary>
            /// Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.
            /// </summary>
            /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
            /// <returns>
            /// true if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, false.
            /// </returns>
            bool IList.Contains(object value) => Helper.IsCompatibleObject(value) && Contains((T) value);

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
            /// <returns>
            /// The index of <paramref name="value" /> if found in the list; otherwise, -1.
            /// </returns>
            int IList.IndexOf(object value) => Helper.IsCompatibleObject(value) ? IndexOf((T) value) : -1;

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
            /// <param name="value">The object to insert into the <see cref="T:System.Collections.IList" />.</param>
            void IList.Insert(int index, object value) => this.Insert(index, (T) value);

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.
            /// </summary>
            /// <returns>true if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, false.</returns>
            bool IList.IsFixedSize => false;

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
            /// </summary>
            /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
            /// </returns>
            bool IList.IsReadOnly => false;

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.
            /// </summary>
            /// <param name="value">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
            void IList.Remove(object value)
            {
                if (!Helper.IsCompatibleObject(value))
                {
                    return;
                }
                Remove((T) value);
            }

            /// <summary>
            /// Gets or sets the <see cref="T:System.Object" /> at the specified index.
            /// </summary>
            /// <value>
            /// The <see cref="T:System.Object" />.
            /// </value>
            /// <param name="index">The index.</param>
            /// <returns></returns>
            object IList.this[int index]
            {
                get => this[index];
                set => this[index] = (T) value;
            }

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
            /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
            void ICollection.CopyTo(Array array, int arrayIndex) => ((ICollection) _impl).CopyTo(array, arrayIndex);

            /// <summary>
            /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
            /// </summary>
            /// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.</returns>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool ICollection.IsSynchronized => false;

            /// <summary>
            /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
            /// </summary>
            /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            object ICollection.SyncRoot
            {
                get
                {
                    if (_syncRoot == null)
                    {
                        Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                    }
                    return _syncRoot;
                }
            }
        }
    }
}
