using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBit : State<bool>, IStateBit
  {
    public StateBit(PlcContainer services): base(services)
    {

    }

    protected override int CompareDataTo(bool data, bool value)
    {
      return data.CompareTo(value);
    }

    protected override Task<bool> HandleGet()
    {
      return _stateClient.GetBit();
    }

    protected override Task HandleSet(bool data)
    {
      return _stateClient.SetBit(data);
    }
  }
}
