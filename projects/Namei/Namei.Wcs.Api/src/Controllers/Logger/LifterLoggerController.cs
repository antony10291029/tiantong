using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class LifterLoggerController: BaseController
  {
    private const string Group = "logger";

    private Logger _logger;

    public LifterLoggerController(Logger logger)
    {
      _logger = logger;
    }

    private void Info(string operation, string lifterId, string floor, string message, string data = "")
    {
      message = $"{lifterId}号梯 - {floor}楼，{message}";
      _logger.Save(Log.From(LogLevel.Info, "wcs.lifter", operation, lifterId, message, data));
    }

    [CapSubscribe(LifterTaskImportedEvent.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
      => Info("imported", param.LifterId, param.Floor, !param.IsFromWms
        ? $"手动触发放货完成信号，正在将信号转发至设备"
        : $"收到 WMS 放货完成信号，正在将信号转发至设备，任务编号: {param.TaskCode}, 条码：{param.BarCode}, 目的楼层: {param.Destination}"
      );

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
      => Info("scanned", param.LifterId, param.Floor, $"收到提升机扫码完成信号，正在向 WMS 查询任务信息");

    [CapSubscribe(LifterTaskQueriedEvent.Message, Group = Group)]
    public void HandleTaskQueried(LifterTaskQueriedEvent param)
      => Info("queried", param.LifterId, param.Floor, $"任务查询完毕，托盘码: {param.Barcode}, 目的楼层: {param.Destination}");

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
      => Info("exported", param.LifterId, param.Floor, $"设备请求取货中，正在判断是否通知 WMS 进行取货");

    [CapSubscribe(LifterTaskPickingEvent.Message, Group = Group)]
    public void HandleTaskPicking(LifterTaskPickingEvent param)
      => Info("picking", param.LifterId, param.Floor, $"已通知 WMS 进行取货，托盘码: {param.Barcode}，任务id: {param.TaskId}");

    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
      => Info("taken", param.LifterId, param.Floor, !param.IsFromWms
        ? $"手动触发取货完成信号，正在将信号转发至设备"
        : $"收到 WMS 取货完成信号，正在将信号转发至设备，任务编号: {param.TaskId}");

    [CapSubscribe(LifterTaskQueryFailedEvent.Message, Group = Group)]
    public void HandleQueriedFailed(LifterTaskQueryFailedEvent param)
      => Info("queried.failed", param.LifterId, param.Floor, $"托盘任务信息查询失败，托盘码: {param.Barcode}");

    [CapSubscribe(LifterTaskPickingFailedEvent.Message, Group = Group)]
    public void HandleTaskPickingFailed(LifterTaskPickingFailedEvent param)
      => Info("picking.failed", param.LifterId, param.Floor, $"通知 WMS 取货失败，托盘码: {param.Barcode}, 错误信息: {param.ErrorMessage}");

    [CapSubscribe(LifterOperationError.Message, Group = Group)]
    public void HandleLifterTaskException(LifterOperationError param)
      => Info(param.Operation, param.LifterId, param.Floor, param.Error);
  }
}
