using Renet.Tcp;
using Wcs.Plc.Protocol;

namespace Wcs.Plc
{
  public class MC3EClientProvider : IStateClientProvider
  {
    protected RenetTcpClient _client;

    public MC3EClientProvider(string host, int port)
    {
      _client = new RenetTcpClient(host, port);
      _client.Connect();
    }

    public IStateClient Resolve()
    {
      return new StateRenetTcpClient(
        _client,
        new MC3EReadRequest(),
        new MC3EReadResponse(),
        new MC3EWriteRequest(),
        new MC3EWriteResponse()
      );
    }
  }
}
