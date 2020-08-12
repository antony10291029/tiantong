using System.Net.Http;

namespace Tiantong.Iot.Utils
{
  public class PlcStateServiceProvider
  {
    private IHttpClientFactory _factory;
    
    public PlcStateServiceProvider(IHttpClientFactory factory)
    {
      _factory = factory;
    }

    public PlcStateService Resolve()
      => new PlcStateService(_factory);
  }
}
