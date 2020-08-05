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
      => _logger.Info($"door.{doorId}", $"{doorId}号自动门，{message}");

    [CapSubscribe(DoorRequestedOpenEvent.Message, Group = Group)]
    public void RequestedOpen(DoorRequestedOpenEvent param)
      => Info(param.DoorId, "正在请求开门");

    [CapSubscribe(DoorRequestedCloseEvent.Message, Group = Group)]
    public void RequestedClose(DoorRequestedCloseEvent param)
      => Info(param.DoorId, "正在请求关门");

    [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
    public void Opened(DoorOpenedEvent param)
      => Info(param.DoorId, "门已打开");

    [CapSubscribe(DoorClosedEvent.Message, Group = Group)]
    public void Closed(DoorClosedEvent param)
      => Info(param.DoorId, "门已关闭");
  }
}
