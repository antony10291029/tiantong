using Renet.Web;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    public AppController(Config config)
    {
      _config = config;
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
  }
}
