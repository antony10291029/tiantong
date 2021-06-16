using Namei.Wcs.Api;
using System.Linq;
using Tiantong.Iot.Utils;

namespace Namei.Wcs.Aggregates
{
  public interface ILifterCommand
  {
    void Import(string floor, string barcode, string destination);

    void SetImported(string floor, bool value);

    void SetTaken(string floor, bool value);

    void SetBarcode(string floor, string code);

    void SetDestination(string from, string to);

    string GetBarcode(string floor);

    string GetDestination(string floor);

    string GetTaskDestination(string floor);
  }

  public class FirstLifterCommand : ILifterCommand
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

    protected readonly PlcStateService _plc;

    public FirstLifterCommand(PlcStateService plc, Config config)
    {
      _plc = plc;
      _plc.Configure(config.PlcUrl, "改造货梯");
    }

    public void Import(string floor, string barcode, string destination)
      => SetImported(floor, true);

    public void SetImported(string floor, bool value)
      => _plc.Set($"{floor}F - 放货完成", value ? "1" : "0");

    public void SetTaken(string floor, bool value)
      => _plc.Set($"{floor}F - 取货完成", value ? "1" : "0");

    public void SetBarcode(string floor, string barcode)
    {

    }

    public void SetDestination(string from, string to)
      => _plc.Set($"{from}F - 目的楼层", to);

    public string GetBarcode(string floor)
      => _plc.Get($"{floor}F - A 段 - 托盘码");

    public string GetDestination(string floor)
      => _plc.Get($"{floor}F - 目的楼层");

    public string GetTaskDestination(string floor)
      => _plc.Get($"{floor}F - A 段 - 目的楼层");
  }

  public class StandardLifterCommand: ILifterCommand
  {
    private readonly PlcStateService _plc;

    public StandardLifterCommand(PlcStateService plc)
      => _plc = plc;

    public void Import(string floor, string barcode, string destination)
    {
      SetBarcode(floor, barcode);
      SetDestination(floor, destination);
      SetImported(floor, true);
    }

    public void SetImported(string floor, bool value)
      => _plc.Set($"{floor}F - A 段 - 放取货状态", value ? "3" : "0");

    public void SetTaken(string floor, bool value)
      => _plc.Set($"{floor}F - A 段 - 放取货状态", value ? "5" : "0");

    public void SetBarcode(string floor, string barcode)
      => _plc.Set($"{floor}F - 任务托盘码", barcode);

    public void SetDestination(string from, string to)
      => _plc.Set($"{from}F - 目的楼层", to);

    public string GetBarcode(string floor)
      => _plc.Get($"{floor}F - A 段 - 托盘码");

    public string GetDestination(string floor)
      => _plc.Get($"{floor}F - 目的楼层");

    public string GetTaskDestination(string floor)
      => _plc.Get($"{floor}F - A 段 - 任务路径").Last().ToString();
  }

  public class SecondLifterCommand: StandardLifterCommand
  {
    public SecondLifterCommand(PlcStateService plc, Config config): base(plc)
    {
      plc.Configure(config.PlcUrl, "提升机 - 1");
    }
  }

  public class ThirdLifterCommand: StandardLifterCommand
  {
    public ThirdLifterCommand(PlcStateService plc, Config config): base(plc)
    {
      plc.Configure(config.PlcUrl, "提升机 - 2");
    }
  }
}
