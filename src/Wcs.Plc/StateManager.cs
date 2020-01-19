using System.Collections.Generic;

namespace Wcs.Plc
{
  using States = Dictionary<string, IState>;

  public class StateManager : IStateManager
  {
    private IPlcContainer Container;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(IPlcContainer container)
    {
      Container = container;
    }

    public IStateManager SetName(string name)
    {
      Name = name;

      return this;
    }

    public IStateBit Bit(string key)
    {
      var state = new StateBit(Container) {
        Key = key,
        Name = Name,
        Length = 1,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateBits Bits(string key, int length = 1)
    {
      var state = new StateBits(Container) {
        Key = key,
        Name = Name,
        Length = length,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateWord Word(string key)
    {
      var state = new StateWord(Container) {
        Key = key,
        Name = Name,
        Length = 1,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateWords Words(string key, int length = 1)
    {
      var state = new StateWords(Container) {
        Key = key,
        Name = Name,
        Length = length,
      };

      States.Add(Name, state);

      return state;
    }
  }
}
