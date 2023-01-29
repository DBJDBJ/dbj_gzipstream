using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Xunit;

namespace gzipstream;

/// <summary>
/// test classes must be public
/// </summary>
public class GZipCompressor
{

    public static byte[] Compress(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                gzipStream.Write(bytes, 0, bytes.Length);
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
                using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    decompressStream.CopyTo(outputStream);
                }
                return outputStream.ToArray();
            }
        }
    }

    #region asynchronous
    public async static Task<byte[]> CompressAsync(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                await gzipStream.WriteAsync(bytes, 0, bytes.Length);
            }
            return memoryStream.ToArray();
        }
    }
    public async static Task<byte[]> DecompressAsync(byte[] bytes)
    {
        using (var memoryStream = new MemoryStream(bytes))
        {
            using (var outputStream = new MemoryStream())
            {
                using (var decompressStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    await decompressStream.CopyToAsync(outputStream);
                }
                return outputStream.ToArray();
            }
        }
    }
    #endregion

    [Fact]
    public void test()
    {
        DBJcore.Writeln("GZip compression testing ---------------------------------------------------");

        DBJcore.Writeln("Length of original string: " + Program.originalString.Length);
        byte[] dataToCompress = Encoding.UTF8.GetBytes(Program.originalString);
        byte[] compressedData = GZipCompressor.Compress(dataToCompress);

        string compressedString = Encoding.UTF8.GetString(compressedData);
        DBJcore.Writeln("Length of compressed string: " + compressedString.Length);
        byte[] decompressedData = GZipCompressor.Decompress(compressedData);
        string deCompressedString = Encoding.UTF8.GetString(decompressedData);
        DBJcore.Writeln("Length of decompressed string: " + deCompressedString.Length);

        Assert.Equal(Program.originalString.Length, deCompressedString.Length);

    }
}
