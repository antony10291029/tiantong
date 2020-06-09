namespace Tiantong.Iot
{
  public class StateString : State<string>
  {
    protected override string FromString(string data)
    {
      return data;
    }

    protected override void HandleDriverBuild()
    {
      _driver.UseString(_length);
    }

    protected override string HandleGet()
    {
      return _driver.GetString() ?? "";
    }

    protected override void HandleSet(string data)
    {
      _driver.SetString(data ?? "");
    }
  }
}