using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Immutable
{
    /// <summary>
    /// An <see cref="System.Collections.Generic.IEnumerator{T}" />-like interface that does not derive from
    /// <see cref="System.IDisposable" />.
    /// </summary>
    /// <typeparam name="T">The type of value to be enumerated.</typeparam>
    /// <remarks>
    /// This interface is useful because some enumerator struct types do not want to implement
    /// <see cref="System.IDisposable" /> since it increases the size of the generated code in foreach.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal interface IStrongEnumerator<out T>
    {
        /// <summary>
        /// Returns the current element.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Advances to the next element.
        /// </summary>
        bool MoveNext();
    }
}
