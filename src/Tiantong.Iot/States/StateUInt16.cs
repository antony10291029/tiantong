using System;

namespace Tiantong.Iot
{
  public class StateUInt16 : StateNumeric<ushort>
  {
    protected override void HandleDriverBuild()
    {
      _driver.UseUInt16();
    }

    protected override ushort HandleGet()
    {
      return _driver.GetUInt16();
    }

    protected override void HandleSet(ushort data)
    {
      _driver.SetUInt16(data);
    }

    protected override void HandleHeartbeat(ref int times)
    {
      Set((ushort) times);
    }
  }
}
