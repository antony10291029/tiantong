using System;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private DomainContext _domain;

    public AppController(Config config, DomainContext domain)
    {
      _config = config;
      _domain = domain;
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
