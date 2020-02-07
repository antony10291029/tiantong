using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWords : State<string>, IStateWords
  {
    public override string Type { get => "Words"; }

    public override IStateWords ToWords()
    {
      return this;
    }

    protected override int CompareDataTo(string data, string value)
    {
      return data.CompareTo(value);
    }

    protected override Task<string> HandleGet()
    {
      return StateClient.GetWords();
    }

    protected override Task HandleSet(string data)
    {
      return StateClient.SetWords(data);
    }
  }
}
