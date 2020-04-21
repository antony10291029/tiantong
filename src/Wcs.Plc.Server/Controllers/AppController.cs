using Microsoft.AspNetCore.Mvc;

namespace Wcs.Plc.Server
{
  [Route("/")]
  [ApiController]
  public class AppController: ControllerBase
  {
    [HttpGet]
    [HttpPost]
    [Route("")]
    public object Home()
    {
      return new {
        message = "天瞳物联网通信平台"
      };
    }
  }
}
