namespace Tiantong.Iot.Plc
{
  public class StateUInt16Builder: StateBuilder<ushort>
  {
    protected override StateWorker<ushort> BuildWorker(IStateDriver driver)
    {
      return new StateUint16Worker(driver);
    }

    protected override int CompareDataTo(ushort data, ushort value)
    {
      return data.CompareTo(value);
    }

  }

  public class StateUint16Worker: StateWorker<ushort>
  {
    public StateUint16Worker(IStateDriver driver): base(driver)
    {
      driver.UseUInt16();
    }

    protected override ushort HandleGet()
    {
      return _driver.GetUInt16();
    }

    protected override void HandleSet(ushort data)
    {
      _driver.SetUInt16(data);
    }
  }
}
