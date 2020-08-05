using Renet.Web;
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
      RcsService rcs
    ) {
      _doors = doors;
      _rcs = rcs;
    }

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
