using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWord : State<int>, IStateWord
  {
    private IInterval _interval;

    public StateWord(IContainer container): base(container)
    {

    }

    ~StateWord()
    {
      if (_interval != null) {
        Unheartbeat();
      }
    }

    protected override Task<int> HandleGet()
    {
      return _stateDriver.GetWord();
    }

    protected override Task HandleSet(int data)
    {
      return _stateDriver.SetWord(data);
    }

    public IStateWord Heartbeat(int time = 1000, int maxTimes = 10000)
    {
      var times = 0;

      _interval = new Interval();
      _interval.SetTime(time);
      _interval.SetHandler(() => {
        if (times < maxTimes) times++;
        else times = 1;

        return SetAsync(times);
      });
      _intervalManager.Add(_interval);

      return this;
    }

    public Task UnheartbeatAsync()
    {
      _intervalManager.Remove(_interval);

      return _interval.WaitAsync();
    }

    public void Unheartbeat()
    {
      UnheartbeatAsync().GetAwaiter().GetResult();      
    }
  }
}
