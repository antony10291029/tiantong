using System;
using System.Linq;
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

    internal HttpPusherLogger HttpPusherLogger { get; private set; }

    internal IHttpPusherClient HttpPusherClient { get; private set; }

    internal DatabaseManager DatabaseManager { get; private set; }

    internal StateManager StateManager { get; private set; }

    internal IntervalManager IntervalManager { get; private set; }

    internal IStateDriverProvider StateDriverProvider { get; private set; }

    internal DatabaseFactory _databaseFactory = new DatabaseFactory();

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
      return new StateManager(StateDriverProvider, OnStateError);
    }

    public virtual void OnStateError(PlcStateError error)
    {
      _databaseFactory.Log(error);
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

      HttpPusherLogger = ResolveHttpPusherLogger();

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

    public IState State(string name)
    {
      return StateManager.StatesByName[name];
    }

    public IState<T> State<T>(string name)
    {
      return StateManager.StatesByName[name] as IState<T>;
    }

    public IState State(int id)
    {
      return StateManager.StatesById[id];
    }

    public IState<T> State<T>(int id)
    {
      return StateManager.StatesById[id] as IState<T>;
    }

    //

    //

    private void Set<T>(IState<T> state, T value)
    {
      state.Set(value);

      if (state.IsWriteLogOn()) {
        _databaseFactory.Log(new PlcStateLog {
          plc_id = _id,
          state_id = state.Id(),
          operation = StateOperation.Write,
          value = value.ToString(),
        });
      }
    }

    public T Get<T>(IState<T> state)
    {
      var value = state.Get();

      if (state.IsReadLogOn()) {
        _databaseFactory.Log(new PlcStateLog {
          plc_id = _id,
          state_id = state.Id(),
          operation = StateOperation.Read,
          value = value.ToString(),
        });
      }

      return value;
    }

    public void Set<T>(int id, T value)
    {
      Set(StateManager.StatesById[id] as IState<T>, value);
    }

    public T Get<T>(int id)
    {
      return Get(StateManager.StatesById[id] as IState<T>);
    }

    public void Set<T>(string name, T value)
    {
      Set(StateManager.StatesByName[name] as IState<T>, value);
    }

    public T Get<T>(string name)
    {
      return Get(StateManager.StatesByName[name] as IState<T>);
    }

    public Dictionary<string, string> GetCurrentStateValues()
    {
      var dict = new Dictionary<string, string>();

      foreach (var pair in StateManager.StatesById) {
        dict.Add(pair.Key.ToString(), pair.Value.CollectString());
      }

      return dict;
    }

    public IPlcWorker Heartbeat(string name, int interval = 1000, int maxValue = 10000)
    {
      var value = 1;

      Action handler = () => {
        if (value < maxValue) {
          value = value + 1;
        } else {
          value = 1;
        }

        State(name).SetString(value.ToString());
      };

      IntervalManager.Add(new Interval(handler, interval));

      return this;
    }

    public IPlcWorker Collect<T>(string name, int interval = 1000)
    {
      Action handler = () => State<T>(name).Collect(interval);

      IntervalManager.Add(new Interval(handler, interval));

      return this;
    }

    public IStateHttpPusher HttpPusher<T>(string name)
    {
      var state = StateManager.StatesByName[name] as IState<T>;
      var pusher = new StateHttpPusher<T>(HttpPusherClient);

      state.AddGetHook(value => pusher.Emit(value));

      return pusher;
    }

    public void Watch(string name, Action<string> handler)
    {
      var state = StateManager.StatesByName[name];
      var watcher = new Watcher<string>();

      state.AddGetHook(value => watcher.Emit(value));
      watcher.On(handler);
    }

    public void Watch<T>(string name, Action<T> handler)
    {
      var state = StateManager.StatesByName[name] as IState<T>;
      var watcher = new Watcher<T>();

      state.AddGetHook(value => watcher.Emit(value));
      watcher.On(handler);
    }

    private void Log(string message)
    {
      _databaseFactory.Log(new PlcLog {
        plc_id = _id,
        message = message
      });
    }

    private void HandleStart()
    {
      HttpPusherLogger?.UseDbContext(DatabaseManager.Resolve());

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
      Log("通信程序开始运行");

      return this;
    }

    public IPlcWorker Stop()
    {
      if (_stoppingToken != null) {
        _stoppingToken.Cancel();
      }

      Log("通信程序已停止");
      HandleStop();

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
            Log($"发生通信异常：{e.Message}");
            HandleStop();
          } catch {}

          while (!_stoppingToken.IsCancellationRequested) {
            try {
              Log($"正在尝试重启");
              await Task.Delay(1000, _stoppingToken.Token);
              HandleStart();
              Log($"服务重启成功");
              break;
            } catch (Exception ex) {
              Log($"服务重启失败，${ex.Message}");
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
