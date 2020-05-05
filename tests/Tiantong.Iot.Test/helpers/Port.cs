using System.Net;
using System.Net.Sockets;

namespace Tiantong.Iot.Test
{
  public class Port
  {
    public static int Free()
    {
      var server = new TcpListener(IPAddress.Loopback, 0);
      server.Start();
      var port = ((IPEndPoint)server.LocalEndpoint).Port;
      server.Stop();

      return port;
    }
  }
}
