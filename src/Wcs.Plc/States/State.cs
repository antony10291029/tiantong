using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  class Hooks<T> : Dictionary<int, Func<T, Task>> {};

  public abstract class State
  {
    public string Name;

    public IStateDriver Driver;

    public IntervalManager IntervalManager;

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

  //

  public class StateHook<T> : IStateHook<T>
  {
    private Action _cancel;

    public StateHook(Action cancel)
    {
      _cancel = cancel;
    }

    public void Cancel() => _cancel();
  }

  //

  public abstract class State<T> : State, IState<T>
  {
    public T CurrentValue;

    public Interval CollectInterval;

    private Hooks<T> _gethooks = new Hooks<T>();

    private Hooks<T> _sethooks = new Hooks<T>();

    private int _id = 0;

    ~State()
    {
      if (CollectInterval != null) {
        Uncollect();
      }
    }

    //

    public void Use(IStatePlugin plugin)
    {
      if (plugin != null) {
        plugin.Install(this);
      }
    }

    public IStateHook<T> AddSetHook(Func<T, Task> hook)
    {
      int id = _id++;
      _sethooks.Add(id, hook);

      return new StateHook<T>(() => RemoveSetHook(id));
    }

    public IStateHook<T> AddGetHook(Func<T, Task> hook)
    {
      int id = _id++;
      _gethooks.Add(id, hook);

      return new StateHook<T>(() => RemoveGetHook(id));
    }

    public IStateHook<T> AddSetHook(Action<T> hook)
    {
      return AddSetHook(data => Task.Run(() => hook(data)));
    }

    public IStateHook<T> AddGetHook(Action<T> hook)
    {
      return AddGetHook(data => Task.Run(() => hook(data)));
    }

    private void RemoveSetHook(int id)
    {
      _gethooks.Remove(id);
    }

    private void RemoveGetHook(int id)
    {
      _sethooks.Remove(id);
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

    protected abstract int CompareDataTo(T data, T value);

    public IState<T> Collect(int time = 1000)
    {
      time = Math.Max(time, 1);
      CollectInterval = new Interval(() => Get(), time);
      IntervalManager.Add(CollectInterval);

      return this;
    }

    public Task UncollectAsync()
    {
      return IntervalManager.RemoveAsync(CollectInterval);
    }

    public void Uncollect()
    {
      UncollectAsync().GetAwaiter().GetResult();
    }

    public void Set(T data)
    {
      var tasks = new List<Task>();
      HandleSet(data);

      foreach (var hook in _sethooks.Values) {
        hook(data);
      }
    }

    public T Get()
    {
      var tasks = new List<Task>();
      var data = CurrentValue = HandleGet();

      foreach (var hook in _gethooks.Values) {
        hook(data);
      }

      return data;
    }

    protected abstract T HandleGet();

    protected abstract void HandleSet(T data);
  }
}
