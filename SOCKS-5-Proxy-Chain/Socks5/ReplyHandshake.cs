using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain.Socks5
{
  public class ReplyHandshake
  {
    public ReplyHandshake(byte ver, byte method)
    {
      _ver = ver;
      _method = method;

      if (!this.CheckCorrectnessParams())
      {
        throw new ProtocolError("Incorrect format of socks5 reply handshake");
      }
    }

    public ReplyHandshake(byte[] buffer, int readedLength, byte neededMethod)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);

      _ver = br.ReadByte();
      _method = br.ReadByte();
      _readedLength = readedLength;
      _neededMethod = neededMethod;

      if (!this.CheckCorrectnessBuffer())
      {
        throw new ProtocolError("Incorrect format of socks5 reply handshake");
      }
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

    private bool CheckCorrectnessParams()
    {
      return _ver != BaseConstants.Versions.SOCKS5 ? false : true;
    }

    private bool CheckCorrectnessBuffer()
    {
      if
      (
        _ver != BaseConstants.Versions.SOCKS5 ||
        _readedLength < REPLY_HANDSHAKE_SIZE ||
        _method != _neededMethod
      )
      {
        return false;
      }
      return true;
    }

    private readonly byte _ver;
    private readonly byte _method;
    private readonly int _readedLength;
    private readonly byte _neededMethod;
    private const int REPLY_HANDSHAKE_SIZE = 2;
  }
}
