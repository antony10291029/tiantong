using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcWorker
  {
    private readonly PlcClient _client;

    private readonly IntervalManager _intervalManager;

    internal DomainContextFactory _domain;

    private CancellationTokenSource _stoppingToken;

    public List<IState> States;

    //

    public PlcWorker(
      PlcClient client,
      IntervalManager manager,
      DomainContextFactory domain
    ) {
      _client = client;
      _domain = domain;
      _intervalManager = manager;
    }

    public PlcClient Client() => _client;

    //

    public void Set(IState state, string value)
    {
      _client.Set(state, value);
    }

    public string Get(IState state)
    {
      return _client.Get(state);
    }

    public void Set(string name, string value)
    {
      var state = _client.State(name);

      try {
        state.Set(value);
      } catch (Exception e) {
        Reconnect(e);
        throw;
      }
    }

    public string Get(string name)
    {
      string value;
      var state = _client.State(name);

      try {
        value = state.Get();
      } catch (Exception e) {
        Reconnect(e);
        throw;
      }

      return value;
    }

    public string Collect(string name, int interval = 1000)
    {
      return _client.StatesByName()[name].Collect(interval);
    }

    public Dictionary<string, string> GetValues()
    {
      var dict = new Dictionary<string, string>();

      foreach (var state in _client.StatesById().Values) {
        if (state.IsCollect) {
          dict.Add(state.Name(), state.CurrentValue);
        }
      }

      return dict;
    }

    public Dictionary<string, string> GetAllValues()
    {
      var dict = new Dictionary<string, string>();

      foreach (var state in _client.StatesById().Values) {
        dict.Add(state.Name(), state.IsCollect ? state.CurrentValue : state.Collect());
      }

      return dict;
    }

    public virtual void Log(string message)
    {
      _domain.Log(new PlcLog {
        plc_id = _client.Options().Id,
        message = message
      });
    }

    public void Test()
    {
      _client.Connect();
      _client.Close();
    }

    public PlcWorker Stop()
    {
      if (_stoppingToken != null) {
        _stoppingToken.Cancel();
      }

      _intervalManager.Stop().Wait();

      Log("通信服务已停止");

      return this;
    }

    public void Connect()
    {
      _client.Connect();
    }

    public void Close()
    {
      _client.Close();
    }

    public void Reconnect(Exception e)
    {
      Log($"通信错误: {e.Message}");
      _intervalManager.Stop().Wait();
    }

    public async Task RunAsync()
    {
      _stoppingToken = new CancellationTokenSource();

      while (!_stoppingToken.IsCancellationRequested) {
        try {
          _client.Connect();
          Log("通信服务启动成功");

          try {
            await _intervalManager.Start().WaitAsync();
          } catch (Exception e) {
            Log($"发生通信错误: {e.Message}");
          }

        } catch (Exception e) {
          Log($"通信服务启动失败: {e.Message}");
        }

        try {
          _client.Close();
        } catch (Exception e) {
          Log($"通信服务关闭失败: {e.Message}");
        }

        if (!_stoppingToken.IsCancellationRequested) {
          Log("通信服务正在重启...");
          await Task.Delay(1000, _stoppingToken.Token);
        }
      }

      _stoppingToken = null;
    }

    public Task WaitAsync()
    {
      return _intervalManager.WaitAsync();
    }

    public void Wait()
    {
      _intervalManager.Wait();
    }

    public void Run()
    {
      RunAsync().GetAwaiter().GetResult();
    }
  }
}