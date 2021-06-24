using Microsoft.AspNetCore.Mvc;
using Midos.Services.Http;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private readonly Config _config;

    public AppController(Config config)
    {
      _config = config;
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
