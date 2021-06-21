using Midos.Domain;
using System.Net.Http;

namespace Namei.Wcs.Aggregates
{
  public class HttpServiceController: Midos.Services.Http.HttpServiceController
  {
    public HttpServiceController(IEventPublisher publisher, IHttpClientFactory factory)
      : base(publisher, factory) {}
  }
}
