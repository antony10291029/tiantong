using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBit : State<bool>, IStateBit
  {
    public override string Type { get => "Bit"; }

    public override IStateBit ToBit()
    {
      return this;
    }

    protected override int CompareDataTo(bool data, bool value)
    {
      return data.CompareTo(value);
    }

    protected override Task<bool> HandleGet()
    {
      return StateClient.GetBit();
    }

    protected override Task HandleSet(bool data)
    {
      return StateClient.SetBit(data);
    }
  }
}
