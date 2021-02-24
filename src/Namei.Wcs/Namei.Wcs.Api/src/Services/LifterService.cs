using DotNetCore.CAP;
using System;
using System.Linq;
using System.Collections.Generic;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class LifterServiceManager
  {
    private Dictionary<string, LifterService> _lifters = new Dictionary<string, LifterService>();

    private ICapPublisher _cap;

    public LifterServiceManager(
      FirstLifterService firstLifter,
      SecondLifterService secondLifter,
      ThirdLifterService thirdLifter,
      ICapPublisher cap
    ) {
      _cap = cap;
      _lifters.Add("1", firstLifter);
      _lifters.Add("2", secondLifter);
      _lifters.Add("3", thirdLifter);
    }

    public LifterService Get(string id)
    {
      if (!_lifters.ContainsKey(id)) {
        throw KnownException.Error($"lifter_id: {id} 设备不存在", 400);
      }

      return _lifters[id];
    }

    public Dictionary<string, LifterService> All() => _lifters;
  }

  public class LifterState
  {
    public bool IsWorking { get; set; }

    public bool IsAlerting { get; set; }

    public string PalletCodeA { get; set; }

    public string PalletCodeB { get; set; }

    public List<LifterFloorState> Floors { get; set; }
  }

  public class LifterFloorState
  {
    // A 段托盘码
    public string PalletCodeA { get; set; }

    // B 段托盘码
    public string PalletCodeB { get; set; }

    // 允许放货
    public bool IsImportAllowed { get; set; }

    // 请求取货
    public bool IsExported { get; set; }
  }

  public abstract class LifterService
  {
    public Dictionary<string, DateTime> ExportedAt = new Dictionary<string, DateTime>() {
      { "1", DateTime.MinValue },
      { "2", DateTime.MinValue },
      { "3", DateTime.MinValue },
      { "4", DateTime.MinValue },
    };

    // 放货完成
    public abstract void SetImported(string floor, bool value);

    // 取货完成
    public abstract void SetPickuped(string floor, bool value);

    // 获取托盘码
    public abstract string GetPalletCode(string floor);

    // 设置托盘码
    public abstract void SetPalletCode(string floor, string code);

    // 设置目的楼层
    public abstract void SetDestination(string from, string to);

    // 是否允许放货
    public abstract bool IsImportAllowed(string floor);

    // 是否允许取货
    public abstract bool IsRequestingPickup(string floor);

    public abstract LifterState GetStates();
  }

  public class FirstLifterService: LifterService
  {
    public static bool GetIsSpare(string data)
      => !MelsecStateHelper.GetBit(data, 1) && !MelsecStateHelper.GetBit(data, 3);

    public static bool GetIsTaskScanned(string data)
      => MelsecStateHelper.GetBit(data, 4);

    public static bool GetIsImportAllowed(string data)
      => MelsecStateHelper.GetBit(data, 6);

    public static bool GetIsRequestingPickup(string data)
      => MelsecStateHelper.GetBit(data, 7);

    public static bool IsTaskScanned(string data, string oldData)
      => GetIsTaskScanned(data) && !GetIsTaskScanned(oldData);

    public static bool IsRequestingPickup(string data, string oldData)
      => GetIsRequestingPickup(data) && !GetIsRequestingPickup(oldData);

    public static bool IsImportAllowed(string data, string oldData)
      => GetIsImportAllowed(data) && !GetIsImportAllowed(oldData);

    public static bool IsSpare(string data, string oldData)
      => GetIsSpare(data) && !GetIsSpare(oldData);

    private PlcStateService _plc;

    public FirstLifterService(PlcStateService plc, Config config)
    {
      _plc = plc;
      _plc.Configure(config.PlcUrl, "改造货梯");
    }

    public override void SetImported(string floor, bool value)
      => _plc.Set($"{floor}F - 放货完成", value ? "1" : "0");

    public override void SetPickuped(string floor, bool value)
      => _plc.Set($"{floor}F - 取货完成", value ? "1" : "0");

    public override string GetPalletCode(string floor)
      => _plc.Get($"{floor}F - A 段 - 托盘码");

    public override void SetDestination(string from, string to)
      => _plc.Set($"{from}F - 目的楼层", to);

    public override bool IsImportAllowed(string floor)
    {
      var state = _plc.Get($"{floor}F - A 段 - 输送机");

      return GetIsImportAllowed(state);
    }

    public override void SetPalletCode(string floor, string code)
    {
      // @Todo: Waiting
    }

    public override bool IsRequestingPickup(string floor)
      => GetIsRequestingPickup(_plc.Get($"{floor}F - A 段 - 输送机"));

    public override LifterState GetStates()
    {
      var states = _plc.GetValues();

      return new LifterState() {
        IsWorking = states["升降平台状态"] != "0",
        IsAlerting = states["故障代码"] != "0",
        PalletCodeA = states["货梯 - A 段 - 托盘码"],
        PalletCodeB = states["货梯 - B 段 - 托盘码"],
        Floors = Enumerable.Range(1, 4).Select(floor => new LifterFloorState {
          PalletCodeA = states[$"{floor}F - A 段 - 托盘码"],
          PalletCodeB = states[$"{floor}F - B 段 - 托盘码"],
          IsImportAllowed = GetIsImportAllowed(states[$"{floor}F - A 段 - 输送机"]),
          IsExported = MelsecStateHelper.GetBit(states[$"{floor}F - A 段 - 输送机"], 7),
        }).ToList()
      };
    }
  }

  public class StandardLifterService: LifterService
  {
    private PlcStateService _plc;

    public StandardLifterService(PlcStateService plc)
    {
      _plc = plc;
    }

    public override void SetImported(string floor, bool value)
      => _plc.Set($"{floor}F - A 段 - 放取货状态", value ? "3" : "0");

    public override void SetPickuped(string floor, bool value)
      => _plc.Set($"{floor}F - A 段 - 放取货状态", value ? "5" : "0");

    public override string GetPalletCode(string floor)
      => _plc.Get($"{floor}F - A 段 - 托盘码");

    public override void SetPalletCode(string floor, string code)
      => _plc.Set($"{floor}F - A 段 - 任务托盘码", code);

    public override void SetDestination(string from, string to)
      => _plc.Set($"{from}F - A 段 - 目标楼层", to);

    public override bool IsImportAllowed(string floor)
      => _plc.Get($"{floor}F - A 段 - 工位状态") == "2";

    public override bool IsRequestingPickup(string floor)
      => _plc.Get($"{floor}F - A 段 - 工位状态") == "3";

    public override LifterState GetStates()
    {
      var states = _plc.GetValues();

      return new LifterState() {
        IsWorking = states["升降平台状态"] != "0",
        IsAlerting = states["故障代码"] != "1",
        PalletCodeA = states["平台内托盘码"],
        PalletCodeB = "0",
        Floors = Enumerable.Range(1, 4).Select(floor => new LifterFloorState {
          PalletCodeA = states[$"{floor}F - A 段 - 托盘码"],
          PalletCodeB = states[$"{floor}F - B 段 - 托盘码"],
          IsImportAllowed = states[$"{floor}F - A 段 - 工位状态"] == "2",
          IsExported = states[$"{floor}F - A 段 - 工位状态"] == "3",
        }).ToList()
      };
    }
  }

  public class SecondLifterService: StandardLifterService
  {
    public SecondLifterService(
      Config config,
      PlcStateService plc
    ): base(plc) {
      plc.Configure(config.PlcUrl, "提升机 - 1");
    }
  }

  public class ThirdLifterService: StandardLifterService
  {
    public ThirdLifterService(
      Config config,
      PlcStateService plc
    ): base(plc) {
      plc.Configure(config.PlcUrl, "提升机 - 2");
    }
  }
}
