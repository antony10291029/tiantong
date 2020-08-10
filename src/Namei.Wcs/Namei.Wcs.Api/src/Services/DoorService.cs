using DotNetCore.CAP;
using Renet;
using System;
using System.Collections.Generic;
using System.Linq;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class DoorServiceManager
  {
    private Dictionary<string, DoorService> _doors = new Dictionary<string, DoorService>();

    public DoorServiceManager(
      ICapPublisher cap,
      DomainContext domain,
      PlcStateServiceProvider provider
    ) {
      foreach (var id in CrashDoor.Enumerate()) {
        _doors.Add(id, new CrashDoorService(id, cap, domain));
      }
      var plc = provider.Resolve();
      plc.Configure("http://localhost:5101", "自动门 - 1F");
      _doors.Add("101", new AutomatedDoorService(plc, "101"));
      _doors.Add("102", new AutomatedDoorService(plc, "102"));
      _doors.Add("103", new AutomatedDoorService(plc, "103"));
      plc = provider.Resolve();
      plc.Configure("http://localhost:5101", "自动门 - 2F");
      _doors.Add("201", new AutomatedDoorService(plc, "201"));
      _doors.Add("202", new AutomatedDoorService(plc, "202"));
    }

    public DoorService Get(string id)
    {
      if (!_doors.ContainsKey(id)) {
        throw KnownException.Error($"设备 `自动门{id}`不存在");
      }

      return _doors[id];
    }

    public List<DoorService> All()
    {
      return _doors
        .OrderBy(kv => kv.Key)
        .Select(kv => kv.Value)
        .ToList();
    }
  }

  public abstract class DoorService
  {
    public abstract string Id { get; }

    public abstract void Open();

    public abstract void Close();

    public abstract bool IsClosed();
  }

  public class AutomatedDoorService: DoorService
  {
    public override string Id { get; }

    private PlcStateService _plc;

    public AutomatedDoorService(PlcStateService plc, string id)
    {
      Id = id;
      _plc = plc;
    }

    public override void Open()
      => _plc.Set($"{Id} # 指令", "1");

    public override void Close()
      => _plc.Set($"{Id} # 指令", "2");

    public override bool IsClosed()
      => _plc.Get($"{Id} # 状态") != "1";
  }

  public class CrashDoorService: DoorService
  {
    public override string Id { get; }

    private DomainContext _domain;

    private ICapPublisher _cap;

    public CrashDoorService(string id, ICapPublisher cap, DomainContext domain)
    {
      Id = id;
      _cap = cap;
      _domain = domain;
    }

    public override void Open()
    {
      var door = _domain.Jobs.First(job => job.name == $"防撞门 - {Id}");

      door.executed_at = DateTime.Now;
      door.is_enable = true;
      _domain.SaveChanges();

      _cap.Publish(DoorOpenedEvent.Message, new DoorOpenedEvent(Id));
    }

    public override void Close()
    {
      var door = _domain.Jobs.First(job => job.name == $"防撞门 - {Id}");
      door.is_enable = false;
      _domain.SaveChanges();

      _cap.Publish(DoorClosedEvent.Message, new DoorClosedEvent(Id));
    }

    public override bool IsClosed()
    {
      return !_domain.Jobs.First(job => job.name == $"防撞门 - {Id}").is_enable;
    }
  }
}
