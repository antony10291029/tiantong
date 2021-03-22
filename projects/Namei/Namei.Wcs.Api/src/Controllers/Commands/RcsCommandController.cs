using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Namei.Wcs.Api
{
  public class RcsServiceController: BaseController
  {
    private ICapPublisher _cap;

    private Logger _logger;

    private RcsService _rcs;

    public RcsServiceController(
      ICapPublisher cap,
      Logger logger,
      RcsService rcs
    ) {
      _cap = cap;
      _logger = logger;
      _rcs = rcs;
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

      public string From { get; set; } = "RCS";
    }

    [HttpPost]
    [Route("/REV_AGC/NotifyTaskInfo")]
    public object HandleRequestOpen([FromBody] DoorParams param)
    {
      var message = "指令未识别";

      if (param.actionTask == "applyLock") {
        message = "正在处理开门指令";
        _cap.Publish(DoorTaskRequestOpenEvent.Message, new DoorTaskRequestOpenEvent(param.deviceIndex, param.uuid));
      } else if (param.actionTask == "releaseDevice") {
        message = "正在处理关门指令";
        _cap.Publish(DoorTaskRequestCloseEvent.Message, new DoorTaskRequestCloseEvent(param.deviceIndex, param.uuid));
      }

      _logger.Save(
        level: Log.UseInfo(),
        klass: "rcs.door",
        operation: "command",
        message: $"收到 {param.From} 自动门指令",
        index: param.uuid,
        data: param
      );

      return new {
        code = 0,
        message = message
      };
    }

    [HttpPost("/rcs/tasks/create")]
    public RcsTaskCreateResult HandleTaskCreate([FromBody] RcsTaskCreateParams param)
    {
      return _rcs.CreateTask(param);
    }

    [HttpPost("/rcs/tasks/continue")]
    public RcsTaskCreateResult HandleTaskContinue([FromBody] RcsTaskContinueParams param)
    {
      return _rcs.ContinueTask(param);
    }

    [HttpPost("/rcs/tasks/cancel")]
    public RcsTaskCancelResult HandleTaskCancel([FromBody] RcsTaskCancelParams param)
    {
      return _rcs.CancelTask(param);
    }
  }
}
