using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Immutable
{
    /// <summary>
    /// Represents an immutable list, which is a strongly typed list of objects that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the list.
    /// </typeparam>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(ImmutableList<>.DebuggerProxy))]
    public partial class ImmutableList<T> 
        : IImmutableList<T>
        , IReadOnlyList<T>
        , IReadOnlyCollection<T>
        , IEnumerable<T>
        , IEnumerable
        , IList<T>
        , ICollection<T>
        , IList
        , ICollection
        , IOrderedCollection<T>
        , IImmutableListQueries<T>
        , IStrongEnumerable<T, List<T>.Enumerator>
    {
        private static class Helper
        {
            internal static void CopyTo(in List<T> list, in T[] array)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (array.Length < list.Count)
                {
                    throw new ArgumentException(nameof(array));
                }
                list.CopyTo(array);
            }
            internal static void CopyTo(in List<T> list, in T[] array, in int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }

                if (array.Length < arrayIndex + list.Count)
                {
                    throw new ArgumentException(nameof(array));
                }
                list.CopyTo(array, arrayIndex);
            }
            internal static void CopyTo(in List<T> list, in int index, in T[] array, in int arrayIndex, int count)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (index >= list.Count)
                {
                    throw new ArgumentException(nameof(index));
                }
                if (arrayIndex + count > array.Length)
                {
                    throw new ArgumentException(nameof(count));
                }
                list.CopyTo(index, array, arrayIndex, count);
            }
            
            internal static int FindIndex(in List<T> list, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindIndex(match);
            }

            internal static int FindIndex(in List<T> list, in int startIndex, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                if (startIndex < 0 || startIndex > list.Count)
                {
                    throw new ArgumentNullException(nameof(startIndex));
                }
                return list.FindIndex(startIndex, match);
            }

            internal static int FindIndex(in List<T> list, in int startIndex, in int count, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                if (startIndex < 0)
                {
                    throw new ArgumentNullException(nameof(startIndex));
                }
                if (count < 0 || startIndex + count > list.Count)
                {
                    throw new ArgumentNullException(nameof(count));
                }
                return list.FindIndex(startIndex, count, match);
            }

            internal static T FindLast(in List<T> list, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLast(match);
            }

            internal static int FindLastIndex(in List<T> list, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLastIndex(match);
            }

            internal static int FindLastIndex(in List<T> list, in int startIndex, in Predicate<T> match)
            {
                if (startIndex < 0 || startIndex >= list.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLastIndex(startIndex, match);
            }

            internal static int FindLastIndex(in List<T> list, in int startIndex, in int count, in Predicate<T> match)
            {
                if (startIndex < 0 || (startIndex - count + 1) < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                if (count > list.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLastIndex(startIndex, count, match);
            }
            
            internal static bool IsCompatibleObject(object value)
            {
                if (value is T)
                {
                    return true;
                }
                return value == null && default (T) == null;
            }

            internal static int IndexOf(
                in IList<T> list, 
                in T item, 
                in int index, 
                in int count,
                in IEqualityComparer<T> equalityComparer)
            {
                var ec = equalityComparer ?? EqualityComparer<T>.Default;
                for (var i = index; i < count; i++)
                {
                    if (ec.Equals(list[i], item))
                    {
                        return i;
                    }
                }
                return -1;
            }
            
            internal static int LastIndexOf(
                in IList<T> list, 
                in T item,
                in int startIndex,
                in int count,
                in IEqualityComparer<T> equalityComparer)
            {
                var ec = equalityComparer ?? EqualityComparer<T>.Default;
                for (var i = count - 1; i >= startIndex; i--)
                {
                    if (ec.Equals(list[i], item))
                    {
                        return i;
                    }
                }
                return -1;
            }
            
            internal static void Sort(
                in List<T> list, 
                in int index, 
                in int count, 
                in IComparer<T> comparer)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (list.Count < index + count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                list.Sort(index, count, comparer);
            }
            
            internal static void Reverse(in List<T> list, in int index, in int count)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (list.Count < index + count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                list.Reverse(index, count);
            }
        }
       
        
        /// <summary>
        /// Gets a reference to an empty <see cref="ImmutableList{T}"/> instance.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global")] 
        public static readonly ImmutableList<T> Empty = ImmutableList.Create<T>();
        
        private readonly List<T> _impl;

        internal ImmutableList(List<T> impl) => _impl = impl;

        /// <inheritdoc cref="IImmutableList{T}.Clear"/>
        public ImmutableList<T> Clear() => Empty;
        IImmutableList<T> IImmutableList<T>.Clear() => Empty;
        
        /// <summary>
        /// Searches the entire sorted <see cref="System.Collections.Immutable.ImmutableList{T}" /> for an element
        /// using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <returns>
        /// The zero-based index of item in the sorted <see cref="System.Collections.Immutable.ImmutableList{T}" />,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is
        /// no larger element, the bitwise complement of <see cref="System.Collections.Immutable.ImmutableList{T}.Count" />.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// The default comparer <see cref="System.Collections.Generic.Comparer{T}.Default" /> cannot
        /// find an implementation of the <see cref="System.IComparable{T}" /> generic interface or
        /// the <see cref="System.IComparable" /> interface for type <typeparamref name="T" />.
        /// </exception>
        public int BinarySearch(T item) => BinarySearch(item, null);
        /// <summary>
        /// Searches the entire sorted <see cref="System.Collections.Immutable.ImmutableList{T}" /> for an element
        /// using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">
        /// The <see cref="System.Collections.Generic.IComparer{T}" /> implementation to use when comparing
        /// elements.-or-null to use the default comparer <see cref="System.Collections.Generic.Comparer{T}.Default" />.
        /// </param>
        /// <returns>
        /// The zero-based index of item in the sorted <see cref="System.Collections.Immutable.ImmutableList{T}" />,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is
        /// no larger element, the bitwise complement of <see cref="System.Collections.Immutable.ImmutableList{T}.Count" />.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// <paramref name="comparer" /> is null, and the default comparer <see cref="System.Collections.Generic.Comparer{T}.Default" />
        /// cannot find an implementation of the <see cref="System.IComparable{T}" /> generic interface
        /// or the <see cref="System.IComparable" /> interface for type <typeparamref name="T" />.
        /// </exception>
        public int BinarySearch(T item, IComparer<T> comparer) => BinarySearch(0, Count, item, comparer);
        /// <summary>
        /// Searches a range of elements in the sorted <see cref="System.Collections.Immutable.ImmutableList{T}" />
        /// for an element using the specified comparer and returns the zero-based index
        /// of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count"> The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="comparer">
        /// The <see cref="System.Collections.Generic.IComparer{T}" /> implementation to use when comparing
        /// elements, or null to use the default comparer <see cref="System.Collections.Generic.Comparer{T}.Default" />.
        /// </param>
        /// <returns>
        /// The zero-based index of item in the sorted <see cref="System.Collections.Immutable.ImmutableList{T}" />,
        /// if item is found; otherwise, a negative number that is the bitwise complement
        /// of the index of the next element that is larger than item or, if there is
        /// no larger element, the bitwise complement of <see cref="System.Collections.Immutable.ImmutableList{T}.Count" />.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="index" /> is less than 0.-or-<paramref name="count" /> is less than 0.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in the <see cref="T:System.Collections.Immutable.ImmutableList`1" />.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// <paramref name="comparer" /> is null, and the default comparer <see cref="System.Collections.Generic.Comparer{T}.Default" />
        /// cannot find an implementation of the <see cref="System.IComparable{T}" /> generic interface
        /// or the <see cref="System.IComparable" /> interface for type <typeparamref name="T" />.
        /// </exception>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return _impl.BinarySearch(
                index,
                count,
                item,
                comparer);
        }
        
        /// <summary>
        /// Creates a collection with the same contents as this collection that
        /// can be efficiently mutated across multiple operations using standard
        /// mutable interfaces.
        /// </summary>
        /// <remarks>
        /// This is an O(1) operation and results in only a single (small) memory allocation.
        /// The mutable collection that is returned is *not* thread-safe.
        /// </remarks>
        public Builder ToBuilder() => new Builder(this);
        
        /// <summary>
        /// See the <see cref="System.Collections.Immutable.IImmutableList{T}" /> interface.
        /// </summary>
        public ImmutableList<T> Add(T value) => new ImmutableList<T>(new List<T>(_impl) { value });
        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);
        
        /// <summary>
        /// See the <see cref="System.Collections.Immutable.IImmutableList{T}" /> interface.
        /// </summary>
        public ImmutableList<T> AddRange(IEnumerable<T> items)
        {
            var result = new List<T>();
            result.AddRange(_impl);
            result.AddRange(items);
            return new ImmutableList<T>(result);
        }
        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);
        
        /// <summary>
        /// See the <see cref="System.Collections.Immutable.IImmutableList{T}" /> interface.
        /// </summary>
        public ImmutableList<T> Insert(int index, T element)
        {
            var result = new List<T>(_impl);
            result.Insert(index, element);
            return new ImmutableList<T>(result);
        }
        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

        /// <summary>
        /// See the <see cref="System.Collections.Immutable.IImmutableList{T}" /> interface.
        /// </summary>
        public ImmutableList<T> InsertRange(int index, IEnumerable<T> items)
        {
            var result = new List<T>(_impl);
            result.InsertRange(index, items);
            return new ImmutableList<T>(result);
        }
        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);

        /// <inheritdoc cref="ImmutableList{T}.Remove(T, IEqualityComparer{T})"/>
        public ImmutableList<T> Remove(T value)
        {
            var result = new List<T>(_impl);
            result.Remove(value);
            return new ImmutableList<T>(result);
        }
        /// <inheritdoc cref="IImmutableList{T}.Remove(T, IEqualityComparer{T})"/>
        public ImmutableList<T> Remove(T value, IEqualityComparer<T> equalityComparer)
        {
            var ec = equalityComparer ?? EqualityComparer<T>.Default;
            var indicesToRemove = new List<int>(_impl.Count);
            for (var i = 0; i < _impl.Count; i++)
            {
                if (ec.Equals(value, _impl[i]))
                {
                    indicesToRemove.Add(i);
                }
            }
            var newList = new List<T>(_impl);
            for (var i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                newList.RemoveAt(indicesToRemove[i]);
            }
            return new ImmutableList<T>(newList);
        }
        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) => Remove(value, equalityComparer);
        
        /// <inheritdoc cref="IImmutableList{T}.RemoveRange(Int32,Int32)"/>
        public ImmutableList<T> RemoveRange(int index, int count)
        {
            var result = new List<T>(_impl);
            result.RemoveRange(index, count);
            return new ImmutableList<T>(result);
        }
        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);
        
        /// <inheritdoc cref="IImmutableList{T}.RemoveRange(IEnumerable{T}, IEqualityComparer{T})"/>
        public ImmutableList<T> RemoveRange([ValidatedNotNull] IEnumerable<T> items) => RemoveRange(items, null);
        
        /// <inheritdoc cref="IImmutableList{T}.RemoveRange(IEnumerable{T}, IEqualityComparer{T})"/>
        public ImmutableList<T> RemoveRange([ValidatedNotNull] IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            var ec = equalityComparer ?? EqualityComparer<T>.Default;
            var indicesToRemove = new List<int>(_impl.Count);
            foreach (var item in items)
            {
                for (var i = 0; i < _impl.Count; i++)
                {
                    if (ec.Equals(item, _impl[i]))
                    {
                        indicesToRemove.Add(i);
                    }
                }
            }
            
            indicesToRemove.Sort(); // ensure removal begins from the largest index first
            
            var newList = new List<T>(_impl);
            for (var i = indicesToRemove.Count - 1; i >= 0; i--)
            {
                newList.RemoveAt(indicesToRemove[i]);
            }
            return new ImmutableList<T>(newList);
        }
        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) 
            => RemoveRange(items, equalityComparer);

        /// <inheritdoc cref="IImmutableList{T}.RemoveAt" />
        public ImmutableList<T> RemoveAt(int index)
        {
            var result = new List<T>(_impl);
            result.RemoveAt(index);
            return new ImmutableList<T>(result);
        }
        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);
        
        /// <inheritdoc cref="IImmutableList{T}.RemoveAll"/>
        public ImmutableList<T> RemoveAll([ValidatedNotNull] Predicate<T> match)
        {
            var result = new List<T>(_impl);
            result.RemoveAll(match);
            return new ImmutableList<T>(result);
        }
        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

        /// <inheritdoc cref="IImmutableList{T}.SetItem" />
        public ImmutableList<T> SetItem(int index, T value) => new ImmutableList<T>(new List<T>(_impl) { [index] = value} );
        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);

        /// <inheritdoc cref="IImmutableList{T}.Replace"/>
        public ImmutableList<T> Replace(
            T oldValue, 
            T newValue, 
            IEqualityComparer<T> equalityComparer)
        {
            var newList = new List<T>(_impl);
            var indexOfOldValue = Helper.IndexOf(_impl, oldValue, 0, _impl.Count, equalityComparer);
            if (indexOfOldValue >= 0)
            {
                newList[indexOfOldValue] = newValue;
                return new ImmutableList<T>(newList);
            }
            return this;
        }
        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) => Replace(oldValue, newValue, equalityComparer);

        /// <summary>
        /// Sorts the elements in the entire <see cref="System.Collections.Immutable.ImmutableList{T}" /> using
        /// the default comparer.
        /// </summary>
        public ImmutableList<T> Sort()
        {
            var newList = new List<T>(_impl);
            newList.Sort();
            return new ImmutableList<T>(newList);
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="System.Collections.Immutable.ImmutableList{T}" /> using
        /// the specified <see cref="System.Comparison{T}" />.
        /// </summary>
        /// <param name="comparison">
        /// The <see cref="System.Comparison{T}" /> to use when comparing elements.
        /// </param>
        /// <returns>The sorted list.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="comparison" /> is null.</exception>
        public ImmutableList<T> Sort([ValidatedNotNull] Comparison<T> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }

            var newList = new List<T>(_impl);
            newList.Sort(comparison);
            return new ImmutableList<T>(newList);
        }
    
        /// <summary>
        /// Sorts the elements in the entire <see cref="System.Collections.Immutable.ImmutableList{T}" /> using
        /// the specified comparer.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="System.Collections.Generic.IComparer{T}" /> implementation to use when comparing
        /// elements, or null to use the default comparer <see cref="System.Collections.Generic.Comparer{T}.Default" />.
        /// </param>
        /// <returns>The sorted list.</returns>
        public ImmutableList<T> Sort(IComparer<T> comparer)
        {
            var c = comparer ?? Comparer<T>.Default;
            var newList = new List<T>(_impl);
            newList.Sort(c);
            return new ImmutableList<T>(newList);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in <see cref="T:System.Collections.Immutable.ImmutableList`1" />
        /// using the specified comparer.
        /// </summary>
        /// <param name="index">
        /// The zero-based starting index of the range to sort.
        /// </param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">
        /// The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing
        /// elements, or null to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" />.
        /// </param>
        /// <returns>The sorted list.</returns>
        public ImmutableList<T> Sort(int index, int count, IComparer<T> comparer)
        {
            var newList = new List<T>(_impl);
            Helper.Sort(newList, index, count, comparer);
            return new ImmutableList<T>(newList);
        }

        /// <summary>
        /// Performs the specified action on each element of the list.
        /// </summary>
        /// <param name="action">
        /// The System.Action&lt;T&gt; delegate to perform on each element of the list.
        /// </param>
        public void ForEach([ValidatedNotNull] Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            _impl.ForEach(action);
        }
        
        /// <summary>
        /// Copies the entire <see cref="System.Collections.Immutable.ImmutableList{T}" /> to a compatible one-dimensional
        /// array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="System.Array" /> that is the destination of the elements
        /// copied from <see cref="System.Collections.Immutable.ImmutableList{T}" />. The <see cref="System.Array" /> must have
        /// zero-based indexing.
        /// </param>
        public void CopyTo([ValidatedNotNull] T[] array) => Helper.CopyTo(_impl, array);

        /// <summary>
        /// Copies the entire <see cref="T:System.Collections.Immutable.ImmutableList`1" /> to a compatible one-dimensional
        /// array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements
        /// copied from <see cref="T:System.Collections.Immutable.ImmutableList`1" />. The <see cref="T:System.Array" /> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in array at which copying begins.
        /// </param>
        public void CopyTo([ValidatedNotNull] T[] array, int arrayIndex) => Helper.CopyTo(_impl, array, arrayIndex);

        /// <summary>
        /// Copies a range of elements from the <see cref="ImmutableList{T}" /> to
        /// a compatible one-dimensional array, starting at the specified index of the
        /// target array.
        /// </summary>
        /// <param name="index">
        /// The zero-based index in the source <see cref="ImmutableList{T}" /> at
        /// which copying begins.
        /// </param>
        /// <param name="array">
        /// The one-dimensional <see cref="Array" /> that is the destination of the elements
        /// copied from <see cref="ImmutableList{T}" />. The <see cref="Array" /> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count) => Helper.CopyTo(_impl, index, array, arrayIndex, count);
        
        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="ImmutableList{T}" />.
        /// </summary>
        /// <param name="index">
        /// The zero-based <see cref="ImmutableList{T}" /> index at which the range
        /// starts.
        /// </param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>
        /// A shallow copy of a range of elements in the source <see cref="ImmutableList{T}" />.
        /// </returns>
        public ImmutableList<T> GetRange(int index, int count)
        {
            if (index < 0 || index >= _impl.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0 || (index + count) > _impl.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var newList = new List<T>(_impl.Count);
            for (var i = index; i < count; i++)
            {
                newList.Add(_impl[i]);
            }
            return new ImmutableList<T>(newList);
        }
        
        /// <summary>
        /// Converts the elements in the current <see cref="ImmutableList{T}" /> to
        /// another type, and returns a list containing the converted elements.
        /// </summary>
        /// <param name="converter">
        /// A <see cref="Func{T, TOutput}" /> delegate that converts each element from
        /// one type to another type.
        /// </param>
        /// <typeparam name="TOutput">
        /// The type of the elements of the target array.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ImmutableList{TOutput}" /> of the target type containing the converted
        /// elements from the current <see cref="ImmutableList{T}" />.
        /// </returns>
        public ImmutableList<TOutput> ConvertAll<TOutput>([ValidatedNotNull] Func<T, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }
            return new ImmutableList<TOutput>(_impl.ConvertAll(converter.Invoke));
        }

        /// <summary>
        /// Determines whether the <see cref="ImmutableList{T}" /> contains elements
        /// that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements
        /// to search for.
        /// </param>
        /// <returns>
        /// true if the <see cref="ImmutableList{T}" /> contains one or more elements
        /// that match the conditions defined by the specified predicate; otherwise,
        /// false.
        /// </returns>
        public bool Exists([ValidatedNotNull] Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }
            return _impl.Exists(match);
        }
        
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the first occurrence within the entire <see cref="ImmutableList{T}" />.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}" /> delegate that defines the conditions of the element
        /// to search for.
        /// </param>
        /// <returns>
        /// The first element that matches the conditions defined by the specified predicate,
        /// if found; otherwise, the default value for type <typeparamref name="T" />.
        /// </returns>
        public T Find([ValidatedNotNull] Predicate<T> match)
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
        /// The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements
        /// to search for.
        /// </param>
        /// <returns>
        /// A <see cref="ImmutableList{T}" /> containing all the elements that match
        /// the conditions defined by the specified predicate, if found; otherwise, an
        /// empty <see cref="ImmutableList{T}" />.
        /// </returns>
        public ImmutableList<T> FindAll([ValidatedNotNull] Predicate<T> match)
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
        /// the entire <see cref="ImmutableList{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        /// to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the
        /// conditions defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindIndex([ValidatedNotNull] Predicate<T> match) => Helper.FindIndex(_impl, match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within
        /// the range of elements in the <see cref="ImmutableList{T}"/> that extends
        /// from the specified index to the last element.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the
        /// conditions defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindIndex(int startIndex, [ValidatedNotNull] Predicate<T> match) => Helper.FindIndex(_impl, startIndex, match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within
        /// the range of elements in the <see cref="ImmutableList{T}"/> that starts
        /// at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>
        /// The zero-based index of the first occurrence of an element that matches the
        /// conditions defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindIndex(int startIndex, int count, [ValidatedNotNull] Predicate<T> match) => Helper.FindIndex(_impl, startIndex, count, match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the last occurrence within the entire <see cref="ImmutableList{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        /// to search for.
        /// </param>
        /// <returns>
        /// The last element that matches the conditions defined by the specified predicate,
        /// if found; otherwise, the default value for type T.
        /// </returns>
        public T FindLast([ValidatedNotNull] Predicate<T> match) => Helper.FindLast(_impl, match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the last occurrence within
        /// the entire <see cref="ImmutableList{T}"/>.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        /// to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that matches the
        /// conditions defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindLastIndex([ValidatedNotNull] Predicate<T> match) => Helper.FindLastIndex(_impl, match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the last occurrence within
        /// the range of elements in the <see cref="ImmutableList{T}"/>; that extends
        /// from the first element to the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        /// to search for.</param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that matches the
        /// conditions defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindLastIndex(int startIndex, Predicate<T> match) => Helper.FindLastIndex(_impl, startIndex, match);

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the last occurrence within
        /// the range of elements in the <see cref="ImmutableList{T}"/>; that contains
        /// the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">
        /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
        /// to search for.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of an element that matches the
        /// conditions defined by match, if found; otherwise, -1.
        /// </returns>
        public int FindLastIndex(int startIndex, int count, [ValidatedNotNull] Predicate<T> match) => Helper.FindLastIndex(_impl, startIndex, count, match);
        
        /// <summary>
        /// See the <see cref="T:System.Collections.Immutable.IImmutableList`1" /> interface.
        /// </summary>
        public int IndexOf(T value) => _impl.IndexOf(value);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// first occurrence within the range of elements in the <see cref="ImmutableList{T}" />
        /// that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ImmutableList{T}" />. The value
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
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref name="item" /> within the range of
        /// elements in the <see cref="ImmutableList{T}" /> that starts at <paramref name="index" /> and
        /// contains <paramref name="count" /> number of elements, if found; otherwise, -1.
        /// </returns>
        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            return Helper.IndexOf(_impl, item, index, count, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the
        /// last occurrence within the range of elements in the <see cref="ImmutableList{T}" />
        /// that contains the specified number of elements and ends at the specified
        /// index.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ImmutableList{T}" />. The value
        /// can be null for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="equalityComparer">
        /// The equality comparer to use in the search.
        /// </param>
        /// <returns>
        /// The zero-based index of the last occurrence of <paramref name="item" /> within the range of elements
        /// in the <see cref="ImmutableList{T}" /> that contains <paramref name="count" /> number of elements
        /// and ends at <paramref name="index" />, if found; otherwise, -1.
        /// </returns>
        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            return Helper.LastIndexOf(_impl, item, index, count, equalityComparer);
        }
        
        /// <summary>
        /// Determines whether every element in the <see cref="ImmutableList{T}" />
        /// matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}" /> delegate that defines the conditions to check against
        /// the elements.
        /// </param>
        /// <returns>
        /// true if every element in the <see cref="ImmutableList{T}" /> matches the
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
        /// See the <see cref="IImmutableList{T}" /> interface.
        /// </summary>
        public bool Contains(T item) => _impl.Contains(item);


        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public List<T>.Enumerator GetEnumerator() => _impl.GetEnumerator();
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _impl.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _impl).GetEnumerator();
        
        /// <inheritdoc cref="Enumerable.Reverse{T}" />
        public ImmutableList<T> Reverse() => new ImmutableList<T>(Enumerable.ToList(Enumerable.Reverse(_impl)));

        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <returns>The reversed list.</returns>
        public ImmutableList<T> Reverse(int index, int count)
        {
            var newList = new List<T>(_impl);
            Helper.Reverse(newList, index, count);
            return new ImmutableList<T>(newList);
        }

        /// <inheritdoc cref="IOrderedCollection{T}.Count" />
        public int Count => _impl.Count;

        /// <summary>
        /// Gets a value indicating whether the current collection is empty.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsEmpty => _impl.Count == 0;

        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        public T this[int index] => _impl[index];
        T IOrderedCollection<T>.this[int index] => this[index];
        
        #region IList<T>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        /// <exception cref="NotSupportedException">Always thrown from the setter.</exception>
        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }
        #endregion IList<T>
        
        #region IList
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        int IList.Add(object item) => throw new NotSupportedException();
        
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void IList.Clear() => throw new NotSupportedException();
        
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void IList.Remove(object item) => throw new NotSupportedException();
        
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
        
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void IList.Insert(int index, object item) => throw new NotSupportedException();

        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void IList.RemoveAt(int index) => throw new NotSupportedException();

        bool IList.IsFixedSize => false;
        bool IList.IsReadOnly => true;

        /// <summary>Gets or sets the value at the specified index.</summary>
        /// <exception cref="T:System.IndexOutOfRangeException">Thrown from getter when <paramref name="index" /> is negative or not less than <see cref="P:System.Collections.Immutable.ImmutableList`1.Count" />.</exception>
        /// <exception cref="T:System.NotSupportedException">Always thrown from the setter.</exception>
        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }
        #endregion IList
        
        #region ICollection<T>
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void ICollection<T>.Add(T item) => throw new NotSupportedException();
        
        /// <exception cref="NotSupportedException">Always thrown.</exception>
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

        /// <exception cref="NotSupportedException">Always thrown.</exception>
        void ICollection<T>.Clear() => throw new NotSupportedException();
        
        bool ICollection<T>.IsReadOnly => true;
        #endregion ICollection<T>
        
        #region ICollection
        void ICollection.CopyTo(Array array, int index) => ((ICollection) _impl).CopyTo(array, index);
        
        bool ICollection.IsSynchronized => true;
        object ICollection.SyncRoot => this;
        #endregion ICollection
    }
}
