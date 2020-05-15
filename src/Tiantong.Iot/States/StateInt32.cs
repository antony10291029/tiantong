namespace Tiantong.Iot
{
  public class StateInt32 : StateNumeric<int>
  {
    public override void SetString(string data)
    {
      HandleSet(short.Parse(data));
    }

    public StateInt32()
    {
      _heartbeatMaxValue = 10000;
    }

    protected override void HandleDriverBuild()
    {
      _driver.UseInt32();
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
      Set(times);
    }

  }
}
