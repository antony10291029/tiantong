using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Midos.Services.Http
{
  public class HttpServiceController: BaseController
  {
    private const string Group = "HttpServiceController";

    private IHttpService _http;

    public HttpServiceController(IHttpService http)
    {
      _http = http;
    }

    [CapSubscribe(HttpPost.Event, Group = Group)]
    public void Post(HttpPost param)
    {
      _http.Post<object, object>(param.Url, param.Data);
    }
  }
}
