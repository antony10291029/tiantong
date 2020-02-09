using System.Collections.Generic;

namespace Wcs.Plc
{
  using States = Dictionary<string, State>;

  public class StateManager : IStateManager
  {
    private Event _event;

    private IStatePlugin _stateLogger;

    private IntervalManager _intervalManager;

    private IStateClientProvider _stateClientProvider;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(Event event_, IntervalManager manager, IStateClientProvider provider, IStatePlugin stateLogger)
    {
      _event = event_;
      _stateLogger = stateLogger;
      _intervalManager = manager;
      _stateClientProvider = provider;
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
        StateClient = _stateClientProvider.Resolve()
      };

      state.Key = key;
      state.Name = Name;
      state.Length = length;
      States.Add(Name, state);

      return state;
    }

    public IStateBit Bit(string key)
    {
      var state = ResolveState<StateBit>(key, 1);

      state.Use(_stateLogger);

      return state;
    }

    public IStateBits Bits(string key, int length = 1)
    {
      var state = ResolveState<StateBits>(key, length);

      state.Use(_stateLogger);

      return state;
    }

    public IStateWord Word(string key)
    {
      var state = ResolveState<StateWord>(key, 1);

      state.Use(_stateLogger);

      return state;
    }

    public IStateWords Words(string key, int length = 1)
    {
      var state = ResolveState<StateWords>(key, length);

      state.Use(_stateLogger);

      return state;
    }
  }
}
