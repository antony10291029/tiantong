using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBits : State<string>, IStateBits
  {
    public override string Type { get => "Bits"; }

    public override IStateBits ToBits()
    {
      return this;
    }

    protected override int CompareDataTo(string data, string value)
    {
      return data.CompareTo(value);
    }

    protected override Task<string> HandleGet()
    {
      return StateClient.GetBits();
    }

    protected override Task HandleSet(string data)
    {
      return StateClient.SetBits(data);
    }
  }
}
