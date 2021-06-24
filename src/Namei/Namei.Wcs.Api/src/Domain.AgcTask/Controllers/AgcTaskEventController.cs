using Microsoft.AspNetCore.Mvc;
using Midos.Eventing;

namespace Namei.Wcs.Aggregates
{
  public class AgcTaskEventController: BaseController
  {
    public const string Group = "AgcTaskEventController";

    private readonly IAgcTaskService _service;

    public AgcTaskEventController(IAgcTaskService service)
      => _service = service;

    [EventSubscribe(AgcTaskCreate.@event, Group)]
    public void Create(AgcTaskCreate param)
      => _service.Create(param);

    [EventSubscribe(AgcTaskClose.@event, Group)]
    public void Close(AgcTaskClose param)
      => _service.Close(param);

    [EventSubscribe(AgcTaskFinish.@event, Group)]
    public void Finish(AgcTaskFinish param)
      => _service.Finish(param);

    [EventSubscribe(AgcTaskFinished.@event, Group)]
    public void Finished(AgcTaskFinished param)
      => _service.Finished(param);

  }
}
