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
      _heartbeatInterval = new Interval();
      _heartbeatInterval.SetTime(time);
      _heartbeatInterval.SetHandler(() => HandleHeartbeat(ref times));
      _intervalManager.Add(_heartbeatInterval);

      return this;
    }

  }
}
