#if POLYFILL_ARRAY_STRUCTURAL_COMPARABLE
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using VDimensions.Shims;

namespace System
{
    /// <summary>
    /// A static class that allows invoking methods for structural comparison on arrays, as if they
    /// implement the <see cref="IStructuralComparable"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ArrayStructuralComparableExtensions
    {
        /// <inheritdoc cref="IStructuralComparable.CompareTo"/>
        public static int CompareTo(
            #if NETSTANDARD || NET35_OR_NEWER
            this 
            #endif
            Array array, 
            object other, 
            IComparer comparer)
        {
            IStructuralComparable comparable = new StructuralArrayAdapter(array);
            return comparable.CompareTo(other, comparer);
        }
    }
}
#endif