using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  public class ReplyLogin
  {
    public ReplyLogin(byte[] buffer, int readedLength)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);
      
      _ver = br.ReadByte();
      _status = br.ReadByte();
      _randomStringLen = br.ReadByte();
      _randomString = br.ReadBytes(_randomStringLen);
      _readedLength = readedLength;
    }
    
    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      string randomString = Encoding.UTF8.GetString(_randomString, 0, _randomStringLen);
      sb.Append($"VER: {_ver}, STATUS: {_status}, RANDOM_STRING_LEN: {_randomStringLen}, RANDOM_STRING: {randomString}");
      return sb.ToString();
    }

     private bool CheckCorrectnessBuffer()
    {
      if
      (
        _ver != BaseConstants.Versions.LOGIN_HMAC ||
        _readedLength < REPLY_LOGIN_HMAC_MIN_SIZE ||
        _readedLength > REPLY_LOGIN_HMAC_MAX_SIZE ||
        _randomStringLen != _randomString.Length
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

    public byte [] RandomString
    {
      get
      {
        return _randomString;
      }
    }

    private readonly byte _ver;
    private readonly byte _status;
    private readonly byte _randomStringLen;
    private readonly byte [] _randomString;
    private readonly int _readedLength;

    private const int REPLY_LOGIN_HMAC_MIN_SIZE = 4;
    private const int REPLY_LOGIN_HMAC_MAX_SIZE = 258;
  }
}
