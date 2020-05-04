using Microsoft.AspNetCore.Mvc;

namespace Tiantong.Iot.Server
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
