using System.Collections.Generic;

namespace System.Collections.Immutable
{
    [Serializable]
    internal sealed class ComparisonComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }
        
        public int Compare(T x, T y) => _comparison(x, y);
    }
}