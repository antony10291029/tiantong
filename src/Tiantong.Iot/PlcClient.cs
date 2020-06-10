using Renet;
using System;
using System.Collections.Generic;
using System.Linq;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcClient
  {
    private PlcClientOptions _options;

    private Dictionary<int, IState> _statesById;

    private Dictionary<string, IState> _statesByName;

    private IStateDriverProvider _driverProvider;

    public PlcClient(PlcClientOptions options)
    {
      _options = options;
      _driverProvider = options.ResolveDriverProvider();

      var states = _options.States().Select(state =>
        state.OnError(_options.OnStateError())
          .Build(_driverProvider.Resolve())
      );

      _statesById = states.ToDictionary(s => s.Id(), s => s);
      _statesByName = states.ToDictionary(s => s.Name(), s => s);
    }

    public PlcClientOptions Options() => _options;

    public Dictionary<int, IState> StatesById() => _statesById;

    public Dictionary<string, IState> StatesByName() => _statesByName;

    public IState State(int id)
    {
      if (!_statesById.ContainsKey(id)) {
        throw KnownException.Error("数据点不存在");
      }

      return _statesById[id];
    }

    public IState State(string name)
    {
      if (!_statesByName.ContainsKey(name)) {
        throw KnownException.Error("数据点不存在");
      }

      return _statesByName[name];
    }

    public IState<T> State<T>(int id)
    {
      if (!_statesById.ContainsKey(id)) {
        throw KnownException.Error("数据点不存在");
      }

      var state = _statesById[id];

      if (!(state is IState<T>)) {
        throw KnownException.Error($"数据点类型错误");
      }

      return state as IState<T>;
    }

    public IState<T> State<T>(string name)
    {
      if (!_statesByName.ContainsKey(name)) {
        throw KnownException.Error($"数据点不存在");
      }

      var state = _statesByName[name];

      if (!(state is IState<T>)) {
        throw KnownException.Error($"数据点类型错误");
      }

      return state as IState<T>;
    }

    public void SetString(IState state, string value)
    {
      state.Build(_driverProvider.Resolve())
        .SetString(value);
    }

    public string GetString(IState state)
    {
      return state.Build(_driverProvider.Resolve())
        .GetString();
    }

    public void Set<T>(int id, T value) => State<T>(id).Set(value);

    public T Get<T>(int id) => State<T>(id).Get();

    public void Set<T>(string name, T value) => State<T>(name).Set(value);

    public T Get<T>(string name) => State<T>(name).Get();

    public void Connect() => _driverProvider.Connect();

    public void Close() => _driverProvider.Close();
  }

  public class PlcClientOptions
  {
    private int _id;

    private string _model = PlcModel.Test;

    private string _name;

    private string _host;

    private int _port;

    private Action<PlcStateError> _onStateError = _ => {};

    private List<IState> _states = new List<IState>();

    private PlcClientOptions Builder(Action builder)
    {
      builder();

      return this;
    }

    public int Id() => _id;

    public string Name() => _name;

    public string Model() => _model;

    public string Host() => _host;

    public int Port() => _port;

    public List<IState> States() => _states;

    public Action<PlcStateError> OnStateError() => _onStateError;

    public PlcClientOptions Id(int id) => Builder(() => _id = id);

    public PlcClientOptions Name(string name) => Builder(() => _name = name);

    public PlcClientOptions Model(string model) => Builder(() => _model = model);

    public PlcClientOptions Host(string host) => Builder(() => _host = host);

    public PlcClientOptions Port(int port) => Builder(() => _port = port);

    public PlcClientOptions OnStateError(Action<PlcStateError> onStateError)
      => Builder(() => _onStateError = onStateError);

    public IStateDriverProvider ResolveDriverProvider() => Model() switch {
      PlcModel.Test => new StateTestDriverProvider(),
      PlcModel.MC3EBinary => new MC3EBinaryDriverProvider(Host(), Port()),
      PlcModel.S7200Smart=> new S7200SmartDriverProvider(Host(), Port()),
      _ => throw new Exception("plc model is not supporting"),
    };

    //

    public IState<T> State<T>()
    {
      var state = typeof(T).Name switch {
        "Boolean" => new StateBool() as IState<T>,
        "UInt16" => new StateUInt16() as IState<T>,
        "Int32" => new StateInt32() as IState<T>,
        "String" => new StateString() as IState<T>,
        _ => throw new Exception($"不支持的数据类型: {typeof(T).Name}")
      };

      _states.Add(state);

      return state;
    }
  }
}