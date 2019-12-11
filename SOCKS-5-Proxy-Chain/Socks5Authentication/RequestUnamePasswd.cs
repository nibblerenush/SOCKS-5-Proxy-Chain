using System.Collections.Generic;
using System.IO;
using System.Text;
using SOCKS_5_Proxy_Chain.Socks5;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  public class RequestUnamePasswd
  {
    public RequestUnamePasswd(byte ver, byte ulen, List<byte> uname, byte plen, List<byte> passwd)
    {
      _ver = ver;
      _ulen = ulen;
      _uname = new List<byte>(uname);
      _plen = plen;
      _passwd = new List<byte>(passwd);
      
      if (!this.CheckCorrectnessParams())
      {
        throw new ProtocolError("Incorrect format of socks5 request uname passwd");
      }
    }

    public byte[] GenBuff()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);

      bw.Write(_ver);
      bw.Write(_ulen);
      bw.Write(_uname.ToArray());
      bw.Write(_plen);
      bw.Write(_passwd.ToArray());
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      string uname = Encoding.UTF8.GetString(_uname.ToArray(), 0, _ulen);
      string passwd = Encoding.UTF8.GetString(_passwd.ToArray(), 0, _plen);
      sb.Append($"VER: {_ver}, ULEN: {_ulen}, UNAME: {uname}, PLEN: {_plen}, PASSWD: {passwd}");
      return sb.ToString();
    }
    
    private bool CheckCorrectnessParams()
    {
      if
      (
        _ver != BaseConstants.Versions.UNAME_PASSWD ||
        _ulen != _uname.Count ||
        _plen != _passwd.Count
      )
      {
        return false;
      }
      return true;
    }
    
    private readonly byte _ver;
    private readonly byte _ulen;
    private readonly List<byte> _uname;
    private readonly byte _plen;
    private readonly List<byte> _passwd;
  }
}
