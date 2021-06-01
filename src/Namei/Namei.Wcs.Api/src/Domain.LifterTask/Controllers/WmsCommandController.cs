using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class WmsCommandController: BaseController
  {
    private readonly Logger _logger;

    private readonly IWmsService _wms;
    
    public WmsCommandController(
      Logger logger,
      IWmsService wms
    ) {
      _wms = wms;
      _logger = logger;
    }

    public class AgvInboundParams
    {
      public string Barcode { get; set; }

      public string LocationCode { get; set; }
    }

    [HttpPost("/wcs/agvInbound")]
    public object HandleAgvInbound([FromBody] AgvInboundParams param)
    {
      _logger.Save(Log.From(
        Log.UseInfo(),
        Log.UseMessage("收到 WMS 下发任务请求"),
        Log.UseClass("wms"),
        Log.UseOperation("agv.inbound"),
        Log.UseIndex("1"),
        Log.UseData(param)
      ));

      return new {
        message = "任务已下发"
      };
    }

    public class AgvCallbackParams
    {
      public string Status { get; set; }

      public string TaskCode { get; set; }
    }

    [HttpPost("/wcs/agvCallback")]
    public object HandleAgvCallback([FromBody] AgvCallbackParams param)
    {
      _logger.Save(Log.From(
        Log.UseInfo(),
        Log.UseMessage("收到 WMS 下发任务请求"),
        Log.UseClass("wms"),
        Log.UseOperation("agv.inbound"),
        Log.UseIndex("1"),
        Log.UseData(param)
      ));

      return new {
        message = "状态已接收"
      };
    }

    public class RequestPickingParams
    {
      public string LifterId { get; set; }

      public string Floor { get; set; }

      public string Barcode { get; set; }

      public string TaskId { get; set; }
    }

    [HttpPost("/wms/requestPicking")]
    public async Task<object> HandleRequestPicking([FromBody] RequestPickingParams param)
    {
      await _wms.RequestPicking(
        lifterId: param.LifterId,
        floor: param.Floor,
        barcode: param.Barcode,
        taskId: param.TaskId
      );

      return NotifyResult.FromVoid().Success("请求取货已发送");
    }
  }
}
