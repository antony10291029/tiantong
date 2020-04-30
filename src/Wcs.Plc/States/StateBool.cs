using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBool : State<bool>, IStateBool
  {
    protected override int CompareDataTo(bool data, bool value)
    {
      return data.CompareTo(value);
    }

    protected override bool HandleGet()
    {
      return StateClient.GetBool();
    }

    protected override void HandleSet(bool data)
    {
      StateClient.SetBool(data);
    }
  }
}
