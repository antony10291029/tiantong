using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Api;
using System;

namespace Namei.Wcs.Aggregates
{
  public class LifterController
  {
    private readonly ICapPublisher _cap;

    private readonly ILifterLogger _logger;

    private readonly ILifterService _service;

    public LifterController(
      ICapPublisher cap,
      ILifterLogger logger,
      ILifterService service
    ) {
      _cap = cap;
      _logger = logger;
      _service = service;
    }

    //

    private void HandleImported(
      string lifterId, string floor, string barcode,
      string destination, string data, string from
    ) {
      _logger.LogInfo(
        "imported", lifterId, floor, $"收到 [{from}] 放货完成指令",
        new { lifterId, floor, barcode, destination, data, from }
      );

      try {
        _cap.Publish(LifterTaskFinished.Message, LifterTaskFinished.From(floor, lifterId));
        _service.HandleImported(lifterId, floor, barcode, destination, data, from);
        _logger.LogSuccess("imported", lifterId, floor, "放货完成指令已处理");
      } catch (Exception e) {
        _logger.LogError("imported", lifterId, floor, $"放货完成指令处理失败: {e.Message}");

        throw;
      }
    }

    private void HandleScanned(string lifterId, string floor, string from)
    {
      _logger.LogInfo(
        "scanned", lifterId, floor, $"收到 [{from}] 扫码完成指令",
        new { lifterId, floor, from }
      );

      try {
        _service.HandleScanned(lifterId, floor);
        _logger.LogSuccess("scanned", lifterId, floor, "扫码完成指令已处理");
      } catch (Exception e) {
        _logger.LogError("scanned", lifterId, floor, $"扫码完成指令处理失败: {e.Message}");

        throw;
      }
    }

    private void HandleExported(string lifterId, string floor, string from)
    {
      _logger.LogInfo(
        "exported", lifterId, floor, $"收到 [{from}] 请求取货指令",
        new { lifterId, floor, from }
      );

      try {
        _service.HandleExported(lifterId, floor);
        _logger.LogSuccess("exported", lifterId, floor, "请求取货指令已处理");
      } catch (Exception e) {
        _logger.LogError("exported", lifterId, floor, $"请求取货指令处理失败: {e.Message}");

        throw;
      }
    }

    private void HandleTaken(string lifterId, string floor, string barcode, string from)
    {
      _logger.LogInfo(
        "taken", lifterId, floor, $"收到 [{from}] 取货完成指令",
        new { lifterId, floor, barcode, from }
      );

      try {
        _cap.Publish(LifterTaskFinished.Message, LifterTaskFinished.From(floor, lifterId));
        _service.HandleTaken(lifterId, floor, barcode);
        _logger.LogSuccess("taken", lifterId, floor, "取货完成指令已处理");
      } catch (Exception e) {
        _logger.LogError("taken", lifterId, floor, $"取货完成指令处理失败: {e.Message}");

        throw;
      }
    }

    //

    public record FinishFromWmsParams
    {
      public string Method { get; set; }

      public string LiftCode { get; set; }

      public string Floor { get; set; }

      public long TaskCode { get; set; }

      public string BarCode { get; set; }

      public string Destination { get; set; }
    }

    [HttpPost("/finish")]
    public object FinishFromWms([FromBody] FinishFromWmsParams param)
    {
      var message = $"放取货指令未识别: {param.Method}";

      if (param.Method == "deliver") {
        message = "放货指令已处理";

        HandleImported(
          param.LiftCode, param.Floor, param.BarCode,
          param.Destination, param.TaskCode.ToString(), LifterTaskFrom.Wms
        );
      } else if (param.Method == "pick") {
        message = "取货指令已处理";

        HandleTaken(param.LiftCode, param.Floor, param.BarCode, LifterTaskFrom.Wms);
      }

      return NotifyResult.FromVoid().Success(message);
    }

    public record ImportParams
    {
      public string LifterId { get; set; }

      public string Floor { get; set; }

      public string Barcode { get; set; }

      public string Destination { get; set; }

      public string Data { get; set; }

      public string From { get; set; }
    }

    [HttpPost("/lifters/imported")]
    public object Import([FromBody] ImportParams param)
    {
      HandleImported(
        param.LifterId, param.Floor, param.Barcode,
        param.Destination, param.Data, param.From ?? LifterTaskFrom.Manual
      );

      return NotifyResult.FromVoid().Success("放货完成指令已处理");
    }

    public record ScanParams
    {
      public string LifterId { get; set; }

      public string Floor { get; set; }
    }

    [HttpPost("/lifters/scanned")]
    public object Scan([FromBody] ScanParams param)
    {
      HandleScanned(param.LifterId, param.Floor, LifterTaskFrom.Manual);

      return NotifyResult.FromVoid().Success("扫码完成指令已处理");
    }

    public record ExportedParams
    {
      public string LifterId { get; set; }

      public string Floor { get; set; }
    }

    [HttpPost("/lifters/exported")]
    public object Exported([FromBody] ExportedParams param)
    {
      HandleExported(param.LifterId, param.Floor, LifterTaskFrom.Manual);

      return NotifyResult.FromVoid().Success("请求取货指令已处理");
    }

    public record TakenParams
    {
      public string LifterId { get; set; }

      public string Floor { get; set; }
    }

    [HttpPost("/lifters/taken")]
    public object Taken([FromBody] TakenParams param)
    {
      HandleTaken(param.LifterId, param.Floor, "",  LifterTaskFrom.Manual);

      return NotifyResult.FromVoid().Success("取货完成指令已处理");
    }
  }
}
