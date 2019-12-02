using System;
using System.Net;

namespace SOCKS_5_Proxy_Chain
{
  public static class BaseFunctions
  {
    public static void HandleException(Exception ex, string FileName, int FileLineNumber)
    {
      Console.WriteLine($"File: {FileName}, Line: {FileLineNumber}, Message: {ex.Message}");
      if (ex.InnerException != null)
      {
        Console.WriteLine($"InnerException Message: {ex.InnerException.Message}");
      }
    }
    
    public static string ConvertFromIntToIpAddress(int number)
    {
      uint invertNumber = (uint)IPAddress.NetworkToHostOrder(number);
      return IPAddress.Parse(invertNumber.ToString()).ToString();
    }
  }
}
