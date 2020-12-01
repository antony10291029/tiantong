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
    private Dictionary<string, IDoorService> _doors = new Dictionary<string, IDoorService>();

    public DoorServiceManager(
      ICapPublisher cap,
      LifterServiceManager lifters,
      PlcStateServiceProvider provider
    ) {
      _doors.Add(CrashDoor.Floor1_1, new CrashDoorService(CrashDoor.Floor1_1, "1", "1", cap, lifters));
      _doors.Add(CrashDoor.Floor2_1, new CrashDoorService(CrashDoor.Floor2_1, "2", "1", cap, lifters));
      _doors.Add(CrashDoor.Floor3_1, new CrashDoorService(CrashDoor.Floor3_1, "3", "1", cap, lifters));
      _doors.Add(CrashDoor.Floor4_1, new CrashDoorService(CrashDoor.Floor4_1, "4", "1", cap, lifters));
      _doors.Add(CrashDoor.Floor1_2, new CrashDoorService(CrashDoor.Floor1_2, "1", "2", cap, lifters));
      _doors.Add(CrashDoor.Floor2_2, new CrashDoorService(CrashDoor.Floor2_2, "2", "2", cap, lifters));
      _doors.Add(CrashDoor.Floor3_2, new CrashDoorService(CrashDoor.Floor3_2, "3", "2", cap, lifters));
      _doors.Add(CrashDoor.Floor4_2, new CrashDoorService(CrashDoor.Floor4_2, "4", "2", cap, lifters));
      _doors.Add(CrashDoor.Floor1_3, new CrashDoorService(CrashDoor.Floor1_3, "1", "3", cap, lifters));
      _doors.Add(CrashDoor.Floor2_3, new CrashDoorService(CrashDoor.Floor2_3, "2", "3", cap, lifters));
      _doors.Add(CrashDoor.Floor3_3, new CrashDoorService(CrashDoor.Floor3_3, "3", "3", cap, lifters));
      _doors.Add(CrashDoor.Floor4_3, new CrashDoorService(CrashDoor.Floor4_3, "4", "3", cap, lifters));

      var plc = provider.Resolve();
      plc.Configure("http://localhost:5101", "自动门 - 1F");
      _doors.Add(AutomatedDoor.Floor1_1, new AutomatedDoorService(plc, "101"));
      _doors.Add(AutomatedDoor.Floor1_2, new AutomatedDoorService(plc, "102"));
      _doors.Add(AutomatedDoor.Floor1_3, new AutomatedDoorService(plc, "103"));

      plc = provider.Resolve();
      plc.Configure("http://localhost:5101", "自动门 - 2F");
      _doors.Add(AutomatedDoor.Floor2_1, new AutomatedDoorService(plc, "201"));
      _doors.Add(AutomatedDoor.Floor2_2, new AutomatedDoorService(plc, "202"));
    }

    public IDoorService Get(string id)
    {
      if (!_doors.ContainsKey(id)) {
        throw KnownException.Error($"设备 `自动门{id}`不存在");
      }

      return _doors[id];
    }

    public List<IDoorService> All()
    {
      return _doors
        .OrderBy(kv => kv.Key)
        .Select(kv => kv.Value)
        .ToList();
    }
  }

  public interface IDoorService
  {
    string Type { get; }

    string Id { get; }

    bool IsOpened { get; }

    bool IsForceOpened { get; set; }

    bool IsAvaliable { get; }

    DateTime OpenedAt { get; }

    DateTime ClosedAt { get; }

    void Open();

    void Close();

    void Clear();

    void OnOpened();

    void OnClosed();
  }

  public class AutomatedDoorService: IDoorService
  {
    private PlcStateService _plc;

    public string Type { get => DoorType.Automatic; }

    public string Id { get; }

    public bool IsForceOpened { get; set; } = false;

    public bool IsOpened
    {
      get => _plc.Get($"{Id} # 状态") == "1";
    }

    public bool IsAvaliable { get => true; }

    public DateTime OpenedAt { get; private set; } = DateTime.MinValue;

    public DateTime ClosedAt { get; private set; } = DateTime.MinValue;

    public AutomatedDoorService(PlcStateService plc, string id)
    {
      Id = id;
      _plc = plc;
    }

    public void Open()
      => _plc.Set($"{Id} # 指令", "1");

    public void Close()
      => _plc.Set($"{Id} # 指令", "0");

    public void OnOpened()
    {
      // 保持开门信号
      OpenedAt = DateTime.Now;
    }

    public void Clear()
    {
      _plc.Set($"{Id} # 指令", "0");
    }

    public void OnClosed()
    {
      Clear();
      ClosedAt = DateTime.Now;
    }
  }

  public class CrashDoorService: IDoorService
  {
    private ICapPublisher _cap;

    private LifterService _lifter;

    public string Type { get => DoorType.Crash; }

    public string Id { get; }

    public string Floor;

    public string LifterId;

    public bool IsForceOpened { get; set; } = false;

    public bool IsOpened { get; private set; } = false;

    public bool IsAvaliable
    {
      get => _lifter.IsImportAllowed(Floor) || _lifter.IsRequestingPickup(Floor);
    }

    public DateTime OpenedAt { get; private set; } = DateTime.MinValue;

    public DateTime ClosedAt { get; private set; } = DateTime.MinValue;

    public CrashDoorService(
      string id,
      string floor,
      string lifterId,
      ICapPublisher cap,
      LifterServiceManager lifters
    ) {
      Id = id;
      Floor = floor;
      LifterId = lifterId;
      _cap = cap;
      _lifter = lifters.Get(LifterId);
    }

    public void Open()
    {
      _cap.Publish(DoorOpenedEvent.Message, new DoorOpenedEvent(Id));
    }

    public void Close()
    {
      _cap.Publish(DoorClosedEvent.Message, new DoorClosedEvent(Id));
    }

    public void Clear()
    {
      Close();
    }

    public void OnOpened()
    {
      IsOpened = true;
      OpenedAt = DateTime.Now;
    }

    public void OnClosed()
    {
      IsOpened = false;
      ClosedAt = DateTime.Now;
    }
  }
}
