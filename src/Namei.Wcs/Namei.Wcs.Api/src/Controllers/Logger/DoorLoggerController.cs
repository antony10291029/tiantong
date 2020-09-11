using System.Linq;
using DotNetCore.CAP;
using Renet.Web;

namespace Namei.Wcs.Api
{
  public class DoorLoggerController: BaseController
  {
    private const string Group = "logger";

    private Logger _logger;

    public DoorLoggerController(Logger logger)
    {
      _logger = logger;
    }

    private void Info(string doorId, string message)
    {
      var doorType = AutomatedDoor.Enumerate().Contains(doorId) ? "自动门" : "防撞门";

      _logger.Info($"door.{doorId}", $"{doorId}号{doorType}，{message}");
    }

    [CapSubscribe(DoorTaskRequestOpenEvent.Message, Group = Group)]
    public void RequestedOpen(DoorTaskRequestOpenEvent param)
      => Info(param.DoorId, $"正在请求开门, 任务 Id: {param.TaskId}");

    [CapSubscribe(DoorTaskRequestCloseEvent.Message, Group = Group)]
    public void RequestedClose(DoorTaskRequestCloseEvent param)
      => Info(param.DoorId, $"正在请求关门, 任务 Id: {param.TaskId}");

    [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
    public void Opened(DoorOpenedEvent param)
      => Info(param.DoorId, "门已打开");

    [CapSubscribe(DoorClosedEvent.Message, Group = Group)]
    public void Closed(DoorClosedEvent param)
      => Info(param.DoorId, "门已关闭");

    [CapSubscribe(DoorTaskHandleEvent.Message, Group = Group)]
    public void Handle(DoorTaskHandleEvent param)
      => Info(param.DoorId, "正在处理开门请求");

    [CapSubscribe(RcsNotifiedEvent.Message, Group = Group)]
    public void RcsNotified(RcsNotifiedEvent param)
      => Info(param.DoorId, $"已通知 RCS 操作完成，action: {param.Action}, uuid: {param.Uuid}, result: {param.Result}");

    [CapSubscribe(RcsNotifiedFailedEvent.Message, Group = Group)]
    public void RcsNotifiedFailed(RcsNotifiedFailedEvent param)
      => Info(param.DoorId, $"通知 RCS 操作完成失败, action: {param.Action}, uuid: {param.Uuid}, result: {param.Result}");

    [CapSubscribe(RcsCommandReceivedEvent.Message, Group = Group)]
    public void RcsCommandReceived(RcsCommandReceivedEvent param)
      => Info(param.DoorId, $"收到 RCS 任务, action: {param.Action}, uuid: {param.Uuid}");
  }
}
