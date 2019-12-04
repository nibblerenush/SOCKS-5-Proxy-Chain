using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SOCKS_5_Proxy_Chain.Socks5
{
  class RequestHandshake
  {
    public RequestHandshake(byte ver, byte nmethods, List<byte> methods)
    {
      _ver = ver;
      _nmethods = nmethods;
      _methods = new List<byte>(methods);

      if (!this.CheckCorrectnessParams())
      {
        throw new ProtocolError("Incorrect format of socks5 request handshake");
      }
    }
    
    public RequestHandshake(byte[] buffer, int readedLength, byte neededMethod)
    {
      MemoryStream ms = new MemoryStream(buffer);
      BinaryReader br = new BinaryReader(ms);

      _ver = br.ReadByte();
      _nmethods = br.ReadByte();
      _methods = new List<byte>(br.ReadBytes(_nmethods));
      _readedLength = readedLength;
      _neededMethod = neededMethod;

      if (!this.CheckCorrectnessBuffer())
      {
        throw new ProtocolError("Incorrect format of socks5 request handshake");
      }
    }

    public byte[] GenBuff()
    {
      MemoryStream ms = new MemoryStream();
      BinaryWriter bw = new BinaryWriter(ms);

      bw.Write(_ver);
      bw.Write(_nmethods);
      bw.Write(_methods.ToArray());
      return ms.ToArray();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append($"VER: {_ver}, NMETHODS: {_nmethods}, METHODS: ");
      foreach (var method in _methods)
      {
        sb.Append($"{method}");
      }
      return sb.ToString();
    }

    private bool CheckCorrectnessParams()
    {
      if
      (
        _ver != BaseConstants.Versions.SOCKS5 ||
        _nmethods != _methods.Count ||
        _nmethods < MIN_METHODS_LENGTH ||
        _nmethods > MAX_METHODS_LENGTH
      )
      {
        return false;
      }

      return true;
    }

    private bool CheckCorrectnessBuffer()
    {
      if
      (
        _ver != BaseConstants.Versions.SOCKS5 ||
        _readedLength < MIN_REQUEST_HANDSHAKE_SIZE ||
        _readedLength > MAX_REQUEST_HANDSHAKE_SIZE
      )
      {
        return false;
      }
      
      return _methods.Contains(_neededMethod);
    }

    private readonly byte _ver;
    private readonly byte _nmethods;
    private readonly List<byte> _methods;
    private readonly int _readedLength;
    private readonly byte _neededMethod;
    
    private const int MIN_REQUEST_HANDSHAKE_SIZE = 3;
    private const int MAX_REQUEST_HANDSHAKE_SIZE = 257;
    private const int MIN_METHODS_LENGTH = 1;
    private const int MAX_METHODS_LENGTH = 255;
  }
}
