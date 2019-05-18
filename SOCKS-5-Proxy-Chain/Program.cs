using System;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Diagnostics;

using System.Threading;
using System.Threading.Tasks;

using System.Text;
using System.Reflection;

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
      
        byte[] buffer = new byte[_bufferSize];
        int readedLength = await netStream.ReadAsync(buffer, 0, buffer.Length);

        //
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
        byte[] remoteBuffer = reqHandToRemote.GenerateBuffer();
        await remoteNetStream.WriteAsync(remoteBuffer, 0, remoteBuffer.Length);

        //
        readedLength = await remoteNetStream.ReadAsync(remoteBuffer, 0, remoteBuffer.Length);
        Socks5ReplyHandshake repHandFromRemote = new Socks5ReplyHandshake(remoteBuffer);
        Debug.WriteLine("Read reply handshake from SOCKS 5 server: {0}", repHandFromRemote);

        // Authenticate to SOCKS 5 server
        remoteBuffer = _reqAuthCreator.GenReqAuth();
        await remoteNetStream.WriteAsync(remoteBuffer, 0, remoteBuffer.Length);

        // Read authenticate reply from SOCKS 5 server
        readedLength = await remoteNetStream.ReadAsync(remoteBuffer, 0, remoteBuffer.Length);
        _reqAuthCreator.CheckRepAuth(remoteBuffer);

        //
        Socks5ReplyHandshake repHand = new Socks5ReplyHandshake(
          BaseConstants.Versions.SOCKS, BaseConstants.Methods.NO_AUTHENTICATION_REQUIRED);
        byte[] repHandBuffer = repHand.GenerateBuffer();
        await netStream.WriteAsync(repHandBuffer, 0, repHandBuffer.Length);
        Debug.WriteLine("Write reply to client: {0}", repHand);

        byte[] buffer2 = new byte[_bufferSize];


        Task t1 = Task.Run
          (
           async () =>
           {
             while (true)
             {
               int readedLength1 = await netStream.ReadAsync(buffer, 0, buffer.Length);
               if (readedLength1 == 0)
               {
                 netStream.Close();
                 break;
               }
               await remoteNetStream.WriteAsync(buffer, 0, readedLength1);
             }
           }
          );

        Task t2 = Task.Run
          (
            async () =>
            {
              while (true)
              {
                int readedLength2 = await remoteNetStream.ReadAsync(buffer2, 0, buffer2.Length);
                if (readedLength2 == 0)
                {
                  remoteNetStream.Close();
                  break;
                }
                await netStream.WriteAsync(buffer2, 0, readedLength2);
              }
            }
          );

        //await t1;
        //await t2;
      
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