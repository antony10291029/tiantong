using Renet.Web;
using DotNetCore.CAP;

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
      => Info(param.LifterId, param.Floor, "放货已完成，开始运输任务");

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
      => Info(param.LifterId, param.Floor, "扫码已完成，正在读取托盘码");

    [CapSubscribe(LifterTaskQueriedEvent.Message, Group = Group)]
    public void HandleTaskQueried(LifterTaskQueriedEvent param)
      => Info(param.LifterId, param.Floor, $"任务查询完毕，托盘码: {param.Barcode}, 目的楼层: {param.Destination}");

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
      => Info(param.LifterId, param.Floor, $"任务已完成，正在请求取货");

    [CapSubscribe(LifterTaskPickingEvent.Message, Group = Group)]
    public void HandleTaskPicking(LifterTaskPickingEvent param)
      => Info(param.LifterId, param.Floor, $"已通知 WMS 请求取货，托盘码: {param.Barcode}");

    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
      => Info(param.LifterId, param.Floor, $"取货完毕");
  }
}
