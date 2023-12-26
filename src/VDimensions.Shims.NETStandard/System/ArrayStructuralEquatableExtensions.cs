#if POLYFILL_ARRAY_STRUCTURAL_EQUATABLE
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using VDimensions.Shims;

namespace System
{
    /// <summary>
    /// A static class that allows invoking methods for checking structural equality on arrays, as if they
    /// implement the <see cref="IStructuralEquatable"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ArrayStructuralEquatableExtensions
    {
        /// <inheritdoc cref="IStructuralEquatable.Equals(object,System.Collections.IEqualityComparer)"/>
        public static bool Equals(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            Array array, 
            object other, 
            IEqualityComparer comparer)
        {
            IStructuralEquatable equatable = new StructuralArrayAdapter(array);
            return equatable.Equals(other, comparer);
        }

        /// <inheritdoc cref="IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer)"/>
        public static int GetHashCode(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            Array array, 
            IEqualityComparer comparer) 
        {
            IStructuralEquatable equatable = new StructuralArrayAdapter(array);
            return equatable.GetHashCode(comparer);
        }
    }
}
#endif