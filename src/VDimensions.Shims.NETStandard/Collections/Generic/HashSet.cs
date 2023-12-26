// #if SHIM_HASHSET
// using System;
// using System.Collections;
// using System.Diagnostics;
// using System.Diagnostics.CodeAnalysis;
// using System.Linq;
//
// namespace System.Collections.Generic
// {
//     /// <summary>
//     /// A generic adapter for the non-generic <see cref="ICollection"/> collection.
//     /// </summary>
//     /// <typeparam name="T">
//     /// The type of the elements in the list.
//     /// </typeparam>
//     /// <seealso cref="ICollection"/>
//     #if NETSTANDARD2_0_OR_NEWER || NETFRAMEWORK
//     [Serializable]
//     #endif
//     [SuppressMessage("ReSharper", "RedundantNameQualifier")]
//     public class HashSet<T> 
//         : ICollection<T>
//         , IReadOnlyCollection<T>, 
//         , ISet<T>, 
//         , IDeserializationCallback, 
//         , ISerializable
//     {
//         [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//         private readonly Hashtable _hashtable;
//         
//         [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//         private readonly IEqualityComparer<T> _comparer;
//
//         /// <summary>
//         /// Creates a new instance of the <see cref="HashSet{T}"/> class.
//         /// </summary>
//         /// <param name="comparer">
//         /// <para>
//         /// The <see cref="IEqualityComparer"/> object that defines the hash code provider and the comparer to use with
//         /// the <see cref="HashSet{T}"/>.
//         /// </para>
//         /// -or-
//         /// <para>
//         /// <c>null</c> to use the default hash code provider and the default comparer.
//         /// The default hash code provider is each key's implementation of <see cref="object.GetHashCode"/>
//         /// and the default comparer is each key's implementation of <see cref="object.Equals(object)"/>.
//         /// </para>
//         /// </param>
//         public HashSet(IEqualityComparer<T> comparer)
//         {
//             _comparer = (comparer ?? EqualityComparer<T>.Default);
//             _hashtable = new Hashtable(comparer as IEqualityComparer ?? new RawEqualityComparerAdapter<T>(_comparer));
//         }
//
//         /// <summary>
//         /// Creates a new instance of the <see cref="GenericSetAdapter{T}"/> class.
//         /// </summary>
//         /// <param name="hashtable">
//         /// The <see cref="Hashtable"/> object that will be represented as a generic
//         /// <see cref="ISet{T}"/>.
//         /// </param>
//         public HashSet(Hashtable hashtable)
//         {
//             _hashtable = hashtable ?? throw new ArgumentNullException(nameof(hashtable));
//             var rawComparer = GenericSetAdapter.Instance.GetComparer(hashtable);
//             _comparer = rawComparer == null 
//                 ? EqualityComparer<T>.Default
//                 : rawComparer as IEqualityComparer<T> ?? new GenericEqualityComparerAdapter<T>(rawComparer);
//         }
//
//         #region Implementation of ISet<T>
//         /// <inheritdoc cref="ISet{T}" />
//         public bool Add(T item)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(item, nameof(item)));
//
//             if (!_hashtable.ContainsKey(item))
//             {
//                 _hashtable.Add(item, item);
//                 return true;
//             }
//
//             return false;
//         }
//
//         /// <inheritdoc cref="ISet{T}.Contains" />
//         public bool Contains(T value) => value != null && _hashtable.ContainsKey(value);
//         
//         /// <inheritdoc cref="ISet{T}.ExceptWith" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public void ExceptWith(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             foreach (var item in other)
//             {
//                 Remove(item);
//             }
//         }
//
//         /// <inheritdoc cref="ISet{T}.IntersectWith" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public void IntersectWith(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var otherSet = new Hashtable(Count, new RawEqualityComparerAdapter<T>(Comparer));
//             foreach (var item in other)
//             {
//                 otherSet.Add(item, item);
//             }
//             var itemsToRemove = new List<T>(Count);
//             foreach (var item in this)
//             {
//                 if (otherSet.ContainsKey(item))
//                 {
//                     itemsToRemove.Add(item);
//                 }
//             }
//             ExceptWith(itemsToRemove);
//         }
//
//         /// <inheritdoc cref="ISet{T}.IsProperSubsetOf" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public bool IsProperSubsetOf(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var counter = 0;
//             return Enumerable.All(
//                 Enumerable.Distinct(other, _comparer), 
//                 item =>
//                 {
//                     counter++;
//                     return _hashtable.ContainsKey(item);
//                     
//                 }) && counter > _hashtable.Count;
//         }
//
//         /// <inheritdoc cref="ISet{T}.IsProperSupersetOf" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public bool IsProperSupersetOf(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var counter = 0;
//             return Enumerable.All(
//                 Enumerable.Distinct(other, _comparer), 
//                 item =>
//                 {
//                     counter++;
//                     return _hashtable.ContainsKey(item);
//                     
//                 }) && counter < _hashtable.Count;
//         }
//
//         /// <inheritdoc cref="ISet{T}.IsSubsetOf" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public bool IsSubsetOf(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var counter = 0;
//             return Enumerable.All(
//                 Enumerable.Distinct(other, _comparer), 
//                 item =>
//                 {
//                     counter++;
//                     return _hashtable.ContainsKey(item);
//                     
//                 }) && counter >= _hashtable.Count;
//         }
//
//         /// <inheritdoc cref="ISet{T}.IsSupersetOf" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public bool IsSupersetOf(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var counter = 0;
//             return Enumerable.All(
//                 Enumerable.Distinct(other, _comparer), 
//                 item =>
//                 {
//                     counter++;
//                     return _hashtable.ContainsKey(item);
//                     
//                 }) && counter <= _hashtable.Count;
//         }
//
//         /// <inheritdoc cref="ISet{T}.Overlaps" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public bool Overlaps(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//
//             return Enumerable.Any(other, Contains);
//         }
//
//         /// <inheritdoc />
//         public bool Remove(T value)
//         {
//             if (value == null)
//             {
//                 return false;
//             }
//             var count = _hashtable.Count;
//             _hashtable.Remove(value);
//             return count > _hashtable.Count;
//         }
//
//         /// <inheritdoc cref="ISet{T}.SetEquals" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public bool SetEquals(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var counter = 0;
//             return Enumerable.All(
//                 Enumerable.Distinct(other, _comparer), 
//                 item =>
//                 {
//                     counter++;
//                     return _hashtable.ContainsKey(item);
//                     
//                 }) && counter == _hashtable.Count;
//         }
//
//         /// <inheritdoc cref="ISet{T}.SymmetricExceptWith" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public void SymmetricExceptWith(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             var otherSet = new Hashtable(Count, new RawEqualityComparerAdapter<T>(Comparer));
//             foreach (var item in other)
//             {
//                 otherSet.Add(item, item);
//             }
//             var itemsToRemove = new List<T>(Count);
//             foreach (var item in this)
//             {
//                 if (!otherSet.ContainsKey(item))
//                 {
//                     itemsToRemove.Add(item);
//                 }
//             }
//             ExceptWith(itemsToRemove);
//         }
//
//         /// <inheritdoc />
//         public bool TryGetValue(T equalValue, out T actualValue)
//         {
//             if (equalValue == null)
//             {
//                 actualValue = default(T);
//                 return false;
//             }
//             
//             if (_hashtable.ContainsKey(equalValue))
//             {
//                 actualValue = (T) _hashtable[equalValue];
//                 return true;
//             }
//
//             actualValue = default(T);
//             return false;
//         }
//
//         /// <inheritdoc cref="ISet{T}.UnionWith" />
//         [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
//         public void UnionWith(IEnumerable<T> other)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(other, nameof(other)));
//             
//             foreach (var item in other)
//             {
//                 Add(item);
//             }
//         }
//
//         /// <inheritdoc />
//         public IEqualityComparer<T> Comparer => _comparer;
//
//         #endregion
//         
//         #region Implementation of ICollection<T>
//         [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
//         void ICollection<T>.Add(T value) => Add(value);
//         
//         /// <inheritdoc />
//         public void Clear() => _hashtable.Clear();
//
//         /// <inheritdoc />
//         public void CopyTo(T[] array, int index)
//         {
//             Verifier.IsNotNull(Verifier.VerifyArgument(array, nameof(array)));
//
//             _hashtable.CopyTo(array, index);
//         }
//
//         /// <inheritdoc cref="ICollection{T}.Count" />
//         public int Count => _hashtable.Count;
//
//         /// <inheritdoc />
//         public bool IsReadOnly => _hashtable.IsReadOnly;
//         #endregion
//
//         #region Implementation of IEnumerable<T>
//         /// <inheritdoc />
//         public IEnumerator<T> GetEnumerator()
//         {
//             foreach (DictionaryEntry element in _hashtable)
//             {
//                 yield return (T) element.Value;
//             }
//         }
//         /// <inheritdoc />
//         IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//         #endregion
//
//         #region Implementation of ICollection 
//         void ICollection.CopyTo(Array array, int index) => _hashtable.CopyTo(array, index);
//
//         /// <inheritdoc />
//         int ICollection.Count => _hashtable.Count;
//
//         /// <inheritdoc />
//         bool ICollection.IsSynchronized => _hashtable.IsSynchronized;
//
//         /// <inheritdoc />
//         object ICollection.SyncRoot => _hashtable.SyncRoot;
//
//         /// <inheritdoc />
//         int ICollection<T>.Count => _hashtable.Count;
//         #endregion
//     }
// }
// #endif