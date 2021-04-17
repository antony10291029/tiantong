using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class RcsServiceController: BaseController
  {
    private ICapPublisher _cap;

    private IRcsService _rcs;

    private Logger _logger;

    public RcsServiceController(
      ICapPublisher cap,
      Logger logger,
      IRcsService rcs
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

    public class NotifyTaskResult: MessageObject
    {
      public int code = 0;
    }

    [HttpPost]
    [Route("/REV_AGC/NotifyTaskInfo")]
    public IResult<object> HandleRequestOpen([FromBody] DoorParams param)
    {
      var data = new NotifyTaskResult();
      var result = Result.FromObject(data).StatusCode(201);

      if (param.actionTask == "applyLock") {
        data.Message = "收到开门任务";
        _cap.Publish(
          RcsDoorEvent.Request,
          RcsDoorEvent.From(
            uuid: param.uuid,
            doorId: param.deviceIndex
          )
        );
      } else if (param.actionTask == "releaseDevice") {
        data.Message = "收到关门任务";
        _cap.Publish(
          RcsDoorEvent.Leave,
          RcsDoorEvent.From(
            uuid: param.uuid,
            doorId: param.deviceIndex
          )
        );
      }

      _logger.Save(
        level: Log.UseInfo(),
        klass: "rcs.door",
        operation: "command",
        message: $"收到 {param.From} 自动门指令",
        index: param.uuid,
        data: param
      );

      return result;
    }
  }
}
