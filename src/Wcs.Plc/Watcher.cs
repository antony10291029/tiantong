using System;

namespace Wcs.Plc
{
  public class Watcher<T> : IWatcher<T>
  {
    private Action _cancel;

    private Action<T> _handler;

    protected Func<T, bool> _when;

    public void Emit(T value)
    {
      if (_when(value)) {
        _handler(value);
      }
    }

    public IWatcher<T> When(Func<T, bool> when)
    {
      _when = when;

      return this;
    }

    public void On(Action<T> handler)
    {
      _handler = handler;
    }

    public void OnCancel(Action cancel)
    {
      _cancel = cancel;
    }

  }
}
