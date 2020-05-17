using System;

namespace Tiantong.Iot
{
  public abstract class Watcher: IWatcher
  {
    protected int _id;

    public IWatcher Id(int id)
    {
      _id = id;

      return this;
    }

    public abstract void On(Action handler);

    public abstract void On(Action<string> handler);

    public abstract IWatcher When(string opt, string value);

  }

  public class Watcher<T> : Watcher, IWatcher<T>
  {
    protected Action<T> _handler;

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

    public override IWatcher When(string opt, string value)
    {
      _when = opt switch {
        ""   => _ => true,
        "="  => data => data.ToString() == value,
        "==" => data => data.ToString() == value,
        "!=" => data => data.ToString() != value,
        "<>" => data => data.ToString() != value,
        ">"  => data => double.Parse(data.ToString()) > double.Parse(value),
        "<"  => data => double.Parse(data.ToString()) < double.Parse(value),
        ">=" => data => double.Parse(data.ToString()) >= double.Parse(value),
        "<=" => data => double.Parse(data.ToString()) <= double.Parse(value),
        _  => throw new Exception("暂时不支持该操作符")
      };

      return this;
    }

    public void On(Action<T> handler)
    {
      _handler = handler;
    }

    public override void On(Action handler)
    {
      On(_ => handler());
    }

    public override void On(Action<string> handler)
    {
      On(data => handler(data.ToString()));
    }

  }

}
