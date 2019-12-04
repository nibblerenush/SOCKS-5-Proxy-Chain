using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SOCKS_5_Proxy_Chain.Socks5;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public class Socks5Transfer: DefaultTransfer
  {
    public Socks5Transfer(string remoteIpAddress, int remotePort): base(remoteIpAddress, remotePort)
    {}
    
    // Socks5 method
    protected override async Task HandshakeAsync(NetworkStream browser, NetworkStream server)
    {
      byte[] buffer = new byte[BUFFER_SIZE];
      
      // From browser
      int readedSize = await browser.ReadAsync(buffer, 0, buffer.Length);
      RequestHandshake reqHand = new RequestHandshake(buffer, readedSize, BaseConstants.Methods.NO_AUTHENTICATION_REQUIRED);
      Console.WriteLine($"Read request handshake from client: {reqHand.ToString()}");

      // Socks5 Handshake with server
      await this.Socks5HandshakeAsync(browser, server);

      // Socks5 Authentication with server
      if (await this.Socks5AuthenticationAsync(browser, server))
      {
        // To browser
        ReplyHandshake repHand = new ReplyHandshake(BaseConstants.Versions.SOCKS5,
                                                    BaseConstants.Methods.NO_AUTHENTICATION_REQUIRED);
        buffer = repHand.GenBuff();
        await browser.WriteAsync(buffer, 0, buffer.Length);
        Console.WriteLine($"Write reply to client: {repHand}");
      }
    }

    private async Task Socks5HandshakeAsync(NetworkStream browser, NetworkStream server)
    {

    }

    private async Task<bool> Socks5AuthenticationAsync(NetworkStream browser, NetworkStream server)
    {
      
    }

    private const int BUFFER_SIZE = 1024;
  }
}
