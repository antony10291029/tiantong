using Renet;

namespace Tiantong.Iot
{
  public class StateUInt16 : State<ushort>
  {
    public override ushort FromString(string data)
    {
      return ushort.Parse(data);
    }

    protected override void HandleDriverBuild()
    {
      _driver.UseUInt16();
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
