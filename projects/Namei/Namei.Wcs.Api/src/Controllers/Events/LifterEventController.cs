using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class LifterEventController: BaseController
  {
    public const string Group = "lifter";

    private ICapPublisher _cap;

    private LifterServiceManager _lifters;

    private WmsService _wms;

    public LifterEventController(
      ICapPublisher cap,
      LifterServiceManager lifters,
      WmsService wms
    ) {
      _cap = cap;
      _lifters = lifters;
      _wms = wms;
    }

    [CapSubscribe(LifterTaskReceived.Message, Group = Group)]
    public void HandleTaskReceived(LifterTaskReceived param)
    {
      _cap.Publish(LifterTaskImported.Message, LifterTaskImported.From(
        lifterId: param.LifterId,
        floor: param.Floor,
        barcode: param.Barcode,
        destination: param.Destination
      ));
    }

    [CapSubscribe(LifterTaskImported.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImported param)
    {
      var lifter = _lifters.Get(param.LifterId);

      if (param.LifterId == "1") {
        lifter.Import(floor: param.Floor);
      } else {
        lifter.Import(
          floor: param.Floor,
          destination: param.Destination,
          barcode: param.Barcode
        );
      }
    }

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
    {
      var lifter = _lifters.Get(param.LifterId);

      if (lifter.GetDestination(param.Floor) != "0") {
        return;
      }

      var barcode = lifter.GetPalletCode(param.Floor);

      try {
        var info = _wms.GetPalletInfo(barcode);
        var taskid = info.TaskId;
        var destination = info.Destination;

        _cap.Publish(LifterTaskQueriedEvent.Message, new LifterTaskQueriedEvent(
          param.LifterId, param.Floor, barcode, destination, taskid
        ));
      } catch {
        _cap.Publish(LifterTaskQueryFailedEvent.Message, new LifterTaskQueryFailedEvent(param.LifterId, param.Floor, barcode));
        return;
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

      if (barcode.Length != 6 || !int.TryParse(barcode, out _)) {
        _cap.Publish(LifterTaskPickingFailedEvent.Message, new LifterTaskPickingFailedEvent(param.LifterId, param.Floor, barcode, "托盘号异常"));
        return;
      }

      try {
        var taskId = _wms.GetPalletInfo(barcode).TaskId;
        _wms.RequestPicking(param.LifterId, param.Floor, barcode, taskId);
        _cap.Publish(LifterTaskPickingEvent.Message, new LifterTaskPickingEvent(param.LifterId, param.Floor, barcode, taskId));
      } catch (Exception e) {
        _cap.Publish(LifterTaskPickingFailedEvent.Message, new LifterTaskPickingFailedEvent(param.LifterId, param.Floor, barcode, e.Message));
      }
    }

    [CapSubscribe(LifterTaskTaken.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTaken param)
    {
      var lifter = _lifters.Get(param.LifterId);

      lifter.SetPickuped(param.Floor, true);

      Task.Delay(2000).ContinueWith(async _ => {
        await _;
        lifter.SetPickuped(param.Floor, false);
      });
    }
  }
}
