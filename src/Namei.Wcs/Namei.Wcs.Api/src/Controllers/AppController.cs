using Renet.Web;
using Microsoft.AspNetCore.Mvc;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class AppController: BaseController
  {
    private Config _config;

    private PlcStateService _plc;

    public AppController(Config config, PlcStateService plc)
    {
      _plc = plc;
      _config = config;
    }

    [HttpGet]
    [HttpPost]
    [Route("/")]
    public ActionResult<object> Home()
    {
      _plc.Configure("http://localhost:5100/", "测试设备 1");

      return new {
        value = _plc.GetAsync("测试数据点 1").GetAwaiter().GetResult(),
        message = $"{_config.AppName} v{_config.AppVersion}"
      };
    }
  }
}
