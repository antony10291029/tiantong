namespace Tiantong.Iot
{
  public class StateBool : State<bool>
  {
    protected override void HandleDriverBuild()
    {
      _driver.UseBool();
    }

    protected override bool HandleGet()
    {
      return _driver.GetBool();
    }

    protected override void HandleSet(bool data)
    {
      _driver.SetBool(data);
    }
  }
}
