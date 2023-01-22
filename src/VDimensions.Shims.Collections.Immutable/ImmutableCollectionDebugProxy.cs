using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal abstract class ImmutableCollectionDebugProxy<TCollection, T>
        where TCollection: class, IEnumerable<T>
    {
        /// <summary>The collection to be enumerated.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected readonly TCollection Collection;
        /// <summary>The simple view of the collection.</summary>
        private T[] _cachedContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableCollectionDebugProxy{TCollection,T}" /> class.
        /// </summary>
        /// <param name="collection">The list to display in the debugger</param>
        protected ImmutableCollectionDebugProxy(TCollection collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <summary>Gets a simple debugger-viewable list.</summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Contents => _cachedContents ?? (_cachedContents = ImmutableExtensions.ToArray<T>(Collection, Count));
        
        protected abstract int Count { get; }
    }
}