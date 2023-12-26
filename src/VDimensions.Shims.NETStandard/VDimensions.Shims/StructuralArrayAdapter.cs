using System;
using System.Collections;

namespace VDimensions.Shims
{
    /// <summary>
    /// <para>
    /// A wrapper class for the <see cref="Array"/> type, that acts as an adapter for the
    /// <see cref="IStructuralEquatable"/> and <see cref="IStructuralComparable"/> interfaces.
    /// </para>
    /// <para>
    /// Until .NETFramework 4.0, the <see cref="Array"/> type did not implement these interfaces. This wrapper can be
    /// used to pass an array instance to an API that accepts <see cref="IStructuralEquatable"/> or
    /// <see cref="IStructuralComparable"/> arguments. 
    /// </para>
    /// </summary>
    #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
    [Serializable]
    #endif
    public sealed class StructuralArrayAdapter 
        : IStructuralEquatable
        , IStructuralComparable
    {
        private static int CombineHashCodes(int h1, int h2) => (((h1 << 5) + h1) ^ h2);

        /// Enables implicit conversion from <see cref="Array"/> instances.
        public static implicit operator StructuralArrayAdapter (Array array) => new StructuralArrayAdapter(array);
        
        private readonly Array _array;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructuralArrayAdapter"/> class.
        /// </summary>
        /// <param name="array">
        /// The wrapped array instance
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> is <c>null</c>.
        /// </exception>
        public StructuralArrayAdapter(Array array)
        {
            _array = array ?? throw new ArgumentNullException(nameof(array));
        }

        #region IStructuralComparable
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null) 
            {
                return 1;
            }

            var o = other as Array;
            if (o == null || _array.Length != o.Length) 
            {
                throw new ArgumentException("The array and other array are not of the same length.", nameof(other));
            }

            var c = 0;
            for (var i = 0; i < o.Length && c == 0; i++)
            {
                c = comparer.Compare(_array.GetValue(i), o.GetValue(i));
            }
            return c;
        }
        #endregion

        #region IStructuralEquatable
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            #if POLYFILL_ARRAY_STRUCTURAL_EQUATABLE
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }
            
            if (other == null) 
            {
                return false;
            }

            if (ReferenceEquals(_array, other)) 
            {
                return true;
            }

            var o = other as Array;
            if (o == null || o.Length != _array.Length) 
            {
                return false;
            }

            for (var i = 0; i < o.Length; i++)
            {
                if (!comparer.Equals(_array.GetValue(i), o.GetValue(i))) 
                {
                    return false;
                }
            }

            return true;
            #else
            return ((IStructuralEquatable) _array).Equals(other, comparer);
            #endif
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            #if POLYFILL_ARRAY_STRUCTURAL_EQUATABLE
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var result = 0;
            for (var i = (_array.Length >= 8 ? _array.Length - 8 : 0); i < _array.Length; i++) 
            {
                result = CombineHashCodes(result, comparer.GetHashCode(_array.GetValue(i)));
            }
            return result;
            #else
            return ((IStructuralEquatable) _array).GetHashCode(comparer);
            #endif
        }
        #endregion IStructuralEquatable
    }
}