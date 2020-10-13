using DotNetCore.CAP;
using Renet.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class LifterEventController: BaseController
  {
    public const string Group = "lifter";

    private ICapPublisher _cap;

    private LifterServiceManager _lifters;

    private LifterTaskService _tasks;

    private WmsService _wms;

    private DomainContext _domain;

    public LifterEventController(
      ICapPublisher cap,
      DomainContext domain,
      LifterServiceManager lifters,
      LifterTaskService tasks,
      WmsService wms
    ) {
      _cap = cap;
      _domain = domain;
      _lifters = lifters;
      _tasks = tasks;
      _wms = wms;
    }

    // events

    [CapSubscribe(LifterTaskImportedEvent.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
    {
      _lifters.Get(param.LifterId).SetImported(param.Floor, true);
    }

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
    {
      var barcode = _lifters.Get(param.LifterId).GetPalletCode(param.Floor);
      var destination = "";
      var taskid = "";

      try {
        var info = _wms.GetPalletInfo(barcode);
        taskid = info.TaskId;
        destination = info.Destination;
      } catch {
        _cap.Publish(LifterTaskQueryFailedEvent.Message, new LifterTaskQueryFailedEvent(param.LifterId, param.Floor, barcode));
        return;
      }

      // 目标楼层与当前楼层相同，则不触发 TaskQueried 事件
      if (param.Floor == destination) {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent(param.LifterId, param.Floor));
      } else {
        _cap.Publish(LifterTaskQueriedEvent.Message, new LifterTaskQueriedEvent(
          param.LifterId, param.Floor, barcode, destination, taskid
        ));
      }
    }

    [CapSubscribe(LifterTaskQueriedEvent.Message, Group = Group)]
    public void HandleTaskQueried(LifterTaskQueriedEvent param)
    {
      _lifters.Get(param.LifterId).SetDestination(param.Floor, param.Destination);
    }

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
    {
      var now = DateTime.Now;
      var lifter = _lifters.Get(param.LifterId);

      // 对于同一台提升机的同一楼层，10 秒内只接收一次取货请求
      if (lifter.ExportedAt[param.Floor].AddSeconds(10) > now) {
        return;
      }

      lifter.ExportedAt[param.Floor] = now;

      var barcode = lifter.GetPalletCode(param.Floor);

      if (barcode.Length != 6 || int.TryParse(barcode, out _)) {
        return;
      }

      try {
        var taskId = _wms.GetPalletInfo(barcode).TaskId;
        _wms.RequestPicking(param.LifterId, param.Floor, barcode, taskId);
        _cap.Publish(LifterTaskPickingEvent.Message, new LifterTaskPickingEvent(param.LifterId, param.Floor, barcode, taskId));
        _tasks.Set(barcode);
      } catch (Exception e) {
        _cap.Publish(LifterTaskPickingFailedEvent.Message, new LifterTaskPickingFailedEvent(param.LifterId, param.Floor, barcode, e.Message));
      }
    }

    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
    {
      var lifter = _lifters.Get(param.LifterId);

      lifter.SetPickuped(param.Floor, true);

      Task.Delay(2000).ContinueWith(async task => {
        await task;
        lifter.SetPickuped(param.Floor, false);
      });
    }
  }
}
