using System;

namespace Tiantong.Iot.Plc
{
  public class Watcher<T>
  {
    private Action<T> _handler;

    protected Func<T, bool> _when;

    public void Emit(T value)
    {
      if (_when(value)) {
        _handler(value);
      }
    }

    public Watcher<T> When(Func<T, bool> when)
    {
      _when = when;

      return this;
    }

    public void On(Action<T> handler)
    {
      _handler = handler;
    }

  }
}
