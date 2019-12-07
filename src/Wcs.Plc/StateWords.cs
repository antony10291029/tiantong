using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWords : State<string>, IStateWords
  {
    public StateWords(IContainer container): base(container)
    {

    }

    protected override Task<string> HandleGet()
    {
      return _stateDriver.GetWords();
    }

    protected override Task HandleSet(string data)
    {
      return _stateDriver.SetWords(data);
    }
  }
}
