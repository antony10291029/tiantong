using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

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
      return Result.FromObject(new {
        message = _config.AppName,
        version = _config.AppVersion,
        env = _config.Env
      });
    }

    [HttpPost("/doot/migrate")]
    public object Test()
    {
      var count = 0;
      Log[] logs;

      do {
        logs = _domain.Logs
          .Where(log => log.Key.StartsWith("door"))
          .Take(200)
          .ToArray();

        foreach (var log in logs) {
          var strs = log.Key.Split(".");

          if (strs.Length < 4) {
            continue;
          }

          log.UseOperator(
            klass: "wcs.door",
            index: strs[2],
            operation: string.Join(".", strs.Skip(3))
          );
          log.UseKey("old");
          count += _domain.SaveChanges();
        }

      } while (logs.Length > 0);

      return count;
    }

    [HttpPost("/lifter/migrate")]
    public object Lifter()
    {
      var count = 0;
      Log[] logs;

      do {
        logs = _domain.Logs
          .Where(log => log.Key.StartsWith("lifter"))
          .Take(200)
          .ToArray();

        foreach (var log in logs) {
          var strs = log.Key.Split(".");

          if (strs.Length < 3) {
            continue;
          }

          log.UseOperator(
            klass: "wcs.lifter",
            index: strs[0].Last().ToString(),
            operation: string.Join(".", strs.Skip(2))
          );
          log.UseKey("old");
          count += _domain.SaveChanges();
        }

      } while (logs.Length > 0);

      return count;
    }
  }
}
