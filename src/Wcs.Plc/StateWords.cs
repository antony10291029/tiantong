using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWords : State<string>, IStateWords
  {
    public StateWords(IPlcContainer services): base(services)
    {

    }

    protected override int CompareDataTo(string data, string value)
    {
      return data.CompareTo(value);
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
