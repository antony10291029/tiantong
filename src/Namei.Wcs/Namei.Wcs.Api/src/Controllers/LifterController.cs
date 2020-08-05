using Renet.Web;
using DotNetCore.CAP;

namespace Namei.Wcs.Api
{
  public class LifterController: BaseController
  {
    private ICapPublisher _cap;

    private LifterServiceManager _lifters;

    private WmsService _wms;

    public LifterController(
      ICapPublisher cap,
      LifterServiceManager lifters,
      WmsService wms
    ) {
      _cap = cap;
      _lifters = lifters;
      _wms = wms;
    }

    // events

    [CapSubscribe(LifterTaskImportedEvent.Message)]
    public void HandleTaskReleased(LifterTaskImportedEvent param)
    {
      _lifters.Get(param.LifterId).Release(param.Floor);
    }

    [CapSubscribe(LifterTaskScannedEvent.Message)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
    {
      var barcode = _lifters.Get(param.LifterId).GetPalletCode(param.Floor);
      var destination = _wms.GetPalletInfo(barcode).Destination;

      _lifters.Get(param.LifterId).SetDestination(param.Floor, destination);
    }

    [CapSubscribe(LifterTaskExportedEvent.Message)]
    public void HandleTaskFinished(LifterTaskExportedEvent param)
    {
      var barcode = _lifters.Get(param.LifterId).GetPalletCode(param.Floor);
      var taskId = _wms.GetPalletInfo(barcode).TaskId;

      _wms.RequestPicking(param.LifterId, param.Floor, barcode, taskId);
    }

    [CapSubscribe(LifterTaskTakenEvent.Message)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
    {
      _lifters.Get(param.LifterId).Pickup(param.Floor);
    }
  }
}
