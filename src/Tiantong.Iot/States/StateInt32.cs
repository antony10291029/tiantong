namespace Tiantong.Iot
{
  public class StateInt32 : StateNumeric<int>
  {
    public StateInt32()
    {
      _heartbeatMaxValue = 10000;
    }

    protected override void HandleDriverBuild()
    {
      _driver.UseInt32();
    }

    protected override int CompareTo(int data, int value)
    {
      return data.CompareTo(value);
    }

    protected override int HandleGet()
    {
      return _driver.GetInt32();
    }

    protected override void HandleSet(int data)
    {
      _driver.SetInt32(data);
    }

    protected override void HandleHeartbeat(ref int times)
    {
      if (times < _heartbeatMaxValue) times++;
      else times = 1;

      Set(times);
    }

  }
}
