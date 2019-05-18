namespace SOCKS_5_Proxy_Chain
{
  static class BaseConstants
  {
    public static class Versions
    {
      public const byte SOCKS = 0x05;
      public const byte UNAME_PASSWD = 0x01;
    }

    public static class Methods
    {
      public const byte NO_AUTHENTICATION_REQUIRED = 0x00;
      public const byte USERNAME_PASSWORD = 0x02;
    }
  }
}
