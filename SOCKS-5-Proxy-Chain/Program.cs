using System;
using System.Threading;
using System.Threading.Tasks;
using SOCKS_5_Proxy_Chain.Transfer;

namespace SOCKS_5_Proxy_Chain
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.WriteLine("Usage: SOCKS-5-Proxy-Chain <configuration file>");
        return;
      }
      
      CancellationTokenSource tokenSource = new CancellationTokenSource();
      TcpServer server = new TcpServer(Settings.Instance(args[0]).Application.Port, TransferType.SOCKS5);
      await server.RunAsync(tokenSource.Token);
    }
  }
}
