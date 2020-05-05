using System;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class StateManager : IStateManager
  {
    private IStatePlugin _stateLogger;

    private IntervalManager _intervalManager;

    private IStateDriverProvider _stateDriverProvider;

    private IWatcherProvider _watcherProvider;

    public Dictionary<int, IState> StatesById = new Dictionary<int, IState>();

    public Dictionary<string, IState> StatesByName = new Dictionary<string, IState>();

    public int _id { get; set; }

    private string _name { get; set; }

    public StateManager(
      IntervalManager manager,
      IStateDriverProvider provider,
      IStatePlugin stateLogger,
      IWatcherProvider watcherProvider
    ) {
      _stateLogger = stateLogger;
      _intervalManager = manager;
      _stateDriverProvider = provider;
      _watcherProvider = watcherProvider;
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


    private T ResolveState<T, U>(string address, int length = 0) where T : State<U>, new()
    {
      var state = new T() {
        _intervalManager = _intervalManager,
        _watcherProvider = _watcherProvider,
        _driver = _stateDriverProvider.Resolve(),
      };

      Add(
        state.Id(_id).Name(_name)
        .Address(address).Length(length)
        .Use(_stateLogger)
      );

      return state;
    }

    public void Bool(string address, Action<IState<bool>> builder)
    {
      var state = ResolveState<StateBool, bool>(address);
      builder(state);
      state.Build();
    }

    public void UInt16(string address, Action<IState<ushort>> builder)
    {
      var state = ResolveState<StateUInt16, ushort>(address);
      builder(state);
      state.Build();
    }

    public void Int32(string address, Action<IState<int>> builder)
    {
      var state = ResolveState<StateInt32, int>(address);
      builder(state);
      state.Build();
    }

    public void String(string address, int length, Action<IState<string>> builder)
    {
      var state = ResolveState<StateString, string>(address, length);
      builder(state);
      state.Build();
    }

    public void UShort(string address, Action<IState<ushort>> builder)
    {
      UInt16(address, builder);
    }

    public void Int(string address, Action<IState<int>> builder)
    {
      Int32(address, builder);
    }

  }
}
