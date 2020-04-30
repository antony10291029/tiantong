using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class StateInt : State<int>, IStateInt
  {
    private Interval _interval;

    ~StateInt()
    {
      if (_interval != null) {
        Unheartbeat();
      }
    }

    protected override int CompareDataTo(int data, int value)
    {
      return data.CompareTo(value);
    }

    protected override int HandleGet()
    {
      return Driver.GetInt();
    }

    protected override void HandleSet(int data)
    {
      Driver.SetInt(data);
    }

    public IStateInt Heartbeat(int time = 1000, int maxTimes = 10000)
    {
      var times = 0;

      time = Math.Max(time, 1);
      _interval = new Interval();
      _interval.SetTime(time);
      _interval.SetHandler(() => {
        if (times < maxTimes) times++;
        else times = 1;

        Set(times);
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
