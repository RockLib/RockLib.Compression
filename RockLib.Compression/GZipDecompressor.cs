using System;
using System.IO;
using System.IO.Compression;

namespace RockLib.Compression
{
    /// <summary>
    /// An implementation of <see cref="IDecompressor"/> that uses GZip.
    /// </summary>
    public class GZipDecompressor : IDecompressor
    {
        /// <summary>
        /// Decompress the contents of the stream into a byte array.
        /// </summary>
        /// <param name="inputStream">The stream to decompress.</param>
        /// <returns>The decompressed byte array.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] Decompress(Stream inputStream)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(inputStream);
#else
            if (inputStream is null)
                throw new ArgumentNullException(nameof(inputStream));
#endif
            using (var outputStream = new MemoryStream())
            {
                using (var gzStream = new GZipStream(inputStream, CompressionMode.Decompress, true))
                {
                    gzStream.CopyTo(outputStream);
                }

                return outputStream.ToArray();
            }
        }
    }
}