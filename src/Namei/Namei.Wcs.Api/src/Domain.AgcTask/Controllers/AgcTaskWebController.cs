using Microsoft.AspNetCore.Mvc;
using Midos.Domain;
using Namei.Wcs.Api;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskWebController: BaseController
  {
    private readonly IAgcTaskService _service;

    public AgcTaskWebController(IAgcTaskService service)
    {
      _service = service;
    }

    [HttpPost("/agc-tasks/create")]
    [HttpPost("/rcs/agc-tasks/create")]
    public INotifyResult<AgcTaskCreateResult> Create([FromBody] AgcTaskCreate param)
    {


      param.Priority ??= "";
      param.TaskId ??= "";
      param.PodCode ??= "";

      var result =_service.Create(param);

      return NotifyResult
        .From(result)
        .StatusCode(result.Code == 0 ? 201 : 400);
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
