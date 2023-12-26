#if POLYFILL_STRING_REPLACE
/// <summary>
/// A static class containing common extensions methods for the <see cref="string"/> class.
/// </summary>
public static class StringPolyfill
{
    /// <summary>
    /// Returns a new <see cref="string"/> in which all occurrences of a <paramref name="oldValue"> specified
    /// string</paramref> in the current instance are replaced with <paramref name="newValue">another specified
    /// string</paramref>.
    /// </summary>
    /// <param name="current">
    /// The current <see cref="string"/> instance to process.
    /// </param>
    /// <param name="oldValue">
    /// The string to be replaced.
    /// </param>
    /// <param name="newValue">
    /// The string to replace all occurrences of oldValue.
    /// </param>
    /// <param name="comparison">
    /// One of the <see cref="System.StringComparison"/> enumeration values that determines how
    /// <paramref name="oldValue"/> is searched within this instance.
    /// </param>
    /// <returns>
    /// A string that is equivalent to the current string except that all instances of <paramref name="oldValue"/>
    /// are replaced with <paramref name="newValue"/>. If <paramref name="oldValue"/> is not found in the current
    /// instance, the method returns the current instance unchanged.
    /// </returns>
    public static string Replace(
        #if NETSTANDARD || NET35_OR_NEWER
        this 
        #endif
        string current, string oldValue, string newValue, System.StringComparison comparison)
    {
        if (current == null)
        {
            throw new System.ArgumentNullException(nameof(current));
        }
        
        if (oldValue == null)
        {
            throw new System.ArgumentNullException(nameof(oldValue));
        }
        if (oldValue.Length == 0)
        {
            throw new System.ArgumentException(nameof(oldValue));
        }
        
        if (newValue == null)
        {
            throw new System.ArgumentNullException(nameof(newValue));
        }
        
        var result = current;
        var index = result.IndexOf(oldValue, comparison);
        do
        {
            if (index < 0)
            {
                return result;
            }
            result = string.Concat(current.Substring(0, index), newValue, current.Substring(index + oldValue.Length));
            index = result.IndexOf(oldValue, comparison);
        }
        while (index >= 0);

        return result;
    }
}
#endif