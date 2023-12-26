using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    partial struct ImmutableArray<T>
    {
        /// <summary>
        /// An array enumerator.
        /// </summary>
        /// <remarks>
        /// It is important that this enumerator does NOT implement <see cref="IDisposable" />.
        /// We want the iterator to inline when we do foreach and to not result in a try/finally frame in the client.
        /// </remarks>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public struct Enumerator
        {
            /// <summary>The array being enumerated.</summary>
            private readonly T[] _array;
            
            /// <summary>The currently enumerated position.</summary>
            /// <value>
            /// <c>-1</c> before the first call to <see cref="MoveNext" />.
            /// <c>&gt;= this.array.Length</c> after <see cref="MoveNext" /> returns <c>false</c>.
            /// </value>
            private int _index;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator" /> struct.
            /// </summary>
            /// <param name="array">
            /// The array to enumerate.
            /// </param>
            internal Enumerator(T[] array)
            {
                _array = array;
                _index = -1;
            }

            /// <summary>
            /// Advances to the next value to be enumerated.
            /// </summary>
            /// <returns>
            /// <c>true</c> if another item exists in the array; <c>false</c> otherwise.
            /// </returns>
            public bool MoveNext() => ++_index < _array.Length;

            /// <summary>
            /// Gets the currently enumerated value.
            /// </summary>
            public T Current => _array[_index];
        }
        
        /// <summary>
        /// An array enumerator that implements <see cref="IEnumerator{T}" /> pattern
        /// (including <see cref="IDisposable" />).
        /// </summary>
        [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
        private class EnumeratorObject : IEnumerator<T>, IEnumerator, IDisposable
        {
            /// <summary>
            /// A shareable singleton for enumerating empty arrays.
            /// </summary>
            private static readonly IEnumerator<T> EmptyEnumerator = new EnumeratorObject(Empty.array);
            
            /// <summary>The array being enumerated.</summary>
            private readonly T[] _array;
            
            /// <summary>The currently enumerated position.</summary>
            /// <value>
            /// <c>-1</c> before the first call to <see cref="ImmutableArray{T}.EnumeratorObject.MoveNext" />.
            /// <c>this.array.Length - 1</c> after <see cref="MoveNext"/> returns <c>false</c>.
            /// </value>
            private int _index;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Collections.Immutable.ImmutableArray`1.Enumerator" /> class.
            /// </summary>
            private EnumeratorObject(T[] array)
            {
                _index = -1;
                _array = array;
            }

            /// <summary>Gets the currently enumerated value.</summary>
            public T Current
            {
                get
                {
                    if ((uint) _index < (uint) _array.Length)
                    {
                        return _array[_index];
                    }
                    throw new InvalidOperationException();
                }
            }

            /// <summary>Gets the currently enumerated value.</summary>
            object IEnumerator.Current => Current;

            /// <summary>
            /// If another item exists in the array, advances to the next value to be enumerated.
            /// </summary>
            /// <returns>
            /// <c>true</c> if another item exists in the array; <c>false</c> otherwise.
            /// </returns>
            public bool MoveNext()
            {
                var num = _index + 1;
                var length = _array.Length;
                if ((uint) num > (uint) length)
                {
                    return false;
                }
                _index = num;
                return (uint) num < (uint) length;
            }

            /// <summary>Resets enumeration to the start of the array.</summary>
            void IEnumerator.Reset() => _index = -1;

            /// <summary>
            /// Disposes this enumerator.
            /// </summary>
            /// <remarks>
            /// Currently has no action.
            /// </remarks>
            public void Dispose() { }

            /// <summary>
            /// Creates an enumerator for the specified array.
            /// </summary>
            internal static IEnumerator<T> Create(T[] array) 
                => array.Length != 0 ? new EnumeratorObject(array) : EmptyEnumerator;
        }
    }
}