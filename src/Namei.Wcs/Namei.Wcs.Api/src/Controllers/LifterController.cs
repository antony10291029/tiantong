using Renet.Web;
using Microsoft.AspNetCore.Mvc;
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

    public class LifterNotify
    {
      public string method;

      public int lifter_id;

      public string floor;
    }

    [Route("/lifters/notify")]
    public object LiftersNotify([FromBody] LifterNotify param)
    {
      var message = "";

      if (param.method == "import") {
        _cap.Publish(LifterTaskImportedEvent.Message, new LifterTaskImportedEvent(param.lifter_id, param.floor));
        message = "收到放货完成通知";
      } else if (param.method == "export") {
        _cap.Publish(LifterTaskTakenEvent.Message, new LifterTaskTakenEvent(param.lifter_id, param.floor));
        message = "收到取货完成通知";
      }

      return new {
        message = message,
        lifter_id = param.lifter_id,
        floor = param.floor,
      };
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
