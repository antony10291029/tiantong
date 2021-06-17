using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Namei.Wcs.Api;
using System;
using System.Text.Json.Serialization;

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

    private void HandleTaken(string lifterId, string floor, string from)
    {
      _logger.LogInfo(
        "taken", lifterId, floor, $"收到 [{from}] 取货完成指令",
        new { lifterId, floor, from }
      );

      try {
        _cap.Publish(LifterTaskFinished.Message, LifterTaskFinished.From(floor, lifterId));
        _service.HandleTaken(lifterId, floor);
        _logger.LogSuccess("taken", lifterId, floor, "取货完成指令已处理");
      } catch (Exception e) {
        _logger.LogError("taken", lifterId, floor, $"取货完成指令处理失败: {e.Message}");

        throw;
      }
    }

    private void HandleClear(string lifterId, string floor, string from)
    {
      _logger.LogInfo(
        "clear", lifterId, floor, $"收到 [{from}] 清除信号指令",
        new { lifterId, floor, from }
      );

      try {
        _service.HandleClear(lifterId, floor);
        _logger.LogSuccess("clear", lifterId, floor, "清除信号指令已处理");
      } catch (Exception e) {
        _logger.LogError("clear", lifterId, floor, $"清除信号指令处理失败：{e.Message}");

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

        HandleTaken(param.LiftCode, param.Floor, LifterTaskFrom.Wms);
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
      HandleTaken(param.LifterId, param.Floor, LifterTaskFrom.Manual);

      return NotifyResult.FromVoid().Success("取货完成指令已处理");
    }

    public record PlcStateParams
    {
      [JsonPropertyName("floor")]
      public string Floor { get; set; }

      [JsonPropertyName("Value")]
      public string Value { get; set; }

      [JsonPropertyName("old_value")]
      public string OldValue { get; set; }
    }

    [HttpPost("reformed-lifters/conveyor/changed")]
    public object ReformedLifterChange([FromBody] PlcStateParams param)
    {
      var message = "输送线状态无需处理";

      if (param.Value == null || param.OldValue == null) {
        return NotifyResult.FromVoid().Success(message);
      }

      var isSpare = FirstLifterCommand.IsSpare(param.Value, param.OldValue);
      var isScanned = FirstLifterCommand.IsTaskScanned(param.Value, param.OldValue);
      var isImportedAllowed = FirstLifterCommand.IsImportAllowed(param.Value, param.OldValue);
      var isRequestingPickup = FirstLifterCommand.IsRequestingPickup(param.Value, param.OldValue);

      if (isSpare) {
        message = "信号清除指令已处理";
        HandleClear(LifterCode.First, param.Floor, LifterTaskFrom.Plc);
      } else if (isScanned) {
        message = "扫码指令已处理";
        HandleScanned(LifterCode.First, param.Floor, LifterTaskFrom.Plc);
      } else if (isRequestingPickup) {
        message = "取货指令已处理";
        HandleExported(LifterCode.First, param.Floor, LifterTaskFrom.Plc);
      }

      if (isImportedAllowed || isRequestingPickup) {
        var doorId = CrashDoor.GetDoorIdFromLifter(param.Floor, "1");

        _cap.Publish(WcsDoorEvent.Opened, WcsDoorEvent.From(doorId));
      }

      return NotifyResult.FromVoid().Success(message);
    }

    public class LifterPlcStateParams
    {
      [JsonPropertyName("lifter_id")]
      public string LifterId { get; set; }

      [JsonPropertyName("floor")]
      public string Floor { get; set; }

      [JsonPropertyName("value")]
      public string Value { get; set; }

      [JsonPropertyName("old_value")]
      public string OldValue { get; set; }
    }

    [HttpPost("/standard-lifters/scanned")]
    public object LifterTaskScanned([FromBody] LifterPlcStateParams param)
    {
      var message = "指令未识别";

      if (param.Value == "1") {
        message = "扫码状态已处理";
        HandleScanned(param.LifterId, param.Floor, LifterTaskFrom.Plc);
      }

      return NotifyResult.FromVoid().Success(message);
    }

    [HttpPost("/standard-lifters/exported")]
    public object HandleLifterTaskExported([FromBody] LifterPlcStateParams param)
    {
      var message = "指令未识别";

      if (param.Value == "3") {
        message = "请求取货指令已处理";
        HandleExported(param.LifterId, param.Floor, LifterTaskFrom.Plc);
      }

      if (param.Value == "2" || param.Value == "3") {
        var doorId =  CrashDoor.GetDoorIdFromLifter(param.Floor, param.LifterId);

        _cap.Publish(WcsDoorEvent.Opened, WcsDoorEvent.From(doorId));
      }

      return NotifyResult.FromVoid().Success(message);
    }

    [HttpPost("/standard-lifters/conveyor/changed")]
    public object StandardLifterConveyorChanged([FromBody] LifterPlcStateParams param)
    {
      var message = "状态无需处理";

      if (param.Value == "1") {
        message = "数据已清空";
        HandleClear(param.LifterId, param.Floor, LifterTaskFrom.Plc);
      }

      return NotifyResult.FromVoid().Success(message);
    }
  }
}
