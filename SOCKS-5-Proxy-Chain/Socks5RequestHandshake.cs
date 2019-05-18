using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain
{
  class Socks5RequestHandshake
  {
    public Socks5RequestHandshake(byte ver, byte nmethods, List<byte> methods)
    {
      _ver = ver;
      _nmethods = nmethods;
      _methods = new List<byte>(methods);
    }

    public Socks5RequestHandshake(byte[] buffer)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);
      //
      _ver = br.ReadByte();
      _nmethods = br.ReadByte();
      _methods = new List<byte>(br.ReadBytes(_nmethods));
    }

    public byte[] GenerateBuffer()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);
      //
      bw.Write(_ver);
      bw.Write(_nmethods);
      bw.Write(_methods.ToArray());
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("VER: {0}, NMETHODS: {1}, METHODS: ", _ver, _nmethods);
      for (int i = 0; i < _nmethods; ++i)
      {
        sb.AppendFormat("{0}", _methods[i]);
      }
      return sb.ToString();
    }

    private readonly byte _ver;
    private readonly byte _nmethods;
    private readonly List<byte> _methods;
  }
}
