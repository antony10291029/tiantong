using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public abstract class StateNumeric<T> : State<T>
  {
    private Interval _interval;

    ~StateNumeric()
    {
      if (_interval != null) {
        Unheartbeat();
      }
    }

    protected abstract void HandleHeartbeat(ref T times, ref T maxValue);

    public IState<T> Heartbeat(int time, T maxValue)
    {
      T times = default;

      time = Math.Max(time, 1);
      _interval = new Interval();
      _interval.SetTime(time);
      _interval.SetHandler(() => HandleHeartbeat(ref times, ref maxValue));
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
