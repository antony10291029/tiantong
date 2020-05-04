namespace Tiantong.Iot
{
  public class StateString : State<string>
  {
    protected override void HandleDriverResolved()
    {
      Driver.UseString(Length);
    }

    protected override int CompareDataTo(string data, string value)
    {
      return data.CompareTo(value);
    }

    protected override string HandleGet()
    {
      return Driver.GetString();
    }

    protected override void HandleSet(string data)
    {
      Driver.SetString(data);
    }
  }
}
