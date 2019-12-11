using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  class UnamePasswdAuthentication : IAuthentication
  {
    public async Task<bool> RunAsync(NetworkStream server)
    {
      var uname = new List<byte>(Encoding.UTF8.GetBytes(Settings.Instance().Server.Method.UnamePasswd.Uname));
      var passwd = new List<byte>(Encoding.UTF8.GetBytes(Settings.Instance().Server.Method.UnamePasswd.Passwd));
      
      var reqUnamePasswd = new RequestUnamePasswd(BaseConstants.Versions.UNAME_PASSWD,
                                                  (byte)uname.Count,
                                                  uname,
                                                  (byte)passwd.Count,
                                                  passwd);
      byte[] buffer = reqUnamePasswd.GenBuff();
      await server.WriteAsync(buffer, 0, buffer.Length);

      int readedLength = await server.ReadAsync(buffer, 0, buffer.Length);
      var repUnamePasswd = new ReplyUnamePasswd(buffer, readedLength);

      return repUnamePasswd.Status == UNAME_PASSWD_SUCCESS ? true : false;
    }

    private const byte UNAME_PASSWD_SUCCESS = 0;
    private const byte UNAME_PASSWD_FAILURE = 1;
  }
}
