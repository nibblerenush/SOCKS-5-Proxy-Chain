using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Connection
{
  public enum ConnectionType
  {
    TEST
  }
  public interface IConnection
  {
    void ProcessAsync(TcpClient browser);
  }
}
