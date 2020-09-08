using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Namei.Wcs.Api
{
  public class DoorTask
  {
    private ICapPublisher _cap;

    private RcsService _rcs;

    public IDoorService Door { get; }

    public string TaskId { get; private set; } = "";

    public int Count { get; private set; } = 0;

    public DateTime RequestedAt { get; private set; } = DateTime.MinValue;

    public DateTime EnteredAt { get; private set; } = DateTime.MinValue;

    public DateTime LeftAt { get; private set; } = DateTime.MinValue;

    public bool HasTask { get => TaskId != ""; }

    public DoorTask(IDoorService door, ICapPublisher cap, RcsService rcs)
    {
      _cap = cap;
      _rcs = rcs;
      Door = door;
    }

    public void Request(string taskId)
    {
      TaskId = taskId;
      RequestedAt = DateTime.Now;
      Handle();
    }

    public void Handle()
    {
      if (!HasTask) {
        return;
      }

      if (Door.IsOpened) {
        Enter();
        return;
      }

      if (!Door.IsAvaliable) {
        return;
      }

      Door.Open();
    }

    public void Enter()
    {
      if (!HasTask) {
        return;
      }

      _rcs.NotifyDoorOpened(Door.Id, TaskId);
      Count++;
      TaskId = "";
      EnteredAt = DateTime.Now;
    }

    public void Leave(string taskId)
    {
      _rcs.NotifyDoorClosing(Door.Id, taskId);

      if (Count > 0) {
        Count--;
        LeftAt = DateTime.Now;
      } else {
        Count = 0;
      }

      if (Count == 0) {
        Door.Close();
      }
    }

    public void Clear()
    {
      Count = 0;
      Door.Close();
    }
  }

  public class DoorTaskManager
  {
    public Dictionary<string, DoorTask> Tasks { get; private set; } = new Dictionary<string, DoorTask>();

    public DoorTaskManager(DoorServiceManager doors, ICapPublisher cap, RcsService rcs)
    {
      Tasks = doors.All().ToDictionary(door => door.Id, door => new DoorTask(door, cap, rcs));
    }
  }

  // 后台托管服务
  public class DoorTaskHostedService: IntervalService
  {
    private ICapPublisher _cap;

    private DoorTaskManager _manager;

    public DoorTaskHostedService(DoorTaskManager manager, ICapPublisher cap)
    {
      _cap = cap;
      _manager = manager;
    }

    protected override Task HandleJob(CancellationToken token)
    {
      foreach (var task in _manager.Tasks.Values) {
        // agc 通过 30s 后，无论是否请求关门，都将强行关门
        if (task.Count > 0 && task.EnteredAt.AddSeconds(30) < DateTime.Now) {
          task.Door.Close();
          task.Clear();
        }

        // 开启 10s 后将强行关闭防撞门
        if (
          task.Door.Type == DoorType.Crash &&
          task.Door.IsOpened &&
          task.Door.OpenedAt.AddSeconds(10) < DateTime.Now
        ) {
          task.Door.Close();
          task.Clear();
        }
      }

      return Task.CompletedTask;
    }
  }
}
