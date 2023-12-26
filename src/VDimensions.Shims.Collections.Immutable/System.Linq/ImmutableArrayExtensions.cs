using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace System.Linq
{
    /// <summary>
    /// LINQ extension method overloads that offer greater efficiency for <see cref="ImmutableArray{T}" /> than the
    /// standard LINQ methods.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public static class ImmutableArrayExtensions
    {
        /// <summary>
        /// A two element array useful for throwing exceptions the way LINQ does.
        /// </summary>
        internal static readonly byte[] TwoElementArray = new byte[2];
        
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the result element.
        /// </typeparam>
        /// <param name="immutableArray">
        /// The immutable array.
        /// </param>
        /// <param name="selector">
        /// The selector.
        /// </param>
        public static IEnumerable<TResult> Select<T, TResult>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, 
            Func<T, TResult> selector)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            return Enumerable.Select(immutableArray.array, selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="T:System.Collections.Generic.IEnumerable`1" />,
        /// flattens the resulting sequences into one sequence, and invokes a result
        /// selector function on each element therein.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="immutableArray" />.
        /// </typeparam>
        /// <typeparam name="TCollection">
        /// The type of the intermediate elements collected by <paramref name="collectionSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the resulting sequence.
        /// </typeparam>
        /// <param name="immutableArray">
        /// The immutable array.
        /// </param>
        /// <param name="collectionSelector">
        /// A transform function to apply to each element of the input sequence.
        /// </param>
        /// <param name="resultSelector">
        /// A transform function to apply to each element of the intermediate sequence.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}" /> whose elements are the result
        /// of invoking the one-to-many transform function <paramref name="collectionSelector" /> on each
        /// element of <paramref name="immutableArray" /> and then mapping each of those sequence elements and their
        /// corresponding source element to a result element.
        /// </returns>
        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<TSource> immutableArray,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            if (collectionSelector == null || resultSelector == null)
            {
                return Enumerable.SelectMany(immutableArray, collectionSelector, resultSelector);
            }
            return immutableArray.Length != 0 
                ? SelectManyIterator(immutableArray, collectionSelector, resultSelector) 
                : Enumerable.Empty<TResult>();
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static IEnumerable<T> Where<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, 
            Func<T, bool> predicate)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            return Enumerable.Where(immutableArray.array, predicate);
        }

        /// <summary>
        /// Gets a value indicating whether any elements are in this collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        /// <param name="immutableArray"></param>
        public static bool Any<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray) => immutableArray.Length > 0;

        /// <summary>
        /// Gets a value indicating whether any elements are in this collection that match a given condition.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        /// <param name="immutableArray"></param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        public static bool Any<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, 
            Func<T, bool> predicate)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            foreach (var obj in immutableArray.array)
            {
                if (predicate(obj))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether all elements in this collection
        /// match a given condition.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        /// <param name="immutableArray"></param>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// <c>true</c> if every element of the source sequence passes the test in the specified predicate, or if the
        /// sequence is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool All<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, 
            Func<T, bool> predicate)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            foreach (var obj in immutableArray.array)
            {
                if (!predicate(obj))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether two sequences are equal according to an equality comparer.
        /// </summary>
        /// <typeparam name="TDerived">
        /// The type of element in the compared array.
        /// </typeparam>
        /// <typeparam name="TBase">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static bool SequenceEqual<TDerived, TBase>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<TBase> immutableArray,
            ImmutableArray<TDerived> items,
            IEqualityComparer<TBase> comparer = null)
            where TDerived : TBase
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            items.ThrowNullRefIfNotInitialized();
            if (ReferenceEquals(immutableArray.array, items.array))
            {
                return true;
            }

            if (immutableArray.Length != items.Length)
            {
                return false;
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<TBase>.Default;
            }
            for (var index = 0; index < immutableArray.Length; ++index)
            {
                if (!comparer.Equals(immutableArray.array[index], items.array[index]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether two sequences are equal according to an equality comparer.
        /// </summary>
        /// <typeparam name="TDerived">
        /// The type of element in the compared array.
        /// </typeparam>
        /// <typeparam name="TBase">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static bool SequenceEqual<TDerived, TBase>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<TBase> immutableArray,
            IEnumerable<TDerived> items,
            IEqualityComparer<TBase> comparer = null)
            where TDerived : TBase
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (comparer == null)
            {
                comparer = EqualityComparer<TBase>.Default;
            }
            var index = 0;
            var length = immutableArray.Length;
            foreach (var y in items)
            {
                if (index == length || !comparer.Equals(immutableArray[index], y))
                {
                  return false;
                }
                ++index;
            }
            return index == length;
        }

        /// <summary>
        /// Determines whether two sequences are equal according to an equality comparer.
        /// </summary>
        /// <typeparam name="TDerived">
        /// The type of element in the compared array.
        /// </typeparam>
        /// <typeparam name="TBase">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static bool SequenceEqual<TDerived, TBase>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<TBase> immutableArray,
            ImmutableArray<TDerived> items,
            Func<TBase, TBase, bool> predicate)
            where TDerived : TBase
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            immutableArray.ThrowNullRefIfNotInitialized();
            items.ThrowNullRefIfNotInitialized();
            if (ReferenceEquals(immutableArray.array, items.array))
            {
                return true;
            }

            if (immutableArray.Length != items.Length)
            {
                return false;
            }
            var index = 0;
            for (var length = immutableArray.Length; index < length; ++index)
            {
                if (!predicate(immutableArray[index], items[index]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static T Aggregate<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, Func<T, T, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (immutableArray.Length == 0)
            {
                return default(T);
            }
            var obj = immutableArray[0];
            var index = 1;
            for (var length = immutableArray.Length; index < length; ++index)
            {
                obj = func(obj, immutableArray[index]);
            }
            return obj;
        }

        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="TAccumulate">
        /// The type of the accumulated value.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static TAccumulate Aggregate<TAccumulate, T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            TAccumulate seed,
            Func<TAccumulate, T, TAccumulate> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            var accumulate = seed;
            foreach (var obj in immutableArray.array)
            {
                accumulate = func(accumulate, obj);
            }
            return accumulate;
        }

        /// <summary>
        /// Applies an accumulator function over a sequence.
        /// </summary>
        /// <typeparam name="TAccumulate">
        /// The type of the accumulated value.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result returned by the result selector.
        /// </typeparam>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static TResult Aggregate<TAccumulate, TResult, T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            TAccumulate seed,
            Func<TAccumulate, T, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector)
        {
            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }
            return resultSelector(Aggregate(immutableArray, seed, func));
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static T ElementAt<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, int index) => immutableArray[index];

        /// <summary>
        /// Returns the element at a specified index in a sequence or a default value if the index is out of range.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static T ElementAtOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, int index) 
            => index < 0 || index >= immutableArray.Length 
                ? default(T) 
                : immutableArray[index];

        /// <summary>
        /// Returns the first element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static T First<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            foreach (T obj in immutableArray.array)
            {
                if (predicate(obj))
                {
                    return obj;
                }
            }
            return Enumerable.First(Enumerable.Empty<T>());
        }

        /// <summary>
        /// Returns the first element in a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        /// <param name="immutableArray"></param>
        public static T First<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray) 
            => immutableArray.Length <= 0 
                ? Enumerable.First(immutableArray.array) 
                : immutableArray[0];

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        public static T FirstOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray) 
            => immutableArray.array.Length == 0 ? default(T) : immutableArray.array[0];

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or a default value if no such element
        /// is found.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static T FirstOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            foreach (var obj in immutableArray.array)
            {
                if (predicate(obj))
                {
                    return obj;
                }
            }
            return default(T);
        }

        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        /// <param name="immutableArray"></param>
        public static T Last<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray) 
            => immutableArray.Length <= 0 
                ? Enumerable.Last(immutableArray.array) 
                : immutableArray[immutableArray.Length - 1];

        /// <summary>
        /// Returns the last element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        public static T Last<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            for (var index = immutableArray.Length - 1; index >= 0; --index)
            {
                if (predicate(immutableArray[index]))
                {
                    return immutableArray[index];
                }
            }
            return Enumerable.Last(Enumerable.Empty<T>());
        }

        /// <summary>
        /// Returns the last element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        public static T LastOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            return Enumerable.LastOrDefault(immutableArray.array);
        }

        /// <summary>
        /// Returns the last element of a sequence that satisfies a condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        public static T LastOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            for (var index = immutableArray.Length - 1; index >= 0; --index)
            {
                if (predicate(immutableArray[index]))
                {
                    return immutableArray[index];
                }
            }
            return default (T);
        }

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        public static T Single<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            return Enumerable.Single(immutableArray.array);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more
        /// than one such element exists.
        /// </summary>
        /// <typeparam name="T">
        /// The type of element contained by the collection.
        /// </typeparam>
        public static T Single<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var flag = true;
            var obj1 = default(T);
            foreach (var obj2 in immutableArray.array)
            {
                if (predicate(obj2))
                {
                    if (!flag)
                    {
                        int num = Enumerable.Single(TwoElementArray);
                    }
                    flag = false;
                    obj1 = obj2;
                }
            }
            if (flag)
            {
                var single = Enumerable.Single(Enumerable.Empty<T>());
            }

            return obj1;
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        public static T SingleOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            return Enumerable.SingleOrDefault(immutableArray.array);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.
        /// </summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        public static T SingleOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var flag = true;
            var obj1 = default (T);
            foreach (var obj2 in immutableArray.array)
            {
                if (predicate(obj2))
                {
                    if (!flag)
                    {
                        var num = (int) Enumerable.Single(TwoElementArray);
                    }
                    flag = false;
                    obj1 = obj2;
                }
            }
            return obj1;
        }

        /// <summary>
        /// Creates a dictionary based on the contents of this array.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>The newly initialized dictionary.</returns>
        public static Dictionary<TKey, T> ToDictionary<TKey, T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            Func<T, TKey> keySelector)
        {
            return ToDictionary(immutableArray, keySelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Creates a dictionary based on the contents of this array.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <returns>The newly initialized dictionary.</returns>
        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector)
        {
            return ToDictionary(immutableArray, keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Creates a dictionary based on the contents of this array.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer to initialize the dictionary with.</param>
        /// <returns>The newly initialized dictionary.</returns>
        public static Dictionary<TKey, T> ToDictionary<TKey, T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            Func<T, TKey> keySelector,
            IEqualityComparer<TKey> comparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            var dictionary = new Dictionary<TKey, T>(comparer);
            foreach (T immutable in immutableArray)
            {
                dictionary.Add(keySelector(immutable), immutable);
            }
            return dictionary;
        }

        /// <summary>
        /// Creates a dictionary based on the contents of this array.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <param name="comparer">The comparer to initialize the dictionary with.</param>
        /// <returns>The newly initialized dictionary.</returns>
        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement, T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray,
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }
            var dictionary = new Dictionary<TKey, TElement>(immutableArray.Length, comparer);
            foreach (var obj in immutableArray.array)
            {
                dictionary.Add(keySelector(obj), elementSelector(obj));
            }
            return dictionary;
        }

        /// <summary>Copies the contents of this array to a mutable array.</summary>
        /// <typeparam name="T">The type of element contained by the collection.</typeparam>
        /// <param name="immutableArray"></param>
        /// <returns>The newly instantiated array.</returns>
        public static T[] ToArray<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T> immutableArray)
        {
            immutableArray.ThrowNullRefIfNotInitialized();
            return immutableArray.array.Length == 0 ? ImmutableArray<T>.Empty.array : (T[]) immutableArray.array.Clone();
        }

        /// <summary>Returns the first element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the collection is empty.</exception>
        public static T First<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T>.Builder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return Any(builder) ? builder[0] : throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns the first element in the collection, or the default value if the collection is empty.
        /// </summary>
        public static T FirstOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T>.Builder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return !Any(builder) ? default(T) : builder[0];
        }

        /// <summary>Returns the last element in the collection.</summary>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the collection is empty.</exception>
        public static T Last<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T>.Builder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return Any(builder) ? builder[builder.Count - 1] : throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns the last element in the collection, or the default value if the collection is empty.
        /// </summary>
        public static T LastOrDefault<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T>.Builder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return !Any(builder) ? default(T) : builder[builder.Count - 1];
        }

        /// <summary>
        /// Returns a value indicating whether this collection contains any elements.
        /// </summary>
        public static bool Any<T>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<T>.Builder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.Count > 0;
        }

        /// <summary>
        /// Provides the core iterator implementation of <see cref="M:System.Linq.ImmutableArrayExtensions.SelectMany``3(System.Collections.Immutable.ImmutableArray{``0},System.Func{``0,System.Collections.Generic.IEnumerable{``1}},System.Func{``0,``1,``2})" />.
        /// </summary>
        private static IEnumerable<TResult> SelectManyIterator<TSource, TCollection, TResult>(
            #if NETSTANDARD || NET35_OR_NEWER
            this
            #endif
            ImmutableArray<TSource> immutableArray,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            var sourceArray = immutableArray.array;
            for (var index = 0; index < sourceArray.Length; ++index)
            {
                var item = sourceArray[index];
                foreach (var collection in collectionSelector(item))
                {
                    yield return resultSelector(item, collection);
                }
            }
        }
    }
}