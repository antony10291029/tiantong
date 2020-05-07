using System.Collections.Generic;

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

    private T ResolveState<T>(string key, int length = 0) where T : State, new()
    {
      var state = new T() {
        Event = _event,
        IntervalManager = _intervalManager,
      };

      state.Name = Name;
      state.Length = length;
      state.UseDriver(_stateDriverProvider.Resolve());
      state.UseAddress(key);
      States.Add(Name, state);

      return state;
    }

    public IStateBool Bool(string key)
    {
      var state = ResolveState<StateBool>(key);

      state.Use(_stateLogger);

      return state;
    }

    public IStateUInt16 UInt16(string key)
    {
      var state = ResolveState<StateUInt16>(key);

      state.Use(_stateLogger);

      return state;
    }

    public IStateInt32 Int32(string key)
    {
      var state = ResolveState<StateInt32>(key);

      state.Use(_stateLogger);

      return state;
    }

    public IStateString String(string key, int length)
    {
      var state = ResolveState<StateString>(key, length);

      state.Use(_stateLogger);
      state.Length = length;

      return state;
    }

    public IStateUInt16 UShort(string key)
    {
      return UInt16(key);
    }

    public IStateInt32 Int(string key)
    {
      return Int32(key);
    }

  }
}
