using DotNetCore.CAP;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class LifterEventController: BaseController
  {
    public const string Group = "lifter";

    private ICapPublisher _cap;

    private LifterServiceManager _lifters;

    private WmsService _wms;

    private DomainContext _domain;

    public LifterEventController(
      ICapPublisher cap,
      DomainContext domain,
      LifterServiceManager lifters,
      WmsService wms
    ) {
      _cap = cap;
      _domain = domain;
      _lifters = lifters;
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

      try {
        destination = _wms.GetPalletInfo(barcode).Destination;
      } catch {
        _cap.Publish(LifterTaskQueryFailedEvent.Message, new LifterTaskQueryFailedEvent(param.LifterId, param.Floor, barcode));
        return;
      }

      // 目标楼层与当前楼层相同，则不触发 TaskQueried 事件
      if (param.Floor == destination) {
        return;
      }

      _cap.Publish(LifterTaskQueriedEvent.Message, new LifterTaskQueriedEvent(
        param.LifterId, param.Floor, barcode, destination
      ));
    }

    [CapSubscribe(LifterTaskQueriedEvent.Message, Group = Group)]
    public void HandleTaskQueried(LifterTaskQueriedEvent param)
    {
      _lifters.Get(param.LifterId).SetDestination(param.Floor, param.Destination);
    }

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
    {
      var barcode = _lifters.Get(param.LifterId).GetPalletCode(param.Floor);

      try {
        var taskId = _wms.GetPalletInfo(barcode).TaskId;
        _wms.RequestPicking(param.LifterId, param.Floor, barcode, taskId);
        _cap.Publish(LifterTaskPickingEvent.Message, new LifterTaskPickingEvent(param.LifterId, param.Floor, barcode, param.TaskId));
      } catch {
        _cap.Publish(LifterTaskPickingFailedEvent.Message, new LifterTaskPickingFailedEvent(param.LifterId, param.Floor, barcode));
      }
    }

    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
    {
      _lifters.Get(param.LifterId).SetPickuped(param.Floor, true);
    }
  }
}
