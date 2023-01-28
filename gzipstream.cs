using System.IO.Compression;
using System.Net;

var uri = new Uri("https://dbj.org");
var req = WebRequest.CreateHttp(uri);

/*
 * Headers
 */
req.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";

/*
 * Execute
 */
try
{
    using (var resp = await req.GetResponseAsync())
    {
        using (var str = resp.GetResponseStream())
        using (var gsr = new GZipStream(str, CompressionMode.Decompress))
        using (var sr = new StreamReader(gsr))

        {
            string s = await sr.ReadToEndAsync();
        }
    }
}
catch (WebException ex)
{
    // "!" is "forgiving null operator"
    using (HttpWebResponse response = (HttpWebResponse)ex.Response!)
    {
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            string respStr = sr.ReadToEnd();
            int statusCode = (int)response.StatusCode;

            string errorMsg = $"Request ({uri}) failed ({statusCode}) on, with error: {respStr}";
        }
    }
}