// #define USE_RANDOM_STRING_SPECIMEN
//using System.Diagnostics;
//using System.Text;
using BenchmarkDotNet.Running;
//using Gee.External.Capstone.M68K;
//using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;

namespace gzipstream;

internal sealed class Program
{
    /// <summary>
    /// prepare data before program starts. make sure this ctor is not
    /// accidentaly called from other static ctors
    /// here we generate data used by tests through <see cref="originalString"/> 
    /// </summary>
    static Program()
    {
#if DEBUG
        throw new Exception("Benchmarking project can be only running in release mode builds!");
#endif

    }
  
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
           // input string is generated in static constructor of the Program
           // in release mode we do not use Xunit Tests 
           // we use this
           var summary = BenchmarkRunner.Run<BenchmarkCompression>();
        } catch(Exception e)
        {
            DBJLog.error(e.Message);
        }

        DBJLog.info("Finished " + DBJLog.app_friendly_name);
    }
} // Program

