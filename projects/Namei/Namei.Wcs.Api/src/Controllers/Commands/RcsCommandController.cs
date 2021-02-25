using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Namei.Wcs.Api
{
  public class RcsServiceController: BaseController
  {
    private ICapPublisher _cap;

    private Logger _logger;

    public RcsServiceController(ICapPublisher cap, Logger logger)
    {
      _cap = cap;
      _logger = logger;
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
    [Route("/REV_AGC/NotifyTaskInfo")]
    public object HandleRequestOpen([FromBody] DoorParams param)
    {
      var message = "指令未识别";

      _logger.Log("info", "test", JsonSerializer.Serialize(param));

      if (param.actionTask == "applyLock") {
        message = "正在处理开门指令";
        _cap.Publish(DoorTaskRequestOpenEvent.Message, new DoorTaskRequestOpenEvent(param.deviceIndex, param.uuid));
      } else if (param.actionTask == "releaseDevice") {
        message = "正在处理关门指令";
        _cap.Publish(DoorTaskRequestCloseEvent.Message, new DoorTaskRequestCloseEvent(param.deviceIndex, param.uuid));
      }

      return new {
        code = 0,
        message = message
      };
    }
  }
}
