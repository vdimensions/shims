#if SHIM_READONLY_COLLECTIONS
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a generic read-only collection of key/value pairs.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of keys in the read-only dictionary.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The type of values in the read-only dictionary.
    /// </typeparam>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public interface IReadOnlyDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified
        /// <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// The key to locate.
        /// </param>
        /// <returns>
        /// <c><see langword="true">true</see></c> if the read-only dictionary contains an element that has the
        /// specified key; otherwise, <c><see langword="false" >false</see></c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is <c><see langword="null" >null</see></c>.
        /// </exception>
        bool ContainsKey(TKey key);
        
        /// <summary>
        /// Gets the value that is associated with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// The key to locate.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; otherwise,
        /// the default value for the type of the <paramref name="value" /> parameter.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// <c><see langword="true">true</see></c> if the object that implements the
        /// <see cref="T:Axle.Collections.ReadOnly.IReadOnlyDictionary`2" /> interface contains an element that has the
        /// specified <paramref name="key"/>; otherwise, <c><see langword="false" >false</see></c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is <c><see langword="null" >null</see></c>.
        /// </exception>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Gets the element that has the specified <paramref name="key"/> in the read-only dictionary.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// The element that has the specified key in the read-only dictionary.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is <c><see langword="null" >null</see></c>.
        /// </exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and <paramref name="key" /> is not found.
        /// </exception>
        TValue this[TKey key] { get; }
        
        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the keys in the read-only dictionary.
        /// </returns>
        IEnumerable<TKey> Keys { get; }
        
        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the values in the read-only dictionary.
        /// </returns>
        IEnumerable<TValue> Values { get; }
    }
}
#endif