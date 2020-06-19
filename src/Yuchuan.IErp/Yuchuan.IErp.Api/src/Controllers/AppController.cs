using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Yuchuan.IErp.Api
{
  [Route("/")]
  public class AppController: BaseController
  {
    public AppController()
    {

    }

    [HttpPost]
    [Route("/hc")]
    public object HealthCheck()
    {
      return new {
        message = "health check"
      };
    }

    [HttpPost]
    [Route("/logout")]
    public object Logout()
    {
      return new {
        message = "logout"
      };
    }
  }
}