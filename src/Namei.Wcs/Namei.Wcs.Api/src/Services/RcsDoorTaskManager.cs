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

    public Dictionary<string, DateTime> EnteringTasks = new Dictionary<string, DateTime>();

    public List<string> RequestingTasks = new List<string>();

    public DoorTask(IDoorService door, ICapPublisher cap, RcsService rcs)
    {
      _cap = cap;
      _rcs = rcs;
      Door = door;
    }

    public void Request(string taskId)
    {
      RequestingTasks.Add(taskId);
      Handle();
    }

    public void Handle()
    {
      if (RequestingTasks.Count == 0) {
        return;
      }

      if (Door.IsOpened) {
        Enter();
      }

      if (!Door.IsAvaliable) {
        return;
      }

      Door.Open();
    }

    public void Enter()
    {
      var taskIds = RequestingTasks.ToArray();
      RequestingTasks.Clear();

      foreach (var taskId in taskIds) {
        _rcs.NotifyDoorOpened(Door.Id, taskId);
        EnteringTasks.Add(taskId, DateTime.Now);
      }
    }

    public void Leave(string taskId)
    {
      _rcs.NotifyDoorClosing(Door.Id, taskId);

      if (EnteringTasks.ContainsKey(taskId)) {
        EnteringTasks.Remove(taskId);
      }
    }

    public void Clear()
    {
      EnteringTasks.Clear();
      Door.Clear();
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
        // 只给 10s 用于 agc 通过自动门
        if (task.Door.Type == DoorType.Automatic) {
          foreach (var enteringTask in task.EnteringTasks) {
            if (enteringTask.Value.AddSeconds(10) < DateTime.Now) {
              task.EnteringTasks.Remove(enteringTask.Key);
              if (task.EnteringTasks.Count == 0) {
                task.Door.Clear();
              }
            }
          }
        }

        // 开启 10s 后将强行关闭防撞门
        if (
          task.Door.IsOpened &&
          task.Door.Type == DoorType.Crash &&
          task.Door.OpenedAt.AddSeconds(10) < DateTime.Now
        ) {
          task.Door.Close();
        }
      }

      return Task.CompletedTask;
    }
  }
}
