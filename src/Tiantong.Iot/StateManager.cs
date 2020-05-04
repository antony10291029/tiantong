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

    public Dictionary<int, IState> StatesById = new Dictionary<int, IState>();

    public Dictionary<string, IState> StatesByName = new Dictionary<string, IState>();

    public int _id { get; set; }

    private string _name { get; set; }

    public StateManager(IntervalManager manager, IStateDriverProvider provider, IStatePlugin stateLogger)
    {
      _stateLogger = stateLogger;
      _intervalManager = manager;
      _stateDriverProvider = provider;
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

    private T ResolveState<T, U>(string address, int length = 0) where T : State<U>, new()
    {
      var state = new T() {
        IntervalManager = _intervalManager,
      };

      state.Id(_id).Name(_name);
      state.Length = length;
      state.UseDriver(_stateDriverProvider.Resolve());
      state.UseAddress(address);
      StatesByName.Add(_name, state);
      if (_id != 0) {
        StatesById.Add(_id, state);
      }
      state.Use(_stateLogger);

      return state;
    }

    public IState<bool> Bool(string address)
    {
      return ResolveState<StateBool, bool>(address);
    }

    public IState<ushort> UInt16(string address)
    {
      return ResolveState<StateUInt16, ushort>(address);
    }

    public IState<int> Int32(string address)
    {
      return ResolveState<StateInt32, int>(address);
    }

    public IState<string> String(string address, int length)
    {
      return ResolveState<StateString, string>(address, length);
    }

    public IState<ushort> UShort(string address)
    {
      return UInt16(address);
    }

    public IState<int> Int(string address)
    {
      return Int32(address);
    }

  }
}
