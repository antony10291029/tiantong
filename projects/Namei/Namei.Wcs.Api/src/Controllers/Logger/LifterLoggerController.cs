using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Namei.Wcs.Api
{
  public class LifterLoggerController: BaseController
  {
    private const string Group = "logger";

    private LifterLogger _logger;

    public LifterLoggerController(LifterLogger logger)
    {
      _logger = logger;
    }

    [CapSubscribe(LifterTaskImported.Message, Group = Group)]
    public void HandleTaskReceived(LifterTaskImported param)
    {
      var from = param.Barcode is null ? "WCS": "WMS";
      var detail = param.Barcode is null ? ""
        : $", 托盘码: {param.Barcode}, 目的楼层: {param.Destination}, TaskCode: {param.TaskCode}";

       _logger.FromLifter(
        operation: "received",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到 {from} 提升机任务{detail}"
      );
    }

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
      => _logger.FromLifter(
        operation: "scanned",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到扫码完成指令"
      );
  }
}
