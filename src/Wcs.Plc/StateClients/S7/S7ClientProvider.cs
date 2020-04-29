using System;
using Wcs.Plc.Snap7;

namespace Wcs.Plc
{
  public class S7ClientProvider : IStateClientProvider
  {
    private S7TcpClient _client;

    public S7ClientProvider(string host, int  port)
    {
      _client = new S7TcpClient(host, port);
      _client.Use200Smart();
      _client.Reconnect();
    }

    public IStateClient Resolve()
    {
      return new S7Client(_client);
    }
  }
}
