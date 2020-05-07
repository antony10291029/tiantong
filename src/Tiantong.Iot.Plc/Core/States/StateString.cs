namespace Tiantong.Iot.Plc
{
  public class StateStringBuilder: StateBuilder<string>
  {
    protected override StateWorker<string> BuildWorker(IStateDriver driver)
    {
      return new StateStringWorker(driver, _length);
    }

    protected override int CompareDataTo(string data, string value)
    {
      return data.CompareTo(value);
    }

  }

  public class StateStringWorker: StateWorker<string>
  {
    public StateStringWorker(IStateDriver driver, int length): base(driver)
    {
      driver.UseString(length);
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
