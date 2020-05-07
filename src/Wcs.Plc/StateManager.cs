using System;
using System.Collections.Generic;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  using States = Dictionary<string, IState>;

  public class StateManager : IStateManager
  {
    private IStatePlugin _stateLogger;

    private IntervalManager _intervalManager;

    private IStateDriverProvider _stateDriverProvider;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(IntervalManager manager, IStateDriverProvider provider, IStatePlugin stateLogger)
    {
      _stateLogger = stateLogger;
      _intervalManager = manager;
      _stateDriverProvider = provider;
    }

    public IStateManager SetName(string name)
    {
      Name = name;

      return this;
    }

    private T ResolveState<T>(string address, int length = 0) where T : StateBuilder, new()
    {
      var state = new T() {
        IntervalManager = _intervalManager,
      };

      state.Name = Name;
      state.Length = length;
      state.UseDriver(_stateDriverProvider.Resolve());
      state.UseAddress(address);
      States.Add(Name, state);

      return state;
    }

    public IStateBuilder<bool> Bool(string address)
    {
      var state = ResolveState<StateBool>(address);

      state.Use(_stateLogger);

      return state;
    }

    public IStateBuilder<ushort> UInt16(string address)
    {
      var state = ResolveState<StateUInt16>(address);

      state.Use(_stateLogger);

      return state;
    }

    public IStateBuilder<int> Int32(string address)
    {
      var state = ResolveState<StateInt32>(address);

      state.Use(_stateLogger);

      return state;
    }

    public IStateBuilder<string> String(string address, int length)
    {
      var state = ResolveState<StateString>(address, length);

      state.Use(_stateLogger);
      state.Length = length;

      return state;
    }

    public IStateBuilder<ushort> UShort(string address)
    {
      return UInt16(address);
    }

    public IStateBuilder<int> Int(string address)
    {
      return Int32(address);
    }

  }
}
