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

    public IState State(int id) => _statesById[id];

    public IState State(string name) => _statesByName[name];

    public IState<T> State<T>(int id) => _statesById[id] as IState<T>;

    public IState<T> State<T>(string name) => _statesByName[name] as IState<T>;

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

    public Action<PlcStateError> OnStateError() => _onStateError;

    public List<IState> States() => _states;

    public PlcClientOptions Id(int id) => Builder(() => _id = id);

    public PlcClientOptions Name(string name) => Builder(() => _name = name);

    public PlcClientOptions Model(string model) => Builder(() => _model = model);

    public PlcClientOptions Host(string host) => Builder(() => _host = host);

    public PlcClientOptions Port(int port) => Builder(() => _port = port);

    public PlcClientOptions OnStateError(Action<PlcStateError> onStateError)
      => Builder(() => _onStateError = onStateError);

    public IStateDriverProvider ResolveDriverProvider() => Model() switch {
      PlcModel.Test => new StateTestDriverProvider(),
      PlcModel.MC3E => new MC3EBinaryDriverProvider(Host(), Port()),
      PlcModel.MC3EBinary => new MC3EBinaryDriverProvider(Host(), Port()),
      PlcModel.S7200Smart=> new S7200SmartDriverProvider(Host(), Port()),
      _ => throw new Exception("plc model is not supporting"),
    };

    //

    private IState<T> AddState<T>(IState<T> state)
    {
      _states.Add(state);

      return state;
    }

    public IState<ushort> UInt16()
    {
      return AddState(new StateUInt16());
    }

    public IState<int> Int()
    {
      return AddState(new StateInt32());
    }

    public IState<string> String()
    {
      return AddState(new StateString());
    }

  }

}
