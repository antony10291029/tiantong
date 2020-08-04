using Renet.Web;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Namei.Wcs.Api
{
  public class ReformedLifterController: BaseController
  {
    private WmsService _wms;

    private ReformedLifterService _lifter;

    private LifterServiceManager _lifters;

    private ICapPublisher _cap;

    public ReformedLifterController(
      ICapPublisher cap,
      LifterServiceManager lifters,
      ReformedLifterService lifter,
      WmsService wms
    ) {
      _cap = cap;
      _lifter = lifter;
      _lifters = lifters;
      _wms = wms;
    }

    public class ConveyorChangedParams
    {
      public string floor { get; set; }

      public string value { get; set; }

      public string old_value { get; set; }
    }

    [HttpPost]
    [Route("reformed-lifters/conveyor/change")]
    public object HandleConveyorChanged([FromBody] ConveyorChangedParams param)
    {
      var message = "";
      var isScanned = _lifter.IsTaskScanned(param.value, param.old_value);
      var isFinished = _lifter.IsTaskFinished(param.value);

      if (isScanned && isFinished) {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent(1, param.floor));
      } else if (isScanned) {
        _cap.Publish(LifterTaskScannedEvent.Message, new LifterTaskScannedEvent(1, param.floor));
      }

      return new {
        message = message
      };
    }
  }
}
