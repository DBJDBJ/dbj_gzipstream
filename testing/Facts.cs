// make sure this is not built with dotnet benchmark projects

using System.Diagnostics;
using System.Text;
using Xunit;

namespace gzipstream;
public class Tests_are_here
{
    static readonly string originalString = string.Empty;
    static Tests_are_here()
    {
#if USE_RANDOM_STRING_SPECIMEN
        using (var specimen_ = new RandomStringSpecimen())
        {
            originalString = specimen_.UrlEncodedRandomString;
        }
#else
        originalString = new StringSpecimen().Payload;
#endif
    }

    [Fact]
    public void Test_Gzip()
    {
        DBJcore.Writeln("GZip compression testing ---------------------------------------------------");

        DBJcore.Writeln("Length of original string: " + originalString.Length);
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        byte[] compressedData = GZipCompressor.Compress(dataToCompress);

        string compressedString = Encoding.UTF8.GetString(compressedData);
        DBJcore.Writeln("Length of compressed string: " + compressedString.Length);
        byte[] decompressedData = GZipCompressor.Decompress(compressedData);
        string deCompressedString = Encoding.UTF8.GetString(decompressedData);
        DBJcore.Writeln("Length of decompressed string: " + deCompressedString.Length);

        Assert.Equal(originalString.Length, deCompressedString.Length);

    }
    [Fact]
    static public void Test_Deflator()
    {
        DBJcore.Writeln("DeflateStream compression testing ---------------------------------------------------");

        DBJcore.Writeln("Length of original string: " + originalString.Length);
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        byte[] compressedData = DeflatorStringCompression.Compress(dataToCompress);

        string compressedString = Encoding.UTF8.GetString(compressedData);
        DBJcore.Writeln("Length of compressed string: " + compressedString.Length);
        byte[] decompressedData = DeflatorStringCompression.Decompress(compressedData);
        string deCompressedString = Encoding.UTF8.GetString(decompressedData);
        DBJcore.Writeln("Length of decompressed string: " + deCompressedString.Length);

        Assert.Equal(originalString.Length, deCompressedString.Length);

    }
    [Fact]
    public void Test_Brotli()
    {
        // this goes to file thus it is perfectly usable from 
        // unit tests, unlike Console which is not
        DBJcore.Writeln("Brotli compression testing ------------------------------------------");
        DBJcore.Writeln("Length of original string: " + originalString.Length);
        /// transform to UTF8 byte []
        byte[] dataToCompress = System.Text.Encoding.UTF8.GetBytes(originalString);
        /// compress the byte []
        byte[] compressedData = Brotli.Compress(dataToCompress) ;
        /// arrive to string 
        string compressedString = Convert.ToBase64String(compressedData);
        DBJcore.Writeln("Length of compressed string: " + compressedString.Length);

        /// decompress the byte []
        byte[] decompressedData = Brotli.Decompress(compressedData);
        /// gt the string and its length
        string deCompressedString = Convert.ToBase64String(decompressedData);
        DBJcore.Writeln("Length of decompressed string: " + deCompressedString.Length);

        // this is net core
        //Debug.Assert(originalString.Length == deCompressedString.Length);
        // and this is Xunit
        Assert.Equal (originalString.Length, deCompressedString.Length);
    }
} // Tests_are_here


