using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wcs.Plc
{
  public class Plc : IPlc
  {
    public IContainer Container { get; set; }

    public IEvent Event
    {
      get => Container.Event;
    }

    public IIntervalManager IntervalManager
    {
      get => Container.IntervalManager;
    }

    public IStateManager StateManager
    {
      get => Container.StateManager;
    }

    public Plc()
    {
      var container = new Container() {
        Event =  new Event(),
        StateDriver = new StateTestDriver(),
        IntervalManager = new IntervalManager(),
      };
      container.StateManager = new StateManager(container);

      Container = container;
    }

    //

    static public IPlcWorker GetWorker()
    {
      return (IPlcWorker)new Plc();
    }

    //

    public IPlcWorker Mode(string mode)
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
      StateManager.Name = name;

      return StateManager;
    }

    //

    public IStateWord Word(string name)
    {
      return StateManager.States[name].Convert<IStateWord>();
    }

    public IStateWords Words(string name)
    {
      return StateManager.States[name].Convert<IStateWords>();
    }

    public IStateBit Bit(string name)
    {
      return StateManager.States[name].Convert<IStateBit>();
    }

    public IStateBits Bits(string name)
    {
      return StateManager.States[name].Convert<IStateBits>();
    }

    //

    public void On<T>(string key, Func<T, Task> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Func<Task> handler)
    {
      Event.On(key, handler);
    }

    public void On<T>(string key, Action<T> handler)
    {
      Event.On<T>(key, handler);
    }

    public void On(string key, Action handler)
    {
      Event.On(key, handler);
    }

    //

    public IComparableWatcher<T> CreateComparableWatcher<T>(string key) where T : IComparable<T>
    {
      var watcher = new ComparableWatcher<T>(Event);
      var state = StateManager.States[key].Convert<IState<T>>();
      var hook = state.AddGetHook(value => watcher.Handle(value));

      return watcher;
    }

    private IWatcher<T> CreateWatcher<T>(string key)
    {
      var watcher = new Watcher<T>(Event);
      var state = StateManager.States[key].Convert<IState<T>>();
      var hook = state.AddGetHook(value => watcher.Handle(value));

      return watcher;
    }

    public IWatcher<T> Watch<T>(string key, T value) where T : IComparable<T>
    {
      return CreateComparableWatcher<T>(key).When(data => data.CompareTo(value) == 0);
    }

    public IWatcher<T> Watch<T>(string key, string opt, T value) where T : IComparable<T>
    {
      return CreateComparableWatcher<T>(key).When(opt, value).Event(key);
    }

    public IWatcher<T> Watch<T>(string key, Func<T, bool> handler)
    {
      return CreateWatcher<T>(key).When(handler);
    }

    //

    public IPlcWorker Start()
    {
      IntervalManager.Start();

      return this;
    }

    public IPlcWorker Stop()
    {
      IntervalManager.Stop();

      return this;
    }

    public Task WaitAsync()
    {
      return IntervalManager.WaitAsync();
    }

    public void Wait()
    {
      IntervalManager.Wait();
    }

    public Task RunAsync()
    {
      return IntervalManager.RunAsync();
    }

    public void Run()
    {
      IntervalManager.Run();
    }
  }
}
