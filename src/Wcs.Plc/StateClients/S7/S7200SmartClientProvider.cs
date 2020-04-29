namespace Wcs.Plc
{
  public class S7200SmartClientProvider : S7ClientProvider
  {
    public S7200SmartClientProvider(string host, int port): base(host, port)
    {
      _client.Use200Smart().Reconnect();
    }

  }
}
