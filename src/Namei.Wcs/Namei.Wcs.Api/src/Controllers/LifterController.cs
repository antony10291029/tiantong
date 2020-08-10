using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet;
using Renet.Web;
using System.Linq;

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

    public class GetLifterLogsParams
    {
      public int page { get; set; }

      public int page_size { get; set; } = 20;
    }

    [HttpPost]
    [Route("/lifters/logs")]
    public object GetLifterLogs([FromBody] GetLifterLogsParams param)
    {
      return _domain.Logs.Where(log => log.key.StartsWith("lifter"))
        .OrderByDescending(log => log.created_at)
        .Paginate(param.page, param.page_size);
    }

    // events

    [CapSubscribe(LifterTaskImportedEvent.Message)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
    {
      _lifters.Get(param.LifterId).Imported(param.Floor);
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
