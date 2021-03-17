using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
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

    private IServiceProvider _service;

    public IDoorService Door { get; }

    public SortedSet<DateTime> EnteringTasks = new SortedSet<DateTime>();

    public List<string> RequestingTasks = new List<string>();

    public DoorTask(IDoorService door, ICapPublisher cap, IServiceProvider service)
    {
      _cap = cap;
      Door = door;
      _service = service;
    }

    private void useRcs(Action<RcsService> handler)
    {
      using (var scope = _service.CreateScope()) {
        var rcs = scope.ServiceProvider.GetService<RcsService>();

        handler(rcs);
      }
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

      if (Door.IsOpened || Door.IsForceOpened) {
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
        EnteringTasks.Add(DateTime.Now);
        useRcs(rcs => rcs.NotifyDoorOpened(Door.Id, taskId));
      }
    }

    public void Leave(string taskId)
    {
      useRcs(rcs => rcs.NotifyDoorClosing(Door.Id, taskId));

      try {
        EnteringTasks.Remove(EnteringTasks.First());
      } catch { }

      if (EnteringTasks.Count == 0) {
        Door.Close();
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

    public DoorTaskManager(DoorServiceManager doors, ICapPublisher cap, IServiceProvider service)
    {
      Tasks = doors.All().ToDictionary(door => door.Id, door => new DoorTask(door, cap, service));
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
      foreach (var door in _manager.Tasks.Values) {
        // 只给 20s 用于 agc 通过自动门
        var tasks = door.EnteringTasks.ToArray();

        foreach (var task in tasks) {
          if (door.RequestingTasks.Count > 0) {
            door.Handle();
          }

          if (task.AddSeconds(20) < DateTime.Now) {
            door.EnteringTasks.Remove(task);
            if (door.EnteringTasks.Count == 0) {
              door.Door.Clear();
            }
          }
        }

        // 开启 10s 后将强行关闭防撞门
        if (
          door.Door.IsOpened &&
          door.Door.Type == DoorType.Crash &&
          door.Door.OpenedAt.AddSeconds(20) < DateTime.Now
        ) {
          door.Door.Close();
        }
      }

      return Task.CompletedTask;
    }
  }
}
