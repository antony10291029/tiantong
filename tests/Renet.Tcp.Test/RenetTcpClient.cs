using NUnit.Framework;

namespace Renet.Tcp.Test
{
  [TestFixture]
  public class RenetTcpClientTest
  {
    [TestCase]
    public void Test()
    {
      var client = new RenetTcpClient("localhost", 2000);
    }
  }
}
