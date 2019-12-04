namespace SOCKS_5_Proxy_Chain.Socks5
{
  public interface IHandshakeCreator
  {
    RequestHandshake Create();
    byte GetMethod();
  }
}
