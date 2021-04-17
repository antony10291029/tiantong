using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace Namei.Wcs.Api
{
  public class DoorWebController: BaseController
  {
    private DomainContext _domain;

    private WcsDoorFactory _doors;

    public DoorWebController(
      DomainContext domain,
      WcsDoorFactory doors
    ) {
      _domain = domain;
      _doors = doors;
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

      _domain.Logs.Where(log => log.CreatedAt < date).Delete();

      return NotifyResult.FromVoid().Success("日志已清理");
    }

    [HttpPost]
    [Route("/doors/states")]
    public object GetStates()
    {
      return AutomatedDoor.Enumerate().Concat(CrashDoor.Enumerate()).Select(id => {
        var door = _doors.Get(id);
        var tasks = _domain.RcsDoorTasks
          .Where(task => task.DoorId == door.DoorId)
          .Where(task => task.Status == RcsDoorTaskStatus.Requested)
          .ToArray();

        return new {
          id = door.DoorId,
          type = door.Type,
          IsError = door.IsError,
          IsOpened = door.IsOpened,
          IsForceOpened = door.HasPassport,
          RequestingTasks = tasks,
          EnteringTasksCount = tasks.Length,
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
      var passport = _domain.Set<WcsDoorPassport>()
        .Find(param.doorId);

      if (passport == null) {
        _domain.Add(passport = WcsDoorPassport.From(param.doorId, 0));
      }

      if (param.value) {
        passport.SetNeverExpired();
      } else {
        passport.SetExpired();
      }

      _domain.SaveChanges();

      return NotifyResult.FromVoid().Success("设置完毕");
    }
  }
}
