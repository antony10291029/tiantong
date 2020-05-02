namespace Wcs.Plc
{
  public class S7200SmartDriverProvider : S7DriverProvider
  {
    public S7200SmartDriverProvider(string host, int port): base(host, port)
    {
    }

    public override void Boot()
    {
      _client.Use200Smart().Connect();
    }
  }
}
