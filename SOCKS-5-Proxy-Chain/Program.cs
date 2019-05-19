using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain
{
  internal class AuthServer
  {
    internal AuthServer(string ipAddr, ushort port)
    {
      _tcpListener = new TcpListener(IPAddress.Parse(ipAddr), port);
      _bufferSize = uint.Parse(Config.GetInst().BufferSize);

      switch (byte.Parse(Config.GetInst().MethodType))
      {
        case BaseConstants.Methods.USERNAME_PASSWORD:
          _reqHandCreator = new UnamePasswdReqHand();
          _reqAuthCreator = new UnamePasswdReqAuth();
          break;
      }
    }

    internal async Task Run()
    {
      _tcpListener.Start();
      while (true)
      {
        TcpClient tcpClient = await _tcpListener.AcceptTcpClientAsync();
        EndPoint endPoint = tcpClient.Client.RemoteEndPoint;
        Console.WriteLine("Client connected: {0}", endPoint);
        ProcessConnection(tcpClient, endPoint);
      }
    }

    private async void ProcessConnection(TcpClient mainTcpClient, EndPoint endPoint)
    {
      NetworkStream netStream = mainTcpClient.GetStream();
      //
      byte[] buffer = new byte[_bufferSize];
      int readedLength = await netStream.ReadAsync(buffer, 0, buffer.Length);
      Socks5RequestHandshake reqHand = new Socks5RequestHandshake(buffer);
      Debug.WriteLine("Read request handshake from client: {0}", reqHand);

      // Connect to SOCKS 5 server
      TcpClient tcpClient = new TcpClient();
      await tcpClient.ConnectAsync(Config.GetInst().RemoteIpAddress, int.Parse(Config.GetInst().RemotePort));
      Console.WriteLine("Remote connected: {0}", tcpClient.Client.RemoteEndPoint);

      //
      NetworkStream remoteNetStream = tcpClient.GetStream();
      Socks5RequestHandshake reqHandToRemote = _reqHandCreator.Create();
      Debug.WriteLine("Write new request handshake to SOCKS 5 server: {0}", reqHandToRemote);
      byte[] writeData = reqHandToRemote.GenerateBuffer();
      await remoteNetStream.WriteAsync(writeData, 0, writeData.Length);

      //
      readedLength = await remoteNetStream.ReadAsync(buffer, 0, buffer.Length);
      Socks5ReplyHandshake repHandFromRemote = new Socks5ReplyHandshake(buffer);
      Debug.WriteLine("Read reply handshake from SOCKS 5 server: {0}", repHandFromRemote);
      
      // Authenticate to SOCKS 5 server
      writeData = _reqAuthCreator.GenReqAuth();
      await remoteNetStream.WriteAsync(writeData, 0, writeData.Length);

      // Read authenticate reply from SOCKS 5 server
      readedLength = await remoteNetStream.ReadAsync(buffer, 0, buffer.Length);
      _reqAuthCreator.CheckRepAuth(buffer);

      //
      Socks5ReplyHandshake repHand = new Socks5ReplyHandshake(
        BaseConstants.Versions.SOCKS, BaseConstants.Methods.NO_AUTHENTICATION_REQUIRED);
      writeData = repHand.GenerateBuffer();
      await netStream.WriteAsync(writeData, 0, writeData.Length);
      Debug.WriteLine("Write reply to client: {0}", repHand);

      Task fromLocalToRemote = Task.Run(
        async () =>
        {
          byte[] dataBuffer = new byte[_bufferSize];
          while (true)
          {
            int readedSize = await netStream.ReadAsync(dataBuffer, 0, dataBuffer.Length);
            if (readedSize == 0)
            {
              netStream.Close();
              Console.WriteLine("Local closed: {0}", endPoint);
              break;
            }
            await remoteNetStream.WriteAsync(dataBuffer, 0, readedSize);
          }
        });

      Task fromRemoteToLocal = Task.Run(
        async () =>
        {
          byte[] dataBuffer = new byte[_bufferSize];
          while (true)
          {
            int readedSize = await remoteNetStream.ReadAsync(dataBuffer, 0, dataBuffer.Length);
            if (readedSize == 0)
            {
              remoteNetStream.Close();
              Console.WriteLine("Remote closed: {0}", tcpClient.Client.RemoteEndPoint);
              break;
            }
            await netStream.WriteAsync(dataBuffer, 0, readedSize);
          }
        });
    }

    private TcpListener _tcpListener;
    private readonly uint _bufferSize;
    private IReqHandCreator _reqHandCreator;
    private IReqAuthCreator _reqAuthCreator;
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      AuthServer authServer = new AuthServer("0.0.0.0", ushort.Parse(Config.GetInst(args[0]).Port));
      Task authServerTask = authServer.Run();
      authServerTask.Wait();
    }
  }
}
