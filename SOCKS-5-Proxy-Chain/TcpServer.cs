using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SOCKS_5_Proxy_Chain.Transfer;

namespace SOCKS_5_Proxy_Chain
{
  public class TcpServer
  {
    public TcpServer(int port, TransferType type)
    {
      _tcpListener = new TcpListener(IPAddress.Any, port);
      SelectConnection(type);
    }

    public TcpServer(string ipAddr, int port, TransferType type)
    {
      _tcpListener = new TcpListener(IPAddress.Parse(ipAddr), port);
      SelectConnection(type);
    }
    
    public async Task RunAsync(CancellationToken token)
    {
      try
      {
        _tcpListener.Start();
        while (true)
        {
          token.ThrowIfCancellationRequested();
          TcpClient browser = await _tcpListener.AcceptTcpClientAsync();
          Console.WriteLine($"Client connected: {browser.Client.RemoteEndPoint}");
          _transfer.ProcessAsync(browser);
        }
      }
      catch (SocketException ex)
      {
        StackFrame sf = new StackFrame(true);
        BaseFunctions.HandleException(ex, sf.GetFileName(), sf.GetFileLineNumber());
      }
    }
    
    private void SelectConnection(TransferType type)
    {
      switch (type)
      {
        case TransferType.DEFAULT:
          _transfer = new DefaultTransfer(Settings.Instance().Server.IpAddress,
                                          Settings.Instance().Server.Port);
          break;
        case TransferType.SOCKS5:
          if (Settings.Instance().Server.Method.Name == "UnamePasswd")
          {
            _transfer = new Socks5Transfer(Settings.Instance().Server.IpAddress,
                                           Settings.Instance().Server.Port,
                                           BaseConstants.Methods.UNAME_PASSWD);
          }
          else if (Settings.Instance().Server.Method.Name == "LoginHmac")
          {
            _transfer = new Socks5Transfer(Settings.Instance().Server.IpAddress,
                                           Settings.Instance().Server.Port,
                                           BaseConstants.Methods.LOGIN_HMAC);
          }
          break;
      }
    }
    
    private TcpListener _tcpListener;
    private DefaultTransfer _transfer;
  }
}
