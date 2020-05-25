using System;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class StateManager : IStateManager
  {
    private IntervalManager _intervalManager;

    private IStateDriverProvider _stateDriverProvider;

    private IHttpPusherClient _httpPusherClient;

    private StateErrorLogger _stateErrorLogger;

    public Dictionary<int, IState> StatesById = new Dictionary<int, IState>();

    public Dictionary<string, IState> StatesByName = new Dictionary<string, IState>();

    public int _id { get; set; }

    private string _name { get; set; }

    public StateManager(
      IntervalManager manager,
      IStateDriverProvider provider,
      IHttpPusherClient httpPusherClient,
      StateErrorLogger stateErrorLogger
    ) {
      _intervalManager = manager;
      _stateDriverProvider = provider;
      _httpPusherClient = httpPusherClient;
      _stateErrorLogger = stateErrorLogger;
    }

    public IStateManager Name(string name)
    {
      _name = name;

      return this;
    }

    public IStateManager Id(int id)
    {
      _id = id;

      return this;
    }

    private void Add(IState state)
    {
      StatesByName.Add(_name, state);
      if (_id != 0) {
        StatesById.Add(_id, state);
      }
    }

    private void ResolveState<T, U>(Action<T> builder, string address, int length = 0) where T : State<U>, new()
    {
      var state = new T() {
        _intervalManager = _intervalManager,
        _httpPusherClient = _httpPusherClient,
        _driver = _stateDriverProvider.Resolve(),
      };

      if (builder != null) {
        builder(state);
      }

      Add(
        state.Id(_id).Name(_name)
        .Address(address).Length(length)
        .UseErrorLogger(_stateErrorLogger)
        .Build()
      );
    }

    public void Bool(string address, Action<IState<bool>> builder = null)
    {
      ResolveState<StateBool, bool>(builder, address);
    }

    public void UInt16(string address, Action<IState<ushort>> builder = null)
    {
      ResolveState<StateUInt16, ushort>(builder, address);
    }

    public void Int32(string address, Action<IState<int>> builder = null)
    {
      ResolveState<StateInt32, int>(builder, address);
    }

    public void String(string address, int length, Action<IState<string>> builder = null)
    {
      ResolveState<StateString, string>(builder, address, length);
    }

    public void UShort(string address, Action<IState<ushort>> builder = null)
    {
      UInt16(address, builder);
    }

    public void Int(string address, Action<IState<int>> builder = null)
    {
      Int32(address, builder);
    }

  }

}
