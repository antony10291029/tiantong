using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class LifterTaskController: BaseController
  {
    private const string Group = "lifter.task";

    private DomainContext _domain;

    private ILifterServiceFactory _lifters;

    public LifterTaskController(
      DomainContext domain,
      ILifterServiceFactory lifters
    ) {
      _domain = domain;
      _lifters = lifters;
    }

    private bool IsNotCreated(LifterTaskImported param)
      => param.Barcode != null &&
        param.TaskCode != null &&
        param.Destination != null &&
        !_domain.LifterTasks.Any(task => task.TaskCode == param.TaskCode);

    [CapSubscribe(LifterTaskImported.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImported param)
    {
      if (IsNotCreated(param)) {
        var task = LifterTask.FromImportedEvent(param);

        _domain.Add(task);
        _domain.SaveChanges();
      }
    }

    [CapSubscribe(LifterTaskExported.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExported param)
    {
      var barcode = _lifters.Get(param.LifterId).GetPalletCode(param.Floor);
      var tasks = _domain.LifterTasks.Where(task =>
        task.Barcode == barcode &&
        task.Status == LifterTaskStatus.Imported
      ).ToArray();

      if (tasks.Length == 0) {
        return;
      }

      foreach (var task in tasks) {
        task.SetExported();
      }

      _domain.SaveChanges();
    }

    [CapSubscribe(LifterTaskTaken.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTaken param)
    {
      var tasks = _domain.LifterTasks.Where(task =>
        task.LifterId == param.LifterId &&
        task.Destination == param.Floor &&
        task.Status == LifterTaskStatus.Exported
      ).ToArray();

      if (tasks.Length == 0) {
        return;
      }

      foreach (var task in tasks) {
        task.SetTaken();
      }

      _domain.SaveChanges();
    }
  }
}
