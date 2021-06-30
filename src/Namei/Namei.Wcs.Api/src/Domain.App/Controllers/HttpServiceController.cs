using System.Net.Http;
using Midos.Eventing;

namespace Namei.Wcs.Aggregates
{
  public class HttpServiceController: Midos.Services.Http.HttpServiceController
  {
    public HttpServiceController(IEventPublisher publisher, IHttpClientFactory factory)
      : base(publisher, factory) {}
  }
}
