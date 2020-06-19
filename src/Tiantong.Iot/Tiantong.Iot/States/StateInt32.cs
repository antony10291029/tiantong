namespace Tiantong.Iot
{
  public class StateInt32 : State<int>
  {
    protected override int FromString(string data)
    {
      return int.Parse(data);
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
  }
}