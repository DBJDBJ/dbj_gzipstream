#define USE_RANDOM_STRING_SPECIMEN
using System.Text;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace gzipstream;

sealed class Program
{
    // user code need to see this 
    internal static string originalString = string.Empty;

    static string configuredString = string.Empty;
    static string randomString = string.Empty;

    public Program()
    {
        configuredString = Cfg.get<string>("string_to_compress", "NOT CONFIGURED; ERROR");

        short configured_specimen_blocks = Cfg.get<short>("specimen_blocks", 0 /* provokes exception */ );

        using (var specimen_ = new StringSpecimen(configured_specimen_blocks))
        {
            randomString = specimen_.UrlEncodedRandomString;
        }

#if USE_RANDOM_STRING_SPECIMEN
        originalString = randomString;
#else
        originalString = configuredString;
#endif
        Log.info("starting with this string specimen: " + originalString);
    }

    public void Run()
    {
        var bencmhark = new BenchmarkCompression();
        bencmhark.GZipCompress();
        bencmhark.BrotliCompress();
    }

    public static void Main()
    {
#if DEBUG
        var app = new Program();
        app.Run();
        DBJcore.Writeln(DBJcore.Name() + " Done");
#else
              var summary = BenchmarkRunner.Run<BenchmarkCompression>();              
#endif
    }

} // Program

