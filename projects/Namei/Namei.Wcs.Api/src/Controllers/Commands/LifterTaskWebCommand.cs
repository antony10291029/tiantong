using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class LifterTaskCommand: BaseController
  {
    private ICapPublisher _cap;

    private LifterLogger _logger;

    private WmsService _wms;

    public LifterTaskCommand(
      ICapPublisher cap,
      WmsService wms,
      LifterLogger logger
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

      public string Operator { get; set; } = "WMS";
    }

    [Route("/finish")]
    [HttpPost("/lifter-tasks/create")]
    public object LiftersNotify([FromBody] LifterNotify param)
    {
      var message = "";

      if (param.Method == "deliver") {
        message = "收到创建提升机任务指令";

        _cap.Publish(LifterTaskImported.Message, LifterTaskImported.From(
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

      _logger.FromLifter(
        operation: "command.finish",
        lifterId: param.LiftCode,
        floor: param.Floor,
        message: ($"收到 {param.Operator} 放、取货指令"),
        data: System.Text.Json.JsonSerializer.Serialize(param),
        useLevel: Log.UseInfo()
      );

      var result = new {
        message = message,
        data = param
      };

      return Result.FromObject(result).StatusCode(201);
    }
  }
}
