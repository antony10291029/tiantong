using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcWorker : IPlcWorker
  {
    public int _id;

    public string _model = PlcModel.Test;

    public string _name;

    public string _host;

    public int _port;

    public IPlcWorkerLogger Logger { get; private set; }

    internal IStatePlugin StateLogger { get; private set; }

    internal IWatcherProvider WatcherProvider { get; private set; }

    internal DatabaseProvider DatabaseProvider { get; private set; }

    internal StateManager StateManager { get; private set; }

    internal IntervalManager IntervalManager { get; private set; }

    internal IStateDriverProvider StateDriverProvider { get; private set; }

    public bool _isStopping = false;

    //

    public IPlcWorker Config(Action<IPlcWorker> configer = null)
    {
      if (configer != null) {
        configer(this);
      }

      return Build();
    }

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

    public IPlcWorker Host(string host, int port = 0)
    {
      _host = host;
      if (port != 0) {
        _port = port;
      }
      
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
      return new StateLogger(_id, IntervalManager, DatabaseProvider.Resolve());
    }

    public virtual PlcWorkerLogger ResolvePlcWorkerLogger()
    {
      return new PlcWorkerLogger(DatabaseProvider.Resolve());
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
      return new WatcherHttpClient(DatabaseProvider.Resolve(), IntervalManager);
    }

    public virtual StateManager ResolveStateManager()
    {
      return new StateManager(IntervalManager, StateDriverProvider, StateLogger, WatcherProvider);
    }

    public IStateDriverProvider ResolveStateDriverProvider()
    {
      return _model switch {
        PlcModel.Test => new StateTestDriverProvider(),
        PlcModel.MC3E => new MC3EDriverProvider(_host, _port),
        PlcModel.S7200Smart=> new S7200SmartDriverProvider(_host, _port),
        _ => throw new Exception("plc model is not supporting"),
      };
    }

    //

    public IPlcWorker Build()
    {
      IntervalManager = new IntervalManager();
      DatabaseProvider = ResolveDatabaseProvider();
      StateDriverProvider = ResolveStateDriverProvider();
      WatcherProvider = ResolveWatcherProvider();
      Logger = ResolvePlcWorkerLogger();
      StateLogger = ResolveStateLogger();
      StateManager = ResolveStateManager();

      return this;
    }

    public virtual IPlcWorker UseTest()
    {
      return Build();
    }

    public virtual IPlcWorker UseS7200Smart(string host, int port = 102)
    {
      Model(PlcModel.S7200Smart).Host(host).Port(port).Build();

      return this;
    }

    public virtual IPlcWorker UseMC3E(string host, int port)
    {
      Model(PlcModel.MC3E).Host(host).Port(port).Build();

      return this;
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

    public Dictionary<string, string> GetCurrentStateValues()
    {
      var dict = new Dictionary<string, string>();

      foreach (var pair in StateManager.StatesById) {
        dict.Add(pair.Key.ToString(), pair.Value.GetCurrentValue());
      }

      return dict;
    }

    //

    private IPlcWorker HandleStart()
    {
      StateDriverProvider.Boot();
      IntervalManager.Run();

      return this;
    }

    private IPlcWorker HandleStop()
    {
      StateDriverProvider.Stop();
      IntervalManager.Stop();

      return this;
    }

    public bool Test()
    {
      try {
        HandleStart().Stop().WaitAsync();

        return true;
      } catch {
        return false;
      }
    }

    // issue:
    //   无法保证在返回 this 之前处理好 task
    public IPlcWorker Start()
    {
      Logger.Log(_id, "设备开始运行");
      Task.Run(() => {
        while (!_isStopping) {
          try {
            HandleStart();
          } catch (Exception e) {
            Logger.Log(_id, $"运行故障: {e.Message}");
            Console.WriteLine(e.Message);
            HandleStop();
            Logger.Log(_id, "正在重启设备");
            Task.Delay(1000).GetAwaiter().GetResult();
          }
        }
      });

      return this;
    }

    public IPlcWorker Stop()
    {
      Logger.Log(_id, "设备已停止运行");
      _isStopping = true;
      IntervalManager.Stop();
      StateDriverProvider.Stop();

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
      RunAsync().GetAwaiter().GetResult();
    }

  }
}
