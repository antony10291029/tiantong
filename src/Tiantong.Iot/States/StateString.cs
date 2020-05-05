namespace Tiantong.Iot
{
  public class StateString : State<string>
  {
    protected override void HandleDriverBuild()
    {
      _driver.UseString(_length);
    }

    protected override int CompareTo(string data, string value)
    {
      return data.CompareTo(value);
    }

    protected override string HandleGet()
    {
      return _driver.GetString();
    }

    protected override void HandleSet(string data)
    {
      _driver.SetString(data);
    }
  }
}
