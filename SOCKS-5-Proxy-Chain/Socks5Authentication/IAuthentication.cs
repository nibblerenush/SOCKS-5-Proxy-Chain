using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  public interface IAuthentication
  {
    Task<bool> RunAsync(NetworkStream server);
  }
}
