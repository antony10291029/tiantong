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

    private void Info(string doorId, string operation, string message, object data = null)
    {
      var doorType = AutomatedDoor.Enumerate().Contains(doorId) ? DoorType.Automatic : DoorType.Crash;
      var doorTypeText = doorType == DoorType.Automatic ? "自动门" : "防撞门";

      message = $"{doorTypeText}: {doorId}, {message}";

      var log = Log.From(
        Log.UseInfo(),
        Log.UseClass("wcs.door"),
        Log.UseOperation(operation),
        Log.UseIndex(doorId),
        Log.UseMessage(message),
        Log.UseData(data)
      );

      _logger.Save(log);
    }

    [CapSubscribe(RcsDoorEvent.Request, Group = Group)]
    public void RequestedOpen(RcsDoorEvent param)
      => Info(
        doorId: param.DoorId,
        operation: "requested.open",
        message: $"正在请求开门",
        data: param
      );

    [CapSubscribe(RcsDoorEvent.Leave, Group = Group)]
    public void RequestedClose(RcsDoorEvent param)
      => Info(
        doorId: param.DoorId,
        operation: "request.close",
        message: $"AGC 已通过，正在请求关门",
        data: param
      );

    [CapSubscribe(RcsDoorEvent.Retry, Group = Group)]
    public void Retry(RcsDoorEvent param)
      => Info(
        doorId: param.DoorId,
        operation: "retry",
        message: "检测到 agc 未离开封锁点，正在重新发送请求....",
        data: param
      );

    [CapSubscribe(WcsDoorEvent.Opened, Group = Group)]
    public void Opened(WcsDoorEvent param)
      => Info(
        doorId: param.DoorId,
        operation: "opened",
        message: "门已打开"
      );

    [CapSubscribe(WcsDoorEvent.Closed, Group = Group)]
    public void Closed(WcsDoorEvent param)
      => Info(
        doorId: param.DoorId,
        operation: "closed",
        message: "门已关闭"
      );
  }
}
