using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Wcs.Plc
{
  class Hooks<T> : Dictionary<int, Func<T, Task>> {};

  public abstract class State<T> : IState<T>
  {
    private Hooks<T> _gethooks = new Hooks<T>();

    private Hooks<T> _sethooks = new Hooks<T>();

    public IPlcContainer Services { get; private set; }

    private int _id = 0;

    private string _key;

    private int _length;

    private IInterval _interval;

    protected IStateDriver _stateDriver;

    protected IIntervalManager _intervalManager
    {
      get => Services.IntervalManager;
    }

    public string Key
    {
      get => _key;
      set {
        _key = value;
        _stateDriver.HandleStateSetKey(value);
      }
    }

    public int Length
    {
      get => _length;
      set {
        _length = value;
        _stateDriver.HandleStateSetLength(value);
      }
    }

    public State(IPlcContainer services)
    {
      Services = services;
      ResolveDriver();
    }

    ~State()
    {
      if (_interval != null) {
        Uncollect();
      }
    }

    //

    public void ResolveDriver()
    {
      _stateDriver = Services.StateDriver.Resolve();
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

    public IState Collect(int time = 1000)
    {
      _interval = new Interval();
      _interval.SetTime(time);
      _interval.SetHandler(GetAsync);
      _intervalManager.Add(_interval);

      return this;
    }

    public Task UncollectAsync()
    {
      Uncollect();

      return _interval.WaitAsync();
    }

    public void Uncollect()
    {
      _intervalManager.Remove(_interval);
      _interval.WaitAsync().ContinueWith(_ => {
        _interval = null;
      });
    }

    public async Task SetAsync(T data)
    {
      _stateDriver.BeforeMessage(this);

      var tasks = new List<Task>();

      await HandleSet(data);

      foreach (var hook in _sethooks.Values) {
        tasks.Add(hook(data));
      }

      await Task.WhenAll(tasks);
    }

    public void Set(T data)
    {
      SetAsync(data).GetAwaiter().GetResult();
    }

    public async Task<T> GetAsync()
    {
      _stateDriver.BeforeMessage(this);

      var tasks = new List<Task>();
      var data = await HandleGet();

      foreach (var hook in _gethooks.Values) {
        tasks.Add(hook(data));
      }
      await Task.WhenAll(tasks);

      return data;
    }

    public T Get()
    {
      return GetAsync().GetAwaiter().GetResult();
    }

    protected abstract Task<T> HandleGet();

    protected abstract Task HandleSet(T data);
  }
}
