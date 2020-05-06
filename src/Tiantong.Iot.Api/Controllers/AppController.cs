using Microsoft.AspNetCore.Mvc;
using Tiantong.Iot;
using Tiantong.Iot.Entities;
using Tiantong.Iot.DB.Sqlite;
using DBCore;
using Renet.Web;

namespace Tiantong.Iot.Api
{
  [Route("/")]
  public class AppController: BaseController
  {
    private Config _config;

    public AppController(Config config)
    {
      _config = config;
    }

    [HttpGet]
    [HttpPost]
    public object Home()
    {
      return new {
        message = _config.AppName,
        version = _config.AppVersion,
      };
    }

  }
}
