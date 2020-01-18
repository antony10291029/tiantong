using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public IPlcContainer Container { get; set; }

    private IEvent _event
    {
      get => Container.Event;
    }

    private IIntervalManager _intervalManager
    {
      get => Container.IntervalManager;
    }

    private IStateManager _stateManager
    {
      get => Container.StateManager;
    }

    public Plc()
    {
      Container = ResolveContainer();
      Container.Plc = this;
    }

    //

    protected virtual IPlcContainer ResolveContainer()
    {
      return new PlcContainer();
    }

    public IPlcWorker Model(string mode)
    {
      return this;
    }

    public IPlcWorker Name(string name)
    {
      return this;
    }

    public IPlcWorker Host(string host)
    {
      return this;
    }

    public IPlcWorker Port(string port)
    {
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

    public IComparableWatcher<T> CreateComparableWatcher<T>(string key) where T : IComparable<T>
    {
      var watcher = new ComparableWatcher<T>(_event);
      var state = _stateManager.States[key].Convert<IState<T>>();
      var hook = state.AddGetHook(value => watcher.Handle(value));

      return watcher;
    }

    private IWatcher<T> CreateWatcher<T>(string key)
    {
      var watcher = new Watcher<T>(_event);
      var state = _stateManager.States[key].Convert<IState<T>>();
      var hook = state.AddGetHook(value => watcher.Handle(value));

      return watcher;
    }

    public IWatcher<T> Watch<T>(string key, T value) where T : IComparable<T>
    {
      return CreateComparableWatcher<T>(key).When(data => data.CompareTo(value) == 0);
    }

    public IWatcher<T> Watch<T>(string key, string opt, T value) where T : IComparable<T>
    {
      return CreateComparableWatcher<T>(key).When(opt, value);
    }

    public IWatcher<T> Watch<T>(string key, Func<T, bool> handler)
    {
      return CreateWatcher<T>(key).When(handler);
    }

    //

    public IPlcWorker Start()
    {
      _intervalManager.Start();

      return this;
    }

    public IPlcWorker Stop()
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
