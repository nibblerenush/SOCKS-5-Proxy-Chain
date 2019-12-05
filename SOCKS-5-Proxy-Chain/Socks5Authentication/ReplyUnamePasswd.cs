using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain
{
  class Socks5ReplyUnamePasswd
  {
    public Socks5ReplyUnamePasswd(byte ver, byte status)
    {
      _ver = ver;
      _status = status;
    }

    public Socks5ReplyUnamePasswd(byte[] buffer)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);
      //
      _ver = br.ReadByte();
      _status = br.ReadByte();
    }

    public byte[] GenerateBuffer()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);
      //
      bw.Write(_ver);
      bw.Write(_status);
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("VER: {0}, STATUS: {1}", _ver, _status);
      return sb.ToString();
    }

    private readonly byte _ver;
    private readonly byte _status;
  }
}
