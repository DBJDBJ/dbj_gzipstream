
using System;
using System.Net;
using System.Text;

// .NET7
internal class RandomStringSpecimen : IDisposable
{

    byte[] bytes = Array.Empty<byte>();
    private string payload_ = string.Empty;
    private string url_encoded_payload_ = string.Empty;

    // externaly configured
    static readonly short max_block_count = 0;
    static readonly short max_block_count_default = 1 ;

    /// <summary>
    /// maximal byte array held will be MaxBlockCount * 1024
    /// thus if MaxBlockCount is 64, max byte [] size is 64KB
    /// </summary>
    public static short MaxBlockCount
    {
        get
        {
            return max_block_count;
        }
    }

    /// <summary>
    /// Use the external configuration
    /// Use <c>max_block_count_default</c> if config value not found
    /// </summary>
    static RandomStringSpecimen()
    {
        max_block_count = DBJCfg.get<short>("max_block_count", 0 /* provokes exception */ );

        if (max_block_count < 1)
            max_block_count = max_block_count_default  ;
        DBJcore.Writerr("key: 'max_block_count' not found in: " + DBJCfg.FileName);

    }

    private readonly int byte_size_ = 0; // can not be 0

    private readonly int payload_size_ = 0;
    private readonly int url_encode_payload_size_ = 0;

    /// <summary>
    /// Create byte[], random string and url encoded random string, inside this constructor
    /// Size in bytes is 1024 * the value of the argument <paramref name="block_count_"/> given
    /// </summary>
    /// <param name="block_count_">must be in 1 ... <see cref="MaxBlockCount"/> range</param>
    /// <exception cref="ArgumentOutOfRangeException">is thrown on argument value out of range 1 .. <see cref="MaxBlockCount"/> </exception>
    public RandomStringSpecimen(short block_count_)
    {
        if ((block_count_ < 1) || (block_count_ > max_block_count))
            throw new ArgumentOutOfRangeException($"argument requirement not satisfied:  0 < block_count_ <= {max_block_count}");

        byte_size_ = block_count_ * 1024;
        bytes = new byte[byte_size_]; // 64 * 1024 = 64KB
        Random rnd = new Random();
        rnd.NextBytes(bytes);
        payload_ = Encoding.ASCII.GetString(bytes);
        url_encoded_payload_ = WebUtility.UrlEncode(payload_);

        payload_size_ = payload_.Length;
        url_encode_payload_size_ = url_encoded_payload_.Length;
    }

    /// <summary>
    /// return the random string generated
    /// </summary>
    public string RandomString { get { return payload_; } }
    /// <summary>
    /// return the size of the random string generated
    /// </summary>
    public int RandomStringSize { get { return payload_size_; } }

    /// <summary>
    /// return the url encoded random string generated
    /// </summary>
    public string UrlEncodedRandomString { get { return url_encoded_payload_; } }
    /// <summary>
    /// return the size of the url encoded random string generated
    /// </summary>
    public int UrlEncodedRandomStringSize { get { return url_encode_payload_size_; } }

    /// <summary>
    /// return the size of the internal byte []
    /// </summary>
    public int ByteSize { get { return byte_size_; } }



    #region IDisposable implementation with finalizer
    private bool isDisposed = false;
    public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                // 
                bytes = Array.Empty<byte>();
                payload_ = string.Empty;
                url_encoded_payload_ = string.Empty;
            }
        }
        isDisposed = true;
    }
    #endregion
}