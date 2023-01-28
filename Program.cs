using System.Text;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace gzipstream;

sealed class Program
{
    internal readonly static string originalString = "To work with BenchmarkDotNet you must install the BenchmarkDotNet package. " +
    "You can do this either via the NuGet Package Manager inside the Visual Studio IDE, " +
    "or by executing the Install-Package BenchmarkDotNet command at the NuGet Package Manager Console";

    public static void Main()
    {
        var bencmhark = new BenchmarkCompression(originalString);
        bencmhark.GZipCompress();
        bencmhark.BrotliCompress();
    }

} // Program

