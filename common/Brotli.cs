using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace gzipstream;

/// <summary>
/// uses the BrotliStream for string compression decompression
/// </summary>
public sealed class Brotli
{
    /// <summary>
    /// compress the array of bytes
    /// </summary>
    /// <param name="bytes">make sure array size is > 1</param>
    /// <returns>compressed array of bytes given</returns>
    public static byte[] Compress(byte[] bytes)
    {
        Debug.Assert(bytes.Length > 0);
        
        using (var memoryStream = new MemoryStream())
        {
            using (var brotli_ = new BrotliStream(memoryStream, CompressionLevel.Optimal))
            {
                brotli_.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }
    }

    /// <summary>
    /// decompress the array of bytes
    /// </summary>
    /// <param name="bytes">make sure array size is > 1</param>
    /// <returns>decompressed array of bytes given</returns>
    public static byte[] Decompress(byte[] bytes)
    {
        Debug.Assert(bytes.Length > 0);

        using (var memoryStream = new MemoryStream(bytes))
        {
            using (var outputStream = new MemoryStream())
            {
                using (var decompressStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                {
                    decompressStream.CopyTo(outputStream);
                }
                return outputStream.ToArray();
            }
        }
    }

    #region asynchronous
    public static async Task<byte[]> CompressAsync(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var brotliStream = new BrotliStream(memoryStream, CompressionLevel.Optimal))
            {
                await brotliStream.WriteAsync(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }
    }
    public static async Task<byte[]> DecompressAsync(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream(bytes))
        {
            using (var outputStream = new MemoryStream())
            {
                using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
                {
                    await brotliStream.CopyToAsync(outputStream);
                }
                return outputStream.ToArray();
            }
        }
    }
    #endregion


} // BrotliStream
