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

    [CapSubscribe(LifterTaskImportedEvent.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
      => _logger.FromLifter(
        operation: "imported",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到放货完成指令"
      );

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
      => _logger.FromLifter(
        operation: "scanned",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到扫码完成指令"
      );

    [CapSubscribe(LifterTaskQueriedEvent.Message, Group = Group)]
    public void HandleTaskQueried(LifterTaskQueriedEvent param)
      => _logger.FromLifter(
        operation: "queried",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"任务查询完毕，托盘码: {param.Barcode}, 目的楼层: {param.Destination}"
      );

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
      => _logger.FromLifter(
        operation: "exported",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"设备请求取货中，正在判断是否通知 WMS 进行取货"
      );

    [CapSubscribe(LifterTaskPickingEvent.Message, Group = Group)]
    public void HandleTaskPicking(LifterTaskPickingEvent param)
      => _logger.FromLifter(
        operation: "picking",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"已通知 WMS 进行取货，托盘码: {param.Barcode}，任务id: {param.TaskId}"
      );

    [CapSubscribe(LifterTaskTaken.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTaken param)
      => _logger.FromLifter(
        operation: "taken",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到 WMS 取货完成信号，{param.Barcode}"
      );

    [CapSubscribe(LifterTaskQueryFailedEvent.Message, Group = Group)]
    public void HandleQueriedFailed(LifterTaskQueryFailedEvent param)
      => _logger.FromLifter(
        operation: "queried.failed",
        lifterId: param.LifterId, 
        floor: param.Floor,
        message: $"托盘任务信息查询失败，托盘码: {param.Barcode}"
      );

    [CapSubscribe(LifterTaskPickingFailedEvent.Message, Group = Group)]
    public void HandleTaskPickingFailed(LifterTaskPickingFailedEvent param)
      => _logger.FromLifter(
        operation: "picking.failed",
        lifterId: param.LifterId, 
        floor: param.Floor,
        message: $"通知 WMS 取货失败，托盘码: {param.Barcode}, 错误信息: {param.ErrorMessage}"
      );
  }
}
