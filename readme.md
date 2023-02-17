
# [Benchmark .NET Core, C# string compression and decompression ](https://www.infoworld.com/article/3660629/how-to-compress-and-decompress-strings-in-c-sharp.html) 

---

Xunit and DotNetBenchmarking can not coexist in one project. Thus we have two projects.
WE are building them in VStudio. One must run the release build for benchmarking. Debug or release build for testing.
Hint: Names of the projects are hints, which is which.

- For benchmarking run `stringcompsressdecompressbench` only in release mode

- For Xunit testing run `stringcompsressdecompresstest` in whichever build you like

Everything uses https://github.com/on-the-cloud-side/dbjcore as a submodule. Visual Studio Shared Code project.

---

-  Compare .NET Core GZip and Brotli compression methods to reduce the size of string data and improve performance in your .NET Core applications.

- DeflateStream is also added just to make it more ... whatever. But it is not.

- **GZip** consistently wins and most importanly can be used in a JSON string value.

