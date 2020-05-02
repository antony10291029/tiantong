using Renet.Tcp;
using Wcs.Plc.Protocol;

namespace Wcs.Plc
{
  public class MC3EDriverProvider : IStateDriverProvider
  {
    protected RenetTcpClient _client;

    public MC3EDriverProvider(string host, int port)
    {
      _client = new RenetTcpClient(host, port);
    }

    public IStateDriver Resolve()
    {
      return new StateTcpDriver(
        _client,
        new MC3EReadRequest(),
        new MC3EReadResponse(),
        new MC3EWriteRequest(),
        new MC3EWriteResponse()
      );
    }

    public void Boot()
    {
      _client.Connect();
    }
  }
}
