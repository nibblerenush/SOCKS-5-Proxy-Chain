namespace SOCKS_5_Proxy_Chain
{
  public static class BaseConstants
  {
    public static class Versions
    {
      public const byte SOCKS5 = 0x05;
      public const byte UNAME_PASSWD = 0x01;
      public const byte LOGIN_HMAC = 0x01;
    }
    
    public static class Methods
    {
      public const byte NO_AUTHENTICATION_REQUIRED = 0x00;
      public const byte UNAME_PASSWD = 0x02;
      public const byte LOGIN_HMAC = 0x03;
    }
  }
}
