namespace System.Text
{
    /// A static class containing extension methods common to the Encoding class.
    public static class EncodingExtensions
    {
        /// <summary>
        /// Decodes a sequence of bytes from the specified byte array into a string
        /// </summary>
        /// <param name="encoding">
        /// The encoding to object used to extract the string data.
        /// </param>
        /// <param name="bytes">
        /// The byte array containing the sequence of bytes to decode.
        /// </param>
        /// <returns>
        /// A string that contains the results of decoding the specified sequence of bytes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Either encoding or bytes is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The byte array contains invalid Unicode code points.
        /// </exception>
        /// <exception cref="DecoderFallbackException">
        /// A fallback occurred (see Character Encoding in the .NET Framework
        /// for complete explanation)-and-System.Text.Encoding.DecoderFallback
        /// is set to System.Text.DecoderExceptionFallback.
        /// </exception>
        #if NETSTANDARD && !NETSTANDARD1_3_OR_NEWER
        public static string GetString(this Encoding encoding, byte[] bytes)
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
        #endif
    }
}