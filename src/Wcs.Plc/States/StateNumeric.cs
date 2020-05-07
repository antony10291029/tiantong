using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public abstract class StateNumeric<T> : State<T>
  {
    ~StateNumeric()
    {
      if (HeartbeatInterval != null) {
        Unheartbeat();
      }
    }

    protected abstract void HandleHeartbeat(ref int times, ref int maxValue);

    public override IStateBuilder<T> Heartbeat(int time, int maxValue)
    {
      int times = 0;

      time = Math.Max(time, 1);
      HeartbeatInterval = new Interval();
      HeartbeatInterval.SetTime(time);
      HeartbeatInterval.SetHandler(() => HandleHeartbeat(ref times, ref maxValue));
      IntervalManager.Add(HeartbeatInterval);

      return this;
    }

  }
}
