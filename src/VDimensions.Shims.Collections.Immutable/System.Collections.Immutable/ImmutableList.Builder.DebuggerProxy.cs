using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
    partial class ImmutableList<T>
    {
        partial class Builder 
        {
            /// <summary>
            /// A simple view of the immutable list that the debugger can show to the developer.
            /// </summary>
            internal sealed class DebuggerProxy : ImmutableCollectionDebugProxy<Builder, T>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ImmutableList{T}.Builder.DebuggerProxy" /> class.
                /// </summary>
                /// <param name="builder">The list to display in the debugger</param>
                public DebuggerProxy(Builder builder) : base(builder) { }

                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                protected override int Count => Collection.Count;
            }
        }
    }
}
