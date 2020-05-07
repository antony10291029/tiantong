using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  //

  public abstract class State: IState
  {
    public string Name;

    public IntervalManager IntervalManager;

    public Interval CollectInterval;

    public Interval HeartbeatInterval;

    public IStateDriver Driver;

    public string Address { get; set; }

    public int Length { get; set; }

    public void UseDriver(IStateDriver driver)
    {
      Driver = driver;
      HandleDriverResolved();
    }

    protected abstract void HandleDriverResolved();

    public void UseAddress(string address)
    {
      Address = address;
      Driver.UseAddress(address);
    }

  }

  public abstract class State<T>: State, IState<T>
  {
    protected T _heartbeatMaxValue;

    private List<Func<T, Task>> _gethooks = new List<Func<T, Task>>();

    private List<Func<T, Task>> _sethooks = new List<Func<T, Task>>();

    public T CurrentValue;

    public void Use(IStatePlugin plugin)
    {
      if (plugin != null) {
        plugin.Install(this);
      }
    }

    public IState<T> AddSetHook(Func<T, Task> hook)
    {
      _sethooks.Add(hook);

      return this;
    }

    public IState<T> AddGetHook(Func<T, Task> hook)
    {
      _gethooks.Add(hook);

      return this;
    }

    public IState<T> AddSetHook(Action<T> hook)
    {
      return AddSetHook(data => Task.Run(() => hook(data)));
    }

    public IState<T> AddGetHook(Action<T> hook)
    {
      return AddGetHook(data => Task.Run(() => hook(data)));
    }

    protected Watcher<T> CreateWatcher()
    {
      var watcher = new Watcher<T>();

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
      return CreateWatcher().When(data => CompareDataTo(data, value) == 0);
    }

    public IWatcher<T> Watch(Func<T, bool> cmp)
    {
      return CreateWatcher().When(cmp);
    }

    public IWatcher<T> Watch(string opt, T value)
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

      return CreateWatcher().When(data => cmp(data));
    }

    protected virtual int CompareDataTo(T data, T value)
    {
      throw new Exception("该操作暂时不支持");
    }

    public IState<T> Collect(int time = 1000)
    {
      if (CollectInterval == null) {
        time = Math.Max(time, 1);
        CollectInterval = new Interval(() => Get(), time);
        IntervalManager.Add(CollectInterval);
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
