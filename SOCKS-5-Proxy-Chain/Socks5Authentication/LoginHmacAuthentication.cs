using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SOCKS_5_Proxy_Chain.Socks5Authentication
{
  class LoginHmacAuthentication : IAuthentication
  {
    public async Task<bool> RunAsync(NetworkStream server)
    {
      var login = new List<byte>(Encoding.UTF8.GetBytes(Settings.Instance().Server.Method.LoginHmac.Login));

      // Login request/reply
      var reqLogin = new RequestLogin(BaseConstants.Versions.LOGIN_HMAC,
                                      (byte)login.Count,
                                      login);

      byte[] buffer = reqLogin.GenBuff();
      await server.WriteAsync(buffer, 0, buffer.Length);

      buffer = new byte [BUFFER_SIZE];
      int readedLength = await server.ReadAsync(buffer, 0, buffer.Length);
      var repLogin = new ReplyLogin(buffer, readedLength);

      // Login check
      if (repLogin.Status != AUTH_SUCCESS)
      {
        return false;
      }

      // Hmac request/reply
      byte[] hmac = GetHmac(repLogin.RandomString);
      var reqHmac = new RequestHmac(BaseConstants.Versions.LOGIN_HMAC,
                                    (byte)hmac.Length,
                                    new List<byte>(hmac));

      buffer = reqHmac.GenBuff();
      await server.WriteAsync(buffer, 0, buffer.Length);

      readedLength = await server.ReadAsync(buffer, 0, buffer.Length);
      var repHmac = new ReplyHmac(buffer, readedLength);

      // Hmac check
      return repHmac.Status == AUTH_SUCCESS ? true : false;
    }

    byte[] GetHmac(byte[] message)
    {
      byte[] hmacKey = Encoding.UTF8.GetBytes(Settings.Instance().Server.Method.LoginHmac.HmacKey);
      HMACSHA256 hmac = new HMACSHA256(hmacKey);
      return hmac.ComputeHash(message);
    }

    private const int BUFFER_SIZE = 1024;
    private const byte AUTH_SUCCESS = 0;
  }
}
