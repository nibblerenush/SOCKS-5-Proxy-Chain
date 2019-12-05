using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain
{
  class Socks5RequestUnamePasswd
  {
    public Socks5RequestUnamePasswd(byte ver, byte ulen, List<byte> uname, byte plen, List<byte> passwd)
    {
      _ver = ver;
      _ulen = ulen;
      _uname = new List<byte>(uname);
      _plen = plen;
      _passwd = new List<byte>(passwd);
    }

    public Socks5RequestUnamePasswd(byte[] buffer)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);
      //
      _ver = br.ReadByte();
      _ulen = br.ReadByte();
      _uname = new List<byte>(br.ReadBytes(_ulen));
      _plen = br.ReadByte();
      _passwd = new List<byte>(br.ReadBytes(_plen));
    }

    public byte[] GenerateBuffer()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);
      //
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
      string username = Encoding.UTF8.GetString(_uname.ToArray(), 0, _ulen);
      string password = Encoding.UTF8.GetString(_passwd.ToArray(), 0, _plen);
      sb.AppendFormat(
        "VER: {0}, ULEN: {1}, UNAME: {2}, PLEN: {3}, PASSWD: {4}",
        _ver, _ulen, username, _plen, password);
      return sb.ToString();
    }

    private readonly byte _ver;
    private readonly byte _ulen;
    private readonly List<byte> _uname;
    private readonly byte _plen;
    private readonly List<byte> _passwd;
  }
}
