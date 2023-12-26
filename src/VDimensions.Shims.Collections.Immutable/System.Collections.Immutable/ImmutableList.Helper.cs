using System.Collections.Generic;

namespace System.Collections.Immutable
{
    partial class ImmutableList
    {
        internal static class Helper
        {
            internal static void CopyTo<T>(in List<T> list, in T[] array)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (array.Length < list.Count)
                {
                    throw new ArgumentException(nameof(array));
                }
                list.CopyTo(array);
            }
            internal static void CopyTo<T>(in IList<T> list, in T[] array, in int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }

                if (array.Length < arrayIndex + list.Count)
                {
                    throw new ArgumentException(nameof(array));
                }
                list.CopyTo(array, arrayIndex);
            }
            internal static void CopyTo<T>(in List<T> list, in int index, in T[] array, in int arrayIndex, int count)
            {
                if (array == null)
                {
                    throw new ArgumentNullException(nameof(array));
                }
                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (index >= list.Count)
                {
                    throw new ArgumentException(nameof(index));
                }
                if (arrayIndex + count > array.Length)
                {
                    throw new ArgumentException(nameof(count));
                }
                list.CopyTo(index, array, arrayIndex, count);
            }
            
            internal static int FindIndex<T>(in List<T> list, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindIndex(match);
            }

            internal static int FindIndex<T>(in List<T> list, in int startIndex, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                if (startIndex < 0 || startIndex > list.Count)
                {
                    throw new ArgumentNullException(nameof(startIndex));
                }
                return list.FindIndex(startIndex, match);
            }

            internal static int FindIndex<T>(in List<T> list, in int startIndex, in int count, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                if (startIndex < 0)
                {
                    throw new ArgumentNullException(nameof(startIndex));
                }
                if (count < 0 || startIndex + count > list.Count)
                {
                    throw new ArgumentNullException(nameof(count));
                }
                return list.FindIndex(startIndex, count, match);
            }

            internal static T FindLast<T>(in List<T> list, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLast(match);
            }

            internal static int FindLastIndex<T>(in List<T> list, in Predicate<T> match)
            {
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLastIndex(match);
            }

            internal static int FindLastIndex<T>(in List<T> list, in int startIndex, in Predicate<T> match)
            {
                if (startIndex < 0 || startIndex >= list.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLastIndex(startIndex, match);
            }

            internal static int FindLastIndex<T>(in List<T> list, in int startIndex, in int count, in Predicate<T> match)
            {
                if (startIndex < 0 || (startIndex - count + 1) < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
                if (count > list.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (match == null)
                {
                    throw new ArgumentNullException(nameof(match));
                }
                return list.FindLastIndex(startIndex, count, match);
            }
            
            internal static bool IsCompatibleObject<T>(object value)
            {
                if (value is T)
                {
                    return true;
                }
                return value == null && default (T) == null;
            }

            internal static int IndexOf<T>(
                in IList<T> list, 
                in T item, 
                in int index, 
                in int count,
                in IEqualityComparer<T> equalityComparer)
            {
                var ec = equalityComparer ?? EqualityComparer<T>.Default;
                for (var i = index; i < count; i++)
                {
                    if (ec.Equals(list[i], item))
                    {
                        return i;
                    }
                }
                return -1;
            }
            
            internal static int LastIndexOf<T>(
                in IList<T> list, 
                in T item,
                in int startIndex,
                in int count,
                in IEqualityComparer<T> equalityComparer)
            {
                var ec = equalityComparer ?? EqualityComparer<T>.Default;
                for (var i = count - 1; i >= startIndex; i--)
                {
                    if (ec.Equals(list[i], item))
                    {
                        return i;
                    }
                }
                return -1;
            }
            
            internal static void Sort<T>(
                in List<T> list, 
                in int index, 
                in int count, 
                in IComparer<T> comparer)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (list.Count < index + count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                list.Sort(index, count, comparer);
            }
            
            internal static void Reverse<T>(in List<T> list, in int index, in int count)
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                if (list.Count < index + count)
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                list.Reverse(index, count);
            }
        }
    }
}
