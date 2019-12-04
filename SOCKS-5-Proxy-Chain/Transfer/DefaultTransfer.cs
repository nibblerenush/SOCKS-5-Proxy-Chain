using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public class DefaultTransfer
  {
    public DefaultTransfer(string remoteIpAddress, int remotePort)
    {
      _remoteIpAddress = remoteIpAddress;
      _remotePort = remotePort;
    }

    // Main method in class Transfer
    public async void ProcessAsync(TcpClient browser)
    {
      try
      {
        using (browser)
        {
          using (TcpClient server = new TcpClient())
          {
            await server.ConnectAsync(_remoteIpAddress, _remotePort);
            Console.WriteLine($"Remote connected: {server.Client.RemoteEndPoint}");
            
            if (await this.HandshakeAsync(browser.GetStream(), server.GetStream()))
            {
              await this.RunTransferringAsync(browser.GetStream(), server.GetStream());
            }
          }
        }
      }
      catch (Exception ex)
      {
        StackFrame sf = new StackFrame(true);
        BaseFunctions.HandleException(ex, sf.GetFileName(), sf.GetFileLineNumber());
      }
    }
    
    // Empty method
    // Need custom implementation for derived classes
    protected virtual async Task<bool> HandshakeAsync(NetworkStream browser, NetworkStream server)
    {
      return await Task.FromResult<bool>(true);
    }

    private async Task RunTransferringAsync(NetworkStream browser, NetworkStream server)
    {
      Task fromBrowserToServer = Task.Run
      (
        async () => await FromBrowserToServerAsync(browser, server)
      );

      Task fromServerToBrowser = Task.Run
      (
        async () => await FromServerToBrowserAsync(server, browser)
      );

      await fromBrowserToServer;
      await fromServerToBrowser;
    }
    
    private async Task FromBrowserToServerAsync(NetworkStream browser, NetworkStream server)
    {
      byte[] buffer = new byte[BUFFER_SIZE];
      int readedSize = 0;
      while ((readedSize = await browser.ReadAsync(buffer, 0, buffer.Length)) > 0)
      {
        await server.WriteAsync(buffer, 0, readedSize);
      }
    }
    
    private async Task FromServerToBrowserAsync(NetworkStream server, NetworkStream browser)
    {
      byte[] buffer = new byte[BUFFER_SIZE];
      int readedSize = 0;
      while ((readedSize = await server.ReadAsync(buffer, 0, buffer.Length)) > 0)
      {
        await browser.WriteAsync(buffer, 0, readedSize);
      }
    }

    private readonly string _remoteIpAddress;
    private readonly int _remotePort;
    private const int BUFFER_SIZE = 2048;
  }
}
