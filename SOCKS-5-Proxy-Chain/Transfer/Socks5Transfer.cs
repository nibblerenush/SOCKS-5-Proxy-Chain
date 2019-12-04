using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using SOCKS_5_Proxy_Chain.Socks5;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public class Socks5Transfer: DefaultTransfer
  {
    public Socks5Transfer(string remoteIpAddress, int remotePort, byte method) : base(remoteIpAddress, remotePort)
    {
      switch (method)
      {
        case BaseConstants.Methods.UNAME_PASSWD:
          _handshakeCreator = new UnamePasswdHandshakeCreator();
          break;
      }
    }
    
    // Socks5 method
    protected override async Task<bool> HandshakeAsync(NetworkStream browser, NetworkStream server)
    {
      byte[] buffer = new byte[BUFFER_SIZE];
      
      // From browser
      int readedSize = await browser.ReadAsync(buffer, 0, buffer.Length);
      RequestHandshake reqHand = new RequestHandshake(buffer, readedSize, BaseConstants.Methods.NO_AUTHENTICATION_REQUIRED);
      Console.WriteLine($"Read request handshake from client: {reqHand.ToString()}");

      // Socks5 Handshake with server
      await this.Socks5HandshakeAsync(server);

      // Socks5 Authentication with server
      if (await this.Socks5AuthenticationAsync(server))
      {
        // To browser
        ReplyHandshake repHand = new ReplyHandshake(BaseConstants.Versions.SOCKS5,
                                                    BaseConstants.Methods.NO_AUTHENTICATION_REQUIRED);
        buffer = repHand.GenBuff();
        await browser.WriteAsync(buffer, 0, buffer.Length);
        Console.WriteLine($"Write reply to client: {repHand}");
        return true;
      }
      return false;
    }

    private async Task Socks5HandshakeAsync(NetworkStream server)
    {
      RequestHandshake reqHand = _handshakeCreator.Create();
      byte[] buffer = reqHand.GenBuff();
      await server.WriteAsync(buffer, 0, buffer.Length);
      Console.WriteLine($"Write new request handshake to server: {reqHand}");
      
      int readedLength = await server.ReadAsync(buffer, 0, buffer.Length);
      ReplyHandshake repHand = new ReplyHandshake(buffer, readedLength, _handshakeCreator.GetMethod());
      Console.WriteLine($"Read reply handshake from server: {repHand}");
    }

    private async Task<bool> Socks5AuthenticationAsync(NetworkStream server)
    {
      return await Task.FromResult<bool>(false);
    }

    private IHandshakeCreator _handshakeCreator;
    private const int BUFFER_SIZE = 1024;
  }
}
