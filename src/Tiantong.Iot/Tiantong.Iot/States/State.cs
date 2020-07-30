using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public abstract class State: IState
  {
    public DateTime CurrentValueChangedAt { get; set; } = DateTime.MinValue;

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

    public int Length() => _length;

    public string Name() => _name;

    public string Address() => _address;

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

    public void AddGetHook(Action<string> hook)
      => AddGetHook((val, oldVal) => hook(val));

    public abstract string Get();

    public abstract string Collect(int interval);

    public abstract void Set(string value);

    protected abstract void HandleDriverBuild();

    public abstract void AddSetHook(Action<string> hook);

    public abstract void AddGetHook(Action<string, string> hook);

  }

  public abstract class State<T>: State, IState<T>
  {
    private List<Action<string, string>> _gethooks = new List<Action<string, string>>();

    private List<Action<string>> _sethooks = new List<Action<string>>();

    private string _currentValue;

    private DateTime _currentValueGetAt = DateTime.MinValue;

    public override void AddGetHook(Action<string, string> hook)
      => _gethooks.Add(hook);

    public override void AddSetHook(Action<string> hook)
      => _sethooks.Add(hook);

    public override string Collect(int collectInterval = 1000)
    {
      if (_currentValueGetAt.AddMilliseconds(collectInterval) > DateTime.Now) {
        return _currentValue;
      } else {
        return Get();
      }
    }

    //

    public override string Get()
    {
      var value = "";

      try {
        value = ToString(HandleGet());
      } catch (Exception e) {
        _onError(new PlcStateError {
          state_id = _id,
          plc_id = _plcId,
          operation = StateOperation.Write,
          value = value,
          message = e.Message,
        });

        throw e;
      }

      if (!value.Equals(_currentValue)) {
        var oldValue = _currentValue;

        foreach (var hook in _gethooks) {
          Task.Run(() => hook(value, oldValue));
        }
      }

      _currentValue = value;
      _currentValueGetAt = DateTime.Now;

      return value;
    }

    public override void Set(string value)
    {
      var data = FromString(value);

      try {
        HandleSet(data);
      } catch (Exception e) {
        _onError(new PlcStateError {
          state_id = _id,
          plc_id = _plcId,
          operation = StateOperation.Write,
          value = value,
          message = e.Message,
        });

        throw e;
      }

      foreach (var hook in _sethooks) {
        Task.Run(() => hook(value));
      }
    }

    public virtual string ToString(T value) => value.ToString();

    public abstract T FromString(string value);

    protected abstract T HandleGet();

    protected abstract void HandleSet(T data);
  }
}
