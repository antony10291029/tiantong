using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Tiantong.Iot
{
  public abstract class State: IState
  {
    public int _id;

    public string _name;

    public string _address;

    public int _length;

    public IStateDriver _driver;

    public Interval _collectInterval;

    public Interval _heartbeatInterval;

    public IntervalManager _intervalManager;

    public IWatcherProvider _watcherProvider;

    protected abstract void HandleDriverBuild();

  }

  public abstract class State<T>: State, IState<T>
  {
    protected T _heartbeatMaxValue;

    private List<Func<T, Task>> _gethooks = new List<Func<T, Task>>();

    private List<Func<T, Task>> _sethooks = new List<Func<T, Task>>();

    public T CurrentValue;

    public IState<T> Use(IStatePlugin plugin)
    {
      plugin?.Install(this);

      return this;
    }

    public IState<T> Id(int id)
    {
      _id = id;

      return this;
    }

    public IState<T> Name(string name)
    {
      _name = name;

      return this;
    }

    public IState<T> Address(string address)
    {
      _address = address;

      return this;
    }

    public IState<T> Length(int length)
    {
      _length = length;

      return this;
    }

    public IState<T> Build()
    {
      HandleDriverBuild();
      _driver.UseAddress(_address);

      return this;
    }

    public void AddSetHook(Func<T, Task> hook)
    {
      _sethooks.Add(hook);
    }

    public void AddGetHook(Func<T, Task> hook)
    {
      _gethooks.Add(hook);
    }

    public void AddSetHook(Action<T> hook)
    {
      AddSetHook(data => Task.Run(() => hook(data)));
    }

    public void AddGetHook(Action<T> hook)
    {
      AddGetHook(data => Task.Run(() => hook(data)));
    }

    protected IWatcher<T> CreateWatcher()
    {
      var watcher = _watcherProvider.Resolve<T>();

      AddGetHook(value => watcher.Emit(value));

      return watcher;
    }

    public IState<T> Watch(Action<T> handler)
    {
      CreateWatcher().When(_ => true).On(handler);

      return this;
    }

    public IWatcher<T> Watch()
    {
      return CreateWatcher().When(_ => true);
    }

    public IWatcher<T> Watch(T value)
    {
      return CreateWatcher().When(data => CompareTo(data, value) == 0);
    }

    public IWatcher<T> Watch(Func<T, bool> cmp)
    {
      return CreateWatcher().When(cmp);
    }

    public IWatcher<T> Watch(string opt, T value)
    {
      Func<T, bool> cmp = opt switch {
        ">"  => data => CompareTo(data, value) > 0,
        "<"  => data => CompareTo(data, value) < 0,
        "="  => data => CompareTo(data, value) == 0,
        "==" => data => CompareTo(data, value) == 0,
        ">=" => data => CompareTo(data, value) >= 0,
        "<=" => data => CompareTo(data, value) <= 0,
        "<>" => data => CompareTo(data, value) != 0,
        "!=" => data => CompareTo(data, value) != 0,
        _    => throw new Exception($"不支持该运算符{opt}")
      };

      return CreateWatcher().When(data => cmp(data));
    }

    protected virtual int CompareTo(T data, T value)
    {
      throw new Exception("该操作暂时不支持");
    }

    public IState<T> Collect(int time = 1000)
    {
      if (_collectInterval == null) {
        time = Math.Max(time, 1);
        _collectInterval = new Interval(() => Get(), time);
        _intervalManager.Add(_collectInterval);
      }

      return this;
    }

    public void Set(T data)
    {
      var tasks = new List<Task>();
      HandleSet(data);

      foreach (var hook in _sethooks) {
        hook(data);
      }
    }

    public T Get()
    {
      var tasks = new List<Task>();
      var data = CurrentValue = HandleGet();

      foreach (var hook in _gethooks) {
        hook(data);
      }

      return data;
    }

    protected abstract T HandleGet();

    protected abstract void HandleSet(T data);

    public virtual IState<T> Heartbeat(int interval = 1000)
    {
      throw new Exception("该类型不支持心跳写入");
    }

    public IState<T> HeartbeatMaxValue(T maxValue)
    {
      _heartbeatMaxValue = maxValue;

      return this;
    }

  }

}
