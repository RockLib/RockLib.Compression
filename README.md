# :warning: Deprecation Warning :warning:

This library has been deprecated and will no longer receive updates.

---

RockLib has been a cornerstone of our open source efforts here at Rocket Mortgage, and it's played a significant role in our journey to drive innovation and collaboration within our organization and the open source community. It's been amazing to witness the collective creativity and hard work that you all have poured into this project.

However, as technology relentlessly evolves, so must we. The decision to deprecate this library is rooted in our commitment to staying at the cutting edge of technological advancements. While this chapter is ending, it opens the door to exciting new opportunities on the horizon.

We want to express our heartfelt thanks to all the contributors and users who have been a part of this incredible journey. Your contributions, feedback, and enthusiasm have been invaluable, and we are genuinely grateful for your dedication. 🚀

---

# RockLib.Compression

This library simplifies the usage of standard compression algorithms, as the standard compression API from .NET is a bit tricky to get right. There two interfaces are , `ICompressor` and `IDecompressor`, two implementations of each interface (GZip and Deflate), along with extension methods for the interface.

 ## ICompressor

 The `ICompressor` interface itself has a `Compress` method that takes a `Stream` parameter containing data and returns a `byte[]` containing the compressed stream.

 The first `ICompressor.Compress` extension method takes a `byte[]` and returns a `byte[]` containing the compressed input array.

 The second `ICompressor.Compress` extension method takes a `string` parameter and an optional `Encoding` parameter. It encodes the `string` into a `byte[]` and returns that array, compressed.

 ## IDecompressor

 The `IDecompressor` interface has a `Decompress` method that takes a `Stream` parameter containing compressed data and returns a `byte[]` with the decompressed stream.

 The `IDecompressor.Decompress` extension method takes a `byte[]` containing compressed data and returns a `byte[]` with the decompressed input array.

 ## Supported Targets

 This library supports the following targets:
   - .NET 6
   - .NET Core 3.1
   - .NET Framework 4.8

 ## Example

 ```csharp
 ICompressor compressor = new GZipCompressor();
 IDecompressor decompressor = new GZipDecompressor();

 byte[] compressed = compressor.Compress("Hello, world!");

 byte[] decompressed = decompressor.Decompress(compressed);

 string originalString = Encoding.UTF8.GetString(decompressed);
 ```
