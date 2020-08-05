using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class WmsServiceController: BaseController
  {
    private ICapPublisher _cap;

    private WmsService _wms;

    public WmsServiceController(
      ICapPublisher cap,
      WmsService wms
    ) {
      _cap = cap;
      _wms = wms;
    }

    public class LifterNotify
    {
      public string method;

      public int lifter_id;

      public string floor;
    }

    [Route("/lifters/notify")]
    public object LiftersNotify([FromBody] LifterNotify param)
    {
      var message = "指令未识别";

      if (!Config.EnableWmsCommands) {
        message = "WMS 指令未开启";
      } if (param.method == "import") {
        _cap.Publish(LifterTaskImportedEvent.Message, new LifterTaskImportedEvent(param.lifter_id, param.floor));
        message = "正在处理放货完成指令";
      } else if (param.method == "export") {
        _cap.Publish(LifterTaskTakenEvent.Message, new LifterTaskTakenEvent(param.lifter_id, param.floor));
        message = "正在处理取货完成指令";
      }

      return new {
        message = message,
        lifter_id = param.lifter_id,
        floor = param.floor,
      };
    }
  }
}
