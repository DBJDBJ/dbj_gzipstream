//https://stackoverflow.com/a/43357353
using System.IO.Compression;
using System.Text;

namespace gzipstream;

public static class DeflatorStringCompression
{
    /// <summary>
    /// Compresses a string and returns a deflate compressed, Base64 encoded string.
    /// </summary>
    /// <param name="uncompressedString">String to compress</param>
    public static byte [] Compress(byte [] uncompressed_bytes )
    {
        byte[] compressedBytes;

        //using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
        using (var uncompressedStream = new MemoryStream(uncompressed_bytes))
        {
            using (var compressedStream = new MemoryStream())
            {
                // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
                {
                    uncompressedStream.CopyTo(compressorStream);
                }

                // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                compressedBytes = compressedStream.ToArray();
            }
        }

        return compressedBytes ;
        //return Convert.ToBase64String(compressedBytes);
    }

    /// <summary>
    /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
    /// </summary>
    /// <param name="compressedString">String to decompress.</param>
    public static byte [] Decompress(byte[] compresed_bytes )
    {
        byte[] decompressedBytes;

        var compressedStream = new MemoryStream(compresed_bytes);

        using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
        {
            using (var decompressedStream = new MemoryStream())
            {
                decompressorStream.CopyTo(decompressedStream);

                decompressedBytes = decompressedStream.ToArray();
            }
        }

        return decompressedBytes;
        //return Encoding.UTF8.GetString(decompressedBytes);
    }


} // DeflatorStringCompression

/*
 You can drop the class below into an existing project and then use thusly:

var uncompressedString = "Hello World!";
var compressedString = uncompressedString.Compress();

and

var decompressedString = compressedString.Decompress();
 */
public static class Extensions
{
    /// <summary>
    /// Compresses a string and returns a deflate compressed, Base64 encoded string.
    /// </summary>
    /// <param name="uncompressedString">String to compress</param>
    public static string Compress(this string uncompressedString)
    {
        byte[] compressedBytes;

        using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
        {
            using (var compressedStream = new MemoryStream())
            {
                // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
                {
                    uncompressedStream.CopyTo(compressorStream);
                }

                // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                compressedBytes = compressedStream.ToArray();
            }
        }

        return Convert.ToBase64String(compressedBytes);
    }

    /// <summary>
    /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
    /// </summary>
    /// <param name="compressedString">String to decompress.</param>
    public static string Decompress(this string compressedString)
    {
        byte[] decompressedBytes;

        var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

        using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
        {
            using (var decompressedStream = new MemoryStream())
            {
                decompressorStream.CopyTo(decompressedStream);

                decompressedBytes = decompressedStream.ToArray();
            }
        }

        return Encoding.UTF8.GetString(decompressedBytes);
    }
}