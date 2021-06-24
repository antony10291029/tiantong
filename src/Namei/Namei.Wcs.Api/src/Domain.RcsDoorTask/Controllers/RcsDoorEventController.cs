using Midos.Eventing;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class RcsDoorEventController
  {
    public const string Group = "RcsDoorController";

    private readonly DomainContext _domain;

    private readonly IWcsDoorFactory _doors;

    private readonly IRcsService _rcs;

    public RcsDoorEventController(
      DomainContext domain,
      IWcsDoorFactory doors,
      IRcsService rcs
    ) {
      _domain = domain;
      _doors = doors;
      _rcs = rcs;
    }

    [EventSubscribe(RcsDoorEvent.Request, Group)]
    public void HandleDoorTaskRequest(RcsDoorEvent param)
    {
      var task = _domain.RcsDoorTasks.Find(param.Uuid);

      if (task == null) {
        task = RcsDoorTask.From(param);
        _domain.Add(task);
      } else {
        task.Request(param.DoorId);
      }

      _domain.SaveChanges();
      _domain.Publish(RcsDoorEvent.Requested, param);
    }

    [EventSubscribe(RcsDoorEvent.Retry, Group)]
    [EventSubscribe(RcsDoorEvent.Requested, Group)]
    public void HandleDoorTaskRequested(RcsDoorEvent param)
    {
      var door = _doors.Get(param.DoorId);

      if (door.HasPassport) {
        _domain.Publish(RcsDoorEvent.Enter, param);
      } else {
        _domain.Publish(
          WcsDoorEvent.Open,
          WcsDoorEvent.From(param)
        );
      }
    }

    [EventSubscribe(WcsDoorEvent.Open, Group)]
    public void HandleDoorOpen(WcsDoorEvent param)
    {
      var door = _doors.Get(param.DoorId);

      if (door.IsOpened) {
        _domain.Publish(WcsDoorEvent.Opened, param);
      }

      door.Open();
    }

    [EventSubscribe(WcsDoorEvent.Opened, Group)]
    public void HandleDoorOpened(WcsDoorEvent param)
    {
      var tasks = _domain.RcsDoorTasks
        .Where(task => task.DoorId == param.DoorId)
        .Where(task => task.Status == RcsDoorTaskStatus.Requested)
        .ToArray();

      foreach (var task in tasks) {
        _domain.Publish(
          RcsDoorEvent.Enter,
          RcsDoorEvent.From(
            uuid: task.Id,
            doorId: task.DoorId
          )
        );
      }
    }

    [EventSubscribe(RcsDoorEvent.Enter, Group)]
    public void HandleDoorTaskEnter(RcsDoorEvent param)
    {
      var task = _domain.RcsDoorTasks.Find(param.Uuid);

      task.Enter();

      if (DoorType.Map[param.DoorId] == DoorType.Crash) {
        _domain.WcsDoorPassports
          .Find(param.DoorId)
          ?.SetExpiredAt(DateTime.MinValue);
      }

      _domain.SaveChanges();
      _domain.Publish(RcsDoorEvent.Entered, param);
    }

    [EventSubscribe(RcsDoorEvent.Entered, Group)]
    public Task HandleDoorTaskEntered(RcsDoorEvent param)
      => _rcs.NotifyDoorOpened(param.DoorId, param.Uuid);

    [EventSubscribe(RcsDoorEvent.Leave, Group)]
    public void HandleDoorTaskLeave(RcsDoorEvent param)
    {
      var task = _domain.RcsDoorTasks.Find(param.Uuid);

      task.Leave();

      _domain.SaveChanges();
      _domain.Publish(RcsDoorEvent.Left, param);
    }

    [EventSubscribe(RcsDoorEvent.Left, Group)]
    public void HandleRcsDoorTaskLeft(RcsDoorEvent param)
    {
      var count = _domain.RcsDoorTasks
        .Where(task => task.DoorId == param.DoorId)
        .Where(task => task.Status != RcsDoorTaskStatus.Left)
        .Count();

      if (count == 0) {
        _domain.Publish(
          WcsDoorEvent.Close,
          WcsDoorEvent.From(param.DoorId)
        );
      }
    }

    [EventSubscribe(WcsDoorEvent.Close, Group)]
    public void HandleDoorClose(WcsDoorEvent param)
    {
      _doors.Get(param.DoorId).Close();
    }

    [EventSubscribe(LifterTaskFinished.Message, Group)]
    [EventSubscribe(LifterTaskTaken.Message, Group)]
    [EventSubscribe(LifterTaskImported.Message, Group)]
    public void HandleLifterTaskTaken(LifterTaskFinished param)
    {
      var doorId = DoorType.GetDoorIdFromLifter(
        floor: param.Floor,
        lifterId: param.LifterId
      );
      var passport = _domain.WcsDoorPassports.Find(doorId);

      if (passport == null) {
        passport = WcsDoorPassport.From(doorId, 15000);
        _domain.Add(passport);
      } else {
        passport.AddMilliseconds(15000);
      }

      _domain.SaveChanges();
      _domain.Publish(
        WcsDoorEvent.Opened,
        WcsDoorEvent.From(doorId)
      );
    }
  }
}
