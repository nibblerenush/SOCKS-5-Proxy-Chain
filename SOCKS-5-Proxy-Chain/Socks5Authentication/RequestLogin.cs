using System.Collections.Generic;
using System.IO;
using System.Text;
using SOCKS_5_Proxy_Chain.Socks5;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  public class RequestLogin
  {
    public RequestLogin(byte ver, byte loginLen, List<byte> login)
    {
      _ver = ver;
      _loginLen = loginLen;
      _login = new List<byte>(login);
      
      if (!this.CheckCorrectnessParams())
      {
        throw new ProtocolError("Incorrect format of socks5 request login");
      }
    }

    public byte[] GenBuff()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);

      bw.Write(_ver);
      bw.Write(_loginLen);
      bw.Write(_login.ToArray());
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      string login = Encoding.UTF8.GetString(_login.ToArray(), 0, _loginLen);
      sb.Append($"VER: {_ver}, LOGINLEN: {_loginLen}, LOGIN: {login}");
      return sb.ToString();
    }
    
    private bool CheckCorrectnessParams()
    {
      if
      (
        _ver != BaseConstants.Versions.LOGIN_HMAC ||
        _loginLen != _login.Count
      )
      {
        return false;
      }
      return true;
    }
    
    private readonly byte _ver;
    private readonly byte _loginLen;
    private readonly List<byte> _login;
  }
}
