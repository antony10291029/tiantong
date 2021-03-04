using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

    private void Info(string doorId, string operation, string message)
    {
      var doorType = AutomatedDoor.Enumerate().Contains(doorId) ? DoorType.Automatic : DoorType.Crash;
      var doorTypeText = doorType == DoorType.Automatic ? "自动门" : "防撞门";

      message = $"{doorId}号{doorTypeText}，{message}";

      var log = Log.From(LogLevel.Info, "wcs.door", operation, doorId, message, "");

      _logger.Save(log);
    }

    [CapSubscribe(DoorTaskRequestOpenEvent.Message, Group = Group)]
    public void RequestedOpen(DoorTaskRequestOpenEvent param)
      => Info(param.DoorId, "requested.open", $"正在请求开门, 任务 Id: {param.TaskId}");

    [CapSubscribe(DoorTaskRequestCloseEvent.Message, Group = Group)]
    public void RequestedClose(DoorTaskRequestCloseEvent param)
      => Info(param.DoorId, "request.close", $"正在请求关门, 任务 Id: {param.TaskId}");

    [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
    public void Opened(DoorOpenedEvent param)
      => Info(param.DoorId, "opened", "门已打开");

    [CapSubscribe(DoorClosedEvent.Message, Group = Group)]
    public void Closed(DoorClosedEvent param)
      => Info(param.DoorId, "closed", "门已关闭");

    [CapSubscribe(DoorTaskHandleEvent.Message, Group = Group)]
    public void Handle(DoorTaskHandleEvent param)
      => Info(param.DoorId, "handle", "正在处理开门请求");

    [CapSubscribe(RcsNotifiedEvent.Message, Group = Group)]
    public void RcsNotified(RcsNotifiedEvent param)
      => Info(param.DoorId, "rcs.notified", $"已通知 RCS 操作完成，action: {param.Action}, uuid: {param.Uuid}, result: {param.Result}");

    [CapSubscribe(RcsNotifiedFailedEvent.Message, Group = Group)]
    public void RcsNotifiedFailed(RcsNotifiedFailedEvent param)
      => Info(param.DoorId, "rcs.notified.failed", $"通知 RCS 操作完成失败, action: {param.Action}, uuid: {param.Uuid}, result: {param.Result}");

    [CapSubscribe(RcsCommandReceivedEvent.Message, Group = Group)]
    public void RcsCommandReceived(RcsCommandReceivedEvent param)
      => Info(param.DoorId, "rcs.received", $"收到 RCS 任务, action: {param.Action}, uuid: {param.Uuid}");
  }
}
