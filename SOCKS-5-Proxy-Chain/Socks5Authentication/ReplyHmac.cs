using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  public class ReplyHmac
  {
    public ReplyHmac(byte[] buffer, int readedLength)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);
      
      _ver = br.ReadByte();
      _status = br.ReadByte();
      _readedLength = readedLength;
    }
    
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append($"VER: {_ver}, STATUS: {_status}");
      return sb.ToString();
    }

     private bool CheckCorrectnessBuffer()
    {
      if
      (
        _ver != BaseConstants.Versions.LOGIN_HMAC ||
        _readedLength != REPLY_HMAC_SIZE
      )
      {
        return false;
      }
      return true;
    }
    
    public byte Status
    {
      get
      {
        return _status;
      }
    }

    private readonly byte _ver;
    private readonly byte _status;
    private readonly int _readedLength;

    private const int REPLY_HMAC_SIZE = 2;
  }
}
