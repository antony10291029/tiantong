using Microsoft.AspNetCore.Mvc;
using Midos.Eventing;

namespace Namei.Wcs.Api
{
  public class RcsServiceController: BaseController
  {
    private readonly IEventPublisher _publisher;

    private readonly Logger _logger;

    public RcsServiceController(
      IEventPublisher publisher,
      Logger logger
    ) {
      _publisher = publisher;
      _logger = logger;
    }

    public class DoorParams
    {
      public string Type { get; set; }

      public string DeviceType { get; set; }

      public string Uuid { get; set; }

      public string DeviceIndex { get; set; }

      public string ActionTask { get; set; }

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

      if (param.ActionTask == "applyLock") {
        data.Message = "收到开门任务";
        _publisher.Publish(
          RcsDoorEvent.Request,
          RcsDoorEvent.From(
            uuid: param.Uuid,
            doorId: param.DeviceIndex
          )
        );
      } else if (param.ActionTask == "releaseDevice") {
        data.Message = "收到关门任务";
        _publisher.Publish(
          RcsDoorEvent.Leave,
          RcsDoorEvent.From(
            uuid: param.Uuid,
            doorId: param.DeviceIndex
          )
        );
      }

      _logger.Save(
        level: Log.UseInfo(),
        klass: "rcs.door",
        operation: "command",
        message: $"收到 {param.From} 自动门指令",
        index: param.Uuid,
        data: param
      );

      return result;
    }
  }
}
