using System.Collections.Generic;

namespace SOCKS_5_Proxy_Chain
{
  interface IReqHandCreator
  {
    Socks5RequestHandshake Create();
  }

  class UnamePasswdReqHand : IReqHandCreator
  {
    public Socks5RequestHandshake Create()
    {
      byte ver = BaseConstants.Versions.SOCKS;
      byte nmethods = 1;
      List<byte> methods = new List<byte>() { BaseConstants.Methods.USERNAME_PASSWORD };
      return new Socks5RequestHandshake(ver, nmethods, methods);
    }
  }
}
