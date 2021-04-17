using System.Reflection.Metadata.Ecma335;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class LifterEventController: BaseController
  {
    public const string Group = "lifter";

    private ICapPublisher _cap;

    private LifterLogger _logger;

    private LifterServiceManager _lifters;

    private WmsService _wms;

    public LifterEventController(
      ICapPublisher cap,
      LifterLogger logger,
      LifterServiceManager lifters,
      WmsService wms
    ) {
      _cap = cap;
      _logger = logger;
      _lifters = lifters;
      _wms = wms;
    }

    [CapSubscribe(LifterTaskImported.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImported param)
    {
      var lifter = _lifters.Get(param.LifterId);

      try {
        if (param.LifterId == "1") {
          lifter.Import(floor: param.Floor);
        } else {
          lifter.Import(
            floor: param.Floor,
            destination: param.Destination,
            barcode: param.Barcode
          );
        }

        _logger.FromLifter(
          operation: "command.imported",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: "放货完成执行完毕",
          useLevel: Log.UseSuccess()
        );
      } catch (Exception e) {
        _logger.FromLifter(
          operation: "command.imported",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: "放货完成执行失败",
          useLevel: Log.UseDanger(),
          data: e.Message
        );
      }
    }

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
    {
      _logger.FromLifter(
        operation: "scanned",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到扫码完成指令"
      );

      var lifter = _lifters.Get(param.LifterId);

      if (lifter.GetDestination(param.Floor) != "0") {
        return;
      }

      var barcode = lifter.GetPalletCode(param.Floor);

      try {
        var info = _wms.GetPalletInfo(barcode);
        var taskCode = info.TaskId;
        var destination = info.Destination;

        _lifters.Get(param.LifterId).SetDestination(param.Floor, destination);

        _logger.FromLifter(
          operation: "task.search",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"托盘任务查询成功, 目的楼层: {destination}, TaskCode {taskCode}",
          useLevel: Log.UseSuccess(),
          data: JsonSerializer.Serialize(info)
        );
      } catch (Exception e) {
        _logger.FromLifter(
          operation: "task.search",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"托盘任务查询失败",
          useLevel: Log.UseDanger(),
          data: e.Message
        );
      }
    }

    [CapSubscribe(LifterTaskExported.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExported param)
    {
      _logger.FromLifter(
        operation: "exported",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"提升机请求取货中，正在判断是否通知 WMS 进行取货",
        useLevel: Log.UseInfo()
      );

      var now = DateTime.Now;
      var lifter = _lifters.Get(param.LifterId);

      // 对于同一台提升机的同一楼层，10 秒内只接收一次取货请求
      if (lifter.ExportedAt[param.Floor].AddSeconds(10) > now) {
        return;
      }

      lifter.ExportedAt[param.Floor] = now;

      var barcode = lifter.GetPalletCode(param.Floor);

      if (barcode.Length != 6 || !int.TryParse(barcode, out _)) {
        _logger.FromLifter(
          operation: "notify.wms.pick",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"请求取货失败，托盘码异常: {barcode}",
          useLevel: Log.UseDanger()
        );

        return;
      }

      try {
        var taskId = _wms.GetPalletInfo(barcode).TaskId;
        var result =_wms.RequestPicking(param.LifterId, param.Floor, barcode, taskId);

        _logger.FromLifter(
          operation: "notify.wms.pick",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"通知 WMS 取货成功, 托盘码: {barcode}",
          data: result,
          useLevel: Log.UseSuccess()
        );
      } catch (Exception e) {
        _logger.FromLifter(
          operation: "notify.wms.pick",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"取货指令处理失败，托盘码: {barcode}",
          useLevel: Log.UseDanger(),
          data: e.Message
        );
      }
    }

    [CapSubscribe(LifterTaskTaken.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTaken param)
    {
      _logger.FromLifter(
        operation: "taken",
        lifterId: param.LifterId,
        floor: param.Floor,
        message: $"收到取货完成指令",
        useLevel: Log.UseInfo()
      );

      try {
        var lifter = _lifters.Get(param.LifterId);

        lifter.SetPickuped(param.Floor, true);

        Task.Delay(2000).ContinueWith(async _ => {
          await _;
          lifter.SetPickuped(param.Floor, false);
        });

        _logger.FromLifter(
          operation: "plc.taken",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"取货完成指令处理完成",
          useLevel: Log.UseSuccess()
        );
      } catch (Exception e) {
        _logger.FromLifter(
          operation: "plc.taken",
          lifterId: param.LifterId,
          floor: param.Floor,
          message: $"取货完成指令处理失败",
          useLevel: Log.UseDanger(),
          data: e.Message
        );
      }
    }
  }
}
