using System;
using System.Threading.Tasks;

namespace Tiantong.Iot
{
  public class PlcWorker : IPlcWorker
  {
    public int _id;

    public string _model = "test";

    public string _name;

    public string _host;

    public int _port;

    internal IStatePlugin StateLogger { get; private set; }

    internal IWatcherProvider WatcherProvider { get; private set; }

    internal DatabaseProvider DatabaseProvider { get; private set; }

    internal StateManager StateManager { get; private set; }

    internal IntervalManager IntervalManager { get; private set; }

    internal IStateDriverProvider StateDriverProvider { get; private set; }

    //

    public IPlcWorker Id(int id)
    {
      _id = id;

      return this;
    }

    public IPlcWorker Model(string model)
    {
      _model = model;

      return this;
    }

    public IPlcWorker Name(string name)
    {
      _name = name;

      return this;
    }

    public IPlcWorker Host(string host)
    {
      _host = host;
      
      return this;
    }

    public IPlcWorker Port(int port)
    {
      _port = port;

      return this;
    }

    //

    //

    public virtual IStatePlugin ResolveStateLogger()
    {
      return new StateLogger(this, IntervalManager, DatabaseProvider.Resolve());
    }

    public virtual DatabaseProvider ResolveDatabaseProvider()
    {
      return new DatabaseProvider();
    }

    public virtual IWatcherProvider ResolveWatcherProvider()
    {
      return new WatcherProvider(ResolveWatcherHttpClient());
    }

    public virtual IWatcherHttpClient ResolveWatcherHttpClient()
    {
      return new WatcherHttpClient();
    }

    public IStateDriverProvider ResolveStateDriverProvider()
    {
      return _model switch {
        "test" => new StateTestDriverProvider(),
        "MC3E" => new MC3EDriverProvider(_host, _port),
        "S7200Smart"=> new S7200SmartDriverProvider(_host, _port),
        _ => throw new Exception("plc model is not supporting"),
      };
    }

    //

    public IPlcWorker Build()
    {
      IntervalManager = new IntervalManager();
      WatcherProvider = ResolveWatcherProvider();
      DatabaseProvider = ResolveDatabaseProvider();
      StateDriverProvider = ResolveStateDriverProvider();
      StateLogger = ResolveStateLogger();
      ResolveStateManager();

      DatabaseProvider.Migrate();

      return this;
    }

    public virtual IPlcWorker UseTest()
    {
      return Build();
    }

    public virtual IPlcWorker UseS7200Smart(string host, int port = 102)
    {
      Model("S7200Smart").Host(host).Port(port).Build();

      return this;
    }

    public virtual IPlcWorker UseMC3E(string host, int port)
    {
      Model("MC3E").Host(host).Port(port).Build();

      return this;
    }

    public virtual void ResolveStateManager()
    {
      StateManager = new StateManager(IntervalManager, StateDriverProvider, StateLogger, WatcherProvider);
    }

    public IStateManager Define(string name, int id = 0)
    {
      return StateManager.Name(name).Id(id);
    }

    //

    public IState<bool> Bool(string name)
    {
      return (IState<bool>) StateManager.StatesByName[name];
    }

    public IState<ushort> UInt16(string name)
    {
      return (IState<ushort>) StateManager.StatesByName[name];
    }

    public IState<int> Int32(string name)
    {
      return (IState<int>) StateManager.StatesByName[name];
    }

    public IState<string> String(string name)
    {
      return (IState<string>) StateManager.StatesByName[name];
    }

    public IState<byte[]> Bytes(string name)
    {
      return (IState<byte[]>) StateManager.StatesByName[name];
    }

    //

    public IState<ushort> UShort(string name)
    {
      return UInt16(name);
    }

    public IState<int> Int(string name)
    {
      return Int32(name);
    }

    //

    public IState<bool> Bool(int id)
    {
      return (IState<bool>) StateManager.StatesById[id];
    }

    public IState<ushort> UInt16(int id)
    {
      return (IState<ushort>) StateManager.StatesById[id];
    }

    public IState<int> Int32(int id)
    {
      return (IState<int>) StateManager.StatesById[id];
    }

    public IState<string> String(int id)
    {
      return (IState<string>) StateManager.StatesById[id];
    }

    public IState<byte[]> Bytes(int id)
    {
      return (IState<byte[]>) StateManager.StatesById[id];
    }

    //

    public IState<ushort> UShort(int id)
    {
      return UInt16(id);
    }

    public IState<int> Int(int id)
    {
      return Int32(id);
    }

    //

    public IPlcWorker Start()
    {
      StateDriverProvider.Boot();
      IntervalManager.Start();

      return this;
    }

    public IPlcWorker Stop()
    {
      IntervalManager.Stop();
      DatabaseProvider.Resolve().SaveChanges();

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
      return Start().WaitAsync();
    }

    public void Run()
    {
      while (true) {
        try {
          RunAsync().GetAwaiter().GetResult();
        } catch (Exception e) {
          Console.WriteLine(e);
          Task.Delay(1000).GetAwaiter().GetResult();
        }
      }
    }

  }
}
