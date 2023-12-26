using System.Collections.Generic;

namespace System.Collections.Immutable
{
    /// <summary>
    /// An adapter that allows a single foreach loop in C# to avoid
    /// boxing an enumerator when possible, but fall back to boxing when necessary.
    /// </summary>
    /// <typeparam name="T">The type of value to be enumerated.</typeparam>
    /// <typeparam name="TEnumerator">The type of the enumerator struct.</typeparam>
    internal struct DisposableEnumeratorAdapter<T, TEnumerator> 
        : IDisposable 
        where TEnumerator: struct, IEnumerator<T>
    {
        /// <summary>The enumerator object to use if not null.</summary>
        private readonly IEnumerator<T> _enumeratorObject;
        /// <summary>
        /// The enumerator struct to use if <see cref="F:System.Collections.Immutable.DisposableEnumeratorAdapter`2._enumeratorObject" /> is <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This field must NOT be readonly because the field's value is a struct and must be able to mutate
        /// in-place. A readonly keyword would cause any mutation to take place in a copy rather than the field.
        /// </remarks>
        private TEnumerator _enumeratorStruct;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Immutable.DisposableEnumeratorAdapter`2" /> struct
        /// for enumerating over a strongly typed struct enumerator.
        /// </summary>
        /// <param name="enumerator">The initialized enumerator struct.</param>
        internal DisposableEnumeratorAdapter(TEnumerator enumerator)
        {
            _enumeratorStruct = enumerator;
            _enumeratorObject = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Immutable.DisposableEnumeratorAdapter`2" /> struct
        /// for enumerating over a (boxed) <see cref="T:System.Collections.Generic.IEnumerable`1" /> enumerator.
        /// </summary>
        /// <param name="enumerator">The initialized enumerator object.</param>
        internal DisposableEnumeratorAdapter(IEnumerator<T> enumerator)
        {
            _enumeratorStruct = default;
            _enumeratorObject = enumerator;
        }

        /// <summary>Gets the current enumerated value.</summary>
        public T Current => _enumeratorObject == null ? _enumeratorStruct.Current : _enumeratorObject.Current;

        /// <summary>Moves to the next value.</summary>
        public bool MoveNext() => _enumeratorObject?.MoveNext() ?? _enumeratorStruct.MoveNext();

        /// <summary>Disposes the underlying enumerator.</summary>
        public void Dispose()
        {
            if (_enumeratorObject != null)
            {
                _enumeratorObject.Dispose();
            }
            else
            {
                _enumeratorStruct.Dispose();
            }
        }

        /// <summary>Returns a copy of this struct.</summary>
        /// <remarks>
        /// This member is here so that it can be used in C# foreach loops.
        /// </remarks>
        public DisposableEnumeratorAdapter<T, TEnumerator> GetEnumerator() => this;
    }
}
