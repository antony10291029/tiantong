namespace Tiantong.Iot
{
  public class StateBool : State<bool>
  {
    protected override void HandleDriverBuild()
    {
      _driver.UseBool();
    }

    protected override int CompareDataTo(bool data, bool value)
    {
      return data.CompareTo(value);
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
