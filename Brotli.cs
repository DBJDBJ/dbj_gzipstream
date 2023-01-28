using System;
using System.IO;
using System.IO.Compression;

namespace gzipstream;
internal sealed class Brotli
{
    public static byte[] Compress(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var brotli_ = new BrotliStream(memoryStream, CompressionLevel.Optimal))
            {
                brotli_.Write(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }
    }

    public static byte[] Decompress(byte[] bytes)
    {
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

    public static void test()
    {
        DBJcore.Writeln("Brotli compression testing");
        DBJcore.Writeln("Length of original string: " + Program.originalString.Length);
        byte[] dataToCompress = System.Text.Encoding.UTF8.GetBytes(Program.originalString);
        byte[] compressedData = Brotli.Compress(dataToCompress);
        string compressedString = Convert.ToBase64String(compressedData);
        DBJcore.Writeln("Length of compressed string: " + compressedString.Length);
        byte[] decompressedData = Brotli.Decompress(compressedData);
        string deCompressedString = Convert.ToBase64String(decompressedData);
        DBJcore.Writeln("Length of decompressed string: " + deCompressedString.Length);
    }

} // BrotliStream
