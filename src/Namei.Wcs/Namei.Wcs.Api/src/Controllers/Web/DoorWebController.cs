using Microsoft.AspNetCore.Mvc;
using Renet.Web;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Namei.Wcs.Api
{
  public class DoorWebController: BaseController
  {
    private DomainContext _domain;

    private DoorServiceManager _doors;

    private DoorTaskManager _taskManager;

    public DoorWebController(
      DomainContext domain,
      DoorServiceManager doors,
      DoorTaskManager taskManager,
      RcsService rcs
    ) {
      _domain = domain;
      _doors = doors;
      _taskManager = taskManager;
    }

    public class ClearLogsParams
    {
      public int days { get; set; }
    }

    [HttpPost("/logs/clear")]
    public object ClearLogs([FromBody] ClearLogsParams param)
    {
      System.Console.WriteLine(param.days);
      var date = DateTime.Now.AddDays(-param.days);

      _domain.Logs.Where(log => log.created_at < date).Delete();

      return Success("日志已清理");
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
          IsError = door.IsError,
          IsOpened = door.IsOpened,
          IsForceOpened = door.IsForceOpened,
          RequestingTasks = task.RequestingTasks,
          EnteringTasksCount = task.EnteringTasks.Count,
        };
      }).ToDictionary(door => door.id, door => door);
    }

    public class SetForceOpenedParams
    {
      public string doorId { get; set; }

      public bool value { get; set; }
    }

    [HttpPost("/doors/force-opened/set")]
    public object SetForceOpened([FromBody] SetForceOpenedParams param)
    {
      _doors.Get(param.doorId).IsForceOpened = param.value;

      return Success("设置完毕");
    }
  }
}
