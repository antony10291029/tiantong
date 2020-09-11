using DotNetCore.CAP;
using Renet.Web;

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

    private void Info(string lifterId, string floor, string message)
      => _logger.Info($"lifter.{lifterId}", $"{lifterId}号梯 - {floor}楼，{message}");

    [CapSubscribe(LifterTaskImportedEvent.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
      => Info(param.LifterId, param.Floor, param.TaskId == null
        ? $"手动触发放货完成信号，正在将信号转发至设备"
        : $"收到 WMS 放货完成信号，任务id: {param.TaskId}，正在将信号转发至设备"
      );

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
      => Info(param.LifterId, param.Floor, $"收到提升机扫码完成信号，正在向 WMS 查询任务信息");

    [CapSubscribe(LifterTaskQueriedEvent.Message, Group = Group)]
    public void HandleTaskQueried(LifterTaskQueriedEvent param)
      => Info(param.LifterId, param.Floor, $"任务查询完毕，托盘码: {param.Barcode}, 目的楼层: {param.Destination}");

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
      => Info(param.LifterId, param.Floor, $"设备请求取货中，正在向 WMS 查询任务信息");

    [CapSubscribe(LifterTaskPickingEvent.Message, Group = Group)]
    public void HandleTaskPicking(LifterTaskPickingEvent param)
      => Info(param.LifterId, param.Floor, $"已通知 WMS 进行取货，托盘码: {param.Barcode}，任务id: {param.TaskId}");

    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
      => Info(param.LifterId, param.Floor, param.TaskId == null
        ? $"手动触发取货完成信号，正在将信号转发至设备"
        : $"收到 WMS 反馈，AGC 取货完毕");
  }
}
