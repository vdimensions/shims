using System.Collections.Generic;

namespace System.Collections.Immutable
{
    /// <summary>
    /// An interface that must be implemented by collections that want to avoid
    /// boxing their own enumerators when using the
    /// <see cref="System.Collections.Immutable.ImmutableExtensions.GetEnumerableDisposable{T, TEnumrator}(System.Collections.Generic.IEnumerable{T})" />
    /// method.
    /// </summary>
    /// <typeparam name="T">The type of value to be enumerated.</typeparam>
    /// <typeparam name="TEnumerator">The type of the enumerator struct.</typeparam>
    internal interface IStrongEnumerable<out T, out TEnumerator> 
        where TEnumerator: struct, IEnumerator<T>
    {
        /// <summary>Gets the strongly-typed enumerator.</summary>
        /// <returns></returns>
        TEnumerator GetEnumerator();
    }
}