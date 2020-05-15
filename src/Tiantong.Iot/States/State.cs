using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public abstract class State: IState
  {
    public DateTime CurrentValueChangedAt { get; set; } = DateTime.Now;

    public int _id;

    public int _plcId;

    public string _name;

    public string _address;

    public int _length;

    public bool _isReadLogOn = false;

    public bool _isWriteLogOn = false;

    public IStateDriver _driver;

    public Interval _collectInterval;

    public Interval _heartbeatInterval;

    public IntervalManager _intervalManager;

    public IWatcherProvider _watcherProvider;

    protected StateErrorLogger _errorLogger;

    protected abstract void HandleDriverBuild();

    public abstract IState Collect(int interval = 1000);

    public virtual IState Heartbeat(int interval = 1000, int maxValue = 10000)
    {
      throw new Exception("该类型不支持心跳写入");
    }

    public abstract IState Use(IStatePlugin plugin);

    public IState UseErrorLogger(StateErrorLogger logger)
    {
      _errorLogger = logger;

      return this;
    }

    public IState Id(int id)
    {
      _id = id;

      return this;
    }

    public IState PlcId(int plcId)
    {
      _plcId = plcId;

      return this;
    }

    public IState Name(string name)
    {
      _name = name;

      return this;
    }

    public IState Address(string address)
    {
      _address = address;

      return this;
    }

    public IState Length(int length)
    {
      _length = length;

      return this;
    }

    public IState IsReadLogOn(bool flag)
    {
      _isReadLogOn = flag;

      return this;
    }

    public IState IsWriteLogOn(bool flag)
    {
      _isWriteLogOn = flag;

      return this;
    }

    public IState Build()
    {
      HandleDriverBuild();
      _driver.UseAddress(_address);

      return this;
    }

    public abstract void SetString(string data);

    public abstract void Watch(Action handler);

    public abstract void Watch(Action<string> handler);

    public abstract IWatcher When(string opt, string value);

    public abstract string GetCurrentValue(int timeGapMilliseconds);

  }

  public abstract class State<T>: State, IState<T>
  {
    private List<Func<T, Task>> _gethooks = new List<Func<T, Task>>();

    private List<Func<T, Task>> _sethooks = new List<Func<T, Task>>();

    private T _currentValue = default(T);

    private DateTime _currentValueGetAt = DateTime.MinValue;

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

    public override IState Use(IStatePlugin plugin)
    {
      plugin?.Install(this);

      return this;
    }

    public override string GetCurrentValue(int timeGapMilliseconds = 1000)
    {
      if (_currentValueGetAt.AddMilliseconds(timeGapMilliseconds) < DateTime.Now) {
        return Get()?.ToString();
      } else {
        return _currentValue?.ToString();
      }
    }

    //

    private IWatcher<T> CreateWatcher()
    {
      var watcher = _watcherProvider.Resolve<T>();

      AddGetHook(value => watcher.Emit(value));

      return watcher;
    }

    //

    public override void Watch(Action handler)
    {
      CreateWatcher().When(_ => true).On(handler);
    }

    public override void Watch(Action<string> handler)
    {
      CreateWatcher().When(_ => true).On(data => handler(data.ToString()));
    }

    public override IWatcher When(string opt, string value)
    {
      return CreateWatcher().When(opt, value);
    }

    public override IState Collect(int time = 1000)
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
      try {
        HandleSet(data);
      } catch (Exception e) {
        _errorLogger.Log(_plcId, _id, StateOperation.Write, data.ToString(), e.Message);
        throw e;
      }

      foreach (var hook in _sethooks) {
        hook(data);
      }
    }

    public T Get()
    {
      try {
        _currentValue = HandleGet();
      } catch (Exception e) {
        _errorLogger.Log(_plcId, _id, StateOperation.Read, null, e.Message);
        throw e;
      }

      _currentValueGetAt = DateTime.Now;

      foreach (var hook in _gethooks) {
        hook(_currentValue);
      }

      return _currentValue;
    }

    protected abstract T HandleGet();

    protected abstract void HandleSet(T data);

  }

}
