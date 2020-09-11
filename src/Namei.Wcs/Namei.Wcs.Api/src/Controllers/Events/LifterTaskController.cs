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

    [CapSubscribe(LifterTaskImportedEvent.Message)]
    public void LifterTaskImported(LifterTaskImportedEvent param)
    {
      if (param.TaskId == null) {
        return;
      }

      _domain.Add(new LifterTask {
        lifter_id = param.LifterId,
        from = param.Floor,
        task_id = param.TaskId,
      });

      _domain.SaveChanges();
    }

    [CapSubscribe(LifterTaskExportedEvent.Message)]
    public void LifterTaskExported(LifterTaskExportedEvent param)
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

    [CapSubscribe(LifterTaskTakenEvent.Message)]
    public void LifterTaskTaken(LifterTaskTakenEvent param)
    {
      if (param.TaskId == null) {
        return;
      }

      var task = _domain.LifterTasks.Where(task => task.task_id == param.TaskId).First();

      if (task == null) {
        return;
      }

      task.status = LifterTaskStatusType.Taken;
      task.taken_at = DateTime.Now;

      _domain.SaveChanges();
    }
  }
}
