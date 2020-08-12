using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class LifterController: BaseController
  {
    private ICapPublisher _cap;

    private LifterServiceManager _lifters;

    private WmsService _wms;

    private DomainContext _domain;

    public LifterController(
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

    public class GetLifterStatesParams
    {
      public string lifter_id { get; set; }
    }

    [HttpPost]
    [Route("/lifters/states")]
    public object GetLifterStates([FromBody] GetLifterStatesParams param)
    {
      return _lifters.Get(param.lifter_id).GetStates();
    }

    // events

    [CapSubscribe(LifterTaskImportedEvent.Message)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
    {
      _lifters.Get(param.LifterId).SetImported(param.Floor, true);
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
      _lifters.Get(param.LifterId).SetPickuped(param.Floor, true);
      Task.Delay(1000).ContinueWith(async task => {
        await task;
        _lifters.Get(param.LifterId).SetPickuped(param.Floor, false);
      });
    }
  }
}
