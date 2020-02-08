using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateWord : State<int>, IStateWord
  {
    private Interval _interval;

    public override string Type { get => "Word"; }

    public override IStateWord ToWord()
    {
      return this;
    }

    ~StateWord()
    {
      if (_interval != null) {
        Unheartbeat();
      }
    }

    protected override int CompareDataTo(int data, int value)
    {
      return data.CompareTo(value);
    }

    protected override Task<int> HandleGet()
    {
      return StateClient.GetWord();
    }

    protected override Task HandleSet(int data)
    {
      return StateClient.SetWord(data);
    }

    public IStateWord Heartbeat(int time = 1000, int maxTimes = 10000)
    {
      var times = 0;

      time = Math.Max(time, 1);
      _interval = new Interval();
      _interval.SetTime(time);
      _interval.SetHandler(() => {
        if (times < maxTimes) times++;
        else times = 1;

        return SetAsync(times);
      });
      IntervalManager.Add(_interval);

      return this;
    }

    public Task UnheartbeatAsync()
    {
      IntervalManager.Remove(_interval);

      return _interval.WaitAsync();
    }

    public void Unheartbeat()
    {
      UnheartbeatAsync().GetAwaiter().GetResult();      
    }
  }
}
