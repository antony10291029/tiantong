using Renet.Net;
using Tiantong.Iot.Protocol;

namespace Tiantong.Iot
{
  public class MC1EBinaryDriverProvider : IStateDriverProvider
  {
    protected RenetTcpClient _client;

    public MC1EBinaryDriverProvider(string host, int port)
    {
      _client = new RenetTcpClient(host, port);
    }

    public IStateDriver Resolve()
    {
      return new StateTcpDriver(
        _client,
        new MC1EBinaryReadRequest(),
        new MC1EBinaryReadResponse(),
        new MC1EBinaryWriteRequest(),
        new MC1EBinaryWriteResponse()
      );
    }

    public void Connect()
    {
      _client.Connect();
    }

    public void Close()
    {
      _client.Close();
    }
  }
}
