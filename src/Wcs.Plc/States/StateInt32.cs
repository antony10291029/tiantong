namespace Wcs.Plc
{
  public class StateInt32 : StateNumeric<int>
  {
    public StateInt32()
    {
      _heartbeatMaxValue = 10000;
    }

    protected override void HandleDriverResolved()
    {
      Driver.UseInt32();
    }

    protected override int CompareDataTo(int data, int value)
    {
      return data.CompareTo(value);
    }

    protected override int HandleGet()
    {
      return Driver.GetInt32();
    }

    protected override void HandleSet(int data)
    {
      Driver.SetInt32(data);
    }

    protected override void HandleHeartbeat(ref int times)
    {
      if (times < _heartbeatMaxValue) times++;
      else times = 1;

      Set(times);
    }

  }
}
