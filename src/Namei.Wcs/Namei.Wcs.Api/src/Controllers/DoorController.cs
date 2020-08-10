using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class DoorController: BaseController
  {
    const string Group = "command";

    private DomainContext _domain;

    private DoorServiceManager _doors;

    private RcsService _rcs;

    public DoorController(
      DomainContext domain,
      DoorServiceManager doors,
      RcsService rcs
    ) {
      _domain = domain;
      _doors = doors;
      _rcs = rcs;
    }

    [HttpPost]
    [Route("/doors/states")]
    public object GetStates()
    {
      return _doors.All().Select(door => new {
        id = door.Id,
        isClosed = door.IsClosed()
      });
    }

    // events

    [CapSubscribe(DoorRequestedOpenEvent.Message, Group = Group)]
    public void HandleRequestingOpen(DoorRequestedOpenEvent param)
    {
      _doors.Get(param.DoorId).Open();
    }

    [CapSubscribe(DoorRequestedCloseEvent.Message, Group = Group)]
    public void HandleRequestingClose(DoorRequestedCloseEvent param)
    {
      _doors.Get(param.DoorId).Close();
    }

    [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
    public void HandleDoorOpened(DoorOpenedEvent param)
    {
      _rcs.NotifyDoorOpened(param.DoorId);
    }

    [CapSubscribe(DoorClosedEvent.Message, Group = Group)]
    public void HandleDoorClosed(DoorClosedEvent param)
    {
      _rcs.NotifyDoorClosed(param.DoorId);
    }
  }
}
