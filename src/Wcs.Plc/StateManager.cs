using System.Collections.Generic;

namespace Wcs.Plc
{
  using States = Dictionary<string, State>;

  public class StateManager : IStateManager
  {
    private PlcContainer Container;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(PlcContainer container)
    {
      Container = container;
    }

    public IStateManager SetName(string name)
    {
      Name = name;

      return this;
    }

    private T ResolveState<T>(string key, int length) where T : State, new()
    {
      var state = new T() {
        Event = Container.Event,
        IntervalManager = Container.IntervalManager,
        StateClient = Container.StateClientProvider.Resolve()
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

      state.Use(Container.StateLogger);

      return state;
    }

    public IStateBits Bits(string key, int length = 1)
    {
      var state = ResolveState<StateBits>(key, length);

      state.Use(Container.StateLogger);

      return state;
    }

    public IStateWord Word(string key)
    {
      var state = ResolveState<StateWord>(key, 1);

      state.Use(Container.StateLogger);

      return state;
    }

    public IStateWords Words(string key, int length = 1)
    {
      var state = ResolveState<StateWords>(key, length);

      state.Use(Container.StateLogger);

      return state;
    }
  }
}
