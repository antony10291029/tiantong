using Renet.Web;
using Microsoft.AspNetCore.Mvc;
using DotNetCore.CAP;

namespace Namei.Wcs.Api
{
  public class DoorController: BaseController
  {
    private DoorServiceManager _doors;

    private ICapPublisher _cap;

    private RcsService _rcs;

    public DoorController(
      DoorServiceManager doors,
      ICapPublisher cap,
      RcsService rcs
    ) {
      _cap = cap;
      _doors = doors;
      _rcs = rcs;
    }

    public class DoorParams
    {
      public int DoorId { get; set; }
    }

    [HttpPost]
    public object HandleRequestOpen([FromBody] DoorParams param)
    {
      _cap.Publish(DoorRequestingOpenEvent.Message, new DoorRequestingOpenEvent(param.DoorId));
      var code = _doors.Get(param.DoorId).IsAvaliable() ? 0 : 1;

      return new {
        code = code,
        message = code == 0 ? "成功" : "失败"
      };
    }

    [HttpPost]
    public object HandleRequestClose([FromBody] DoorParams param)
    {
      _cap.Publish(DoorRequestingCloseEvent.Message, new DoorRequestingCloseEvent(param.DoorId));

      return new {
        code = 0,
        message = "成功"
      };
    }

    [HttpPost]
    public void HandleDoorOpened([FromBody] DoorParams param)
    {
      _cap.Publish(DoorOpenedEvent.Message, new DoorOpenedEvent(param.DoorId));
    }

    [HttpPost]
    public void HandleDoorClosed([FromBody] DoorParams param)
    {
      _cap.Publish(DoorClosedEvent.Message, new DoorClosedEvent(param.DoorId));
    }

    // events

    [CapSubscribe(DoorRequestingOpenEvent.Message)]
    public void HandleRequestingOpen(DoorRequestingOpenEvent param)
    {
      _doors.Get(param.DoorId).Open();
    }

    [CapSubscribe(DoorRequestingCloseEvent.Message)]
    public void HandleRequestingClose(DoorRequestingCloseEvent param)
    {
      _doors.Get(param.DoorId).Close();
    }

    [CapSubscribe(DoorOpenedEvent.Message)]
    public void HandleDoorOpened(DoorOpenedEvent param)
    {
      _rcs.NotifyDoorOpened(param.DoorId);
    }

    [CapSubscribe(DoorClosedEvent.Message)]
    public void HandleDoorClosed(DoorClosedEvent param)
    {
      _rcs.NotifyDoorClosed(param.DoorId);
    }
  }
}
