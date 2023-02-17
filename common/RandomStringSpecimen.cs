
using System;
using System.Diagnostics;
using System.Net;
using System.Text;

internal class StringSpecimen 
{
    /// <summary>
    /// return the random string generated
    /// </summary>
    public int PayloadSize { get { return payload_size_; } }
    protected int payload_size_ = 0;

    /// <summary>
    /// return the byte []
    /// </summary>
    public byte[] Bytes { get { return bytes_; } }
    protected byte[] bytes_ = Array.Empty<byte>();


    /// <summary>
    /// return the random string generated
    /// </summary>
    public string Payload { get { return payload_; } }
    protected string payload_ = string.Empty;

    /// <summary>
    /// return the size of the internal byte []
    /// </summary>
    public int ByteSize { get { return byte_size_; } }
    protected int byte_size_ = 0; // can not be 0

    public StringSpecimen()
    {
        payload_ = DBJCfg.get<string>("string_to_compress", "ERROR");
        Debug.Assert(payload_ != "ERROR");
        DBJLog.info("Starting with CONFIGURED string specimen. Obtained from : " + DBJCfg.FileName );
        
        payload_size_ = payload_.Length;

        bytes_ = Encoding.ASCII.GetBytes(payload_);
        byte_size_ = bytes_.Length;
    }

}

// .NET7
internal class RandomStringSpecimen : StringSpecimen, IDisposable
{
    /// <summary>
    /// maximal byte array held will be MaxBlockCount * 1024
    /// thus if MaxBlockCount is 64, max byte [] size is 64KB
    /// </summary>
    public static short MaxBlockCount  { get { return max_block_count; } }
    static readonly short max_block_count = 0;
    static readonly short max_block_count_default = 1;

    /// <summary>
    /// Use the external configuration
    /// Use <c>max_block_count_default</c> if config value not found
    /// </summary>
    static RandomStringSpecimen()
    {
        max_block_count = DBJCfg.get<short>("max_block_count", 0 /* provokes exception */ );

        if (max_block_count < 1)
        {
            max_block_count = max_block_count_default;
            DBJLog.error("key: 'max_block_count' not found in: " + DBJCfg.FileName + ", going to use default value: " + max_block_count_default);
        }

        configured_specimen_blocks = DBJCfg.get<short>("specimen_blocks", 0 /* provokes exception */ );

        if (( configured_specimen_blocks < 1) || (configured_specimen_blocks > max_block_count) )
        {
            configured_specimen_blocks = 1;
            DBJLog.error("key: 'specimen_blocks' not found in: " + DBJCfg.FileName + ", going to use default value: " + 1);
        }

    }
    static readonly short configured_specimen_blocks = 0;
    private readonly int url_encode_payload_size_ = 0;

    /// <summary>
    /// Create byte[], random string and url encoded random string, inside this constructor
    /// Size in bytes is 1024 * the value of the argument <paramref name="block_count_"/> given
    /// </summary>
    /// <param name="block_count_">must be in 1 ... <see cref="MaxBlockCount"/> range</param>
    /// <exception cref="ArgumentOutOfRangeException">is thrown on argument value out of range 1 .. <see cref="MaxBlockCount"/> </exception>
    public RandomStringSpecimen( )
    {
        byte_size_ = configured_specimen_blocks * 1024;
        bytes_ = new byte[byte_size_]; // 64 * 1024 = 64KB
        Random rnd = new Random();
        rnd.NextBytes(bytes_);
        payload_ = Encoding.ASCII.GetString(bytes_);
        url_encoded_payload_ = WebUtility.UrlEncode(payload_);

        payload_size_ = payload_.Length;
        url_encode_payload_size_ = url_encoded_payload_.Length;

        DBJLog.info("Starting with RANDOM string specimen");
        DBJLog.info("Byte Size: " + byte_size_);
        DBJLog.info("String Size: " + payload_size_);
        DBJLog.info("Url Encoded String Size: " + url_encode_payload_size_);
    }

    /// <summary>
    /// return the size of the random string generated
    /// </summary>
    public int RandomStringSize { get { return payload_size_; } }

    /// <summary>
    /// return the url encoded random string generated
    /// </summary>
    public string UrlEncodedRandomString { get { return url_encoded_payload_; } }
    private string url_encoded_payload_ = string.Empty;

    /// <summary>
    /// return the size of the url encoded random string generated
    /// </summary>
    public int UrlEncodedRandomStringSize { get { return url_encode_payload_size_; } }

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
                bytes_ = Array.Empty<byte>();
                payload_ = string.Empty;
            }
        }
        isDisposed = true;
    }
    #endregion

}