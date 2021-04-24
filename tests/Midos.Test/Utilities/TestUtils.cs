using System.Net;
using System.Net.Sockets;

namespace Midos.Test
{
  public class TestUtils
  {
    public static int GetFreePort()
    {
      var server = new TcpListener(IPAddress.Loopback, 0);

      server.Start();

      var port = ((IPEndPoint)server.LocalEndpoint).Port;

      server.Stop();

      return port;
    }
  }
}
