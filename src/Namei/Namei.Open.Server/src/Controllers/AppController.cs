using Microsoft.AspNetCore.Mvc;

namespace Namei.Open.Server
{
  public class AppController
  {
    [HttpGet]
    [HttpPost]
    [Route("/")]
    public object Home()
    {
      return new {
        app = "Namei.Open.Api",
        version = "0.0.1"
      };
    }
  }
}
