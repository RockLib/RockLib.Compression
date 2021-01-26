# RockLib.Compression [![Build status](https://ci.appveyor.com/api/projects/status/8ict9oe53r76vo1p?svg=true)](https://ci.appveyor.com/project/RockLib/rocklib-compression) [![NuGet](https://img.shields.io/nuget/vpre/RockLib.Compression.svg)](https://www.nuget.org/packages/RockLib.Compression)

This library is meant to simplify the usage of standard compression algorithms, as the standard compression API from .NET is a bit tricky to get right. There are two interfaces, `ICompressor` and `IDecompressor`, two implementations of each interface (GZip and Deflate), along with extension methods for the interface.
 
 ##### ICompressor
 
 The `ICompressor` interface itself has a `Compress` method that takes a `Stream` parameter containing data and returns a `byte[]` containing the compressed stream.
 
 The first `ICompressor.Compress` extension method takes a `byte[]` and returns a `byte[]` containing the compressed input array.
 
 The second `ICompressor.Compress` extension method takes a `string` parameter and an optional `Encoding` parameter. It encodes the `string` into a `byte[]` and returns that array, compressed.

##### IDecompressor

The `IDecompressor` interface has a `Decompress` method that takes a `Stream` parameter containing compressed data and returns a `byte[]` with the decompressed stream.

 The `IDecompressor.Decompress` extension method takes a `byte[]` containing compressed data and returns a `byte[]` with the decompressed input array.
 
 ### Example
 
 ```c#
 ICompressor compressor = new GZipCompressor();
 IDecompressor decompressor = new GZipDecompressor();
 
 byte[] compressed = compressor.Compress("Hello, world!"); 
 
 byte[] decompressed = decompressor.Decompress(compressed);
 
 string originalString = Encoding.UTF8.GetString(decompressed);
 ```
 
