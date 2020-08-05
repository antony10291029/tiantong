using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class RcsServiceController: BaseController
  {
    private ICapPublisher _cap;

    public RcsServiceController(ICapPublisher cap)
    {
      _cap = cap;
    }

    public class DoorParams
    {
      public int DoorId { get; set; }

      public string Method { get; set; }
    }

    [HttpPost]
    [Route("/doors/request")]
    public object HandleRequestOpen([FromBody] DoorParams param)
    {
      var message = "指令未识别";

      if (!Config.EnableRcsCommands) {
        message = "RCS 指令未开启";
      } else if (param.Method == "open") {
        message = "正在处理开门指令";
        _cap.Publish(DoorRequestingOpenEvent.Message, new DoorRequestingOpenEvent(param.DoorId));
      } else if (param.Method == "close") {
        message = "正在处理关门指令";
        _cap.Publish(DoorRequestingCloseEvent.Message, new DoorRequestingCloseEvent(param.DoorId));
      }

      return new {
        code = 0,
        message = message
      };
    }
  }
}
