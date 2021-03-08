using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class LifterTaskCommand: BaseController
  {
    private ICapPublisher _cap;

    private Logger _logger;

    private WmsService _wms;

    public LifterTaskCommand(
      ICapPublisher cap,
      WmsService wms,
      Logger logger
    ) {
      _cap = cap;
      _logger = logger;
      _wms = wms;
    }

    public class LifterNotify
    {
      public string Method { get; set; }

      public string LiftCode { get; set; }

      public string Floor { get; set; }

      public long TaskCode { get; set; }

      public string BarCode { get; set; }

      public string Destination { get; set; }

      public string Operator { get; set; } = "wms";
    }

    [Route("/finish")]
    [HttpPost("/lifter-tasks/create")]
    public object LiftersNotify([FromBody] LifterNotify param)
    {
      var message = "";

      if (param.Method == "deliver") {
        message = "收到创建提升机任务指令";

        _cap.Publish(LifterTaskReceived.Message, LifterTaskReceived.From(
          lifterId: param.LiftCode,
          floor: param.Floor,
          taskCode: param.TaskCode.ToString(),
          barcode: param.BarCode,
          destination: param.Destination
        ));
      } else if (param.Method == "pick") {
        message = "收到取货完成指令";

        _cap.Publish(LifterTaskTaken.Message, LifterTaskTaken.From(
          lifterId: param.LiftCode,
          floor: param.Floor
        ));
      } else {
        message = $"指令未识别：{param.Method}";

        _cap.Publish(LifterOperationError.Message, LifterOperationError.From(
          lifterId: param.LiftCode,
          floor: param.Floor,
          operation: "wms.finish",
          message: message
        ));
      }

      var json = System.Text.Json.JsonSerializer.Serialize(param);

      _logger.Save(Log.From(
        Log.UseClass("wms.api"),
        Log.UseOperation("finish"),
        Log.UseIndex("0"),
        Log.UseMessage("收到 WMS 指令"),
        Log.UseData(json),
        Log.UseInfo()
      ));

      var result = new {
        message = message,
        data = param
      };

      return Result.FromObject(result).StatusCode(201);
    }
  }
}
