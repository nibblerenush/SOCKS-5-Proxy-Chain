using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Connection
{
  public class TestConnection : IConnection
  {
    public TestConnection(string remoteIpAddr, int remotePort)
    {
      _remoteIpAddr = remoteIpAddr;
      _remotePort = remotePort;
    }

    public async void ProcessAsync(TcpClient browser)
    {
      try
      {
        using (browser)
        {
          using (TcpClient server = new TcpClient())
          {
            await server.ConnectAsync(_remoteIpAddr, _remotePort);
            Console.WriteLine($"Remote connected: {server.Client.RemoteEndPoint}");
            
            Task fromBrowser = Task.Run(async () => await FromBrowserToServer(browser.GetStream(), server.GetStream()));
            Task fromServer = Task.Run(async () => await FromServerToBrowser(browser.GetStream(), server.GetStream()));
            
            await fromBrowser;
            await fromServer;
          }
        }
      }
      catch (Exception ex)
      {
        StackFrame sf = new StackFrame(true);
        BaseFunctions.HandleException(ex, sf.GetFileName(), sf.GetFileLineNumber());
      }
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

    private async Task FromServerToBrowser(NetworkStream browser, NetworkStream server)
    {
      byte[] buffer = new byte[BUFFER_SIZE];
      int readedSize = 0;
      while ((readedSize = await server.ReadAsync(buffer, 0, buffer.Length)) > 0)
      {
        await browser.WriteAsync(buffer, 0, readedSize);
      }
    }

    private string _remoteIpAddr;
    private int _remotePort;
    private const int BUFFER_SIZE = 1024;
  }
}
