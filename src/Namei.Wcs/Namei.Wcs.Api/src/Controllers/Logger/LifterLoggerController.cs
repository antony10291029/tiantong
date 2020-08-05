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
      => _logger.Info($"lifter.{lifterId}", $"{lifterId}号提升机 - {floor}楼，{message}");

    [CapSubscribe(LifterDoorRequestedOpenEvent.Message, Group = Group)]
    public void HandleDoorRequested(LifterDoorRequestedOpenEvent param)
      => Info(param.LifterId, param.Floor, "正在请求开门");

    [CapSubscribe(LifterDoorRequestedCloseEvent.Message, Group = Group)]
    public void HandleDoorRequested(LifterDoorRequestedCloseEvent param)
      => Info(param.LifterId, param.Floor, "正在请求关门");

    [CapSubscribe(LifterTaskImportedEvent.Message, Group = Group)]
    public void HandleTaskImported(LifterTaskImportedEvent param)
      => Info(param.LifterId, param.Floor, "放货已完成，正在启动输送机");

    [CapSubscribe(LifterTaskScannedEvent.Message, Group = Group)]
    public void HandleTaskScanned(LifterTaskScannedEvent param)
      => Info(param.LifterId, param.Floor, "扫码已完成，正在读取托盘码");

    [CapSubscribe(LifterTaskStartedEvent.Message, Group = Group)]
    public void HandleTaskStarted(LifterTaskStartedEvent param)
      => Info(param.LifterId, param.Floor, $"任务设置完毕[{param.Destination} 楼]，正在等待执行");

    [CapSubscribe(LifterTaskExportedEvent.Message, Group = Group)]
    public void HandleTaskExported(LifterTaskExportedEvent param)
      => Info(param.LifterId, param.Floor, $"任务已完成，正在请求取货");

    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleTaskTaken(LifterTaskTakenEvent param)
      => Info(param.LifterId, param.Floor, $"取货完毕");
  }
}
