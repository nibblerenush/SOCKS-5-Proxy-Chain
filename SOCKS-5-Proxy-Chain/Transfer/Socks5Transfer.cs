using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public class Socks5Transfer: Transfer
  {
    public Socks5Transfer(string remoteIpAddress, int remotePort): base(remoteIpAddress, remotePort)
    {}
    
    // Socks5 method
    protected override async Task HandshakeAsync(TcpClient browser, TcpClient server)
    {
      await Task.CompletedTask;
    }
  }
}
