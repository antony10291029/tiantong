using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class DoorWebController: BaseController
  {
    private DoorServiceManager _doors;

    private DoorTaskManager _taskManager;

    public DoorWebController(
      DomainContext domain,
      DoorServiceManager doors,
      DoorTaskManager taskManager,
      RcsService rcs
    ) {
      _doors = doors;
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
          IsOpened = door.IsOpened,
          RequestingTasks = task.RequestingTasks,
          EnteringTasksCount = task.EnteringTasks.Count,
        };
      }).ToDictionary(door => door.id, door => door);
    }
  }
}
