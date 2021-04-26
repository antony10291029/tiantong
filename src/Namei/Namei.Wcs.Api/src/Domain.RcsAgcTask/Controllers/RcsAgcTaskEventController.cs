using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Aggregates
{
  public class RcsAgcTaskEventController: BaseController
  {
    private IRcsAgcTaskService _rcsTaskService;

    public RcsAgcTaskEventController(IRcsAgcTaskService rcsTaskService)
      => _rcsTaskService = rcsTaskService;

    [CapSubscribe(RcsAgcTaskCreate.Message, Group = "rcs.tasks")]
    public void Create(RcsAgcTaskCreate param)
      => _rcsTaskService.Create(param);

    [CapSubscribe(RcsAgcTaskStart.Message, Group = "rcs.tasks")]
    public void Start(RcsAgcTaskStart param)
      => _rcsTaskService.Start(param);

    [CapSubscribe(RcsAgcTaskStarted.Message, Group = "rcs.tasks")]
    public void Started(RcsAgcTaskStarted param)
      => _rcsTaskService.Started(param);

    [CapSubscribe(RcsAgcTaskClose.Message, Group = "rcs.tasks")]
    public void Close(RcsAgcTaskClose param)
      => _rcsTaskService.Close(param);

    [CapSubscribe(RcsAgcTaskFinish.Message, Group = "rcs.tasks")]
    public void Finish(RcsAgcTaskFinish param)
      => _rcsTaskService.Finish(param);

    [CapSubscribe(RcsAgcTaskFinished.Message, Group = "rcs.tasks")]
    public void Finished(RcsAgcTaskFinished param)
      => _rcsTaskService.Finished(param);

    // 任务创建后自动执行
    [CapSubscribe(RcsAgcTaskCreated.Message, Group = "rcs.tasks")]
    public void Created(RcsAgcTaskCreated param)
      => _rcsTaskService.Start(RcsAgcTaskStart.From(param));

  }
}
