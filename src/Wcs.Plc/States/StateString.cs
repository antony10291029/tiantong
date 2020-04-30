using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateString : State<string>, IStateString
  {
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
