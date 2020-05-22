using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot
{
  public class PlcWorker : IPlcWorker
  {
    public int _id { get; set; }

    public string _model { get; set; } = PlcModel.Test;

    public string _name { get; set; }

    public string _host { get; set; }

    public int _port { get; set; }

    public IPlcWorkerLogger Logger { get; private set; }

    internal IStateLogger StateLogger { get; private set; }

    internal StateErrorLogger StateErrorLogger { get; private set; }

    internal HttpPusherLogger HttpPusherLogger { get; private set; }

    internal IHttpPusherClient HttpPusherClient { get; private set; }

    internal DatabaseManager DatabaseManager { get; private set; }

    internal StateManager StateManager { get; private set; }

    internal IntervalManager IntervalManager { get; private set; }

    internal IStateDriverProvider StateDriverProvider { get; private set; }

    private CancellationTokenSource _stoppingToken;

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

    public virtual IStateLogger ResolveStateLogger()
    {
      return new StateLogger(_id, IntervalManager);
    }

    public virtual IPlcWorkerLogger ResolvePlcWorkerLogger()
    {
      return new PlcWorkerLogger();
    }

    public virtual StateErrorLogger ResolveStateErrorLogger()
    {
      return new StateErrorLogger();
    }

    public virtual DatabaseManager ResolveDatabaseManager()
    {
      return new DatabaseManager();
    }

    public virtual HttpPusherLogger ResolveHttpPusherLogger()
    {
      return new HttpPusherLogger(IntervalManager);
    }

    public virtual IHttpPusherClient ResolveHttpPusherClient()
    {
      return new HttpPusherClient(HttpPusherLogger);
    }

    public virtual StateManager ResolveStateManager()
    {
      return new StateManager(IntervalManager, StateDriverProvider, StateLogger, HttpPusherClient, StateErrorLogger);
    }

    public IStateDriverProvider ResolveStateDriverProvider()
    {
      return _model switch {
        PlcModel.Test => new StateTestDriverProvider(),
        PlcModel.MC3E => new MC3EBinaryDriverProvider(_host, _port),
        PlcModel.MC3EBinary => new MC3EBinaryDriverProvider(_host, _port),
        PlcModel.S7200Smart=> new S7200SmartDriverProvider(_host, _port),
        _ => throw new Exception("plc model is not supporting"),
      };
    }

    //

    public IPlcWorker Build()
    {
      IntervalManager = new IntervalManager();
      DatabaseManager = ResolveDatabaseManager();
      StateDriverProvider = ResolveStateDriverProvider();

      Logger = ResolvePlcWorkerLogger();
      StateLogger = ResolveStateLogger();
      HttpPusherLogger = ResolveHttpPusherLogger();
      StateErrorLogger = ResolveStateErrorLogger();

      HttpPusherClient = ResolveHttpPusherClient();
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

    private void HandleStart()
    {
      Logger?.UseDbContext(DatabaseManager.Resolve(false));
      HttpPusherLogger?.UseDbContext(DatabaseManager.Resolve());
      StateErrorLogger?.UseDbContext(DatabaseManager.Resolve());
      StateLogger?.UseDbContext(DatabaseManager.Resolve());

      StateDriverProvider.Boot();
      IntervalManager.Start();
    }

    private void HandleStop()
    {
      try {
        IntervalManager.Stop().Wait();      
      } catch {}

      try {
        StateDriverProvider.Stop();
      } catch {}

      DatabaseManager.DisposeDbPool();
    }

    public void Test()
    {
      HandleStart();
      HandleStop();
      Wait();
    }

    public IPlcWorker Start()
    {
      HandleStart();
      Logger.Log(_id, "通信程序开始运行");

      return this;
    }

    public IPlcWorker Stop()
    {
      if (_stoppingToken != null) {
        _stoppingToken.Cancel();
      }

      Logger.Log(_id, "通信程序已停止");
      HandleStop();
      Logger.Dispose();

      return this;
    }

    public async Task RunAsync()
    {
      _stoppingToken = new CancellationTokenSource();

      while (!_stoppingToken.IsCancellationRequested) {
        try {
          await Start().WaitAsync();

          break;
        } catch (Exception e) {
          try {
            Logger.Log(_id, $"发生通信异常：{e.Message}");
            HandleStop();
          } catch {}

          while (!_stoppingToken.IsCancellationRequested) {
            try {
              Logger.Log(_id, $"正在尝试重启");
              await Task.Delay(1000, _stoppingToken.Token);
              Logger.Dispose();
              HandleStart();
              Logger.Log(_id, $"服务重启成功");
              break;
            } catch (Exception ex) {
              Logger.Log(_id, $"服务重启失败，${ex.Message}");
            } finally {
              HandleStop();
            }
          }

        }
      }

      _stoppingToken = null;
    }

    public Task WaitAsync()
    {
      return IntervalManager.WaitAsync();
    }

    public void Wait()
    {
      IntervalManager.Wait();
    }

    public void Run()
    {
      RunAsync().GetAwaiter().GetResult();
    }

  }

}
