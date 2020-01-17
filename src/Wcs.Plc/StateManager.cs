using System.Collections.Generic;

namespace Wcs.Plc
{
  using States = Dictionary<string, IState>;

  public class StateManager : IStateManager
  {
    private IPlcContainer _services;

    public States States { get; } = new States();

    public string Name { get; set; }

    public StateManager(IPlcContainer services)
    {
      _services = services;
    }

    public IStateManager SetName(string name)
    {
      Name = name;

      return this;
    }

    public IStateBit Bit(string key)
    {
      var state = new StateBit(_services) {
        Key = key,
        Length = 1,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateBits Bits(string key, int length = 1)
    {
      var state = new StateBits(_services) {
        Key = key,
        Length = length,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateWord Word(string key)
    {
      var state = new StateWord(_services) {
        Key = key,
        Length = 1,
      };

      States.Add(Name, state);

      return state;
    }

    public IStateWords Words(string key, int length = 1)
    {
      var state = new StateWords(_services) {
        Key = key,
        Length = length,
      };

      States.Add(Name, state);

      return state;
    }
  }
}
