using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Transfer
{
  public class DefaultTransfer : ITransfer
  {
    public DefaultTransfer(string remoteIpAddress, int remotePort)
    {
      _remoteIpAddress = remoteIpAddress;
      _remotePort = remotePort;
    }
    
    virtual public async void ProcessAsync(TcpClient browser)
    {
      try
      {
        using (browser)
        {
          using (TcpClient server = new TcpClient())
          {
            await server.ConnectAsync(_remoteIpAddress, _remotePort);
            Console.WriteLine($"Remote connected: {server.Client.RemoteEndPoint}");

            await Transfer(browser.GetStream(), server.GetStream());
          }
        }
      }
      catch (Exception ex)
      {
        StackFrame sf = new StackFrame(true);
        BaseFunctions.HandleException(ex, sf.GetFileName(), sf.GetFileLineNumber());
      }
    }

    protected async Task Transfer(NetworkStream browser, NetworkStream server)
    {
      Task fromBrowserToServer = Task.Run
      (
        async () => await FromBrowserToServer(browser, server)
      );

      Task fromServerToBrowser = Task.Run
      (
        async () => await FromServerToBrowser(server, browser)
      );

      await fromBrowserToServer;
      await fromServerToBrowser;
    }
    
    private async Task FromBrowserToServer(NetworkStream browser, NetworkStream server)
    {
      byte[] buffer = new byte[BUFFER_SIZE];
      int readedSize = 0;
      while ((readedSize = await browser.ReadAsync(buffer, 0, buffer.Length)) > 0)
      {
        await server.WriteAsync(buffer, 0, readedSize);
      }
    }
    
    private async Task FromServerToBrowser(NetworkStream server, NetworkStream browser)
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
