using System;

namespace Wcs.Plc
{
  public class Watcher<T> : IWatcher<T> where T : IComparable
  {
    private IEvent _event;

    private Action _cancel;

    private Action<T> _emmiter;

    protected Func<T, bool> _when;

    public Watcher(IEvent event_)
    {
      _event = event_;
    }

    public void Handle(T value)
    {
      if (_when(value)) {
        _emmiter(value);
      }
    }

    public void OnCancel(Action cancel)
    {
      _cancel = cancel;
    }

    public void Cancel()
    {
      _cancel();
    }

    public IWatcher<T> When(Func<T, bool> when)
    {
      _when = when;

      return this;
    }

    public IWatcher<T> When(string opt, T value)
    {
      switch (opt) {
        case ">":
          _when = data => data.CompareTo(value) > 0;
          break;
        case "<":
          _when = data => data.CompareTo(value) < 0;
          break;
        case ">=":
          _when = data => data.CompareTo(value) >= 0;
          break;
        case "<=":
          _when = data => data.CompareTo(value) <= 0;
          break;
        case "!=":
          _when = data => data.CompareTo(value) != 0;
          break;
        default:
          _when = data => data.CompareTo(value) == 0;
          break;
      }

      return this;
    }

    public void Event(string key)
    {
      _emmiter = value => _event.Emit(key, value);
    }

    public void EventVoid(string key)
    {
      _emmiter = _ => _event.Emit(key);
    }

    public void Event<R>(string key, R payload)
    {
      _emmiter = _ => _event.Emit<R>(key, payload);
    }

    public void Event(string key, Func<T, T> handler)
    {
      _emmiter = value => _event.Emit(key, handler(value));
    }

    public void Event<R>(string key, Func<T, R> handler)
    {
      _emmiter = value => _event.Emit<R>(key, handler(value));
    }
  }
}
