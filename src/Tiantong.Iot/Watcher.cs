using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace Tiantong.Iot
{
  public class Watcher<T> : IWatcher<T>
  {
    private int _plcId;

    private int _stateId;

    private int _watcherId;

    private IWatcherHttpClient _httpClient;

    private Action _cancel;

    private Action<T> _handler;

    protected Func<T, bool> _when;

    public IWatcher<T> Id(int plcId, int stateId, int watcherId)
    {
      _plcId = plcId;
      _stateId = stateId;
      _watcherId = watcherId;

      return this;
    }

    public Watcher(IWatcherHttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public void Emit(T value)
    {
      if (_when(value)) {
        try {
          _handler(value);
        } catch (Exception e) {
          Console.WriteLine(e);
        }
      }
    }

    public IWatcher<T> When(Func<T, bool> when)
    {
      _when = when;

      return this;
    }

    public void On(Action<T> handler)
    {
      _handler = handler;
    }

    public void HttpPost(string url, string valueKey, bool toString = false, string json = null, Encoding encoding = null)
    {
      var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json ?? "{}");
      _handler = value => {
        if (toString) {
          data[valueKey] = value.ToString();
        } else {
          data[valueKey] = value;
        }

        _httpClient.PostAsync(_plcId, _stateId, _watcherId, url, JsonSerializer.Serialize(data), encoding);
      };
    }

    public void OnCancel(Action cancel)
    {
      _cancel = cancel;
    }

  }
}
