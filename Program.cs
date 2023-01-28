using System.Text;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace gzipstream;

sealed class Program
{
    internal static readonly string originalString = string.Empty;

    public Program()
    {
        var originalString = Cfg.get<string>("string_to_compress", "NOT CONFIGURED THIS IS ERROR SIGNAL");
    }

    public void Run()
    {
        var bencmhark = new BenchmarkCompression(originalString);
        bencmhark.GZipCompress();
        bencmhark.BrotliCompress();
    }

    public static void Main()
    {
        var app = new Program();
        app.Run();
    }

} // Program

