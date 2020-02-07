namespace Wcs.Plc
{
  public class StateTestClientProvider : IStateClientProvider
  {
    public IStateClient Resolve()
    {
      var client = new StateTestClient();
      client.Store = new StateTestClientStore();

      return client;
    }
  }
}
