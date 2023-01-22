using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    partial class ImmutableList<T>
    {
        /// <summary>
        /// A simple view of the immutable list that the debugger can show to the developer.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedType.Global")]
        internal sealed class DebuggerProxy : ImmutableCollectionDebugProxy<ImmutableList<T>, T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ImmutableList{T}.DebuggerProxy" /> class.
            /// </summary>
            /// <param name="list">The list to display in the debugger</param>
            public DebuggerProxy(ImmutableList<T> list) : base(list) { }
        
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            protected override int Count => Collection.Count;
        }
    }
}
