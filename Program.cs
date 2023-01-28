using System.Text;

string originalString = "To work with BenchmarkDotNet you must install the BenchmarkDotNet package. " +
"You can do this either via the NuGet Package Manager inside the Visual Studio IDE, " +
"or by executing the Install-Package BenchmarkDotNet command at the NuGet Package Manager Console";


byte[] dataToCompress = Encoding.UTF8.GetBytes(originalString);
byte[] compressedData = GZipCompressor.Compress(dataToCompress);

string compressedString = Encoding.UTF8.GetString(compressedData);
Console.WriteLine("Length of compressed string: " + compressedString.Length);
byte[] decompressedData = GZipCompressor.Decompress(compressedData);
string deCompressedString = Encoding.UTF8.GetString(decompressedData);
Console.WriteLine("Length of decompressed string: " + deCompressedString.Length);

