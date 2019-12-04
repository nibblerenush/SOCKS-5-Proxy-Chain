using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain.Socks5
{
  class ReplyHandshake
  {
    public ReplyHandshake(byte ver, byte method)
    {
      _ver = ver;
      _method = method;
    }

    public ReplyHandshake(byte[] buffer)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);

      _ver = br.ReadByte();
      _method = br.ReadByte();
    }

    public byte[] GenBuff()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);

      bw.Write(_ver);
      bw.Write(_method);
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append($"VER: {_ver}, METHOD: {_method}");
      return sb.ToString();
    }

    private readonly byte _ver;
    private readonly byte _method;
  }
}
