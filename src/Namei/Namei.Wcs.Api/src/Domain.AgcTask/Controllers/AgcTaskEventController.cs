using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskEventController: BaseController
  {
    public const string Group = "AgcTaskEventController";

    private readonly IAgcTaskService _service;

    public AgcTaskEventController(IAgcTaskService service)
      => _service = service;

    [CapSubscribe(AgcTaskCreate.@event, Group = Group)]
    public void Create(AgcTaskCreate param)
      => _service.Create(param);

    [CapSubscribe(AgcTaskStart.@event, Group = Group)]
    public void Start(AgcTaskStart param)
      => _service.Start(param);

    [CapSubscribe(AgcTaskStarted.@event, Group = Group)]
    public void Started(AgcTaskStarted param)
      => _service.Started(param);

    [CapSubscribe(AgcTaskClose.@event, Group = Group)]
    public void Close(AgcTaskClose param)
      => _service.Close(param);

    [CapSubscribe(AgcTaskFinish.@event, Group = Group)]
    public void Finish(AgcTaskFinish param)
      => _service.Finish(param);

    [CapSubscribe(AgcTaskFinished.@event, Group = Group)]
    public void Finished(AgcTaskFinished param)
      => _service.Finished(param);

    // 任务创建后自动执行
    [CapSubscribe(AgcTaskCreated.@event, Group = Group)]
    public void Created(AgcTaskCreated param)
      => _service.Start(AgcTaskStart.From(param));

  }
}
