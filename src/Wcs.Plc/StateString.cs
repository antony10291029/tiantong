using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateString : State<string>, IStateString
  {
    public override string Type { get => "String"; }

    public override IStateString ToStateString()
    {
      return this;
    }

    protected override int CompareDataTo(string data, string value)
    {
      return data.CompareTo(value);
    }

    protected override Task<string> HandleGet()
    {
      return StateClient.GetString();
    }

    protected override Task HandleSet(string data)
    {
      return StateClient.SetString(data);
    }
  }
}
