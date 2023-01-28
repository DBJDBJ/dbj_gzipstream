
using System;
using System.Text;
using System.Net;

// .NET7
internal class StringSpecimen : IDisposable
{
    
    byte[] bytes = Array.Empty<byte>();
    private  string payload_ = string.Empty;
    private  string url_encoded_payload_ = string.Empty;

    static readonly int max_block_count = 64;
    
    private readonly int byte_size_ = 0; // can not be 0

    private readonly int payload_size_ = 0;
    private readonly int url_encode_payload_size_ = 0; 

    public StringSpecimen ( short block_count_ = 64 )
	{
        if ((block_count_ < 1) || (block_count_ > max_block_count))
            throw new ArgumentOutOfRangeException("requirement not satisfied:  0 < block_count_ <= 64");

        byte_size_ = block_count_ * 1024;
        bytes = new byte[byte_size_]; // 64 * 1024 = 64KB
		Random rnd = new Random();
		rnd.NextBytes(bytes);
        payload_ = Encoding.ASCII.GetString(bytes);
        url_encoded_payload_ = WebUtility.UrlEncode(payload_);

        payload_size_ = payload_.Length;
        url_encode_payload_size_ = url_encoded_payload_.Length;
    }

    public string RandomString { get { return payload_; } }
    public int RandomStringSize { get { return payload_size_; } }

    public string UrlEncodedRandomString { get { return url_encoded_payload_;  } }
    public int UrlEncodedRandomStringSize { get { return url_encode_payload_size_;  } }

    public int ByteSize { get { return byte_size_ ;  } }



    #region IDisposable implementation with finalizer
    private bool isDisposed = false;
public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
protected virtual void Dispose(bool disposing) {
  if (!isDisposed) {
    if (disposing) {
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