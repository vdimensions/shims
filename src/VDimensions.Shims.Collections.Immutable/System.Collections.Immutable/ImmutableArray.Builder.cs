using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    partial struct ImmutableArray<T>
    {
        /// <summary>
        /// A writable array accessor that can be converted into an <see cref="ImmutableArray{T}" />
        /// instance without allocating memory.
        /// </summary>
        [DebuggerDisplay("Count = {Count}")]
        [DebuggerTypeProxy(typeof (ImmutableArray<>.Builder.DebuggerProxy))]
        [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public sealed partial class Builder 
            : IList<T>
            , ICollection<T>
            , IEnumerable<T>
            , IEnumerable
            , IReadOnlyList<T>
            , IReadOnlyCollection<T>
        {
            /// <summary>The backing array for the builder.</summary>
            private T[] _elements;
            /// <summary>The number of initialized elements in the array.</summary>
            private int _count;

            /// <summary>
            /// Initializes a new instance of the <see cref="ImmutableArray{T}.Builder" /> class.
            /// </summary>
            /// <param name="capacity">The initial capacity of the internal array.</param>
            internal Builder(int capacity)
            {
                if (capacity < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(capacity));
                }
                _elements = new T[capacity];
                _count = 0;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ImmutableArray{T}.Builder" /> class.
            /// </summary>
            internal Builder() : this(8) { }

            /// <summary>
            /// Get and sets the length of the internal array.  When set the internal array is
            /// reallocated to the given capacity if it is not already the specified length.
            /// </summary>
            public int Capacity
            {
                get => _elements.Length;
                set
                {
                    if (value < _count)
                    {
                        throw new ArgumentException("Capacity was less than the current Count of elements.", nameof (value));
                    }

                    if (value == _elements.Length)
                    {
                        return;
                    }
                    if (value > 0)
                    {
                        var destinationArray = new T[value];
                        if (_count > 0)
                        {
                            Array.Copy(_elements, 0, destinationArray, 0, _count);
                        }
                        _elements = destinationArray;
                    }
                    else
                    {
                        _elements = Empty.array;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the length of the builder.
            /// </summary>
            /// <remarks>
            /// If the value is decreased, the array contents are truncated.
            /// If the value is increased, the added elements are initialized to the default value of type
            /// <typeparamref name="T" />.
            /// </remarks>
            public int Count
            {
                get => _count;
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }
                    if (value < _count)
                    {
                        if (_count - value > 64)
                        {
                            Array.Clear(_elements, value, _count - value);
                        }
                        else
                        {
                            for (var index = value; index < Count; ++index)
                            {
                                _elements[index] = default(T);
                            }
                        }
                    }
                    else if (value > _count)
                    {
                        EnsureCapacity(value);
                    }
                    _count = value;
                }
            }

            /// <summary>
            /// Gets or sets the element at the specified index.
            /// </summary>
            /// <param name="index">
            /// The index.
            /// </param>
            /// <returns></returns>
            /// <exception cref="IndexOutOfRangeException">
            /// </exception>
            public T this[int index]
            {
                get => index < Count ? _elements[index] : throw new IndexOutOfRangeException();
                set
                {
                    if (index >= Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    _elements[index] = value;
                }
            }

            bool ICollection<T>.IsReadOnly => false;

            /// <summary>
            /// Returns an immutable copy of the current contents of this collection.
            /// </summary>
            /// <returns>An immutable array.</returns>
            public ImmutableArray<T> ToImmutable() => Count == 0 ? Empty : new ImmutableArray<T>(ToArray());

            /// <summary>
            /// Extracts the internal array as an <see cref="ImmutableArray{T}" /> and replaces it
            /// with a zero length array.
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// When <see cref="ImmutableArray{T}.Builder.Count" /> doesn't equal <see cref="ImmutableArray{T}.Builder.Capacity" />.
            /// </exception>
            public ImmutableArray<T> MoveToImmutable()
            {
                if (Capacity != Count)
                {
                    throw new InvalidOperationException("MoveToImmutable can only be performed when Count equals Capacity.");
                }
                var elements = _elements;
                _elements = Empty.array;
                _count = 0;
                return new ImmutableArray<T>(elements);
            }

            /// <summary>
            /// Removes all items from the <see cref="ICollection{T}" />.
            /// </summary>
            public void Clear() => Count = 0;

            /// <summary>
            /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
            /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
            public void Insert(int index, T item)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                EnsureCapacity(Count + 1);
                if (index < Count)
                {
                    Array.Copy(_elements, index, _elements, index + 1, Count - index);
                }
                ++_count;
                _elements[index] = item;
            }

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
            public void Add(T item)
            {
                EnsureCapacity(Count + 1);
                _elements[_count++] = item;
            }

            /// <summary>Adds the specified items to the end of the array.</summary>
            /// <param name="items">The items.</param>
            [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
            public void AddRange(IEnumerable<T> items)
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                if (ImmutableExtensions.TryGetCount(items, out var count))
                {
                    EnsureCapacity(Count + count);
                }
                foreach (var obj in items)
                {
                    Add(obj);
                }
            }

            /// <summary>Adds the specified items to the end of the array.</summary>
            /// <param name="items">The items.</param>
            public void AddRange(params T[] items)
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                var count = Count;
                Count += items.Length;
                Array.Copy(items, 0, _elements, count, items.Length);
            }

            /// <summary>Adds the specified items to the end of the array.</summary>
            /// <param name="items">The items.</param>
            public void AddRange<TDerived>(TDerived[] items) where TDerived : T
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                var count = Count;
                Count += items.Length;
                Array.Copy(items, 0, _elements, count, items.Length);
            }

            /// <summary>Adds the specified items to the end of the array.</summary>
            /// <param name="items">The items.</param>
            /// <param name="length">The number of elements from the source array to add.</param>
            public void AddRange(T[] items, int length)
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                if (length < 0 || length > items.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(length));
                }
                var count = Count;
                Count += length;
                Array.Copy(items, 0, _elements, count, length);
            }

            /// <summary>
            /// Adds the specified items to the end of the array.
            /// </summary>
            /// <param name="items">
            /// The items.
            /// </param>
            public void AddRange(ImmutableArray<T> items) => this.AddRange(items, items.Length);

            /// <summary>
            /// Adds the specified items to the end of the array.
            /// </summary>
            /// <param name="items">
            /// The items.
            /// </param>
            /// <param name="length">
            /// The number of elements from the source array to add.
            /// </param>
            public void AddRange(ImmutableArray<T> items, int length)
            {
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(length));
                }
                if (items.array == null)
                {
                    return;
                }
                AddRange(items.array, length);
            }

            /// <summary>
            /// Adds the specified items to the end of the array.
            /// </summary>
            /// <param name="items">
            /// The items.
            /// </param>
            public void AddRange<TDerived>(ImmutableArray<TDerived> items) where TDerived : T
            {
                if (items.array == null)
                {
                    return;
                }
                AddRange(items.array);
            }

            /// <summary>
            /// Adds the specified items to the end of the array.
            /// </summary>
            /// <param name="items">
            /// The items.
            /// </param>
            public void AddRange(Builder items)
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                AddRange(items._elements, items.Count);
            }

            /// <summary>Adds the specified items to the end of the array.</summary>
            /// <param name="items">The items.</param>
            public void AddRange<TDerived>(ImmutableArray<TDerived>.Builder items) where TDerived : T
            {
                if (items == null)
                {
                    throw new ArgumentNullException(nameof(items));
                }
                AddRange(items._elements, items.Count);
            }

            /// <summary>Removes the specified element.</summary>
            /// <param name="element">The element.</param>
            /// <returns>A value indicating whether the specified element was found and removed from the collection.</returns>
            public bool Remove(T element)
            {
                var index = IndexOf(element);
                if (index < 0)
                {
                    return false;
                }
                RemoveAt(index);
                return true;
            }

            /// <summary>
            /// Removes the <see cref="IList{T}" /> item at the specified index.
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove.</param>
            public void RemoveAt(int index)
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (index < Count - 1)
                {
                    Array.Copy(_elements, index + 1, _elements, index, Count - index - 1);
                }
                --Count;
            }

            /// <summary>
            /// Determines whether the <see cref="ICollection{T}" /> contains a specific value.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="ICollection{T}" />.</param>
            /// <returns>
            /// true if <paramref name="item" /> is found in the <see cref="ICollection{T}" />; otherwise, false.
            /// </returns>
            public bool Contains(T item) => this.IndexOf(item) >= 0;

            /// <summary>
            /// Creates a new array with the current contents of this Builder.
            /// </summary>
            public T[] ToArray()
            {
                var destinationArray = new T[Count];
                Array.Copy(_elements, 0, destinationArray, 0, Count);
                return destinationArray;
            }

            /// <summary>Copies the current contents to the specified array.</summary>
            /// <param name="array">The array to copy to.</param>
            /// <param name="index">The starting index of the target array.</param>
            public void CopyTo(T[] array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (!(index >= 0 && index + Count <= array.Length))
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                Array.Copy(_elements, 0, array, index, Count);
            }

            /// <summary>
            /// Resizes the array to accommodate the specified capacity requirement.
            /// </summary>
            /// <param name="capacity">The required capacity.</param>
            private void EnsureCapacity(int capacity)
            {
                if (_elements.Length >= capacity)
                {
                    return;
                }
                Array.Resize(ref _elements, Math.Max(_elements.Length * 2, capacity));
            }

            /// <summary>
            /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
            /// </summary>
            /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
            /// <returns>
            /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
            /// </returns>
            public int IndexOf(T item) => IndexOf(item, 0, _count, EqualityComparer<T>.Default);

            /// <summary>Searches the array for the specified item.</summary>
            /// <param name="item">The item to search for.</param>
            /// <param name="startIndex">The index at which to begin the search.</param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int IndexOf(T item, int startIndex) => IndexOf(item, startIndex, Count - startIndex, EqualityComparer<T>.Default);

            /// <summary>Searches the array for the specified item.</summary>
            /// <param name="item">The item to search for.</param>
            /// <param name="startIndex">The index at which to begin the search.</param>
            /// <param name="count">The number of elements to search.</param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int IndexOf(T item, int startIndex, int count) => IndexOf(item, startIndex, count, EqualityComparer<T>.Default);

            /// <summary>Searches the array for the specified item.</summary>
            /// <param name="item">The item to search for.</param>
            /// <param name="startIndex">The index at which to begin the search.</param>
            /// <param name="count">The number of elements to search.</param>
            /// <param name="equalityComparer">
            /// The equality comparer to use in the search.
            /// If <c>null</c>, <see cref="P:System.Collections.Generic.EqualityComparer`1.Default" /> is used.
            /// </param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int IndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer)
            {
                if (count == 0 && startIndex == 0)
                {
                    return -1;
                }
                if (startIndex < 0 || startIndex >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                if (count < 0 || startIndex + count > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
                if (ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
                {
                    return Array.IndexOf(_elements, item, startIndex, count);
                }
                for (var index = startIndex; index < startIndex + count; ++index)
                {
                    if (equalityComparer.Equals(_elements[index], item))
                    {
                        return index;
                    }
                }
                return -1;
            }

            /// <summary>Searches the array for the specified item in reverse.</summary>
            /// <param name="item">The item to search for.</param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int LastIndexOf(T item) => Count == 0 ? -1 : LastIndexOf(item, Count - 1, Count, EqualityComparer<T>.Default);

            /// <summary>Searches the array for the specified item in reverse.</summary>
            /// <param name="item">The item to search for.</param>
            /// <param name="startIndex">The index at which to begin the search.</param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int LastIndexOf(T item, int startIndex)
            {
                if (Count == 0 && startIndex == 0)
                {
                    return -1;
                }
                if (startIndex < 0 || startIndex >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                return LastIndexOf(item, startIndex, startIndex + 1, EqualityComparer<T>.Default);
            }

            /// <summary>Searches the array for the specified item in reverse.</summary>
            /// <param name="item">The item to search for.</param>
            /// <param name="startIndex">The index at which to begin the search.</param>
            /// <param name="count">The number of elements to search.</param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int LastIndexOf(T item, int startIndex, int count) => LastIndexOf(item, startIndex, count, EqualityComparer<T>.Default);

            /// <summary>Searches the array for the specified item in reverse.</summary>
            /// <param name="item">The item to search for.</param>
            /// <param name="startIndex">The index at which to begin the search.</param>
            /// <param name="count">The number of elements to search.</param>
            /// <param name="equalityComparer">The equality comparer to use in the search.</param>
            /// <returns>The 0-based index into the array where the item was found; or -1 if it could not be found.</returns>
            public int LastIndexOf(
                T item,
                int startIndex,
                int count,
                IEqualityComparer<T> equalityComparer)
            {
                if (count == 0 && startIndex == 0)
                {
                    return -1;
                }
                if (startIndex < 0 || startIndex >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                if (count < 0 || startIndex - count + 1 < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
                if (ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
                {
                    return Array.LastIndexOf(_elements, item, startIndex, count);
                }
                for (var index = startIndex; index >= startIndex - count + 1; --index)
                {
                    if (equalityComparer.Equals(item, _elements[index]))
                    {
                        return index;
                    }
                }
                return -1;
            }

            /// <summary>Reverses the order of elements in the collection.</summary>
            public void Reverse()
            {
                var index1 = 0;
                var index2 = _count - 1;
                var elements = _elements;
                for (; index1 < index2; --index2)
                {
                    T obj = elements[index1];
                    elements[index1] = elements[index2];
                    elements[index2] = obj;
                    ++index1;
                }
            }

            /// <summary>Sorts the array.</summary>
            public void Sort()
            {
                if (Count <= 1)
                {
                    return;
                }
                Array.Sort(_elements, 0, Count, Comparer<T>.Default);
            }

            /// <summary>
            /// Sorts the elements in the entire array using
            /// the specified <see cref="Comparison{T}" />.
            /// </summary>
            /// <param name="comparison">
            /// The <see cref="Comparison{T}" /> to use when comparing elements.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="comparison" /> is null.
            /// </exception>
            public void Sort(Comparison<T> comparison)
            {
                if (comparison == null)
                {
                    throw new ArgumentNullException(nameof(comparison));
                }

                if (Count <= 1)
                {
                    return;
                }
                Array.Sort(_elements, comparison);
            }

            /// <summary>
            /// Sorts the array.
            /// </summary>
            /// <param name="comparer">
            /// The comparer to use in sorting. If <c>null</c>, the default comparer is used.
            /// </param>
            public void Sort(IComparer<T> comparer)
            {
                if (Count <= 1)
                {
                    return;
                }
                Array.Sort(_elements, 0, _count, comparer);
            }

            /// <summary>
            /// Sorts the array.
            /// </summary>
            /// <param name="index">
            /// The index of the first element to consider in the sort.
            /// </param>
            /// <param name="count">
            /// The number of elements to include in the sort.
            /// </param>
            /// <param name="comparer">
            /// The comparer to use in sorting. If <c>null</c>, the default comparer is used.
            /// </param>
            public void Sort(int index, int count, IComparer<T> comparer)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (count < 0 || index + count > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (count <= 1)
                {
                    return;
                }
                Array.Sort(_elements, index, count, comparer);
            }

            /// <summary>Returns an enumerator for the contents of the array.</summary>
            /// <returns>An enumerator.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                for (var i = 0; i < Count; ++i)
                {
                    yield return this[i];
                }
            }

            /// <summary>Returns an enumerator for the contents of the array.</summary>
            /// <returns>An enumerator.</returns>
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

            /// <summary>Returns an enumerator for the contents of the array.</summary>
            /// <returns>An enumerator.</returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>Adds items to this collection.</summary>
            /// <typeparam name="TDerived">The type of source elements.</typeparam>
            /// <param name="items">The source array.</param>
            /// <param name="length">The number of elements to add to this array.</param>
            private void AddRange<TDerived>(TDerived[] items, int length) where TDerived: T
            {
                EnsureCapacity(Count + length);
                var count = Count;
                Count += length;
                var elements = _elements;
                for (var index = 0; index < length; ++index)
                {
                    elements[count + index] = items[index];
                }
            }
        }
    }
}