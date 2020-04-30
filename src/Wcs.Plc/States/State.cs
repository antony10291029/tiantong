using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  class Hooks<T> : Dictionary<int, Func<T, Task>> {};

  public abstract class State
  {
    public string Name;

    public Event Event;

    public IStateDriver Driver;

    public IntervalManager IntervalManager;

    public Task HookTasks;

    public abstract string Key { get; set; }

    public abstract int Length { get; set; }

    public void UseAddress(string key, int length)
    {
      Key = key;
      Length = length;
      Driver.SetAddress(key, length);
    }

  }

  public class StateConversationException : Exception
  {
    public string From;

    public string To;

    public override string Message
    {
      get => $"fail to convert state from `{From}` to `{To}`";
    }

    public StateConversationException()
    {

    }

    public StateConversationException(string from, string to)
    {
      To = to;
      From = from;
    }
  }

  public abstract class State<T> : State, IState<T>
  {
    public T CurrentValue;

    private Hooks<T> _gethooks = new Hooks<T>();

    private Hooks<T> _sethooks = new Hooks<T>();

    private int _id = 0;

    private Interval _interval;

    public override string Key { get; set; }

    public override int Length { get; set; }

    ~State()
    {
      if (_interval != null) {
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

    public S Convert<S>() where S : IState
    {
      return (S)(object) this;
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

    protected IWatcher<T> CreateWatcher()
    {
      var watcher = new Watcher<T>(Event);

      AddGetHook(value => watcher.Handle(value));

      return watcher;
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

    public void On(string key, Func<T, Task> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Action<T> handler)
    {
      Event.On<T>(key, handler);
    }

    public IState Collect(int time = 1000)
    {
      time = Math.Max(time, 1);
      _interval = new Interval(() => Get(), time);
      IntervalManager.Add(_interval);

      return this;
    }

    public Task UncollectAsync()
    {
      Uncollect();

      return _interval.WaitAsync();
    }

    public void Uncollect()
    {
      IntervalManager.Remove(_interval);
      _interval.WaitAsync().ContinueWith(_ => {
        _interval = null;
      });
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
