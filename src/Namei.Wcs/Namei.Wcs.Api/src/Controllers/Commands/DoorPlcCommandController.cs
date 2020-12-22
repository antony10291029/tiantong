using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class DoorPlcCommandController: BaseController
  {
    private ICapPublisher _cap;

    public DoorPlcCommandController(ICapPublisher cap)
    {
      _cap = cap;
    }

    public class DoorStateChangedParams
    {
      public string door_id { get; set; }

      public string value { get; set; }
    }

    [HttpPost("/doors/state/changed")]
    public object DoorStateChanged([FromBody] DoorStateChangedParams param)
    {
      var message = "指令未识别";

      if (!Config.EnableDoorsCommands) {
        message = "自动门指令未开启";
      } else if (param.value == "12") {
        message = "正在处理开门完成指令";
        _cap.Publish(DoorOpenedEvent.Message, new DoorOpenedEvent(param.door_id));
      } else if (param.value == "22") {
        message = "正在处理关门完成指令";
        _cap.Publish(DoorClosedEvent.Message, new DoorClosedEvent(param.door_id));
      }

      return new { message };
    }
  }
}
