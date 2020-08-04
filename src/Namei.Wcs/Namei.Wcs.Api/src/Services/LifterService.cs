using DotNetCore.CAP;
using Renet;
using System.Collections.Generic;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Api
{
  public class LifterServiceManager
  {
    private Dictionary<int, LifterService> _lifters = new Dictionary<int, LifterService>();

    private ICapPublisher _cap;

    public LifterServiceManager(
      FirstLifterService firstLifter,
      SecondLifterService secondLifter,
      ThirdLifterService thirdLifter,
      ICapPublisher cap
    ) {
      // 待添加两台提升机
      _cap = cap;
      _lifters.Add(1, firstLifter);
      _lifters.Add(2, secondLifter);
      _lifters.Add(3, thirdLifter);
    }

    public LifterService Get(int id)
    {
      if (!_lifters.ContainsKey(id)) {
        throw KnownException.Error($"lifter_id: {id} 设备不存在", 400);
      }

      return _lifters[id];
    }
  }

  public abstract class LifterService
  {
    // 放货完成
    public abstract void Release(string floor);

    // 取货完成
    public abstract void Pickup(string floor);

    // 获取托盘码
    public abstract string GetPalletCode(string floor);

    // 设置目的楼层
    public abstract void SetDestination(string from, string to);

    // 是否允许放货
    public abstract bool IsImportAllowed(string floor);

    // 是否允许取货
    public abstract bool IsRequestingPickup(string floor);
  }

  public class FirstLifterService: LifterService
  {
    private PlcStateService _plc;

    public FirstLifterService(PlcStateService plc)
    {
      _plc = plc;
      _plc.Configure("http://localhost:5101", "改造货梯");
    }

    public override void Release(string floor)
      => _plc.Set($"{floor}F - 放货完成", "1");

    public override void Pickup(string floor)
      => _plc.Set($"{floor}F - 取货完成", "1");

    public override string GetPalletCode(string floor)
      => _plc.Get($"{floor}F - A 段 - 托盘码");

    public override void SetDestination(string from, string to)
      => _plc.Set($"{from}F - 目的楼层", to);

    public override bool IsImportAllowed(string floor)
      => MelsecStateHelper.GetBit(_plc.Get($"{floor} - A 段 - 输送机"), 6);

    public override bool IsRequestingPickup(string floor)
      => MelsecStateHelper.GetBit(_plc.Get($"{floor} - A 段 - 输送机"), 7);

    private bool GetIsTaskScanned(string data)
      => MelsecStateHelper.GetBit(data, 4);

    public bool IsTaskScanned(string floor)
      => GetIsTaskScanned(_plc.Get($"{floor} - A 段 - 输送机"));

    public bool IsTaskScanned(string data, string oldData)
      => GetIsTaskScanned(data) && !GetIsTaskScanned(oldData);
  }

  public class StandardLifterService: LifterService
  {
    private PlcStateService _plc;

    public StandardLifterService(PlcStateService plc)
    {
      _plc = plc;
    }

    public override void Release(string floor)
      => _plc.Set($"{floor}F - A 段 - 放取货状态", "3");

    public override void Pickup(string floor)
      => _plc.Set($"{floor}F - A 段 - 放取货状态", "5");

    public override string GetPalletCode(string floor)
      => _plc.Get($"{floor}F - A 段 - 托盘码");

    public override void SetDestination(string from, string to)
      => _plc.Set($"{from}F - A 段 - 目标楼层", to);

    public override bool IsImportAllowed(string floor)
      => _plc.Get($"{floor}F - A 段 - 工位状态") == "2";

    public override bool IsRequestingPickup(string floor)
      => _plc.Get($"{floor}F - A 段 - 工位状态") == "3";
  }

  public class SecondLifterService: StandardLifterService
  {
    public SecondLifterService(PlcStateService plc): base(plc)
    {

    }
  }

  public class ThirdLifterService: StandardLifterService
  {
    public ThirdLifterService(PlcStateService plc): base(plc)
    {

    }
  }
}
