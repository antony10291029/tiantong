using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class WmsCommandController: BaseController
  {
    private Logger _logger;
    
    public WmsCommandController(Logger logger)
    {
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
  }
}
