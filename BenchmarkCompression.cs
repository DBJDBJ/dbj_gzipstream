using System.Text;
using BenchmarkDotNet.Attributes;

namespace gzipstream;

[MemoryDiagnoser]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public sealed class BenchmarkCompression
{
    readonly string originalString = string.Empty ;

    public BenchmarkCompression( )
    {
        originalString = Program.originalString;
    }

    [Benchmark]
    public void GZipCompress()
    {
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        var compressedData = GZipCompressor.Compress(dataToCompress);
    }

    [Benchmark]
    public void BrotliCompress()
    {
        byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
        var compressedData = Brotli.Compress(dataToCompress);
    }
}