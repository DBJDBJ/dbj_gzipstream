// #define USE_RANDOM_STRING_SPECIMEN
using System.Diagnostics;
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

    /// <summary>
    /// prepare data before program starts. make sure this ctor is not
    /// accidentaly called from other static ctors
    /// </summary>
    static Program()
    {
        configuredString = DBJCfg.get<string>("string_to_compress", "ERROR");

        Debug.Assert(configuredString != "ERROR");

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
        // these are both XUnit "facts"
        //GZipCompressor.test();
        //Brotli.test();
    }

#if ! DEBUG
    //[STAThread]
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
#endif // RELEASE
} // Program

