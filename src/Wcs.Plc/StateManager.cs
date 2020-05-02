using System;
using System.Collections.Generic;
using Wcs.Plc.Entities;

namespace Wcs.Plc
{
  using States = Dictionary<string, State>;

  public class StateManager : IStateManager
  {
    private Event _event;

    private IStatePlugin _stateLogger;

    private IntervalManager _intervalManager;

    private IStateDriverProvider _stateDriverProvider;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(Event event_, IntervalManager manager, IStateDriverProvider provider, IStatePlugin stateLogger)
    {
      _event = event_;
      _stateLogger = stateLogger;
      _intervalManager = manager;
      _stateDriverProvider = provider;
    }

    public IStateManager SetName(string name)
    {
      Name = name;

      return this;
    }

    private T ResolveState<T>(string address, int length = 0) where T : State, new()
    {
      var state = new T() {
        Event = _event,
        IntervalManager = _intervalManager,
      };

      state.Name = Name;
      state.Length = length;
      state.UseDriver(_stateDriverProvider.Resolve());
      state.UseAddress(address);
      States.Add(Name, state);

      return state;
    }

    public IStateBool Bool(string address)
    {
      var state = ResolveState<StateBool>(address);

      state.Use(_stateLogger);

      return state;
    }

    public IStateUInt16 UInt16(string address)
    {
      var state = ResolveState<StateUInt16>(address);

      state.Use(_stateLogger);

      return state;
    }

    public IStateInt32 Int32(string address)
    {
      var state = ResolveState<StateInt32>(address);

      state.Use(_stateLogger);

      return state;
    }

    public IStateString String(string address, int length)
    {
      var state = ResolveState<StateString>(address, length);

      state.Use(_stateLogger);
      state.Length = length;

      return state;
    }

    public IStateUInt16 UShort(string address)
    {
      return UInt16(address);
    }

    public IStateInt32 Int(string address)
    {
      return Int32(address);
    }

  }
}
