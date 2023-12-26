using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
#if !NET40_OR_NEWER
using VDimensions.Shims;
#endif

namespace System.Collections.Immutable
{
    /// <summary>
    /// A readonly array with O(1) indexable lookup time.
    /// </summary>
    /// <typeparam name="T">
    /// The type of element stored by the array.</typeparam>
    /// <devremarks>
    /// This type has a documented contract of being exactly one reference-type field in size.
    /// Our own <see cref="T:System.Collections.Immutable.ImmutableInterlocked" /> class depends on it, as well as
    /// others externally.
    /// <para>
    /// IMPORTANT NOTICE FOR MAINTAINERS AND REVIEWERS:
    /// This type should be thread-safe. As a struct, it cannot protect its own fields
    /// from being changed from one thread while its members are executing on other threads
    /// because structs can change *in place* simply by reassigning the field containing
    /// this struct. Therefore it is extremely important that
    /// ** Every member should only dereference <c>this</c> ONCE. **
    /// If a member needs to reference the array field, that counts as a dereference of <c>this</c>.
    /// Calling other instance members (properties or methods) also counts as dereferencing <c>this</c>.
    /// Any member that needs to use <c>this</c> more than once must instead
    /// assign <c>this</c> to a local variable and use that for the rest of the code instead.
    /// This effectively copies the one field in the struct to a local variable so that
    /// it is insulated from other threads.
    /// </para>
    /// </devremarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public partial struct ImmutableArray<T> 
        : IReadOnlyList<T>
        , IReadOnlyCollection<T>
        , IEnumerable<T>
        , IEnumerable
        , IList<T>
        , ICollection<T>
        , IEquatable<ImmutableArray<T>>
        //, IImmutableList<T>
        , IList
        , ICollection
        , IImmutableArray
        , IStructuralComparable
        , IStructuralEquatable
    {
        /// <summary>
        /// An empty (initialized) instance of <see cref="ImmutableArray{T}" />.
        /// </summary>
        public static readonly ImmutableArray<T> Empty = new ImmutableArray<T>(new T[0]);
        
        /// <summary>
        /// The backing field for this instance. References to this value should never be shared with outside code.
        /// </summary>
        /// <remarks>
        /// This would be private, but we make it internal so that our own extension methods can access it.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        internal T[] array;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct
        /// *without making a defensive copy*.
        /// </summary>
        /// <param name="items">
        /// The array to use. May be <c>null</c> for "default" arrays.
        /// </param>
        internal ImmutableArray(T[] items) => array = items;

        /// <summary>
        /// Checks equality between two instances.
        /// </summary>
        /// <param name="left">
        /// The instance to the left of the operator.
        /// </param>
        /// <param name="right">
        /// The instance to the right of the operator.
        /// </param>
        /// <returns>
        /// <c>true</c> if the values' underlying arrays are reference equal; <c>false</c> otherwise.
        /// </returns>
        [NonVersionable]
        public static bool operator == (ImmutableArray<T> left, ImmutableArray<T> right) => left.Equals(right);

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">
        /// The instance to the left of the operator.
        /// </param>
        /// <param name="right">
        /// The instance to the right of the operator.
        /// </param>
        /// <returns>
        /// <c>true</c> if the values' underlying arrays are reference not equal; <c>false</c> otherwise.
        /// </returns>
        [NonVersionable]
        public static bool operator != (ImmutableArray<T> left, ImmutableArray<T> right) => !left.Equals(right);

        /// <summary>
        /// Checks equality between two instances.
        /// </summary>
        /// <param name="left">
        /// The instance to the left of the operator.
        /// </param>
        /// <param name="right">
        /// The instance to the right of the operator.
        /// </param>
        /// <returns>
        /// <c>true</c> if the values' underlying arrays are reference equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator == (ImmutableArray<T>? left, ImmutableArray<T>? right) 
            => left.GetValueOrDefault().Equals(right.GetValueOrDefault());

        /// <summary>
        /// Checks inequality between two instances.
        /// </summary>
        /// <param name="left">
        /// The instance to the left of the operator.
        /// </param>
        /// <param name="right">
        /// The instance to the right of the operator.
        /// </param>
        /// <returns>
        /// <c>true</c> if the values' underlying arrays are reference not equal; <c>false</c> otherwise.
        /// </returns>
        public static bool operator != (ImmutableArray<T>? left, ImmutableArray<T>? right) 
            => !left.GetValueOrDefault().Equals(right.GetValueOrDefault());

        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get.
        /// </param>
        /// <returns>
        /// The element at the specified index in the read-only list.
        /// </returns>
        public T this[int index]
        {
            [NonVersionable] 
            get => array[index];
        }

        /// <summary>
        /// Gets a value indicating whether this collection is empty.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsEmpty
        {
            [NonVersionable] 
            get => Length == 0;
        }

        /// <summary>
        /// Gets the number of array in the collection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Length
        {
            [NonVersionable] 
            get => array.Length;
        }

        /// <summary>
        /// Gets the number of array in the collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int IReadOnlyCollection<T>.Count
        {
            get
            {
                var immutableArray = this;
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return immutableArray.Length;
            }
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>The element.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                var immutableArray = this;
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return immutableArray[index];
            }
        }

        /// <summary>
        /// Gets a value indicating whether this struct was initialized without an actual array instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDefault => array == null;

        /// <summary>
        /// Gets a value indicating whether this struct is empty or uninitialized.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsDefaultOrEmpty
        {
            get
            {
                var immutableArray = this;
                return immutableArray.array == null || immutableArray.array.Length == 0;
            }
        }

        /// <summary>
        /// Gets the string to display in the debugger watches window for this instance.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                var immutableArray = this;
                if (immutableArray.IsDefault)
                {
                    return "Uninitialized";
                }
                return string.Format(CultureInfo.CurrentCulture, "Length = {0}", immutableArray.Length);
            }
        }

        /// <summary>
        /// Searches the array for the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int IndexOf(T item)
        {
            var immutableArray = this;
            return immutableArray.IndexOf(item, 0, immutableArray.Length, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches the array for the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <param name="equalityComparer">
        /// The equality comparer to use in the search.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int IndexOf(T item, int startIndex, IEqualityComparer<T> equalityComparer)
        {
            var immutableArray = this;
            return immutableArray.IndexOf(item, startIndex, immutableArray.Length - startIndex, equalityComparer);
        }

        /// <summary>
        /// Searches the array for the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int IndexOf(T item, int startIndex)
        {
            var immutableArray = this;
            return immutableArray.IndexOf(item, startIndex, immutableArray.Length - startIndex, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches the array for the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <param name="count">
        /// The number of elements to search.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int IndexOf(T item, int startIndex, int count) 
            => IndexOf(item, startIndex, count, EqualityComparer<T>.Default);

        /// <summary>
        /// Searches the array for the specified item.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <param name="count">
        /// The number of elements to search.
        /// </param>
        /// <param name="equalityComparer">
        /// The equality comparer to use in the search. If <c>null</c>, <see cref="EqualityComparer{T}.Default" /> is
        /// used.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int IndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            if (count == 0 && startIndex == 0)
            {
                return -1;
            }

            if (startIndex < 0 || startIndex >= immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            if (count < 0 || startIndex + count > immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            if (ReferenceEquals(equalityComparer, EqualityComparer<T>.Default))
            {
                return Array.IndexOf(immutableArray.array, item, startIndex, count);
            }
            for (var index = startIndex; index < startIndex + count; ++index)
            {
                if (equalityComparer.Equals(immutableArray.array[index], item))
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Searches the array for the specified item in reverse.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int LastIndexOf(T item)
        {
            var immutableArray = this;
            return immutableArray.Length == 0 
                ? -1 
                : immutableArray.LastIndexOf(item, immutableArray.Length - 1, immutableArray.Length, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches the array for the specified item in reverse.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int LastIndexOf(T item, int startIndex)
        {
            var immutableArray = this;
            return immutableArray.Length == 0 && startIndex == 0 
                ? -1 
                : immutableArray.LastIndexOf(item, startIndex, startIndex + 1, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Searches the array for the specified item in reverse.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <param name="count">
        /// The number of elements to search.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int LastIndexOf(T item, int startIndex, int count) 
            => LastIndexOf(item, startIndex, count, EqualityComparer<T>.Default);

        /// <summary>
        /// Searches the array for the specified item in reverse.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <param name="startIndex">
        /// The index at which to begin the search.
        /// </param>
        /// <param name="count">
        /// The number of elements to search.
        /// </param>
        /// <param name="equalityComparer">
        /// The equality comparer to use in the search.
        /// </param>
        /// <returns>
        /// The 0-based index into the array where the item was found; or <c>-1</c> if it could not be found.
        /// </returns>
        public int LastIndexOf(
            T item,
            int startIndex,
            int count,
            IEqualityComparer<T> equalityComparer)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            if (startIndex == 0 && count == 0)
            {
                return -1;
            }
            if (startIndex < 0 || startIndex >= immutableArray.Length)
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
                return Array.LastIndexOf(immutableArray.array, item, startIndex, count);
            }
            for (var index = startIndex; index >= startIndex - count + 1; --index)
            {
                if (equalityComparer.Equals(item, immutableArray.array[index]))
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="item"/> exists in the array.
        /// </summary>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// <c>true</c> if an equal value was found in the array; <c>false</c> otherwise.
        /// </returns>
        public bool Contains(T item) => this.IndexOf(item) >= 0;

        /// <summary>
        /// Copies the contents of this array to the specified array.
        /// </summary>
        /// <param name="destination">
        /// The array to copy to.
        /// </param>
        public void CopyTo(T[] destination)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            Array.Copy(immutableArray.array, 0, destination, 0, immutableArray.Length);
        }

        /// <summary>
        /// Copies the contents of this array to the specified array.
        /// </summary>
        /// <param name="destination">
        /// The array to copy to.
        /// </param>
        /// <param name="destinationIndex">
        /// The index into the <paramref name="destination"/> array to which the first copied element is written.
        /// </param>
        public void CopyTo(T[] destination, int destinationIndex)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            Array.Copy(immutableArray.array, 0, destination, destinationIndex, immutableArray.Length);
        }

        /// <summary>
        /// Copies the contents of this array to the specified array.
        /// </summary>
        /// <param name="sourceIndex">
        /// The index into this collection of the first element to copy.
        /// </param>
        /// <param name="destination">
        /// The array to copy to.
        /// </param>
        /// <param name="destinationIndex">
        /// The index into the <paramref name="destination"/> array to which the first copied element is written.
        /// </param>
        /// <param name="length">
        /// The number of elements to copy.
        /// </param>
        public void CopyTo(int sourceIndex, T[] destination, int destinationIndex, int length)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            Array.Copy(immutableArray.array, sourceIndex, destination, destinationIndex, length);
        }

        /// <summary>
        /// Returns a new array with the specified value inserted at the specified position.
        /// </summary>
        /// <param name="index">
        /// The 0-based index into the array at which the new item should be added.
        /// </param>
        /// <param name="item">
        /// The item to insert at the start of the array.
        /// </param>
        /// <returns>A new array.</returns>
        public ImmutableArray<T> Insert(int index, T item)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            if (index < 0 || index > immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            
            if (immutableArray.Length == 0)
            {
                return ImmutableArray.Create(item);
            }
            var objArray = new T[immutableArray.Length + 1];
            Array.Copy(immutableArray.array, 0, objArray, 0, index);
            objArray[index] = item;
            Array.Copy(immutableArray.array, index, objArray, index + 1, immutableArray.Length - index);
            return new ImmutableArray<T>(objArray);
        }

        /// <summary>
        /// Inserts the specified values at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index at which to insert the value.
        /// </param>
        /// <param name="items">
        /// The elements to insert.
        /// </param>
        /// <returns>
        /// The new immutable collection.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ImmutableArray<T> InsertRange(int index, IEnumerable<T> items)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            if (index < 0 || index > immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            
            if (immutableArray.Length == 0)
            {
                return ImmutableArray.CreateRange(items);
            }
            var count = ImmutableExtensions.GetCount(ref items);
            if (count == 0)
            {
                return immutableArray;
            }
            var objArray = new T[immutableArray.Length + count];
            Array.Copy(immutableArray.array, 0, objArray, 0, index);
            var num = index;
            foreach (var obj in items)
            {
                objArray[num++] = obj;
            }
            Array.Copy(immutableArray.array, index, objArray, index + count, immutableArray.Length - index);
            return new ImmutableArray<T>(objArray);
        }

        /// <summary>
        /// Inserts the specified values at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index at which to insert the value.
        /// </param>
        /// <param name="items">
        /// The elements to insert.
        /// </param>
        /// <returns>
        /// The new immutable collection.
        /// </returns>
        public ImmutableArray<T> InsertRange(int index, ImmutableArray<T> items)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            ThrowNullRefIfNotInitialized(items);
            if (index >= 0 && index <= immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (immutableArray.IsEmpty)
            {
                return items;
            }
            return items.IsEmpty ? immutableArray : immutableArray.InsertRange(index, items.array);
        }

        /// <summary>
        /// Returns a new array with the specified value inserted at the end.
        /// </summary>
        /// <param name="item">
        /// The item to insert at the end of the array.</param>
        /// <returns>
        /// A new array.
        /// </returns>
        public ImmutableArray<T> Add(T item)
        {
            var immutableArray = this;
            return immutableArray.Length == 0 
                ? ImmutableArray.Create(item) 
                : immutableArray.Insert(immutableArray.Length, item);
        }

        /// <summary>
        /// Adds the specified values to this array.
        /// </summary>
        /// <param name="items">
        /// The values to add.
        /// </param>
        /// <returns>
        /// A new array with the elements added.
        /// </returns>
        public ImmutableArray<T> AddRange(IEnumerable<T> items)
        {
            var immutableArray = this;
            return immutableArray.InsertRange(immutableArray.Length, items);
        }

        /// <summary>
        /// Adds the specified values to this array.
        /// </summary>
        /// <param name="items">
        /// The values to add.
        /// </param>
        /// <returns>
        /// A new array with the elements added.
        /// </returns>
        public ImmutableArray<T> AddRange(ImmutableArray<T> items)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            ThrowNullRefIfNotInitialized(items);
            if (immutableArray.IsEmpty)
            {
                return items;
            }
            return items.IsEmpty ? immutableArray : immutableArray.AddRange(items.array);
        }

        /// <summary>
        /// Returns an array with the item at the specified position replaced.
        /// </summary>
        /// <param name="index">
        /// The index of the item to replace.
        /// </param>
        /// <param name="item">
        /// The new item.
        /// </param>
        /// <returns>
        /// The new array.
        /// </returns>
        public ImmutableArray<T> SetItem(int index, T item)
        {
            var immutableArray = this;
            if (index < 0 || index >= immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            var objArray = new T[immutableArray.Length];
            Array.Copy(immutableArray.array, 0, objArray, 0, immutableArray.Length);
            objArray[index] = item;
            return new ImmutableArray<T>(objArray);
        }

        // /// <summary>
        // /// Replaces the first equal element in the list with the specified element.
        // /// </summary>
        // /// <param name="oldValue">
        // /// The element to replace.
        // /// </param>
        // /// <param name="newValue">
        // /// The element to replace the old element with.
        // /// </param>
        // /// <returns>
        // /// The new list -- even if the value being replaced is equal to the new value for that position.
        // /// </returns>
        // /// <exception cref="ArgumentException">
        // /// Thrown when the old value does not exist in the list.
        // /// </exception>
        // public ImmutableArray<T> Replace(T oldValue, T newValue) 
        //     => Replace(oldValue, newValue, EqualityComparer<T>.Default);

        // /// <summary>
        // /// Replaces the first equal element in the list with the specified element.
        // /// </summary>
        // /// <param name="oldValue">
        // /// The element to replace.
        // /// </param>
        // /// <param name="newValue">
        // /// The element to replace the old element with.
        // /// </param>
        // /// <param name="equalityComparer">
        // /// The equality comparer to use in the search.
        // /// If <c>null</c>, <see cref="EqualityComparer{T}.Default" /> is used.
        // /// </param>
        // /// <returns>
        // /// The new list -- even if the value being replaced is equal to the new value for that position.
        // /// </returns>
        // /// <exception cref="ArgumentException">
        // /// Thrown when the old value does not exist in the list.
        // /// </exception>
        // public ImmutableArray<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer)
        // {
        //     var list = this;
        //     var index = list.IndexOf(oldValue, equalityComparer);
        //     if (index < 0)
        //     {
        //         throw new ArgumentException("Cannot find the old value", nameof (oldValue));
        //     }
        //     return list.SetItem(index, newValue);
        // }

        // /// <summary>
        // /// Returns an array with the first occurrence of the specified element removed from the array.
        // /// If no match is found, the current array is returned.
        // /// </summary>
        // /// <param name="item">
        // /// The item to remove.
        // /// </param>
        // /// <returns>
        // /// The new array.
        // /// </returns>
        // public ImmutableArray<T> Remove(T item) => Remove(item, EqualityComparer<T>.Default);

        // /// <summary>
        // /// Returns an array with the first occurrence of the specified element removed from the array.
        // /// If no match is found, the current array is returned.
        // /// </summary>
        // /// <param name="item">
        // /// The item to remove.
        // /// </param>
        // /// <param name="equalityComparer">
        // /// The equality comparer to use in the search.
        // /// If <c>null</c>, <see cref="EqualityComparer{T}.Default" /> is used.
        // /// </param>
        // /// <returns>
        // /// The new array.
        // /// </returns>
        // public ImmutableArray<T> Remove(T item, IEqualityComparer<T> equalityComparer)
        // {
        //     var list = this;
        //     list.ThrowNullRefIfNotInitialized();
        //     var index = list.IndexOf(item, equalityComparer);
        //     return index >= 0 ? list.RemoveAt(index) : list;
        // }

        /// <summary>
        /// Returns an array with the element at the specified position removed.
        /// </summary>
        /// <param name="index">
        /// The 0-based index into the array for the element to omit from the returned array.
        /// </param>
        /// <returns>
        /// The new array.
        /// </returns>
        public ImmutableArray<T> RemoveAt(int index) => RemoveRange(index, 1);

        /// <summary>
        /// Returns an array with the elements at the specified position removed.
        /// </summary>
        /// <param name="index">
        /// The 0-based index into the array for the element to omit from the returned array.
        /// </param>
        /// <param name="length">
        /// The number of elements to remove.
        /// </param>
        /// <returns>
        /// The new array.
        /// </returns>
        public ImmutableArray<T> RemoveRange(int index, int length)
        {
            var immutableArray = this;
            if (index < 0 || index > immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (length < 0 || index + length > immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            if (length == 0)
            {
                return immutableArray;
            }
            var objArray = new T[immutableArray.Length - length];
            Array.Copy(immutableArray.array, 0, objArray, 0, index);
            Array.Copy(immutableArray.array, index + length, objArray, index, immutableArray.Length - index - length);
            return new ImmutableArray<T>(objArray);
        }

        // /// <summary>
        // /// Removes the specified values from this array.
        // /// </summary>
        // /// <param name="items">
        // /// The items to remove if matches are found in this array.
        // /// </param>
        // /// <returns>
        // /// A new array with the elements removed.
        // /// </returns>
        // public ImmutableArray<T> RemoveRange(IEnumerable<T> items) => RemoveRange(items, EqualityComparer<T>.Default);

        // /// <summary>
        // /// Removes the specified values from this array.
        // /// </summary>
        // /// <param name="items">
        // /// The items to remove if matches are found in this array.
        // /// </param>
        // /// <param name="equalityComparer">
        // /// The equality comparer to use in the search.
        // /// If <c>null</c>, <see cref="EqualityComparer{T}.Default" /> is used.
        // /// </param>
        // /// <returns>
        // /// A new array with the elements removed.
        // /// </returns>
        // [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        // public ImmutableArray<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        // {
        //     var list = this;
        //     list.ThrowNullRefIfNotInitialized();
        //     if (items == null)
        //     {
        //         throw new ArgumentNullException(nameof(items));
        //     }
        //     var indexesToRemove = new SortedSet<int>();
        //     using (var enumerator = items.GetEnumerator())
        //     {
        //         label_5:
        //         while (enumerator.MoveNext())
        //         {
        //             var current = enumerator.Current;
        //             var num = ImmutableList.IndexOf(list, current, equalityComparer);
        //             while (true)
        //             {
        //                 if (num >= 0 && !indexesToRemove.Add(num) && num + 1 < list.Length)
        //                 {
        //                     num = list.IndexOf(current, num + 1, equalityComparer);
        //                 }
        //                 else
        //                 {
        //                     goto label_5;
        //                 }
        //             }
        //         }
        //     }
        //     return list.RemoveAtRange(indexesToRemove);
        // }

        // /// <summary>
        // /// Removes the specified values from this array.
        // /// </summary>
        // /// <param name="items">
        // /// The items to remove if matches are found in this array.
        // /// </param>
        // /// <returns>
        // /// A new array with the elements removed.
        // /// </returns>
        // public ImmutableArray<T> RemoveRange(ImmutableArray<T> items) 
        //     => RemoveRange(items, EqualityComparer<T>.Default);

        // /// <summary>
        // /// Removes the specified values from this list.
        // /// </summary>
        // /// <param name="items">
        // /// The items to remove if matches are found in this list.
        // /// </param>
        // /// <param name="equalityComparer">
        // /// The equality comparer to use in the search.
        // /// </param>
        // /// <returns>
        // /// A new array with the elements removed.
        // /// </returns>
        // public ImmutableArray<T> RemoveRange(ImmutableArray<T> items, IEqualityComparer<T> equalityComparer)
        // {
        //     var immutableArray = this;
        //     if (items.array == null)
        //     {
        //         throw new ArgumentNullException(nameof(items));
        //     }
        //     if (items.IsEmpty)
        //     {
        //         immutableArray.ThrowNullRefIfNotInitialized();
        //         return immutableArray;
        //     }
        //     return items.Length == 1 
        //         ? immutableArray.Remove(items[0], equalityComparer) 
        //         : immutableArray.RemoveRange(items.array, equalityComparer);
        // }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        /// The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements to remove.
        /// </param>
        /// <returns>
        /// The new array.
        /// </returns>
        public ImmutableArray<T> RemoveAll(Predicate<T> match)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();

            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            if (immutableArray.IsEmpty)
            {
                return immutableArray;
            }
            var indexesToRemove = (List<int>) null;
            for (var index = 0; index < immutableArray.array.Length; ++index)
            {
                if (match(immutableArray.array[index]))
                {
                    if (indexesToRemove == null)
                    {
                        indexesToRemove = new List<int>();
                    }
                    indexesToRemove.Add(index);
                }
            }
            return indexesToRemove == null 
                ? immutableArray 
                : immutableArray.RemoveAtRange(indexesToRemove);
        }

        /// <summary>
        /// Returns an empty array.
        /// </summary>
        public ImmutableArray<T> Clear() => Empty;

        /// <summary>
        /// Returns a sorted instance of this array.
        /// </summary>
        public ImmutableArray<T> Sort()
        {
            var immutableArray = this;
            return immutableArray.Sort(0, immutableArray.Length, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="ImmutableArray{T}" /> using the specified
        /// <see cref="Comparison{T}" />.
        /// </summary>
        /// <param name="comparison">
        /// The <see cref="Comparison{T}" /> to use when comparing elements.
        /// </param>
        /// <returns>
        /// The sorted list.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparison" /> is <c>null</c>.
        /// </exception>
        public ImmutableArray<T> Sort(Comparison<T> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }
            return Sort(new ComparisonComparer<T>(comparison));
        }

        /// <summary>
        /// Returns a sorted instance of this array.
        /// </summary>
        /// <param name="comparer">
        /// The comparer to use in sorting. If <c>null</c>, the default comparer is used.
        /// </param>
        public ImmutableArray<T> Sort(IComparer<T> comparer)
        {
            var immutableArray = this;
            return immutableArray.Sort(0, immutableArray.Length, comparer);
        }

        /// <summary>
        /// Returns a sorted instance of this array.
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
        public ImmutableArray<T> Sort(int index, int count, IComparer<T> comparer)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0 || index + count > immutableArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            
            if (count > 1)
            {
                if (comparer == null)
                {
                    comparer = Comparer<T>.Default;
                }
                var flag = false;
                for (var index1 = index + 1; index1 < index + count; ++index1)
                {
                    if (comparer.Compare(immutableArray.array[index1 - 1], immutableArray.array[index1]) > 0)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    var objArray = new T[immutableArray.Length];
                    Array.Copy(immutableArray.array, 0, objArray, 0, immutableArray.Length);
                    Array.Sort(objArray, index, count, comparer);
                    return new ImmutableArray<T>(objArray);
                }
            }
            return immutableArray;
        }

        /// <summary>
        /// Returns a builder that is populated with the same contents as this array.
        /// </summary>
        /// <returns>
        /// The new builder.
        /// </returns>
        public Builder ToBuilder()
        {
            var items = this;
            if (items.Length == 0)
            {
                return new Builder();
            }
            var builder = new Builder(items.Length);
            builder.AddRange(items);
            return builder;
        }

        /// <summary>
        /// Returns an enumerator for the contents of the array.
        /// </summary>
        /// <returns>
        /// An enumerator.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            return new Enumerator(immutableArray.array);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var immutableArray = this;
            return immutableArray.array != null ? immutableArray.array.GetHashCode() : 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) 
            => obj is IImmutableArray immutableArray && array == immutableArray.Array;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
        /// <c>false</c>.
        /// </returns>
        [NonVersionable]
        public bool Equals(ImmutableArray<T> other) => array == other.array;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct based on the contents
        /// of an existing instance, allowing a covariant static cast to efficiently reuse the existing array.
        /// </summary>
        /// <param name="items">
        /// The array to initialize the array with. No copy is made.
        /// </param>
        /// <remarks>
        /// Covariant upcasts from this method may be reversed by calling the
        /// <see cref="ImmutableArray{T}.As{TDerived}" />  or <see cref="ImmutableArray{T}.CastArray{TDerived}" />
        /// method.
        /// </remarks>
        [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
        public static ImmutableArray<T> CastUp<TDerived>(ImmutableArray<TDerived> items) where TDerived: class, T 
            => new ImmutableArray<T>(items.array);

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableArray{T}" /> struct by casting the underlying
        /// array to an array of type <typeparam name="TOther" />.
        /// </summary>
        /// <exception cref="InvalidCastException">
        /// Thrown if the cast is illegal.
        /// </exception>
        public ImmutableArray<TOther> CastArray<TOther>() where TOther: class 
            => new ImmutableArray<TOther>(Enumerable.ToArray(Enumerable.Cast<TOther>(array)));

        /// <summary>
        /// Creates an immutable array for this array, cast to a different element type.
        /// </summary>
        /// <typeparam name="TOther">The type of array element to return.</typeparam>
        /// <returns>
        /// A struct typed for the base element type. If the cast fails, an instance is returned whose
        /// <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </returns>
        /// <remarks>
        /// Arrays of derived elements types can be cast to arrays of base element types without reallocating the array.
        /// These upcasts can be reversed via this same method, casting an array of base element types to their derived
        /// types. However, downcasting is only successful when it reverses a prior upcasting operation.
        /// </remarks>
        public ImmutableArray<TOther> As<TOther>() where TOther : class => new ImmutableArray<TOther>(array as TOther[]);

        /// <summary>
        /// Filters the elements of this array to those assignable to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}" /> that contains elements from the input sequence of type
        /// <typeparamref name="TResult" />.
        /// </returns>
        public IEnumerable<TResult> OfType<TResult>()
        {
            var immutableArray = this;
            return immutableArray.array == null || immutableArray.array.Length == 0 
                ? Enumerable.Empty<TResult>() 
                : immutableArray.array.OfType<TResult>();
        }

        #region IList<T>
        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        /// <summary>
        /// Gets or sets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get.
        /// </param>
        /// <returns>
        /// The element at the specified index in the read-only list.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Always thrown from the setter.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        T IList<T>.this[int index]
        {
            get
            {
                var immutableArray = this;
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return immutableArray[index];
            }
            set => throw new NotSupportedException();
        }
        #endregion IList<T>

        #region ICollection<T>
        void ICollection<T>.Add(T item) => throw new NotSupportedException();

        void ICollection<T>.Clear() => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

        /// <summary>
        /// Gets the number of array in the collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int ICollection<T>.Count
        {
            get
            {
                var immutableArray = this;
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return immutableArray.Length;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<T>.IsReadOnly => true;
        #endregion ICollection<T>

        #region IEnumerable<T>
        /// <summary>
        /// Returns an enumerator for the contents of the array.
        /// </summary>
        /// <returns>
        /// An enumerator.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var immutableArray = this;
            immutableArray.ThrowInvalidOperationIfNotInitialized();
            return EnumeratorObject.Create(immutableArray.array);
        }
        #endregion IEnumerable<T>

        #region IEnumerable
        /// <summary>
        /// Returns an enumerator for the contents of the array.
        /// </summary>
        /// <returns>
        /// An enumerator.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            var immutableArray = this;
            immutableArray.ThrowInvalidOperationIfNotInitialized();
            return EnumeratorObject.Create(immutableArray.array);
        }
        #endregion IEnumerable

        #region IImmutableList<T>
        // IImmutableList<T> IImmutableList<T>.Clear()
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.Clear();
        // }
        //
        // IImmutableList<T> IImmutableList<T>.Add(T value)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.Add(value);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.AddRange(items);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.Insert(index, element);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.InsertRange(index, items);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.Remove(value, equalityComparer);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.RemoveAll(match);
        // }

        // IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.RemoveRange(items, equalityComparer);
        // }

        // IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.RemoveRange(index, count);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.RemoveAt(index);
        // }
        //
        // IImmutableList<T> IImmutableList<T>.SetItem(int index, T value)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.SetItem(index, value);
        // }

        // IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer)
        // {
        //     var immutableArray = this;
        //     immutableArray.ThrowInvalidOperationIfNotInitialized();
        //     return immutableArray.Replace(oldValue, newValue, equalityComparer);
        // }
        #endregion IImmutableList<T>

        #region ICollection
        /// <summary>
        /// Copies the elements of the <see cref="ICollection" /> to an <see cref="Array" />, starting at a particular
        /// <see cref="Array" /> <paramref name="index"/>.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array" /> that is the destination of the elements copied from
        /// <see cref="ICollection" />. The <see cref="Array" /> must have zero-based indexing.
        /// </param>
        /// <param name="index">
        /// The zero-based index in <paramref name="array" /> at which copying begins.
        /// </param>
        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        void ICollection.CopyTo(Array array, int index)
        {
            var immutableArray = this;
            immutableArray.ThrowInvalidOperationIfNotInitialized();
            Array.Copy(immutableArray.array, 0, array, index, immutableArray.Length);
        }

        /// <summary>
        /// Gets the size of the array.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int ICollection.Count
        {
            get
            {
                var immutableArray = this;
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return immutableArray.Length;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection.IsSynchronized => true;

        /// <exception cref="NotSupportedException">
        /// Always thrown.
        /// </exception>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object ICollection.SyncRoot => throw new NotSupportedException();
        #endregion ICollection

        #region IList
        /// <summary>
        /// Adds an item to the <see cref="IList" />.
        /// </summary>
        /// <param name="value">
        /// The object to add to the <see cref="IList" />.
        /// </param>
        /// <returns>
        /// The position into which the new element was inserted, or <c>-1</c> to indicate that the item was not
        /// inserted into the collection,
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Always thrown.
        /// </exception>
        int IList.Add(object value) => throw new NotSupportedException();

        /// <summary>
        /// Removes all items from the <see cref="IList" />.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Always thrown.
        /// </exception>
        void IList.Clear() => throw new NotSupportedException();

        /// <summary>
        /// Determines whether the <see cref="IList" /> contains a specific value.
        /// </summary>
        /// <param name="value">
        /// The object to locate in the <see cref="IList" />.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="Object" /> is found in the <see cref="IList" />; otherwise, <c>false</c>.
        /// </returns>
        bool IList.Contains(object value)
        {
            var immutableArray = this;
            immutableArray.ThrowInvalidOperationIfNotInitialized();
            return immutableArray.Contains((T) value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList" />.
        /// </summary>
        /// <param name="value">
        /// The object to locate in the <see cref="IList" />.
        /// </param>
        /// <returns>
        /// The index of <paramref name="value" /> if found in the list; otherwise, <c>-1</c>.
        /// </returns>
        int IList.IndexOf(object value)
        {
            var immutableArray = this;
            immutableArray.ThrowInvalidOperationIfNotInitialized();
            return immutableArray.IndexOf((T) value);
        }

        /// <summary>
        /// Inserts an item to the <see cref="IList" /> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="value" /> should be inserted.
        /// </param>
        /// <param name="value">
        /// The object to insert into the <see cref="IList" />.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Always thrown.
        /// </exception>
        void IList.Insert(int index, object value) => throw new NotSupportedException();
        
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="IList" />.
        /// </summary>
        /// <param name="value">
        /// The object to remove from the <see cref="IList" />.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Always thrown.
        /// </exception>
        void IList.Remove(object value) => throw new NotSupportedException();

        /// <summary>
        /// Removes the <see cref="IList{T}" /> item at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// Always thrown.
        /// </exception>
        void IList.RemoveAt(int index) => throw new NotSupportedException();

        /// <summary>
        /// Returns an array with items at the specified indexes removed.
        /// </summary>
        /// <param name="indexesToRemove">
        /// A **sorted set** of indexes to elements that should be omitted from the returned array.
        /// </param>
        /// <returns>
        /// The new array.
        /// </returns>
        private ImmutableArray<T> RemoveAtRange(ICollection<int> indexesToRemove)
        {
            var immutableArray = this;
            immutableArray.ThrowNullRefIfNotInitialized();
            if (indexesToRemove == null)
            {
                throw new ArgumentNullException(nameof(indexesToRemove));
            }
            if (indexesToRemove.Count == 0)
            {
                return immutableArray;
            }
            var objArray = new T[immutableArray.Length - indexesToRemove.Count];
            var destinationIndex = 0;
            var num1 = 0;
            var num2 = -1;
            foreach (var num3 in indexesToRemove)
            {
                var length = num2 == -1 ? num3 : num3 - num2 - 1;
                Array.Copy(immutableArray.array, destinationIndex + num1, objArray, destinationIndex, length);
                ++num1;
                destinationIndex += length;
                num2 = num3;
            }
            Array.Copy(immutableArray.array, destinationIndex + num1, objArray, destinationIndex, immutableArray.Length - (destinationIndex + num1));
            return new ImmutableArray<T>(objArray);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fixed size.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
        /// </value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsFixedSize => true;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IList.IsReadOnly => true;

        /// <summary>
        /// Gets or sets the <see cref="Object" /> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Object" />.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">
        /// Always thrown from the setter.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="ImmutableArray{T}.IsDefault" /> property returns true.
        /// </exception>
        object IList.this[int index]
        {
            get
            {
                var immutableArray = this;
                immutableArray.ThrowInvalidOperationIfNotInitialized();
                return immutableArray[index];
            }
            set => throw new NotSupportedException();
        }
        #endregion IList
        
        #region IStructuralEquatable
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            var immutableArray = this;
            var otherArray = other as Array;
            if (otherArray == null && other is IImmutableArray otherImmutableArray)
            {
                if (immutableArray.array == null && otherImmutableArray.Array == null)
                {
                    return true;
                }
                if (immutableArray.array == null)
                {
                    return false;
                }
                otherArray = otherImmutableArray.Array;
            }

            if (otherArray != null && immutableArray.array == null)
            {
                return false;
            }

            #if NET40_OR_NEWER
            IStructuralEquatable arr = immutableArray.array;
            #else
            IStructuralEquatable arr = new StructuralArrayAdapter(immutableArray.array);
            #endif
            return arr.Equals(otherArray, comparer);
        }
        
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            var immutableArray = this;
            #if NET40_OR_NEWER
            IStructuralEquatable arr = immutableArray.array;
            #else
            IStructuralEquatable arr = new StructuralArrayAdapter(immutableArray.array);
            #endif
            return immutableArray.array == null ? immutableArray.GetHashCode() : arr.GetHashCode(comparer);
        }
        #endregion IStructuralEquatable
        
        #region IStructuralComparable
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            var immutableArray = this;
            var otherArray = other as Array;
            if ((otherArray == null) && other is IImmutableArray otherImmutableArray)
            {
                if (immutableArray.array == null && otherImmutableArray.Array == null)
                {
                    return 0;
                }

                if (immutableArray.array == null ^ otherImmutableArray.Array == null)
                {
                    throw new ArgumentException(
                        "Object is not a array with the same initialization state as the array to compare it to.", 
                        nameof(other));
                }
                
                otherArray = otherImmutableArray.Array;
            }

            if (otherArray != null)
            {
                if (immutableArray.array == null)
                {
                    throw new ArgumentException(
                        "Object is not a array with the same initialization state as the array to compare it to.", 
                        nameof(other));
                }
                
                #if NET40_OR_NEWER
                IStructuralComparable arr = immutableArray.array;
                #else
                IStructuralComparable arr = new StructuralArrayAdapter(immutableArray.array);
                #endif
                return arr.CompareTo(otherArray, comparer);
            }
            throw new ArgumentException("Object is not an array with the same number of elements as the array to compare it to.", nameof (other));
        }
        #endregion IStructuralComparable

        /// <summary>
        /// Throws a null reference exception if the array field is null.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedVariable")]
        internal void ThrowNullRefIfNotInitialized()
        {
            var length = array.Length;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException" /> if the <see cref="ImmutableArray{T}.array" /> field is
        /// null, i.e. the
        /// <see cref="ImmutableArray{T}.IsDefault" /> property returns <c>true</c>.
        /// The <see cref="InvalidOperationException" /> message specifies that the operation cannot be performed
        /// on a default instance of <see cref="ImmutableArray{T}" />.
        /// <para>
        /// This is intended for explicitly implemented interface method and property implementations.
        /// </para>
        /// </summary>
        private void ThrowInvalidOperationIfNotInitialized()
        {
            if (IsDefault)
            {
                throw new InvalidOperationException("This operation cannot be performed on a default instance of ImmutableArray<T>. Consider initializing the array, or checking the ImmutableArray<T>.IsDefault property.");
            }
        }

        /// <summary>
        /// Throws a <see cref="NullReferenceException" /> if the specified array is uninitialized.
        /// </summary>
        private static void ThrowNullRefIfNotInitialized(ImmutableArray<T> array) => array.ThrowNullRefIfNotInitialized();

        #region IImmutableArray
        void IImmutableArray.ThrowInvalidOperationIfNotInitialized() => ThrowInvalidOperationIfNotInitialized();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Array IImmutableArray.Array => array;
        #endregion
    }
}