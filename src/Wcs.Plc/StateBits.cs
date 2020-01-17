using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBits : State<string>, IStateBits
  {
    public StateBits(IPlcContainer services): base(services)
    {

    }

    protected override Task<string> HandleGet()
    {
      return _stateDriver.GetBits();
    }

    protected override Task HandleSet(string data)
    {
      return _stateDriver.SetBits(data);
    }
  }
}
