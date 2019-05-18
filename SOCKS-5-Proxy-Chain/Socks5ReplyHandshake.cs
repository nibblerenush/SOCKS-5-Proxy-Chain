using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain
{
  class Socks5ReplyHandshake
  {
    public Socks5ReplyHandshake(byte ver, byte method)
    {
      _ver = ver;
      _method = method;
    }

    public Socks5ReplyHandshake(byte[] buffer)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);
      //
      _ver = br.ReadByte();
      _method = br.ReadByte();
    }

    public byte[] GenerateBuffer()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);
      //
      bw.Write(_ver);
      bw.Write(_method);
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("VER: {0}, METHOD: {1}", _ver, _method);
      return sb.ToString();
    }

    private readonly byte _ver;
    private readonly byte _method;
  }
}
