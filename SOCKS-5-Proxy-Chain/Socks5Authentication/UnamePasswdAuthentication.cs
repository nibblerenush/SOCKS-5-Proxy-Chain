using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  class UnamePasswdAuthentication : IAuthentication
  {
    public async Task<bool> RunAsync(NetworkStream server)
    {
      return await Task.FromResult<bool>(false);
    }
    /*public byte[] GenReqAuth()
    {
      string username = Config.GetInst().Username;
      List<byte> uname = new List<byte>(Encoding.UTF8.GetBytes(username));
      string password = Config.GetInst().Password;
      List<byte> passwd = new List<byte>(Encoding.UTF8.GetBytes(password));
      //
      Socks5RequestUnamePasswd reqUnamePasswd = new Socks5RequestUnamePasswd(
        BaseConstants.Versions.UNAME_PASSWD, (byte)uname.Count, uname, (byte)passwd.Count, passwd);
      Debug.WriteLine(reqUnamePasswd);
      //
      return reqUnamePasswd.GenerateBuffer();
    }

    public void CheckRepAuth(byte[] reply)
    {
      Socks5ReplyUnamePasswd repUnamePasswd = new Socks5ReplyUnamePasswd(reply);
      Debug.WriteLine(repUnamePasswd);
    }*/
  }
}
