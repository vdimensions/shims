#if NETSTANDARD && !NETSTANDARD1_3_OR_NEWER
namespace System.Text
{
    /// <summary>
    /// A static class to contain extension methods for the <see cref="System.Text.Encoding"/> type.
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// Decodes a sequence of bytes from the specified byte array into a string.
        /// </summary>
        /// <param name="encoding">
        /// The <see cref="System.Text.Encoding"/> instance this extension method is invoked upon.
        /// </param>
        /// <param name="bytes">
        /// The byte array containing the sequence of bytes to decode. 
        /// </param>
        /// <returns>
        /// A <see cref="string"/> that contains the results of decoding the specified sequence of bytes.
        /// </returns>
        public static string GetString(this System.Text.Encoding encoding, byte[] bytes)
        {
            if (encoding == null) 
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            if (bytes == null) 
            {
                throw new ArgumentNullException(nameof(bytes));
            }
            return encoding.GetString(bytes, 0, bytes.Length);
        }
    }
}
#endif