using System.IO;
using System.Text.Json;

namespace SOCKS_5_Proxy_Chain
{
  public class UnamePasswd
  {
    public string Uname { get; set; }
    public string Passwd { get; set; }
  }

  public class Method
  {
    public UnamePasswd UnamePasswd { get; set; }
  }

  public class Server
  {
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public Method Method { get; set; }
  }

  public class Application
  {
    public string IpAddress { get; set; }
    public int Port { get; set; }
  }
  
  public class Settings
  {
    public Server Server { get; set; }
    public Application Application { get; set; }
    
    private static Settings instance;
    
    public static Settings Instance(string filepath = "")
    {
      if (string.IsNullOrEmpty(filepath))
      {
        if (instance != null)
        {
          return instance;
        }
        else
        {
          instance = new Settings();
          return instance;
        }
      }
      else
      {
        string jsonString = File.ReadAllText(filepath);
        instance = JsonSerializer.Deserialize<Settings>(jsonString);
        return instance;
      }
    }
  }
}
