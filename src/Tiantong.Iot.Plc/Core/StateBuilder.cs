using System;
using System.Collections.Generic;
namespace Tiantong.Iot.Plc
{
  public class StateBuilder
  {

  }

  public abstract class StateBuilder<T>: StateBuilder
  {
    private int _id;

    private string _name;

    private string _address;

    protected int _length;

    private bool _isCollect;

    private int _collectInterval;

    private bool _isHeartbeat;

    private int _heartbeatInterval;

    private int _heartbeatMaxValue;

    private List<Watcher<T>> _watchers;

    public StateBuilder<T> Name(string name)
    {
      _name = name;

      return this;
    }

    public StateBuilder<T> Address(string address)
    {
      _address = address;

      return this;
    }

    public StateBuilder<T> Length(int length)
    {
      _length = length;

      return this;
    }

    public StateBuilder<T> Collect(int interval = 1000)
    {
      _isCollect = true;
      _collectInterval = interval;

      return this;
    }

    public StateBuilder<T> Heartbeat(int interval = 1000, int maxValue = 10000)
    {
      _isHeartbeat = true;
      _heartbeatInterval = interval;
      _heartbeatMaxValue = maxValue;

      return this;
    }

    protected abstract int CompareDataTo(T data, T value);

    public Watcher<T> Watch(Action<T> handler)
    {
      var watcher = new Watcher<T>().When(_ => true);
      watcher.On(handler);
      _watchers.Add(watcher);

      return watcher;
    }

    public Watcher<T> Watch()
    {
      var watcher = new Watcher<T>().When(_ => true);
      _watchers.Add(watcher);

      return watcher;
    }

    public Watcher<T> Watch(T value)
    {
      var watcher = new Watcher<T>().When(data => CompareDataTo(data, value) == 0);
      _watchers.Add(watcher);

      return watcher;
    }

    public Watcher<T> Watch(Func<T, bool> cmp)
    {
      var watcher = new Watcher<T>().When(cmp);
      _watchers.Add(watcher);

      return watcher;
    }

    public Watcher<T> Watch(string opt, T value)
    {
      Func<T, bool> cmp = opt switch {
        ">"  => data => CompareDataTo(data, value) > 0,
        "<"  => data => CompareDataTo(data, value) < 0,
        "="  => data => CompareDataTo(data, value) == 0,
        "==" => data => CompareDataTo(data, value) == 0,
        ">=" => data => CompareDataTo(data, value) >= 0,
        "<=" => data => CompareDataTo(data, value) <= 0,
        "<>" => data => CompareDataTo(data, value) != 0,
        "!=" => data => CompareDataTo(data, value) != 0,
        _    => throw new Exception($"不支持该运算符{opt}")
      };

      var watcher = new Watcher<T>().When(data => cmp(data));
      _watchers.Add(watcher);

      return watcher;
    }

    //

    protected abstract StateWorker<T> BuildWorker(IStateDriver driver);

    //

    public StateWorker<T> Build(IStateDriver driver)
    {
      var state = BuildWorker(driver);

      return state;
    }
  }
}
