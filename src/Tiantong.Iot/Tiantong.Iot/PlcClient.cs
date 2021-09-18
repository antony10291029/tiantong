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

      var states = _options.States.Select(state =>
        state.OnError(_options.OnStateError)
          .OnLog(_options.OnStateLog)
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

    public void Set(IState state, string value)
    {
      state.Build(_driverProvider.Resolve()).Set(value);
    }

    public string Get(IState state)
    {
      return state.Build(_driverProvider.Resolve()).Get();
    }

    public void Connect() => _driverProvider.Connect();

    public void Close() => _driverProvider.Close();
  }

  public class PlcClientOptions
  {
    public readonly int Id;

    public readonly string Name;

    public readonly string Model;

    public readonly string Host;

    public readonly int Port;

    public readonly List<IState> States = new List<IState>();

    public readonly Action<PlcStateError> OnStateError;

    public readonly Action<PlcStateLog> OnStateLog;

    public PlcClientOptions(
      int id, string name,
      string model, string host, int port,
      Action<PlcStateError> onStateError,
      Action<PlcStateLog> onStateLog
    ) {
      Id = id;
      Name = name;
      Model = model;
      Host = host;
      Port = port;
      OnStateError = onStateError;
      OnStateLog = onStateLog;
    }

    private PlcClientOptions Configure(Action handler)
    {
      handler();

      return this;
    }

    public IStateDriverProvider ResolveDriverProvider() => Model switch {
      PlcModel.Test => new StateTestDriverProvider(),
      PlcModel.MC1EBinary => new MC1EBinaryDriverProvider(Host, Port),
      PlcModel.MC3EBinary => new MC3EBinaryDriverProvider(Host, Port),
      PlcModel.S7200Smart => new S7200SmartDriverProvider(Host, Port),
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

      States.Add(state);

      return state;
    }
  }
}
