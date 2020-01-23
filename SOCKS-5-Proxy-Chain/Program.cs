using System;
using System.Threading;
using System.Threading.Tasks;
using SOCKS_5_Proxy_Chain.Transfer;

using System.Security.Cryptography;
using System.Text;

namespace SOCKS_5_Proxy_Chain
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      /*string key_string = "1234";
      byte [] key = Encoding.ASCII.GetBytes(key_string);
      HMACSHA1 hmac = new HMACSHA1(key);

      string msg_string = "Hello World!";
      byte [] msg = Encoding.ASCII.GetBytes(msg_string);
      byte [] hash = hmac.ComputeHash(msg);
      
      StringBuilder sb = new StringBuilder(hash.Length * 2);
      foreach (byte b in hash)
      {
        sb.AppendFormat("{0:x2}", b);
      }
      Console.WriteLine(sb.ToString());*/



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
