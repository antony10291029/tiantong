using System;
using System.Threading.Tasks;

namespace Tiantong.Iot
{
  public abstract class StateNumeric<T> : State<T>
  {
    protected abstract void HandleHeartbeat(ref int times);

    public override IState<T> Heartbeat(int time)
    {
      int times = 0;

      time = Math.Max(time, 1);
      HeartbeatInterval = new Interval();
      HeartbeatInterval.SetTime(time);
      HeartbeatInterval.SetHandler(() => HandleHeartbeat(ref times));
      IntervalManager.Add(HeartbeatInterval);

      return this;
    }

  }
}
