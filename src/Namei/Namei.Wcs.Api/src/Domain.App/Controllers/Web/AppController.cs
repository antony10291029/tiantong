using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Midos.Services.Http;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private readonly Config _config;

    public AppController(Config config, ICapPublisher cap)
    {
      _config = config;
      cap.Publish(HttpPost.Event, HttpPost.From(
        "http://172.16.2.62/wcs/home",
        new { Home = "home", Id = 1234 }
      ));
    }

    [HttpGet]
    [HttpPost]
    [Route("/")]
    public IResult<object> Home()
    {
      return Result.FromObject(new {
        message = _config.AppName,
        version = _config.AppVersion,
        env = _config.Env
      });
    }
  }
}
