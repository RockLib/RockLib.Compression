using System;
using System.IO;

namespace RockLib.Compression
{
    /// <summary>
    /// Defines an interface for compression.
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        /// Compress the contents of the stream into a byte array.
        /// </summary>
        /// <param name="inputStream">The stream to compress.</param>
        /// <returns>The compressed byte array.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        byte[] Compress(Stream inputStream);
    }
}