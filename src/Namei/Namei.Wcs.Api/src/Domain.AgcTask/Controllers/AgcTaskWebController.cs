using Microsoft.AspNetCore.Mvc;
using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskWebController: BaseController
  {
    private readonly IAgcTaskService _service;

    public AgcTaskWebController(IAgcTaskService service)
    {
      _service = service;
    }

    public struct CreateResult: IMessageObject
    {
      public int Code { get; set; }

      public string Message { get; set; }

      public object Data { get; set; }
    }

    [HttpPost("/agc-tasks/create")]
    [HttpPost("/rcs/agc-tasks/create")]
    public INotifyResult<CreateResult> Create([FromBody] AgcTaskCreate param)
    {
      var result = new CreateResult {
        Code = 0,
        Message = "任务已创建",
        Data = param
      };

      param.Priority ??= "";
      param.TaskId ??= "";
      param.PodCode ??= "";

      _service.Create(param);

      return NotifyResult
        .From(result)
        .Success("AGC 任务已创建");
    }

    [HttpPost("/agc-tasks/start")]
    public INotifyResult<IMessageObject> Start([FromBody] AgcTaskStart param)
    {
      _service.Start(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务下发成功");
    }

    [HttpPost("/agc-tasks/finish")]
    public INotifyResult<IMessageObject> Finish([FromBody] AgcTaskFinish param)
    {
      _service.Finish(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务已完成");
    }

    [HttpPost("/agc-tasks/close")]
    public INotifyResult<IMessageObject> Close([FromBody] AgcTaskClose param)
    {
      _service.Close(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务已关闭");
    }

    [HttpPost("/agc-tasks/search")]
    public IPagination<AgcTask> Search([FromBody] QueryParams param)
    {
      return _service.Search(param);
    }

  }
}
