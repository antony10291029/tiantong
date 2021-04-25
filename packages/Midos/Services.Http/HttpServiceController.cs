using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

    public new struct Response
    {

    }

    [CapSubscribe(HttpPost.Event, Group = Group)]
    public void Post(HttpPost param)
    {
      _http.Post<object>(param.Url, param.Data);
    }
  }
}
