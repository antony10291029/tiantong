namespace Tiantong.Iot
{
  public class StateString : State<string>
  {
    public override string FromString(string data)
    {
      return data;
    }

    public override string ToString(string value)
    {
      return value ?? "";
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
