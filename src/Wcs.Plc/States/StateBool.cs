namespace Wcs.Plc
{
  public class StateBool : State<bool>
  {
    protected override void HandleDriverResolved()
    {
      Driver.UseBool();
    }

    protected override int CompareDataTo(bool data, bool value)
    {
      return data.CompareTo(value);
    }

    protected override bool HandleGet()
    {
      return Driver.GetBool();
    }

    protected override void HandleSet(bool data)
    {
      Driver.SetBool(data);
    }
  }
}
