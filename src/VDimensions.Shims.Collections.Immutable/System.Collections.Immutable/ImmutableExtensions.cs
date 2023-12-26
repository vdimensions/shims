using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Immutable
{
    /// <summary>Extension methods for immutable types.</summary>
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    internal static class ImmutableExtensions
    {
        /// <summary>
        /// Tries to divine the number of elements in a sequence without actually enumerating each element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The enumerable source.</param>
        /// <param name="count">Receives the number of elements in the enumeration, if it could be determined.</param>
        /// <returns><c>true</c> if the count could be determined; <c>false</c> otherwise.</returns>
        internal static bool TryGetCount<T>(this IEnumerable<T> sequence, out int count)
        {
            return TryGetCountWithoutEnumerating<T>(sequence, out count);
        }

        /// <summary>
        /// Tries to divine the number of elements in a sequence without actually enumerating each element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The enumerable source.</param>
        /// <param name="count">Receives the number of elements in the enumeration, if it could be determined.</param>
        /// <returns><c>true</c> if the count could be determined; <c>false</c> otherwise.</returns>
        private static bool TryGetCountWithoutEnumerating<T>(this IEnumerable sequence, out int count)
        {
            switch (sequence)
            {
                case ICollection collection:
                    count = collection.Count;
                    return true;
                case ICollection<T> genericCollection:
                    count = genericCollection.Count;
                    return true;
                case IReadOnlyCollection<T> readOnlyCollection:
                    count = readOnlyCollection.Count;
                    return true;
                default:
                    count = 0;
                    return false;
            }
        }

        /// <summary>
        /// Gets the number of elements in the specified sequence,
        /// while guaranteeing that the sequence is only enumerated once
        /// in total by this method and the caller.
        /// </summary>
        /// <typeparam name="T">The type of element in the collection.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <returns>The number of elements in the sequence.</returns>
        internal static int GetCount<T>(ref IEnumerable<T> sequence)
        {
            if (!TryGetCount(sequence, out var count))
            {
                var list = Enumerable.ToList(sequence);
                count = list.Count;
                sequence = list;
            }
            return count;
        }

        /// <summary>Gets a copy of a sequence as an array.</summary>
        /// <typeparam name="T">The type of element.</typeparam>
        /// <param name="sequence">The sequence to be copied.</param>
        /// <param name="count">The number of elements in the sequence.</param>
        /// <returns>The array.</returns>
        /// <remarks>
        /// This is more efficient than the <see cref="M:System.Linq.Enumerable.ToArray``1(System.Collections.Generic.IEnumerable{``0})" /> extension method
        /// because that only tries to cast the sequence to <see cref="T:System.Collections.Generic.ICollection`1" /> to determine
        /// the count before it falls back to reallocating arrays as it enumerates.
        /// </remarks>
        internal static T[] ToArray<T>(this IEnumerable<T> sequence, int count)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var array = new T[count];
            var num = 0;
            foreach (T obj in sequence)
            {
                if (num >= count)
                {
                    throw new ArgumentException();
                }
                array[num++] = obj;
            }
            if (num != count)
            {
                throw new ArgumentException();
            }
            return array;
        }

        /// <summary>
        /// Provides a known wrapper around a sequence of elements that provides the number of elements
        /// and an indexer into its contents.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="sequence">The collection.</param>
        /// <returns>An ordered collection.  May not be thread-safe.  Never null.</returns>
        internal static IOrderedCollection<T> AsOrderedCollection<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }
            switch (sequence)
            {
                case IOrderedCollection<T> orderedCollection:
                    return orderedCollection;
                case IList<T> collection:
                    return new ListOfTWrapper<T>(collection);
                default:
                    return new FallbackWrapper<T>(sequence);
            }
        }

        /// <summary>
        /// Clears the specified stack.  For empty stacks, it avoids the call to <see cref="Stack{T}.Clear" />, which
        /// avoids a call into the runtime's implementation of <see cref="Array.Clear(Array,Int32,Int32)" />, helping performance,
        /// in particular around inlining.  <see cref="Stack{T}.Count" /> typically gets inlined by today's JIT, while
        /// <see cref="Stack{T}.Clear" /> and <see cref="Array.Clear(Array,Int32,Int32)" /> typically don't.
        /// </summary>
        /// <typeparam name="T">Specifies the type of data in the stack to be cleared.</typeparam>
        /// <param name="stack">The stack to clear.</param>
        internal static void ClearFastWhenEmpty<T>(this Stack<T> stack)
        {
            if (stack.Count <= 0)
            {
                return;
            }
            stack.Clear();
        }

        /// <summary>
        /// Gets a disposable enumerable that can be used as the source for a C# foreach loop
        /// that will not box the enumerator if it is of a particular type.
        /// </summary>
        /// <typeparam name="T">The type of value to be enumerated.</typeparam>
        /// <typeparam name="TEnumerator">The type of the Enumerator struct.</typeparam>
        /// <param name="enumerable">The collection to be enumerated.</param>
        /// <returns>A struct that enumerates the collection.</returns>
        internal static DisposableEnumeratorAdapter<T, TEnumerator> GetEnumerableDisposable<T, TEnumerator>(
            this IEnumerable<T> enumerable)
            where TEnumerator: struct, IEnumerator<T>
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            return enumerable is IStrongEnumerable<T, TEnumerator> strongEnumerable 
                ? new DisposableEnumeratorAdapter<T, TEnumerator>(strongEnumerable.GetEnumerator()) 
                : new DisposableEnumeratorAdapter<T, TEnumerator>(enumerable.GetEnumerator());
        }

        /// <summary>
        /// Wraps a <see cref="IList{T}" /> as an ordered collection.
        /// </summary>
        /// <typeparam name="T">The type of element in the collection.</typeparam>
        private class ListOfTWrapper<T> 
            : IOrderedCollection<T>
            , IEnumerable<T>
            , IEnumerable
        {
            /// <summary>The list being exposed.</summary>
            private readonly IList<T> _collection;

            /// <summary>
            /// Initializes a new instance of the <see cref="ListOfTWrapper{T}" /> class.
            /// </summary>
            /// <param name="collection">The collection.</param>
            internal ListOfTWrapper(IList<T> collection)
            {
                _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            }

            /// <summary>Gets the count.</summary>
            public int Count => _collection.Count;

            /// <summary>
            /// Gets the <typeparamref name="T" /> at the specified index.
            /// </summary>
            public T this[int index] => _collection[index];

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
        }

        /// <summary>
        /// Wraps any <see cref="IEnumerable{T}" /> as an ordered, indexable list.
        /// </summary>
        /// <typeparam name="T">The type of element in the collection.</typeparam>
        private class FallbackWrapper<T> : IOrderedCollection<T>, IEnumerable<T>, IEnumerable
        {
            /// <summary>The original sequence.</summary>
            private readonly IEnumerable<T> _sequence;
            /// <summary>The list-ified sequence.</summary>
            private IList<T> _collection;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Collections.Immutable.ImmutableExtensions.FallbackWrapper`1" /> class.
            /// </summary>
            /// <param name="sequence">The sequence.</param>
            internal FallbackWrapper(IEnumerable<T> sequence)
            {
                _sequence = sequence ?? throw new ArgumentNullException(nameof(sequence));
            }

            /// <summary>Gets the count.</summary>
            public int Count
            {
                get
                {
                    if (_collection == null)
                    {
                        if (_sequence.TryGetCount(out var count))
                        {
                            return count;
                        }
                        _collection = _sequence.ToArray();
                    }
                    return _collection.Count;
                }
            }

            /// <summary>
            /// Gets the <typeparamref name="T" /> at the specified index.
            /// </summary>
            public T this[int index]
            {
                get
                {
                    if (_collection == null)
                    {
                        _collection = _sequence.ToArray();
                    }
                    return _collection[index];
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator() => _sequence.GetEnumerator();

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
