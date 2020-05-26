using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public abstract class State: IState
  {
    public DateTime CurrentValueChangedAt { get; set; } = DateTime.Now;

    protected int _id { get; set; }

    protected int _plcId;

    protected string _name { get; set; }

    protected string _address;

    protected int _length;

    protected bool _isReadLogOn = false;

    protected bool _isWriteLogOn = false;

    protected Action<PlcStateError> _onError = _ => {};

    public IStateDriver _driver;

    public int Id() => _id;

    public string Name() => _name;

    public bool IsReadLogOn() => _isReadLogOn;

    public bool IsWriteLogOn() => _isWriteLogOn;

    private IState Builder(Action handler)
    {
      handler();

      return this;
    }

    public IState Id(int id) => Builder(() => _id = id);

    public IState PlcId(int plcId) => Builder(() => _plcId = plcId);

    public IState Name(string name) => Builder(() => _name = name);

    public IState Address(string address) => Builder(() => _address = address);

    public IState Length(int length) => Builder(() => _length = length);

    public IState IsReadLogOn(bool value) => Builder(() => _isReadLogOn = value);

    public IState IsWriteLogOn(bool value) => Builder(() => _isWriteLogOn = value);

    public IState OnError(Action<PlcStateError> onError)
    {
      _onError = onError;

      return this;
    }

    public IState Build(IStateDriver driver)
    {
      _driver = driver;
      HandleDriverBuild();
      _driver.UseAddress(_address);

      return this;
    }

    public abstract void SetString(string data);

    protected abstract void HandleDriverBuild();

    public abstract string CollectString(int cacheInterval = 1000);

    public abstract void AddGetHook(Action<string> hook);

  }

  public abstract class State<T>: State, IState<T>
  {
    private List<Action<T>> _gethooks = new List<Action<T>>();

    private List<Action<T>> _sethooks = new List<Action<T>>();

    private T _currentValue = default(T);

    // private Task<T> _getTask = null;

    // private Task<T> _setTask = null;

    private DateTime _currentValueGetAt = DateTime.MinValue;

    public void AddSetHook(Action<T> hook)
    {
      _sethooks.Add(data => hook(data));
    }

    public void AddGetHook(Action<T> hook)
    {
      _gethooks.Add(data => hook(data));
    }

    public override void AddGetHook(Action<string> hook)
    {
      AddGetHook(value => hook(value.ToString()));
    }

    public override string CollectString(int cacheInterval = 1000)
    {
      if (_currentValueGetAt.AddMilliseconds(cacheInterval) <= DateTime.Now) {
        return Get().ToString();
      } else {
        return _currentValue.ToString();
      }
    }

    public T Collect(int cacheInterval = 1000)
    {
      if (_currentValueGetAt.AddMilliseconds(cacheInterval) < DateTime.Now) {
        return Get();
      } else {
        return _currentValue;
      }
    }

    //

    public void Set(T data)
    {
      try {
        HandleSet(data);
      } catch (Exception e) {
        _onError(new PlcStateError {
          state_id = _id,
          plc_id = _plcId,
          operation = StateOperation.Write,
          value = data.ToString(),
          message = e.Message,
        });

        throw e;
      }

      foreach (var hook in _sethooks) {
        Task.Run(() => hook(data));
      }
    }

    public T Get()
    {
      try {
        _currentValue = HandleGet();
      } catch (Exception e) {
        _onError(new PlcStateError {
          state_id = _id,
          plc_id = _plcId,
          operation = StateOperation.Write,
          value = "",
          message = e.Message,
        });

        throw e;
      }

      _currentValueGetAt = DateTime.Now;

      foreach (var hook in _gethooks) {
        Task.Run(() => hook(_currentValue));
      }

      return _currentValue;
    }

    protected abstract T HandleGet();

    protected abstract void HandleSet(T data);

  }

}
