using DotNetCore.CAP;
using System;
using System.Data;
using System.Linq;

namespace Namei.Wcs.Api
{
  public class RcsDoorEventController
  {
    public const string Group = "RcsDoorController";

    private DomainContext _domain;

    private IWcsDoorFactory _doors;

    private IRcsService _rcs;

    public RcsDoorEventController(
      DomainContext domain,
      IWcsDoorFactory doors,
      IRcsService rcs
    ) {
      _domain = domain;
      _doors = doors;
      _rcs = rcs;
    }

    [CapSubscribe(RcsDoorEvent.Request, Group = Group)]
    public void HandleDoorTaskRequest(RcsDoorEvent param)
    {
      var task = _domain.RcsDoorTasks.Find(param.Uuid);

      if (task == null) {
        _domain.Add(task = RcsDoorTask.From(param));
      } else {
        task.Request(param.DoorId);
      }

      _domain.SaveChanges();
      _domain.Publish(RcsDoorEvent.Requested, param);
    }

    [CapSubscribe(RcsDoorEvent.Retry, Group = Group)]
    [CapSubscribe(RcsDoorEvent.Requested, Group = Group)]
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

    [CapSubscribe(WcsDoorEvent.Open, Group = Group)]
    public void HandleDoorOpen(WcsDoorEvent param)
    {
      var door = _doors.Get(param.DoorId);

      if (door.IsOpened) {
        _domain.Publish(WcsDoorEvent.Opened, param);
      }

      door.Open();
    }

    [CapSubscribe(WcsDoorEvent.Opened, Group = Group)]
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

    [CapSubscribe(RcsDoorEvent.Enter, Group = Group)]
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

    [CapSubscribe(RcsDoorEvent.Entered, Group = Group)]
    public void HandleDoorTaskEntered(RcsDoorEvent param)
    {
      _rcs.NotifyDoorOpened(param.DoorId, param.Uuid);
    }

    [CapSubscribe(RcsDoorEvent.Leave, Group = Group)]
    public void HandleDoorTaskLeave(RcsDoorEvent param)
    {
      var task = _domain.RcsDoorTasks.Find(param.Uuid);

      task.Leave();

      _domain.SaveChanges();
      _domain.Publish(RcsDoorEvent.Left, param);
    }

    [CapSubscribe(RcsDoorEvent.Left, Group = Group)]
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

    [CapSubscribe(WcsDoorEvent.Close, Group = Group)]
    public void HandleDoorClose(WcsDoorEvent param)
    {
      _doors.Get(param.DoorId).Close();
    }

    [CapSubscribe(LifterTaskTaken.Message, Group = Group)]
    [CapSubscribe(LifterTaskImported.Message, Group = Group)]
    public void HandleLifterTaskTaken(LifterTaskTaken param)
    {
      var doorId = DoorType.GetDoorIdFromLifter(
        floor: param.Floor,
        lifterId: param.LifterId
      );

      var passport = _domain.WcsDoorPassports.Find(doorId);

      if (passport == null) {
        _domain.Add(passport = WcsDoorPassport.From(doorId, 15000));
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
