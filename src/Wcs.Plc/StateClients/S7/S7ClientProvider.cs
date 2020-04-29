using Wcs.Plc.Protocol;

namespace Wcs.Plc
{
  public abstract class S7ClientProvider : IStateClientProvider
  {
    protected S7TcpClient _client;

    public S7ClientProvider(string host, int port)
    {
      _client = new S7TcpClient(host, port);
    }

    public IStateClient Resolve()
    {
      return new StateRenetTcpClient(
        _client,
        new S7ReadRequest(),
        new S7ReadResponse(),
        new S7WriteRequest(),
        new S7WriteResponse()
      );
    }
  }
}
