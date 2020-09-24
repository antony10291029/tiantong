using System.Text.RegularExpressions;
using DotNetCore.CAP;
using Renet.Web;
using System;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterTaskController: BaseController
  {
    public const string Group = "lifter.task";

    private DomainContext _domain;

    public LifterTaskController(DomainContext domain)
    {
      _domain = domain;
    }

    [CapSubscribe(LifterTaskQueriedEvent.Message)]
    public void LifterTaskQueried(LifterTaskQueriedEvent param)
    {
      if (param.TaskId == null) {
        return;
      }

      _domain.Add(new LifterTask {
        lifter_id = param.LifterId,
        from = param.Floor,
        task_id = param.TaskId,
        to = param.Destination,
        pallet_code = param.Barcode,
      });

      _domain.SaveChanges();
    }

    [CapSubscribe(LifterTaskPickingEvent.Message)]
    public void HandleLifterTaskPicking(LifterTaskPickingEvent param)
    {
      if (param.TaskId == null) {
        return;
      }

      var task = _domain.LifterTasks.Where(task => task.task_id == param.TaskId).FirstOrDefault();

      if (task == null) {
        return;
      }

      task.status = LifterTaskStatusType.Exported;
      task.exported_at = DateTime.Now;

      _domain.SaveChanges();
    }
  }
}
