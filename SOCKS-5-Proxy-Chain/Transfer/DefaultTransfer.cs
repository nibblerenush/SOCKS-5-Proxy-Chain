using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public class DefaultTransfer: Transfer
  {
    public DefaultTransfer(string remoteIpAddress, int remotePort): base(remoteIpAddress, remotePort)
    {}
    
    // Empty method
    protected override async Task HandshakeAsync(TcpClient browser, TcpClient server)
    {
      await Task.CompletedTask;
    }
  }
}
