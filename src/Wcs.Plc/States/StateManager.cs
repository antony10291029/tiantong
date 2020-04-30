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

    private T ResolveState<T>(string key, int length) where T : State, new()
    {
      var state = new T() {
        Event = _event,
        IntervalManager = _intervalManager,
        Driver = _stateDriverProvider.Resolve()
      };

      state.Name = Name;
      state.UseAddress(key, length);
      States.Add(Name, state);

      return state;
    }

    public IStateBool Bool(string key, int length = 1)
    {
      var state = ResolveState<StateBool>(key, length);

      state.Use(_stateLogger);

      return state;
    }

    public IStateInt Int(string key, int length = 4)
    {
      var state = ResolveState<StateInt>(key, length);

      state.Use(_stateLogger);

      return state;
    }

    public IStateString String(string key, int length = 10)
    {
      var state = ResolveState<StateString>(key, length);

      state.Use(_stateLogger);

      return state;
    }
  }
}
