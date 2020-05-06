using System;
using System.Threading.Tasks;

namespace Tiantong.Iot
{
  public abstract class StateNumeric<T> : State<T>
  {
    protected int _heartbeatMaxValue;

    protected abstract void HandleHeartbeat(ref int times);

    public override IState Heartbeat(int time = 1000, int maxValue = 10000)
    {
      if (_heartbeatInterval != null) {
        return this;
      }

      _heartbeatMaxValue = maxValue;
      int times = 0;

      time = Math.Max(time, 1);
      _heartbeatInterval = new Interval();
      _heartbeatInterval.SetTime(time);
      _heartbeatInterval.SetHandler(() => {
        if (times < _heartbeatMaxValue) {
          times++;
        } else {
          times = 1;
        }

        HandleHeartbeat(ref times);
      });
      _intervalManager.Add(_heartbeatInterval);

      return this;
    }

  }
}
