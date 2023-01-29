#define USE_RANDOM_STRING_SPECIMEN
using System.Text;
using BenchmarkDotNet.Running;
using Gee.External.Capstone.M68K;
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
        configuredString = DBJCfg.get<string>("string_to_compress", "ERROR");

        if (configuredString == "ERROR")
            throw new Exception("key: 'string_to_compress' not found in: " +  DBJCfg.FileName );

        short configured_specimen_blocks = DBJCfg.get<short>("specimen_blocks", 0 /* provokes exception */ );

        using (var specimen_ = new RandomStringSpecimen(configured_specimen_blocks))
        {
            randomString = specimen_.UrlEncodedRandomString;
            DBJcore.Writeln("Starting with RANDOM string specimen");
            DBJcore.Writeln("Byte Size: " + specimen_.ByteSize );
            DBJcore.Writeln("String Size: " + specimen_.RandomStringSize );
            DBJcore.Writeln("Url Encoded String Size: " + specimen_.UrlEncodedRandomStringSize );
        }

#if USE_RANDOM_STRING_SPECIMEN
        originalString = randomString;
#else
        originalString = configuredString;
        DBJcore.Writeln("Starting with CONFIGURED string specimen.");
#endif
    }

    public void Run()
    {
        GZipCompressor.test();
        Brotli.test();
    }

    public static void Main()
    {
        try
        {
            var app = new Program();
#if DEBUG
        app.Run();
        DBJcore.Writeln(DBJcore.Name() + " Done");
#else
            var summary = BenchmarkRunner.Run<BenchmarkCompression>();
#endif
        } catch(Exception e)
        {
            DBJcore.Writerr(e.Message);
        }

        DBJcore.Writeln("Finished " + DBJLog.app_friendly_name);
    }

} // Program

