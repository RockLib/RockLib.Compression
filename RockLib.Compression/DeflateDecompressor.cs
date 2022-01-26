using System;
using System.IO;
using System.IO.Compression;

namespace RockLib.Compression
{
    /// <summary>
    /// An implementation of <see cref="IDecompressor"/> that uses a deflate stream.
    /// </summary>
    public class DeflateDecompressor : IDecompressor
    {
        /// <summary>
        /// Decompress the contents of the stream into a byte array.
        /// </summary>
        /// <param name="inputStream">The stream to decompress.</param>
        /// <returns>The decompressed byte array.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] Decompress(Stream inputStream)
        {
            if (inputStream is null) throw new ArgumentNullException(nameof(inputStream));
            using (var outputStream = new MemoryStream())
            {
                using (var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress, true))
                {
                    deflateStream.CopyTo(outputStream);
                }

                return outputStream.ToArray();
            }
        }
    }
}