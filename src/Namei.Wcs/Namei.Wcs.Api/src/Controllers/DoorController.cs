using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class DoorController: BaseController
  {
    const string Group = "command";

    private static object _lock = new object();

    private DomainContext _domain;

    private DoorServiceManager _doors;

    private DoorTaskManager _taskManager;

    private RcsService _rcs;

    public DoorController(
      DomainContext domain,
      DoorServiceManager doors,
      DoorTaskManager taskManager,
      RcsService rcs
    ) {
      _domain = domain;
      _doors = doors;
      _rcs = rcs;
      _taskManager = taskManager;
    }

    [HttpPost]
    [Route("/doors/states")]
    public object GetStates()
    {
      return AutomatedDoor.Enumerate().Concat(CrashDoor.Enumerate()).Select(id => {
        var door = _doors.Get(id);
        var task = _taskManager.Tasks[id];

        return new {
          id = door.Id,
          type = door.Type,
          IsOpened = !door.IsOpened,
          taskId = task.TaskId,
          count = task.Count,
        };
      });
    }

    // events

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
