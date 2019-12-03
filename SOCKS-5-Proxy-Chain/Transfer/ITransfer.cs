using System.Net.Sockets;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public interface ITransfer
  {
    void ProcessAsync(TcpClient browser);
  }
}
