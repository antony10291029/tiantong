using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateBit : State<bool>, IStateBit
  {
    public StateBit(IContainer container): base(container)
    {

    }

    protected override Task<bool> HandleGet()
    {
      return _stateDriver.GetBit();
    }

    protected override Task HandleSet(bool data)
    {
      return _stateDriver.SetBit(data);
    }
  }
}
