using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class LifterCommandController: BaseController
  {
    private LifterServiceManager _lifters;

    private ICapPublisher _cap;

    private WmsService _wms;

    public LifterCommandController(
      ICapPublisher cap,
      LifterServiceManager lifters,
      WmsService wms
    ) {
      _cap = cap;
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
    [Route("reformed-lifters/conveyor/changed")]
    public object ConveyorChanged([FromBody] ConveyorChangedParams param)
    {
      var lifter = _lifters.Get("1");
      var message = "输送线状态无需处理";

      if (param.value == null || param.old_value == null) {
        return new { message };
      }
 
      var isScanned = FirstLifterService.IsTaskScanned(param.value, param.old_value);
      var isImportedAllowed = FirstLifterService.IsImportAllowed(param.value, param.old_value);
      var isRequestingPickup = FirstLifterService.IsRequestingPickup(param.value, param.old_value);
      var isSpare = FirstLifterService.IsSpare(param.value, param.old_value);

      if (isRequestingPickup) {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent("1", param.floor));
        message = "正在处理取货指令";
      } else if (isScanned) {
        _cap.Publish(LifterTaskScannedEvent.Message, new LifterTaskScannedEvent("1", param.floor));
        message = "正在处理读码指令";
      } else if (isSpare) {
        lifter.SetImported(param.floor, false);
        message = "正在清除信号";
      }

      if (isImportedAllowed || isRequestingPickup) {
        var doorId = CrashDoor.GetDoorIdFromLifter(param.floor, "1");

        _cap.Publish(DoorTaskHandleEvent.Message, new DoorTaskHandleEvent(doorId));
      }

      return new { message };
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
    public object LifterTaskExported([FromBody] LifterTaskExportedParams param)
    {
      var message = "指令未识别";

      if (param.value == "3") {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent(param.lifter_id, param.floor));
        message = "正在处理取货指令";
      }

      if (param.value == "2" || param.value == "3") {
        var doorId =  CrashDoor.GetDoorIdFromLifter(param.floor, param.lifter_id);

        _cap.Publish(DoorTaskHandleEvent.Message, new DoorTaskHandleEvent(doorId));
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
