using System.Collections.Generic;

namespace SOCKS_5_Proxy_Chain.Socks5
{
  public class UnamePasswdHandshakeCreator : IHandshakeCreator
  {
    public RequestHandshake Create()
    {
      byte ver = BaseConstants.Versions.SOCKS5;
      byte nmethods = 1;
      List<byte> methods = new List<byte>() { BaseConstants.Methods.UNAME_PASSWD };
      return new RequestHandshake(ver, nmethods, methods);
    }

    public byte GetMethod()
    {
      return BaseConstants.Methods.UNAME_PASSWD;
    }
  }
}
