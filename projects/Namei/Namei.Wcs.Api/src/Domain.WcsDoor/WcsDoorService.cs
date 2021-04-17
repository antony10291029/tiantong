using System;
using System.Linq;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public interface IWcsDoorFactory
  {
    IWcsDoorService Get(string doorId);
  }

  public class WcsDoorFactory: IWcsDoorFactory
  {
    private Config _config;

    private DomainContext _domain;

    private LifterServiceManager _lifters;

    private PlcStateServiceProvider _plc;

    public WcsDoorFactory(
      Config config,
      DomainContext domain,
      LifterServiceManager lifters,
      PlcStateServiceProvider plc
    ) {
      _config = config;
      _domain = domain;
      _lifters = lifters;
      _plc = plc;
    }

    public IWcsDoorService Get(string doorId)
    {
      if (DoorType.Map[doorId] == DoorType.Automatic) {
        var plc = _plc.Resolve();

        plc.Configure(_config.PlcUrl, DoorType.GetPlcName(doorId));
        return new WcsAutomaticDoorService(doorId, _domain, plc);
      } else if (DoorType.Map[doorId] == DoorType.Crash) {
        return new WcsLifterDoorService(doorId, _domain, _lifters);
      } else {
        throw KnownException.Error($"自动门类型不存在: {doorId}");
      }
    }
  }

  public interface IWcsDoorService
  {
    string DoorId { get; }

    string Type { get; }

    bool IsError { get; }

    bool IsOpened { get; }

    bool HasPassport { get; }

    void Open();

    void Close();

    void Clear();
  }

  public class WcsAutomaticDoorService: IWcsDoorService
  {
    private PlcStateService _plc;

    private DomainContext _domain;

    public string DoorId { get; }

    public string Type { get; } = DoorType.Automatic;

    public WcsAutomaticDoorService(
      string doorId,
      DomainContext domain,
      PlcStateService plc
    ) {
      DoorId = doorId;
      _domain = domain;
      _plc = plc;
    }

    public bool IsError
    {
      get => _plc.Get($"{DoorId} # 异常") != "0";
    }

    public bool IsOpened
    {
      get => _plc.Get($"{DoorId} # 状态") == "1";
    }

    public bool HasPassport
    {
      get => _domain.WcsDoorPassports.Find(DoorId)?.ExpiredAt > DateTime.Now;
    }

    public void Open()
      => _plc.Set($"{DoorId} # 指令", "1");

    public void Close()
      => Clear();
    
    public void Clear()
      => _plc.Set($"{DoorId} # 指令", "0");
  }

  public class WcsLifterDoorService: IWcsDoorService
  {
    private DomainContext _domain;

    private LifterService _lifter;

    public string DoorId { get; }

    public string Type { get; } = DoorType.Crash;

    public WcsLifterDoorService(
      string doorId,
      DomainContext domain,
      LifterServiceManager lifters
    ) {
      DoorId = doorId;
      _domain = domain;
      _lifter = lifters.Get(DoorType.GetLifterId(doorId));
    }

    private string _floor
    {
      get => DoorType.GetFloor(DoorId);
    }

    public bool IsError
    {
      get => false;
    }

    public bool IsOpened
    {
      get => false;
    }

    public bool HasPassport
    {
      get => _domain.WcsDoorPassports.Find(DoorId)?.ExpiredAt > DateTime.Now;
    }

    public void Open()
    {
      if (_lifter.IsImportAllowed(_floor) || _lifter.IsRequestingPickup(_floor)) {
        _domain.Publish(WcsDoorEvent.Opened, WcsDoorEvent.From(DoorId));
      }
    }

    public void Close()
    {
      var passport = _domain.WcsDoorPassports
        .FirstOrDefault(door => DoorId == DoorId);

      if (passport != null) {
        passport.SetExpired();
        _domain.SaveChanges();
      }
    }

    public void Clear()
    {

    }
  }
}
