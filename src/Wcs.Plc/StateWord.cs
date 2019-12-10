using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWord : State<int>, IStateWord
  {
    protected int _heartbeatIntervalId = 0;

    public StateWord(IContainer container): base(container)
    {

    }
    
    ~StateWord()
    {
      if (_heartbeatIntervalId != 0) {
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

    public IStateWord Heartbeat(int time = 1000)
    {
      var interval = new Interval();
      var times = 1;

      interval.SetTime(time);
      interval.SetHandler(() => SetAsync(times++));
      _heartbeatIntervalId = _intervalManager.Add(interval);

      return this;
    }

    public Task UnheartbeatAsync()
    {
      return _intervalManager.RemoveAsync(_heartbeatIntervalId);
    }

    public void Unheartbeat()
    {
      UnheartbeatAsync().GetAwaiter().GetResult();      
    }
  }
}