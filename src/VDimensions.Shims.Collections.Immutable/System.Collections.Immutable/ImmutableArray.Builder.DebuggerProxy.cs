using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    partial struct ImmutableArray<T>
    {
        partial class Builder
        {
            /// <summary>
            /// A simple view of the immutable collection that the debugger can show to the developer.
            /// </summary>
            [SuppressMessage("ReSharper", "UnusedMember.Global")]
            internal sealed class DebuggerProxy : ImmutableCollectionDebugProxy<Builder, T>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="DebuggerProxy" /> class.
                /// </summary>
                public DebuggerProxy(Builder collection) : base(collection) { }

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                protected override int Count => Collection.Count;
                
                /// <summary>Gets a simple debugger-viewable collection.</summary>
                [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
                public T[] A => Collection.ToArray();
            }
        }
    }
}