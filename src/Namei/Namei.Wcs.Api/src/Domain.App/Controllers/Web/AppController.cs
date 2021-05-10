using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private DomainContext _domain;

    private ICapPublisher _cap;

    public AppController(Config config, DomainContext domain, ICapPublisher cap)
    {
      _cap = cap;
      _config = config;
      _domain = domain;
    }

    [HttpGet]
    [HttpPost]
    [Route("/")]
    public IResult<object> Home()
    {
      throw new Exception("test exception", new Exception("Inner exception"));

      return Result.FromObject(new {
        message = _config.AppName,
        version = _config.AppVersion,
        env = _config.Env
      });
    }
  }
}
