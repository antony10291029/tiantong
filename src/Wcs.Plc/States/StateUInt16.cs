using System;

namespace Wcs.Plc
{
  public class StateUInt16 : StateNumeric<ushort>
  {
    public StateUInt16()
    {
      _heartbeatMaxValue = 10000;
    }

    protected override void HandleDriverResolved()
    {
      Driver.UseUInt16();
    }

    protected override int CompareDataTo(ushort data, ushort value)
    {
      return data.CompareTo(value);
    }

    protected override ushort HandleGet()
    {
      return Driver.GetUInt16();
    }

    protected override void HandleSet(ushort data)
    {
      Driver.SetUInt16(data);
    }

    protected override void HandleHeartbeat(ref int times)
    {
      if (times < _heartbeatMaxValue) times++;
      else times = 1;

      Set((ushort) times);
    }
  }
}
