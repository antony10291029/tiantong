using Renet.Web;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Namei.Wcs.Api
{
  public class LifterCommandController: BaseController
  {
    private WmsService _wms;

    private FirstLifterService _lifter;

    private LifterServiceManager _lifters;

    private ICapPublisher _cap;

    public LifterCommandController(
      ICapPublisher cap,
      LifterServiceManager lifters,
      FirstLifterService lifter,
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
    public object ConveyorChanged([FromBody] ConveyorChangedParams param)
    {
      var message = "输送线状态无需处理";
      var isScanned = _lifter.IsTaskScanned(param.value, param.old_value);
      var isFinished = _lifter.IsRequestingPickup(param.value);

      if (!Config.EnableLifterCommands) {
        message = "货梯指令未开启";
      } else if (isScanned && isFinished) {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent("1", param.floor));
        message = "正在处理取货指令";
      } else if (isScanned) {
        _cap.Publish(LifterTaskScannedEvent.Message, new LifterTaskScannedEvent("1", param.floor));
        message = "正在处理读码指令";
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

      if (!Config.EnableHoistersCommands) {
        message = "提升机指令未开启";
      } else if (param.value == "1") {
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

      if (!Config.EnableHoistersCommands) {
        message = "提升机指令未开启";
      } else if (param.value == "3") {
        _cap.Publish(LifterTaskExportedEvent.Message, new LifterTaskExportedEvent(param.lifter_id, param.floor));
        message = "正在处理取货指令";
      }

      return new { message };
    }
  }
}
