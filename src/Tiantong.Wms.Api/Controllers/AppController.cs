using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Tiantong.Wms.Api
{
  public class AppController : BaseController
  {
    private Config _config;

    public AppController(Config config)
    {
      _config = config;
    }

    [HttpGet]
    [HttpPost]
    [Route("/")]
    public object Home()
    {
      return JsonMessage(_config.APP_NAME);
    }

    [HttpPost]
    [Route("/version")]
    public object Version()
    {
      return new {
        version = _config.APP_VERSION
      };
    }

  }
}
