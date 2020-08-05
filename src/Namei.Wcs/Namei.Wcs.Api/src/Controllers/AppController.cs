using Renet.Web;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private ICapPublisher _cap;

    public AppController(Config config, ICapPublisher cap)
    {
      _config = config;
      _cap = cap;
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

    [HttpPost]
    [Route("/publish")]
    public object Publish()
    {
      _cap.Publish("test", 1000);

      return new { value = "adf" };
    }

    [CapSubscribe("test")]
    public void HandleOne(int value)
    {
      System.Console.WriteLine("Handler1" + value);
    }

    [CapSubscribe("test")]
    public void HandleTwo(int value)
    {
      System.Console.WriteLine("Handler2" + value);
    }
  }
}
