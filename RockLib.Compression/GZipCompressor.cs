using System;
using System.IO;
using System.IO.Compression;

namespace RockLib.Compression
{
    /// <summary>
    /// An implementation of <see cref="ICompressor"/> that uses GZip.
    /// </summary>
    public class GZipCompressor : ICompressor
    {
        /// <summary>
        /// Compress the contents of the stream into a byte array.
        /// </summary>
        /// <param name="inputStream">The stream to compress.</param>
        /// <returns>The compressed byte array.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] Compress(Stream inputStream)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(inputStream);
#else
            if (inputStream is null)
                throw new ArgumentNullException(nameof(inputStream));
#endif
            using (var outputStream = new MemoryStream())
            {
                using (var gzStream = new GZipStream(outputStream, CompressionMode.Compress, true))
                {
                    inputStream.CopyTo(gzStream);
                }

                return outputStream.ToArray();
            }
        }
    }
}