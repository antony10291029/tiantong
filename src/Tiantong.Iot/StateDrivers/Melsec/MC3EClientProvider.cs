using Renet.Tcp;
using Tiantong.Iot.Protocol;

namespace Tiantong.Iot
{
  public class MC3EBinaryDriverProvider : IStateDriverProvider
  {
    protected RenetTcpClient _client;

    public MC3EBinaryDriverProvider(string host, int port)
    {
      _client = new RenetTcpClient(host, port);
    }

    public IStateDriver Resolve()
    {
      return new StateTcpDriver(
        _client,
        new MC3EBinaryReadRequest(),
        new MC3EBinaryReadResponse(),
        new MC3EBinaryWriteRequest(),
        new MC3EBinaryWriteResponse()
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
