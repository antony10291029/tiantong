using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tiantong.Iot.Entities;

namespace Tiantong.Iot.Api
{
  public class PlcWorker
  {
    private PlcClient _client;

    private IntervalManager _intervalManager;

    internal DomainContextFactory _domain;

    private CancellationTokenSource _stoppingToken;

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

    private void Set<T>(IState<T> state, T value)
    {
      try {
        state.Set(value);
      } catch (Exception e) {
        Reconnect(e);
      }

      if (state.IsWriteLogOn()) {
        _domain.Log(new PlcStateLog {
          plc_id = _client.Options().Id(),
          state_id = state.Id(),
          operation = StateOperation.Write,
          value = value?.ToString() ?? "",
        });
      }
    }

    private T Get<T>(IState<T> state)
    {
      T value;

      try {
        value = state.Get();
      } catch (Exception e) {
        Reconnect(e);
        throw e;
      }

      if (state.IsReadLogOn()) {
        _domain.Log(new PlcStateLog {
          plc_id = _client.Options().Id(),
          state_id = state.Id(),
          operation = StateOperation.Read,
          value = value.ToString() ?? "",
        });
      }

      return value;
    }

    public void Set<T>(int id, T value)
    {
      Set(_client.State<T>(id), value);
    }

    public T Get<T>(int id)
    {
      return Get(_client.State<T>(id));
    }

    public void Set<T>(string name, T value)
    {
      Set(_client.State<T>(name), value);
    }

    public T Get<T>(string name)
    {
      return Get(_client.State<T>(name));
    }

    public Dictionary<string, string> GetCurrentStateValues()
    {
      var dict = new Dictionary<string, string>();

      foreach (var pair in _client.StatesById()) {
        dict.Add(pair.Key.ToString(), pair.Value.CollectString());
      }

      return dict;
    }

    public void SetString(int id, string value)
    {
      _client.State(id).SetString(value);
    }

    public string GetString(IState state)
    {
      return _client.GetString(state);
    }

    public void SetString(IState state, string value)
    {
      _client.SetString(state, value);
    }

    public virtual void Log(string message)
    {
      _domain.Log(new PlcLog {
        plc_id = _client.Options().Id(),
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
        } catch {}

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