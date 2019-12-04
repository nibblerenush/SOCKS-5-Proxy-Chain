using System;

namespace SOCKS_5_Proxy_Chain.Socks5
{
  public class ProtocolError: Exception
  {
    public ProtocolError(string message): base(message)
    {}
    
    public ProtocolError(string message, Exception ex): base(message, ex)
    {}
  }
}
