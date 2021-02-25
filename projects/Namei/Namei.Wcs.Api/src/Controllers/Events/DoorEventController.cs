using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Namei.Wcs.Api
{
  public class DoorEventController: BaseController
  {
    const string Group = "command";

    private DoorServiceManager _doors;

    public DoorEventController(
      DomainContext domain,
      DoorServiceManager doors,
      DoorTaskManager taskManager,
      RcsService rcs
    ) {
      _doors = doors;
    }

    [CapSubscribe(DoorOpenedEvent.Message, Group = Group)]
    public void HandleDoorOpened(DoorOpenedEvent param)
    {
      _doors.Get(param.DoorId).OnOpened();
    }

    [CapSubscribe(DoorClosedEvent.Message, Group = Group)]
    public void HandleDoorClosed(DoorClosedEvent param)
    {
      _doors.Get(param.DoorId).OnClosed();
    }

    // 放货完成
    [CapSubscribe(LifterTaskImportedEvent.Message, Group = Group)]
    public void HandleLifterTaskImported(LifterTaskImportedEvent param)
    {
      var doorId = CrashDoor.GetDoorIdFromLifter(param.Floor, param.LifterId);

      _doors.Get(doorId).Open();
    }

    // 取货完成
    [CapSubscribe(LifterTaskTakenEvent.Message, Group = Group)]
    public void HandleLifterTaskTaken(LifterTaskTakenEvent param)
    {
      var doorId = CrashDoor.GetDoorIdFromLifter(param.Floor, param.LifterId);

      _doors.Get(doorId).Open();
    }
  }
}
