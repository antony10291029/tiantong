using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public abstract class StateNumeric<T> : State<T>
  {
    public Interval HeartbeatInterval;

    ~StateNumeric()
    {
      if (HeartbeatInterval != null) {
        Unheartbeat();
      }
    }

    protected abstract void HandleHeartbeat(ref T times, ref T maxValue);

    public IState<T> Heartbeat(int time, T maxValue)
    {
      T times = default;

      time = Math.Max(time, 1);
      HeartbeatInterval = new Interval();
      HeartbeatInterval.SetTime(time);
      HeartbeatInterval.SetHandler(() => HandleHeartbeat(ref times, ref maxValue));
      IntervalManager.Add(HeartbeatInterval);

      return this;
    }

    public Task UnheartbeatAsync()
    {
      return IntervalManager.RemoveAsync(HeartbeatInterval);
    }

    public void Unheartbeat()
    {
      UnheartbeatAsync().GetAwaiter().GetResult();      
    }
  }
}
