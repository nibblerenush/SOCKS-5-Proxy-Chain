using System.Xml;

namespace SOCKS_5_Proxy_Chain
{
  class Config
  {
    private static Config instance;

    private Config(string filepath)
    {
      xmlDoc = new XmlDocument();
      xmlDoc.Load(filepath);
    }

    private readonly XmlDocument xmlDoc;

    public string RemoteIpAddress
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/common/SOCKS-5-Proxy/ip_address");
        return node.InnerText;
      }
    }
    public string RemotePort
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/common/SOCKS-5-Proxy/port");
        return node.InnerText;
      }
    }
    public string Port
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/common/application/port");
        return node.InnerText;
      }
    }
    public string MethodType
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/common/application/method_type");
        return node.InnerText;
      }
    }
    public string BufferSize
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/common/application/buffer_size");
        return node.InnerText;
      }
    }
    public string Username
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/method/username_password/username");
        return node.InnerText;
      }
    }
    public string Password
    {
      get
      {
        XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/settings/method/username_password/password");
        return node.InnerText;
      }
    }

    public static Config GetInst(string filepath = "")
    {
      if (instance == null)
      {
        instance = new Config(filepath);
      }
      return instance;
    }
  }
}
