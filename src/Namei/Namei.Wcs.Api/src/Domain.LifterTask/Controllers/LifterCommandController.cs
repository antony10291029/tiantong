using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class LifterCommandController: BaseController
  {
    private ILifterServiceFactory _lifters;

    private ICapPublisher _cap;

    private IWmsService _wms;

    public LifterCommandController(
      ICapPublisher cap,
      ILifterServiceFactory lifters,
      IWmsService wms
    ) {
      _cap = cap;
      _lifters = lifters;
      _wms = wms;
    }

    public class LifterTaskScannedParams
    {
      public string lifter_id { get; set; }

      public string floor { get; set; }

      public string value { get; set; }
    }

    [HttpPost]
    [Route("/standard-lifters/scanned")]
    public object LifterTaskScanned([FromBody] LifterTaskScannedParams param)
    {
      var message = "扫码状态无需处理";

      if (param.value == "1") {
        _cap.Publish(LifterTaskScannedEvent.Message, new LifterTaskScannedEvent(param.lifter_id, param.floor));
        message = "正在处理读码指令";
      }

      return new { message };
    }

    public class LifterTaskExportedParams
    {
      public string lifter_id { get; set; }

      public string floor { get; set; }

      public string value { get; set; }
    }

    [HttpPost]
    [Route("/standard-lifters/exported")]
    public object HandleLifterTaskExported([FromBody] LifterTaskExportedParams param)
    {
      var message = "指令未识别";

      if (param.value == "3") {
        _cap.Publish(LifterTaskExported.Message, LifterTaskExported.From(
          lifterId: param.lifter_id,
          floor: param.floor
        ));
        message = "正在处理取货指令";
      }

      if (param.value == "2" || param.value == "3") {
        var doorId =  CrashDoor.GetDoorIdFromLifter(param.floor, param.lifter_id);

        _cap.Publish(WcsDoorEvent.Opened, WcsDoorEvent.From(doorId));
      }

      return NotifyResult.FromVoid()
        .Success(message);
    }

    public class StandardLifterConveyorChangedParams
    {
      public string lifter_id { get; set; }

      public string floor { get; set; }

      public string value { get; set; }
    }

    [HttpPost("/standard-lifters/conveyor/changed")]
    public object StandardLifterConveyorChanged([FromBody] StandardLifterConveyorChangedParams param)
    {
      var message = "状态无需处理";

      if (param.value == "1") {
        _lifters.Get(param.lifter_id).SetDestination(param.floor, "0");
        _lifters.Get(param.lifter_id).SetImported(param.floor, false);
        message = "数据已清空";
      }

      return NotifyResult.FromVoid()
        .Success(message);
    }
  }
}
