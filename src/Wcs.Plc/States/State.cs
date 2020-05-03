using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  //

  public abstract class StateBuilder: IState
  {
    public IntervalManager IntervalManager;

    public Interval CollectInterval;

    public Interval HeartbeatInterval;

    public string Name;

    public IStateDriver Driver;

    public Task HookTasks;

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

  public abstract class StateBuilder<T>: StateBuilder, IStateBuilder<T>
  {

    private List<Func<T, Task>> _gethooks = new List<Func<T, Task>>();

    private List<Func<T, Task>> _sethooks = new List<Func<T, Task>>();

    public T CurrentValue;

    public IStateBuilder<T> AddSetHook(Func<T, Task> hook)
    {
      _sethooks.Add(hook);

      return this;
    }

    public IStateBuilder<T> AddGetHook(Func<T, Task> hook)
    {
      _gethooks.Add(hook);

      return this;
    }

    public IStateBuilder<T> AddSetHook(Action<T> hook)
    {
      return AddSetHook(data => Task.Run(() => hook(data)));
    }

    public IStateBuilder<T> AddGetHook(Action<T> hook)
    {
      return AddGetHook(data => Task.Run(() => hook(data)));
    }

    protected Watcher<T> CreateWatcher()
    {
      var watcher = new Watcher<T>();

      AddGetHook(value => watcher.Emit(value));

      return watcher;
    }

    public IStateBuilder<T> Watch(Action<T> handler)
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
      return CreateWatcher().When(data => {
        switch (opt) {
          case ">":
            return CompareDataTo(data, value) > 0;
          case "<":
            return CompareDataTo(data, value) < 0;
          case "=":
          case "==":
            return CompareDataTo(data, value) == 0;
          case ">=":
            return CompareDataTo(data, value) >= 0;
          case "<=":
            return CompareDataTo(data, value) <= 0;
          case "!=":
          case "<>":
            return CompareDataTo(data, value) != 0;
          default:
            throw new Exception($"Watcher operation '{opt}' is invalid");
        }
      });
    }

    protected virtual int CompareDataTo(T data, T value)
    {
      throw new Exception("该操作暂时不支持");
    }

    public IStateBuilder<T> Collect(int time = 1000)
    {
      time = Math.Max(time, 1);
      CollectInterval = new Interval(() => Get(), time);
      IntervalManager.Add(CollectInterval);

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


    public virtual IStateBuilder<T> Heartbeat(int time, int maxValue)
    {
      throw new Exception("该类型不支持心跳写入");
    }

  }

  public abstract class State<T>: StateBuilder<T>, IState<T>
  {
    //

    public void Use(IStatePlugin plugin)
    {
      if (plugin != null) {
        plugin.Install(this);
      }
    }

  }
}
