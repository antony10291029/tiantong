using System;
using Wcs.Plc.Snap7;

namespace Wcs.Plc
{
  public class S7ClientProvider : IStateClientProvider
  {
    private S7TcpClient _client;

    public S7ClientProvider()
    {
      _client = new S7TcpClient("192.168.20.3", 102);
      _client.Use200Smart();
      _client.Connect();
    }

    public IStateClient Resolve()
    {
      Console.WriteLine("resolved");
      return new S7Client(_client);
    }
  }
}
