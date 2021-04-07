using Microsoft.AspNetCore.Mvc;
using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskController: BaseController
  {
    private IRcsAgcTaskService _rcsTaskService;

    public RcsAgcTaskController(IRcsAgcTaskService rcsTaskService) {
      _rcsTaskService = rcsTaskService;
    }

    [HttpPost("/rcs/agc-tasks/create")]
    public INotifyResult<IMessageObject> Create([FromBody] RcsAgcTaskCreate param)
    {
      _rcsTaskService.Create(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务已创建");
    }

    [HttpPost("/rcs/agc-tasks/start")]
    public INotifyResult<IMessageObject> Start([FromBody] RcsAgcTaskStart param)
    {
      _rcsTaskService.Start(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务下发成功");
    }

    [HttpPost("/rcs/agc-tasks/finish")]
    public INotifyResult<IMessageObject> Finish([FromBody] RcsAgcTaskFinish param)
    {
      _rcsTaskService.Finish(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务已完成");
    }

    [HttpPost("/rcs/agc-tasks/close")]
    public INotifyResult<IMessageObject> Close([FromBody] RcsAgcTaskClose param)
    {
      _rcsTaskService.Close(param);

      return NotifyResult
        .FromVoid()
        .Success("AGC 任务已关闭");
    }

    [HttpPost("/rcs/agc-tasks/search")]
    public IPagination<RcsAgcTask> Search([FromBody] IQueryParams param)
    {
      return _rcsTaskService.Search(param);
    }

  }
}
