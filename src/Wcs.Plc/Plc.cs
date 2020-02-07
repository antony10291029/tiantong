using System;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public PlcContainer Container { get; set; }

    private Event _event
    {
      get => Container.Event;
    }

    private IntervalManager _intervalManager
    {
      get => Container.IntervalManager;
    }

    private StateManager _stateManager
    {
      get => Container.StateManager;
    }

    public Plc()
    {
      Container = ResolveContainer();
      Container.Plc = this;
    }

    //

    protected virtual PlcContainer ResolveContainer()
    {
      return new PlcContainer();
    }

    public IPlc Id(int id)
    {
      Container.PlcConnection.Id = id;

      return this;
    }

    public IPlc Model(string model)
    {
      Container.PlcConnection.Model = model;

      return this;
    }

    public IPlc Name(string name)
    {
      Container.PlcConnection.Name = name;

      return this;
    }

    public IPlc Host(string host)
    {
      Container.PlcConnection.Host = host;
      
      return this;
    }

    public IPlc Port(string port)
    {
      Container.PlcConnection.Port = port;

      return this;
    }

    public IStateManager State(string name)
    {
      _stateManager.Name = name;

      return _stateManager;
    }

    //

    public IStateWord Word(string name)
    {
      return _stateManager.States[name].Convert<IStateWord>();
    }

    public IStateWords Words(string name)
    {
      return _stateManager.States[name].Convert<IStateWords>();
    }

    public IStateBit Bit(string name)
    {
      return _stateManager.States[name].Convert<IStateBit>();
    }

    public IStateBits Bits(string name)
    {
      return _stateManager.States[name].Convert<IStateBits>();
    }

    //

    public void On<T>(string key, Func<T, Task> handler)
    {
      _event.On<T>(key, handler);
    }

    public void On(string key, Func<Task> handler)
    {
      _event.On(key, handler);
    }

    public void On<T>(string key, Action<T> handler)
    {
      _event.On<T>(key, handler);
    }

    public void On(string key, Action handler)
    {
      _event.On(key, handler);
    }

    //

    public IPlc Start()
    {
      _intervalManager.Start();

      return this;
    }

    public IPlc Stop()
    {
      _intervalManager.Stop();

      return this;
    }

    public Task WaitAsync()
    {
      return _intervalManager.WaitAsync();
    }

    public void Wait()
    {
      _intervalManager.Wait();
    }

    public Task RunAsync()
    {
      return _intervalManager.RunAsync();
    }

    public void Run()
    {
      _intervalManager.Run();
    }
  }
}
