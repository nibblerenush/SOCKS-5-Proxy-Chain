using System.Collections.Generic;
using System.IO;
using System.Text;
using SOCKS_5_Proxy_Chain.Socks5;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  public class RequestHmac
  {
    public RequestHmac(byte ver, byte hmacLen, List<byte> hmac)
    {
      _ver = ver;
      _hmacLen = hmacLen;
      _hmac = new List<byte>(hmac);
      
      if (!this.CheckCorrectnessParams())
      {
        throw new ProtocolError("Incorrect format of socks5 request hmac");
      }
    }

    public byte[] GenBuff()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);

      bw.Write(_ver);
      bw.Write(_hmacLen);
      bw.Write(_hmac.ToArray());
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      string hmac = Encoding.UTF8.GetString(_hmac.ToArray(), 0, _hmacLen);
      sb.Append($"VER: {_ver}, HMAC_LEN: {_hmacLen}, HMAC: {hmac}");
      return sb.ToString();
    }
    
    private bool CheckCorrectnessParams()
    {
      if
      (
        _ver != BaseConstants.Versions.LOGIN_HMAC ||
        _hmacLen != _hmac.Count
      )
      {
        return false;
      }
      return true;
    }
    
    private readonly byte _ver;
    private readonly byte _hmacLen;
    private readonly List<byte> _hmac;
  }
}
