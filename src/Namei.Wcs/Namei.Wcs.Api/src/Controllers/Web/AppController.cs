using System;
using Renet.Web;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Z.EntityFramework.Plus;

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
    public ActionResult<object> Home()
    {
      return new {
        message = $"{_config.AppName} v{_config.AppVersion}"
      };
    }

    public class ClearLogsParams
    {
      public int days { get; set; }
    }

    [HttpPost("/logs/clear")]
    public object ClearLogs([FromBody] ClearLogsParams param)
    {
      System.Console.WriteLine(param.days);
      var date = DateTime.Now.AddDays(-param.days);

      _domain.Logs.Where(log => log.created_at < date).Delete();

      return new { message = "日志已清理" };
    }
  }
}
