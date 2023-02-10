// make sure this is not built with dotnet benchmark projects
#define USE_RANDOM_STRING_SPECIMEN
using System.Diagnostics;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Runtime.CompilerServices;

namespace gzipstream;
public class Tests_are_here
{
    private readonly ITestOutputHelper output;
    static readonly string originalString = string.Empty;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void writeln(string txt)
    {
        output.WriteLine(txt);
    }

    public Tests_are_here(ITestOutputHelper output)
    {
        this.output = output;
    }
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

#if USE_RANDOM_STRING_SPECIMEN
    [Fact]
    public void Test_64bit_encoding()
    {
        writeln("Test_64bit_encoding ---------------------------------------------------");

        writeln("Length of original UrlEncodedRandomString: " + originalString.Length);
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        byte[] compressedData = GZipCompressor.Compress(dataToCompress);

        string compressedString = Encoding.UTF8.GetString(compressedData);
        writeln("Length of GZIP compressed UrlEncodedRandomString: " + compressedString.Length);
        byte[] decompressedData = GZipCompressor.Decompress(compressedData);
        string deCompressedString = Encoding.UTF8.GetString(decompressedData);
        writeln("Length of decompressed UrlEncodedRandomString: " + deCompressedString.Length);

        Assert.Equal(originalString.Length, deCompressedString.Length);

    }
#endif
    [Fact]
    public void Test_Gzip()
    {
        writeln("GZip compression testing ---------------------------------------------------");

        writeln("Length of original string: " + originalString.Length);
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        byte[] compressedData = GZipCompressor.Compress(dataToCompress);

        string compressedString = Encoding.UTF8.GetString(compressedData);
        writeln("Length of compressed string: " + compressedString.Length);
        byte[] decompressedData = GZipCompressor.Decompress(compressedData);
        string deCompressedString = Encoding.UTF8.GetString(decompressedData);
        writeln("Length of decompressed string: " + deCompressedString.Length);

        Assert.Equal(originalString.Length, deCompressedString.Length);

    }
    [Fact]
    public void Test_Deflator()
    {
        writeln("DeflateStream compression testing ---------------------------------------------------");

        writeln("Length of original string: " + originalString.Length);
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        byte[] compressedData = DeflatorStringCompression.Compress(dataToCompress);

        string compressedString = Encoding.UTF8.GetString(compressedData);
        writeln("Length of compressed string: " + compressedString.Length);
        byte[] decompressedData = DeflatorStringCompression.Decompress(compressedData);
        string deCompressedString = Encoding.UTF8.GetString(decompressedData);
        writeln("Length of decompressed string: " + deCompressedString.Length);

        Assert.Equal(originalString.Length, deCompressedString.Length);

    }
    [Fact]
    public void Test_Brotli()
    {
        // this goes to file thus it is perfectly usable from 
        // unit tests, unlike Console which is not
        writeln("Brotli compression testing ------------------------------------------");
        writeln("Length of original string: " + originalString.Length);
        /// transform to UTF8 byte []
        byte[] dataToCompress = System.Text.Encoding.UTF8.GetBytes(originalString);
        /// compress the byte []
        byte[] compressedData = Brotli.Compress(dataToCompress) ;
        /// arrive to string 
        string compressedString = Convert.ToString(compressedData);
        writeln("Length of compressed string: " + compressedString.Length);

        /// decompress the byte []
        byte[] decompressedData = Brotli.Decompress(compressedData);
        /// gt the string and its length
        var decompressed_string = System.Text.Encoding.UTF8.GetString(decompressedData);
        // string deCompressedString = Convert.ToBase64String(decompressedData);
        writeln("Length of decompressed string: " + decompressed_string.Length);

        // this is net core
        //Debug.Assert(originalString.Length == deCompressedString.Length);
        // and this is Xunit
        Assert.Equal (originalString.Length, decompressed_string.Length);
    }
} // Tests_are_here


