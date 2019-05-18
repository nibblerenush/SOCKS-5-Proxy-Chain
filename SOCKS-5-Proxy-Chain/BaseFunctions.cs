using System.Net;

namespace SOCKS_5_Proxy_Chain
{
  static class BaseFunctions
  {
    public static string ConvertFromIntToIpAddress(int number)
    {
      uint invertNumber = (uint)IPAddress.NetworkToHostOrder(number);
      return IPAddress.Parse(invertNumber.ToString()).ToString();
    }
  }
}
