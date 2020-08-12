using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class RcsServiceController: BaseController
  {
    private ICapPublisher _cap;

    private DomainContext _domain;

    public RcsServiceController(ICapPublisher cap, DomainContext domain)
    {
      _cap = cap;
      _domain = domain;
    }

    public class DoorParams
    {
      public string type { get; set; }

      public string deviceType { get; set; }

      public string uuid { get; set; }

      public string deviceIndex { get; set; }

      public string actionTask { get; set; }

      public string Src { get; set; }

      public string Dst { get; set; }
    }

    [HttpPost]
    [Route("/rcs/NotifyTaskInfo")]
    public object HandleRequestOpen([FromBody] DoorParams param)
    {
      var message = "指令未识别";
      var task = new RcsTask {
        type = param.type,
        uuid = param.uuid,
        device_type = param.deviceType,
        device_index = param.deviceIndex,
        action_task = param.actionTask,
        src = param.Src,
        dst = param.Dst
      };

      if (!Config.EnableRcsCommands) {
        message = "RCS 指令未开启";
      } else if (param.actionTask == "applyLock") {
        message = "正在处理开门指令";
        _domain.Add(task);
        _domain.SaveChanges();
        _cap.Publish(DoorRequestedOpenEvent.Message, new DoorRequestedOpenEvent(param.deviceIndex));
      } else if (param.actionTask == "releaseDevice") {
        message = "正在处理关门指令";
        _domain.Add(task);
        _domain.SaveChanges();
        _cap.Publish(DoorRequestedCloseEvent.Message, new DoorRequestedCloseEvent(param.deviceIndex));
      }

      return new {
        code = 0,
        message = message
      };
    }
  }
}
